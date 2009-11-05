using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    unsafe class WeightedAverage
    {
        public static ColorPalette Process(Bitmap bmp, int numColors)
        {
            int w = bmp.Width, h = bmp.Height, s = w * h, count = 0;

            float* pData = stackalloc float[numColors * 4];
            float* weights = stackalloc float[numColors];

            ColorF4* dPtr = (ColorF4*)pData;

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            for (ARGBPixel* sPtr = (ARGBPixel*)data.Scan0, ceiling = sPtr + s; sPtr < ceiling; sPtr++)
            {
                ColorF4 p = (ColorF4)(*sPtr);
                float distance = float.MaxValue;
                int index = -1;
                for (int x = 0; x < numColors; x++)
                {
                    if (p == dPtr[x])
                    {
                        weights[x] += 1.0f;
                        index = -1;
                        break;
                    }

                    if (x >= count)
                    {
                        dPtr[x] = (ColorF4)(*sPtr);
                        weights[x] = 1.0f;
                        count++;
                        index = -1;
                        break;
                    }

                    float d = p.DistanceTo(dPtr[x]);
                    if (d < distance)
                    {
                        distance = d;
                        index = x;
                    }
                }
                if (index != -1)
                {
                    dPtr[index].Factor(p, 1.0f / ++weights[index]);
                }
            }
            bmp.UnlockBits(data);

            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.None, numColors);
            for (int i = 0; i < count; i++)
                pal.Entries[i] = dPtr[i].ToColor();
            return pal;
        }

    }
}
