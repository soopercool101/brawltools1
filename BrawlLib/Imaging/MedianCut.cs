using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    unsafe class MedianCut
    {

        public static void Quantize(Bitmap bmp, int numColors)
        {
            int w = bmp.Width, h = bmp.Height, size = w * h;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            //If less colors than requested, skip palette creation

            //Generate histogram
            //Generate palette
            //Sort colors
            //Map indices

            bmp.UnlockBits(data);
        }

        private static void GetHistogram(ARGBPixel* src, int size)
        {
            LabPixel lab;
            int r, g, b;
            for(int i = 0 ; i < size ; i++)
            {
                lab = (LabPixel)(*src++);
                r = (int)(lab.L * CIE.lrat + 0.5);
                g = (int)((lab.A - CIE.lowA) * CIE.arat + 0.5);
                b = (int)((lab.B - CIE.lowB) * CIE.brat + 0.5);
            }
        }


        private static void Init(int numColors)
        {
        }

    }
}
