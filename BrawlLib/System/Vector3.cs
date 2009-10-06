using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace System
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public struct Vector3
    {
        public float _x;
        public float _y;
        public float _z;

        public Vector3(float x, float y, float z) { _x = x; _y = y; _z = z; }
        public Vector3(float s) { _x = s; _y = s; _z = s; }

        private const float _colorFactor = 1.0f / 255.0f;
        public static explicit operator Vector3(Color c) { return new Vector3(c.R * _colorFactor, c.G * _colorFactor, c.B * _colorFactor); }
        public static explicit operator Color(Vector3 v) { return Color.FromArgb((int)(v._x / _colorFactor), (int)(v._y / _colorFactor), (int)(v._z / _colorFactor)); }

        public static Vector3 operator -(Vector3 v) { return new Vector3(-v._x, -v._y, -v._z); }
        public static Vector3 operator +(Vector3 v1, Vector3 v2) { return new Vector3(v1._x + v2._x, v1._y + v2._y, v1._z + v2._z); }
        public static Vector3 operator -(Vector3 v1, Vector3 v2) { return new Vector3(v1._x - v2._x, v1._y - v2._y, v1._z - v2._z); }
        public static Vector3 operator -(Vector3 v1, float f) { return new Vector3(v1._x - f, v1._y - f, v1._z - f); }
        public static Vector3 operator *(Vector3 v1, Vector3 v2) { return new Vector3(v1._x * v2._x, v1._y * v2._y, v1._z * v2._z); }
        public static Vector3 operator *(Vector3 v1, float s) { return new Vector3(v1._x * s, v1._y * s, v1._z * s); }
        public static Vector3 operator *(float s, Vector3 v1) { return new Vector3(v1._x * s, v1._y * s, v1._z * s); }
        public static Vector3 operator /(Vector3 v1, Vector3 v2) { return new Vector3(v1._x / v2._x, v1._y / v2._y, v1._z / v2._z); }
        public static Vector3 operator /(Vector3 v1, float s) { return new Vector3(v1._x / s, v1._y / s, v1._z / s); }

        public static float Dot(Vector3 v1, Vector3 v2) { return (v1._x * v2._x) + (v1._y * v2._y) + (v1._z * v2._z); }
        public float Dot(Vector3 v) { return (_x * v._x) + (_y * v._y) + (_z * v._z); }

        public static Vector3 Clamp(Vector3 v1, float min, float max) { v1.Clamp(min, max); return v1; }
        public void Clamp(float min, float max) { this.Max(min); this.Min(max); }

        public static Vector3 Min(Vector3 v1, Vector3 v2) { return new Vector3(Math.Min(v1._x, v2._x), Math.Min(v1._y, v2._y), Math.Min(v1._z, v2._z)); }
        public static Vector3 Min(Vector3 v1, float f) { return new Vector3(Math.Min(v1._x, f), Math.Min(v1._y, f), Math.Min(v1._z, f)); }
        public void Min(Vector3 v) { _x = Math.Min(_x, v._x); _y = Math.Min(_y, v._y); _z = Math.Min(_z, v._z); }
        public void Min(float f) { _x = Math.Min(_x, f); _y = Math.Min(_y, f); _z = Math.Min(_z, f); }

        public static Vector3 Max(Vector3 v1, Vector3 v2) { return new Vector3(Math.Max(v1._x, v2._x), Math.Max(v1._y, v2._y), Math.Max(v1._z, v2._z)); }
        public static Vector3 Max(Vector3 v1, float f) { return new Vector3(Math.Max(v1._x, f), Math.Max(v1._y, f), Math.Max(v1._z, f)); }
        public void Max(Vector3 v) { _x = Math.Max(_x, v._x); _y = Math.Max(_y, v._y); _z = Math.Max(_z, v._z); }
        public void Max(float f) { _x = Math.Max(_x, f); _y = Math.Max(_y, f); _z = Math.Max(_z, f); }

        public float DistanceTo(Vector3 v) { Vector3 v1 = this - v; return Vector3.Dot(v1, v1); }
        public static Vector3 Lerp(Vector3 v1, Vector3 v2, float median) { return (v1 * median) + (v2 * (1.0f - median)); }

        public static Vector3 Truncate(Vector3 v)
        {
            return new Vector3(
                v._x > 0.0f ? (float)Math.Floor(v._x) : (float)Math.Ceiling(v._x),
                v._y > 0.0f ? (float)Math.Floor(v._y) : (float)Math.Ceiling(v._z),
                v._z > 0.0f ? (float)Math.Floor(v._z) : (float)Math.Ceiling(v._z));
        }
    }
}
