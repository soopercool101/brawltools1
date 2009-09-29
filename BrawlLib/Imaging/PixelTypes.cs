using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace BrawlLib.Imaging
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ARGBPixel
    {
        public byte B, G, R, A;

        public ARGBPixel(byte a, byte r, byte g, byte b) { A = a; R = r; G = g; B = b; }

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
}
