using System;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct Vector2
    {
        public float _x;
        public float _y;
        
        public Vector2(float x, float y) { _x = x; _y = y; }
        public Vector2(float s) { _x = s; _y = s; }

        public static Vector2 operator -(Vector2 v) { return new Vector2(-v._x, -v._y); }
        public static Vector2 operator +(Vector2 v1, Vector2 v2) { return new Vector2(v1._x + v2._x, v1._y + v2._y); }
        public static Vector2 operator -(Vector2 v1, Vector2 v2) { return new Vector2(v1._x - v2._x, v1._y - v2._y); }
        public static Vector2 operator -(Vector2 v1, float f) { return new Vector2(v1._x - f, v1._y - f); }
        public static Vector2 operator *(Vector2 v1, Vector2 v2) { return new Vector2(v1._x * v2._x, v1._y * v2._y); }
        public static Vector2 operator *(Vector2 v1, float s) { return new Vector2(v1._x * s, v1._y * s); }
        public static Vector2 operator *(float s, Vector2 v1) { return new Vector2(v1._x * s, v1._y * s); }
        public static Vector2 operator /(Vector2 v1, Vector2 v2) { return new Vector2(v1._x / v2._x, v1._y / v2._y); }
        public static Vector2 operator /(Vector2 v1, float s) { return new Vector2(v1._x / s, v1._y / s); }

        public static float Dot(Vector2 v1, Vector2 v2) { return (v1._x * v2._x) + (v1._y * v2._y); }
        public float Dot(Vector2 v) { return (_x * v._x) + (_y * v._y); }

        public static Vector2 Clamp(Vector2 v1, float min, float max) { v1.Clamp(min, max); return v1; }
        public void Clamp(float min, float max) { this.Max(min); this.Min(max); }

        public static Vector2 Min(Vector2 v1, Vector2 v2) { return new Vector2(Math.Min(v1._x, v2._x), Math.Min(v1._y, v2._y)); }
        public static Vector2 Min(Vector2 v1, float f) { return new Vector2(Math.Min(v1._x, f), Math.Min(v1._y, f)); }
        public void Min(Vector2 v) { _x = Math.Min(_x, v._x); _y = Math.Min(_y, v._y); }
        public void Min(float f) { _x = Math.Min(_x, f); _y = Math.Min(_y, f); }

        public static Vector2 Max(Vector2 v1, Vector2 v2) { return new Vector2(Math.Max(v1._x, v2._x), Math.Max(v1._y, v2._y)); }
        public static Vector2 Max(Vector2 v1, float f) { return new Vector2(Math.Max(v1._x, f), Math.Max(v1._y, f)); }
        public void Max(Vector2 v) { _x = Math.Max(_x, v._x); _y = Math.Max(_y, v._y); }
        public void Max(float f) { _x = Math.Max(_x, f); _y = Math.Max(_y, f); }

        public float DistanceTo(Vector2 v) { Vector2 v1 = this - v; return Vector2.Dot(v1, v1); }
        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float median) { return (v1 * median) + (v2 * (1.0f - median)); }

        public static Vector2 Truncate(Vector2 v)
        {
            return new Vector2(
                v._x > 0.0f ? (float)Math.Floor(v._x) : (float)Math.Ceiling(v._x),
                v._y > 0.0f ? (float)Math.Floor(v._y) : (float)Math.Ceiling(v._y));
        }
    }
}
