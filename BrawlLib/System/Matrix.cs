﻿using System;
using System.Runtime.InteropServices;
using BrawlLib.Modeling;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Matrix
    {
        public static readonly Matrix Identity = ScaleMatrix(1.0f, 1.0f, 1.0f);

        fixed float _values[16];

        public float* Data { get { fixed (float* ptr = _values)return ptr; } }

        public float this[int x, int y]
        {
            get { return Data[(y << 2) + x]; }
            set { Data[(y << 2) + x] = value; }
        }
        public float this[int index]
        {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public Matrix Reverse()
        {
            Matrix m;
            float* pOut = (float*)&m;
            fixed (float* p = _values)
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        *pOut++ = p[(x << 2) + y];
            return m;
        }

        public Vector3 GetPoint()
        {
            fixed (float* p = _values)
                return new Vector3(p[12], p[13], p[14]);
        }

        public static Matrix ScaleMatrix(float x, float y, float z)
        {
            Matrix m = new Matrix();
            float* p = (float*)&m;
            p[0] = x;
            p[5] = y;
            p[10] = z;
            p[15] = 1.0f;
            return m;
        }
        public static Matrix TranslationMatrix(float x, float y, float z)
        {
            Matrix m = Identity;
            float* p = (float*)&m;
            p[12] = x;
            p[13] = y;
            p[14] = z;
            return m;
        }
        public static Matrix RotationMatrix(Vector3 angles) { return RotationMatrix(angles._x, angles._y, angles._z); }
        public static Matrix RotationMatrix(float x, float y, float z)
        {
            float cosx = (float)Math.Cos(x * Maths._deg2radf);
            float sinx = (float)Math.Sin(x * Maths._deg2radf);
            float cosy = (float)Math.Cos(y * Maths._deg2radf);
            float siny = (float)Math.Sin(y * Maths._deg2radf);
            float cosz = (float)Math.Cos(z * Maths._deg2radf);
            float sinz = (float)Math.Sin(z * Maths._deg2radf);

            Matrix m = Identity;
            float* p = (float*)&m;

            m[0] = cosy * cosz;
            m[1] = sinz * cosy;
            m[2] = -siny;
            m[4] = (sinx * cosz * siny - cosx * sinz);
            m[5] = (sinx * sinz * siny + cosz * cosx);
            m[6] = sinx * cosy;
            m[8] = (sinx * sinz + cosx * cosz * siny);
            m[9] = (cosx * sinz * siny - sinx * cosz);
            m[10] = cosx * cosy;

            return m;
        }

        public void Translate(float x, float y, float z)
        {
            fixed (float* p = _values)
            {
                p[12] += (p[0] * x) + (p[4] * y) + (p[8] * z);
                p[13] += (p[1] * x) + (p[5] * y) + (p[9] * z);
                p[14] += (p[2] * x) + (p[6] * y) + (p[10] * z);
                p[15] += (p[3] * x) + (p[7] * y) + (p[11] * z);
            }
        }

        public void Multiply(Matrix* m)
        {
            Matrix m2 = this;

            float* s1 = (float*)m, s2 = (float*)&m2;

            fixed (float* dPtr = _values)
            {
                int index = 0;
                float val;
                for (int b = 0; b < 16; b += 4)
                    for (int a = 0; a < 4; a++)
                    {
                        val = 0.0f;
                        for (int x = b, y = a; y < 16; y += 4)
                            val += s1[x++] * s2[y];
                        dPtr[index++] = val;
                    }
            }
        }

        public Vector3 Multiply(Vector3 v)
        {
            Vector3 nv = new Vector3();
            fixed (float* p = _values)
            {
                nv._x = (p[0] * v._x) + (p[4] * v._y) + (p[8] * v._z) + p[12];
                nv._y = (p[1] * v._x) + (p[5] * v._y) + (p[9] * v._z) + p[13];
                nv._z = (p[2] * v._x) + (p[6] * v._y) + (p[10] * v._z) + p[14];
            }
            return nv;
        }
        public static Vector3 operator *(Matrix m, Vector3 v)
        {
            Vector3 nv;
            float* p = (float*)&m;
            nv._x = (p[0] * v._x) + (p[4] * v._y) + (p[8] * v._z) + p[12];
            nv._y = (p[1] * v._x) + (p[5] * v._y) + (p[9] * v._z) + p[13];
            nv._z = (p[2] * v._x) + (p[6] * v._y) + (p[10] * v._z) + p[14];
            return nv;
        }
        public static Vector4 operator *(Matrix m, Vector4 v)
        {
            Vector4 nv;
            float* dPtr = (float*)&nv;
            float* p0 = (float*)&m, p1 = p0 + 4, p2 = p0 + 8, p3 = p0 + 12;
            for (int i = 0; i < 4; i++)
                dPtr[i] = (p0[i] * v.x) + (p1[i] * v.y) + (p2[i] * v.z) + (p3[i] * v.w);
            return nv;
        }

        internal void Multiply(float p)
        {
            fixed (float* dPtr = _values)
            {
                for (int i = 0; i < 16; i++)
                    dPtr[i] *= p;
            }
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix dm;
            float* s1 = (float*)&m2, s2 = (float*)&m1, d = (float*)&dm;

            int index = 0;
            float val;
            for (int b = 0; b < 16; b += 4)
                for (int a = 0; a < 4; a++)
                {
                    val = 0.0f;
                    for (int x = b, y = a; y < 16; y += 4)
                        val += s1[x++] * s2[y];
                    d[index++] = val;
                }

            return dm;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            float* dPtr = (float*)&m1;
            float* sPtr = (float*)&m2;
            for (int i = 0; i < 16; i++)
                *dPtr++ += *sPtr++;
            return m1;
        }
        public static Matrix operator *(Matrix m, float f)
        {
            float* p = (float*)&m;
            for (int i = 0; i < 16; i++)
                *p++ *= f;
            return m;
        }
        public static Matrix operator -(Matrix m)
        {
            float* p = (float*)&m;
            int i = 0;
            while (i++ < 16) *p = -*p++;
            return m;
        }

        public override string ToString()
        {
            return String.Format("({0},{1},{2},{3})({4},{5},{6},{7})({8},{9},{10},{11})({12},{13},{14},{15})", this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7], this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15]);
        }


        public void RotateX(float x)
        {
            float var1, var2;
            float cosx = (float)Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float)Math.Sin(x / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[4]; var2 = p[8];
                p[4] = (var1 * cosx) + (var2 * sinx);
                p[8] = (var1 * -sinx) + (var2 * cosx);

                var1 = p[5]; var2 = p[9];
                p[5] = (var1 * cosx) + (var2 * sinx);
                p[9] = (var1 * -sinx) + (var2 * cosx);

                var1 = p[6]; var2 = p[10];
                p[6] = (var1 * cosx) + (var2 * sinx);
                p[10] = (var1 * -sinx) + (var2 * cosx);
            }
        }
        public void RotateY(float y)
        {
            float var1, var2;
            float cosy = (float)Math.Cos(y / 180.0f * Math.PI);
            float siny = (float)Math.Sin(y / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[0]; var2 = p[8];
                p[0] = (var1 * cosy) + (var2 * -siny);
                p[8] = (var1 * siny) + (var2 * cosy);

                var1 = p[1]; var2 = p[9];
                p[1] = (var1 * cosy) + (var2 * -siny);
                p[9] = (var1 * siny) + (var2 * cosy);

                var1 = p[2]; var2 = p[10];
                p[2] = (var1 * cosy) + (var2 * -siny);
                p[10] = (var1 * siny) + (var2 * cosy);
            }
        }
        public void RotateZ(float z)
        {
            float var1, var2;
            float cosz = (float)Math.Cos(z / 180.0f * Math.PI);
            float sinz = (float)Math.Sin(z / 180.0f * Math.PI);

            fixed (float* p = _values)
            {
                var1 = p[0]; var2 = p[4];
                p[0] = (var1 * cosz) + (var2 * sinz);
                p[4] = (var1 * -sinz) + (var2 * cosz);

                var1 = p[1]; var2 = p[5];
                p[1] = (var1 * cosz) + (var2 * sinz);
                p[5] = (var1 * -sinz) + (var2 * cosz);

                var1 = p[2]; var2 = p[6];
                p[2] = (var1 * cosz) + (var2 * sinz);
                p[6] = (var1 * -sinz) + (var2 * cosz);
            }
        }

        public Matrix GetRotationMatrix()
        {
            Matrix m = Identity;
            float* p = (float*)&m;
            fixed (float* src = _values)
            {
                m[0] = src[0];
                m[1] = src[1];
                m[2] = src[2];
                m[4] = src[4];
                m[5] = src[5];
                m[6] = src[6];
                m[8] = src[8];
                m[9] = src[9];
                m[10] = src[10];
            }
            return m;
        }

        //Derive Euler angles from matrix, simply by reversing the transformation process.
        //Vulnerable to gimbal lock, quaternions may be a better solution
        public Vector3 GetAngles()
        {
            float x, y, z, c;
            fixed (float* p = _values)
            {
                y = (float)Math.Asin(-p[2]);
                if ((Maths._halfPif - (float)Math.Abs(y)) < 0.0001f)
                {
                    //Gimbal lock, occurs when the y rotation falls on pi/2 or -pi/2
                    z = 0.0f;
                    if (y > 0)
                        x = (float)Math.Atan2(p[4], p[8]);
                    else
                        x = (float)Math.Atan2(p[4], -p[8]);
                }
                else
                {
                    c = (float)Math.Cos(y);
                    x = (float)Math.Atan2(p[6] / c, p[10] / c);
                    z = (float)Math.Atan2(p[1] / c, p[0] / c);

                    //180 z/x inverts y, use second option
                    if (Maths._pif - Math.Abs(z) < 0.05f)
                    {
                        y = Maths._pif - y;
                        c = (float)Math.Cos(y);
                        x = (float)Math.Atan2(p[6] / c, p[10] / c);
                        z = (float)Math.Atan2(p[1] / c, p[0] / c);
                    }
                }
            }
            return new Vector3(x, y, z) * Maths._rad2degf;
        }

        public FrameState Derive()
        {
            FrameState state = new FrameState();

            fixed (float* p = _values)
            {
                //Translation is easy!
                state._translate = *(Vector3*)&p[12];

                //Scale, use sqrt of rotation columns
                state._scale._x = (float)Math.Round(Math.Sqrt(p[0] * p[0] + p[1] * p[1] + p[2] * p[2]), 4);
                state._scale._y = (float)Math.Round(Math.Sqrt(p[4] * p[4] + p[5] * p[5] + p[6] * p[6]), 4);
                state._scale._z = (float)Math.Round(Math.Sqrt(p[8] * p[8] + p[9] * p[9] + p[10] * p[10]), 4);

                state._rotate = GetAngles();
            }

            state.CalcTransforms();
            return state;
        }

        internal void Scale(float x, float y, float z)
        {
            Matrix m = ScaleMatrix(x, y, z);
            this.Multiply(&m);
        }

        public static explicit operator Matrix(Matrix43 m)
        {
            Matrix m1;
            float* sPtr = (float*)&m;
            float* dPtr = (float*)&m1;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = 0.0f;
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = 0.0f;
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = 0.0f;
            dPtr[12] = sPtr[3];
            dPtr[13] = sPtr[7];
            dPtr[14] = sPtr[11];
            dPtr[15] = 1.0f;

            return m1;
        }
        public static explicit operator Matrix43(Matrix m)
        {
            Matrix43 m1;
            float* sPtr = (float*)&m;
            float* dPtr = (float*)&m1;

            dPtr[0] = sPtr[0];
            dPtr[1] = sPtr[4];
            dPtr[2] = sPtr[8];
            dPtr[3] = sPtr[12];
            dPtr[4] = sPtr[1];
            dPtr[5] = sPtr[5];
            dPtr[6] = sPtr[9];
            dPtr[7] = sPtr[13];
            dPtr[8] = sPtr[2];
            dPtr[9] = sPtr[6];
            dPtr[10] = sPtr[10];
            dPtr[11] = sPtr[14];

            return m1;
        }

        public static Matrix ProjectionMatrix(float fovY, float aspect, float nearZ, float farZ)
        {
            Matrix m;

            float* p = (float*)&m;

            float cotan = (float)(1.0 / Math.Tan(fovY / 2 * Math.PI / 180.0));

            p[0] = cotan / aspect;
            p[5] = cotan;
            p[10] = (farZ + nearZ) / (nearZ - farZ);
            p[11] = -1.0f;
            p[14] = (2.0f * farZ * nearZ) / (nearZ - farZ);

            p[1] = p[2] = p[3] = p[4] = p[6] = p[7] = p[8] = p[9] = p[12] = p[13] = p[15] = 0.0f;

            return m;
        }
        public static Matrix ReverseProjectionMatrix(float fovY, float aspect, float nearZ, float farZ)
        {
            Matrix m;

            float* p = (float*)&m;

            float cotan = (float)(1.0 / Math.Tan(fovY / 2 * Math.PI / 180.0));
            float val = (2.0f * farZ * nearZ) / (nearZ - farZ);

            p[0] = aspect / cotan;
            p[5] = 1.0f / cotan;
            p[11] = 1.0f / val;
            p[14] = -1.0f;
            p[15] = (farZ + nearZ) / (nearZ - farZ) / val;

            p[1] = p[2] = p[3] = p[4] = p[6] = p[7] = p[8] = p[9] = p[10] = p[12] = p[13] = 0.0f;

            return m;
        }

        public static Matrix TransformMatrix(Vector3 scale, Vector3 rotate, Vector3 translate)
        {
            Matrix m;
            float* d = (float*)&m;

            float cosx = (float)Math.Cos(rotate._x * Maths._deg2radf);
            float sinx = (float)Math.Sin(rotate._x * Maths._deg2radf);
            float cosy = (float)Math.Cos(rotate._y * Maths._deg2radf);
            float siny = (float)Math.Sin(rotate._y * Maths._deg2radf);
            float cosz = (float)Math.Cos(rotate._z * Maths._deg2radf);
            float sinz = (float)Math.Sin(rotate._z * Maths._deg2radf);

            d[0] = scale._x * cosy * cosz;
            d[1] = scale._x * sinz * cosy;
            d[2] = -scale._x * siny;
            d[3] = 0.0f;
            d[4] = scale._y * (sinx * cosz * siny - cosx * sinz);
            d[5] = scale._y * (sinx * sinz * siny + cosz * cosx);
            d[6] = scale._y * sinx * cosy;
            d[7] = 0.0f;
            d[8] = scale._z * (sinx * sinz + cosx * cosz * siny);
            d[9] = scale._z * (cosx * sinz * siny - sinx * cosz);
            d[10] = scale._z * cosx * cosy;
            d[11] = 0.0f;
            d[12] = translate._x;
            d[13] = translate._y;
            d[14] = translate._z;
            d[15] = 1.0f;

            return m;
        }

        public static Matrix ReverseTransformMatrix(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            float cosx = (float)Math.Cos(rotation._x * Maths._deg2radf);
            float sinx = (float)Math.Sin(rotation._x * Maths._deg2radf);
            float cosy = (float)Math.Cos(rotation._y * Maths._deg2radf);
            float siny = (float)Math.Sin(rotation._y * Maths._deg2radf);
            float cosz = (float)Math.Cos(rotation._z * Maths._deg2radf);
            float sinz = (float)Math.Sin(rotation._z * Maths._deg2radf);

            scale._x = 1 / scale._x;
            scale._y = 1 / scale._y;
            scale._z = 1 / scale._z;
            translation._x = -translation._x;
            translation._y = -translation._y;
            translation._z = -translation._z;

            Matrix m;
            float* p = (float*)&m;

            p[0] = scale._x * cosy * cosz;
            p[1] = scale._y * (sinx * siny * cosz - cosx * sinz);
            p[2] = scale._z * (cosx * siny * cosz + sinx * sinz);
            p[3] = 0.0f;


            p[4] = scale._x * cosy * sinz;
            p[5] = scale._y * (sinx * siny * sinz + cosx * cosz);
            p[6] = scale._z * (cosx * siny * sinz - sinx * cosz);
            p[7] = 0.0f;

            p[8] = -scale._x * siny;
            p[9] = scale._y * sinx * cosy;
            p[10] = scale._z * cosx * cosy;
            p[11] = 0.0f;

            p[12] = (translation._x * p[0]) + (translation._y * p[4]) + (translation._z * p[8]);
            p[13] = (translation._x * p[1]) + (translation._y * p[5]) + (translation._z * p[9]);
            p[14] = (translation._x * p[2]) + (translation._y * p[6]) + (translation._z * p[10]);
            p[15] = 1.0f;

            return m;
        }

        public static Matrix AxisAngleMatrix(Vector3 point1, Vector3 point2)
        {
            Matrix m = Matrix.Identity;
            //Equal points will cause a corrupt matrix
            if (point1 != point2)
            {
                float* pOut = (float*)&m;

                point1 = point1.Normalize();
                point2 = point2.Normalize();

                Vector3 vSin = point1.Cross(point2);
                Vector3 axis = vSin.Normalize();

                float cos = point1.Dot(point2);
                Vector3 vt = axis * (1.0f - cos);

                pOut[0] = vt._x * axis._x + cos;
                pOut[5] = vt._y * axis._y + cos;
                pOut[10] = vt._z * axis._z + cos;

                vt._x *= axis._y;
                vt._z *= axis._x;
                vt._y *= axis._z;

                pOut[1] = vt._x + vSin._z;
                pOut[2] = vt._z - vSin._y;
                pOut[4] = vt._x - vSin._z;
                pOut[6] = vt._y + vSin._x;
                pOut[8] = vt._z + vSin._y;
                pOut[9] = vt._y - vSin._x;
            }
            return m;
        }
    }
}
