using System;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vector4
    {
        public float x, y, z, w;

        public Vector4(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }

        public static explicit operator Vector3(Vector4 v) { return new Vector3(v.x / v.w, v.y / v.w, v.z / v.w); }
        public static explicit operator Vector4(Vector3 v) { return new Vector4(v._x, v._y, v._z, 1.0f); }

        public static Vector4 operator *(Vector4 v, float f) { return new Vector4(v.x * f, v.y * f, v.z * f, v.w * f); }
        public static Vector4 operator /(Vector4 v, float f) { return new Vector4(v.x / f, v.y / f, v.z / f, v.w / f); }
        public static Vector4 operator -(Vector4 v1, Vector4 v2) { return new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w); }

        public float Length() { return (float)Math.Sqrt(Dot()); }
        public float Dot() { return x * x + y * y + z * z + w * w; }
        public float Dot(Vector4 v) { return x * v.x + y * v.y + z * v.z + w * v.w; }
        public Vector4 Normalize() { return this * (1.0f / Length()); }

        public float Dot3() { return x * x + y * y + z * z; }
        public float Dot3(Vector4 v) { return x * v.x + y * v.y + z * v.z; }
        public float Length3() { return (float)Math.Sqrt(Dot3()); }
        public Vector4 Normalize3()
        {
            float scale = 1.0f / Length3();
            return new Vector4(x * scale, y * scale, z * scale, w);
        }
    }
}
