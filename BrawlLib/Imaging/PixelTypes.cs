using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace BrawlLib.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ARGBPixel
    {
        private const float ColorFactor = 1.0f / 255.0f;

        public byte B, G, R, A;

        public ARGBPixel(byte a, byte r, byte g, byte b) { A = a; R = r; G = g; B = b; }
        public ARGBPixel(byte intensity) { A = 255; R = intensity; G = intensity; B = intensity; }

        public int DistanceTo(Color c)
        {
            int val = A - c.A;
            int dist = val * val;
            val = R - c.R;
            dist += val * val;
            val = G - c.G;
            dist += val * val;
            val = B - c.B;
            dist += val * val;
            return dist;
        }
        public int DistanceTo(ARGBPixel p)
        {
            int val = A - p.A;
            int dist = val * val;
            val = R - p.R;
            dist += val * val;
            val = G - p.G;
            dist += val * val;
            val = B - p.B;
            return dist + val;
        }
        public bool IsGreyscale()
        {
            return (R == G) && (G == B);
        }
        public int Greyscale() { return (R + G + B) / 3; }

        public static explicit operator ARGBPixel(int val) { return *((ARGBPixel*)&val); }
        public static explicit operator int(ARGBPixel p) { return *((int*)&p); }
        public static explicit operator ARGBPixel(uint val) { return *((ARGBPixel*)&val); }
        public static explicit operator uint(ARGBPixel p) { return *((uint*)&p); }
        public static explicit operator ARGBPixel(Color val) { return (ARGBPixel)val.ToArgb(); }
        public static explicit operator Color(ARGBPixel p) { return Color.FromArgb((int)p); }
        public static explicit operator Vector3(ARGBPixel p) { return new Vector3(p.R * ColorFactor, p.G * ColorFactor, p.B * ColorFactor); }

        public ARGBPixel Min(ARGBPixel p) { return new ARGBPixel(Math.Min(A, p.A), Math.Min(R, p.R), Math.Min(G, p.G), Math.Min(B, p.B)); }
        public ARGBPixel Max(ARGBPixel p) { return new ARGBPixel(Math.Max(A, p.A), Math.Max(R, p.R), Math.Max(G, p.G), Math.Max(B, p.B)); }

        public static bool operator ==(ARGBPixel p1, ARGBPixel p2) { return *((uint*)(&p1)) == *((uint*)(&p2)); }
        public static bool operator !=(ARGBPixel p1, ARGBPixel p2) { return *((uint*)(&p1)) != *((uint*)(&p2)); }

        public override string ToString()
        {
            return String.Format("A:{0:X}, R:{1:X}, G:{2:X}, B:{3:X}", A, R, G, B);
        }
        public override int GetHashCode() { return (int)this; }
        public override bool Equals(object obj)
        {
            if (obj is ARGBPixel) return (ARGBPixel)obj == this;
            return false;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBAPixel
    {
        public byte A, B, G, R;

        public static explicit operator RGBAPixel(ARGBPixel p) { return new RGBAPixel() { A = p.A, B = p.B, G = p.G, R = p.R }; }
        public static explicit operator ARGBPixel(RGBAPixel p) { return new ARGBPixel() { A = p.A, B = p.B, G = p.G, R = p.R }; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ABGRPixel
    {
        public byte R, G, B, A;

        public ABGRPixel(byte a, byte b, byte g, byte r) { A = a; B = b; G = g; R = r; }

        public static explicit operator ABGRPixel(ARGBPixel p) { return new ABGRPixel() { A = p.A, B = p.B, G = p.G, R = p.R }; }
        public static explicit operator ARGBPixel(ABGRPixel p) { return new ARGBPixel() { A = p.A, B = p.B, G = p.G, R = p.R }; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBPixel
    {
        public byte B, G, R;

        public static explicit operator RGBPixel(ARGBPixel p) { return new RGBPixel() { R = p.R, G = p.G, B = p.B }; }
        public static explicit operator ARGBPixel(RGBPixel p) { return new ARGBPixel() { A = 0xFF, R = p.R, G = p.G, B = p.B }; }

        public static explicit operator Color(RGBPixel p) { return Color.FromArgb(p.R, p.G, p.B); }
        public static explicit operator RGBPixel(Color p) { return new RGBPixel() { R = p.R, G = p.G, B = p.B }; }

        public static RGBPixel FromIntensity(byte value) { return new RGBPixel() { R = value, G = value, B = value }; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGB555Pixel
    {
        public ushort _data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGB15Pixel
    {
        public ushort _data;
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
