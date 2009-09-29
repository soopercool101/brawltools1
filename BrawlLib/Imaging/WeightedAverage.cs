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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        unsafe struct ColorF4
        {
            private const float ColorFactor = 1.0f / 255.0f;

            public float A;
            public float R;
            public float G;
            public float B;

            public ColorF4(float a, float r, float g, float b)
            {
                A = a;
                R = r;
                G = g;
                B = b;
            }

            public float DistanceTo(ColorF4 p)
            {
                float a = A - p.A;
                float r = R - p.R;
                float g = G - p.G;
                float b = B - p.B;
                return (a * a) + (r * r) + (g * g) + (b * b);
            }

            public Color ToColor()
            {
                return Color.FromArgb((int)(A / ColorFactor + 0.5f), (int)(R / ColorFactor + 0.5f), (int)(G / ColorFactor + 0.5f), (int)(B / ColorFactor + 0.5f));
            }

            public static ColorF4 Factor(ColorF4 p1, ColorF4 p2, float factor)
            {
                float f1 = factor, f2 = 1.0f - factor;
                return new ColorF4((p1.A * f1) + (p2.A * f2), (p1.R * f1) + (p2.R * f2), (p1.G * f1) + (p2.G * f2), (p1.B * f1) + (p2.B * f2));
            }
            public void Factor(ColorF4 p, float factor)
            {
                float f1 = 1.0f - factor, f2 = factor;
                A = (A * f1) + (p.A * f2);
                R = (R * f1) + (p.R * f2);
                G = (G * f1) + (p.G * f2);
                B = (B * f1) + (p.B * f2);
            }

            public static explicit operator ColorF4(ARGBPixel p) { return new ColorF4(p.A * ColorFactor, p.R * ColorFactor, p.G * ColorFactor, p.B * ColorFactor); }

            public static bool operator ==(ColorF4 p1, ColorF4 p2) { return (p1.A == p2.A) && (p1.R == p2.R) && (p1.G == p2.G) && (p1.B == p2.B); }
            public static bool operator !=(ColorF4 p1, ColorF4 p2) { return (p1.A != p2.A) || (p1.R != p2.R) || (p1.G != p2.G) || (p1.B != p2.B); }

            public static ColorF4 operator *(ColorF4 c1, ColorF4 c2) { return new ColorF4(c1.A * c2.A, c1.R * c2.R, c1.G * c2.G, c1.B * c2.B); }
            public static ColorF4 operator *(ColorF4 c1, float f) { return new ColorF4(c1.A * f, c1.R * f, c1.G * f, c1.B * f); }
            public static ColorF4 operator +(ColorF4 c1, ColorF4 c2) { return new ColorF4(c1.A + c2.A, c1.R + c2.R, c1.G + c2.G, c1.B + c2.B); }
            public static ColorF4 operator /(ColorF4 c1, float f) { return new ColorF4(c1.A / f, c1.R / f, c1.G / f, c1.B / f); }

            public override bool Equals(object obj)
            {
                if (obj is ColorF4)
                    return this == (ColorF4)obj;
                return base.Equals(obj);
            }
            public override int GetHashCode() { return base.GetHashCode(); }
        }
    }
}
