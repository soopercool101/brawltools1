using System;
using System.Runtime.InteropServices;

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
            Matrix m = new Matrix();
            float* p = (float*)&m;
            p[12] = x;
            p[13] = y;
            p[14] = z;
            p[0] = p[5] = p[10] = p[15] = 1.0f;
            return m;
        }
        public static Matrix RotationMatrix(float x, float y, float z)
        {
            float cosx = (float)Math.Cos(x / 180.0f * Math.PI);
            float sinx = (float)Math.Sin(x / 180.0f * Math.PI);
            float cosy = (float)Math.Cos(y / 180.0f * Math.PI);
            float siny = (float)Math.Sin(y / 180.0f * Math.PI);
            float cosz = (float)Math.Cos(z / 180.0f * Math.PI);
            float sinz = (float)Math.Sin(z / 180.0f * Math.PI);

            Matrix m = Identity;
            float* p = (float*)&m;

            //p[5] = cosx;
            //p[6] = sinx;
            //p[9] = -sinx;
            //p[10] = cosx;

            //Matrix m2 = Identity;
            //float* p2 = (float*)&m2;
            //p2[0] = cosy;
            //p2[2] = -siny;
            //p2[8] = siny;
            //p2[10] = cosy;

            //Matrix m3 = Identity;
            //float* p3 = (float*)&m3;
            //p3[0] = cosz;
            //p3[1] = -sinz;
            //p3[4] = sinz;
            //p3[5] = cosz;

            //m.Multiply(&m2);
            //m.Multiply(&m3);


            m[0] = cosy * cosz;
            m[4] = cosy * sinz;
            m[8] = -siny;
            m[1] = (sinx * siny * cosz - cosx * sinz);
            m[5] = (sinx * siny * sinz + cosx * cosz);
            m[9] = sinx * cosy;
            m[2] = (cosx * siny * cosz + sinx * sinz);
            m[6] = (cosx * siny * sinz - sinx * cosz);
            m[10] = cosx * cosy;
            p[15] = 1.0f;

            //p[0] = cosy * cosz;
            //p[1] = (sinx * siny * cosz) - (cosx * sinz);
            //p[2] = (cosx * siny * cosz) + (sinx * sinz);
            //p[4] = cosy * sinz;
            //p[5] = (sinx * siny * sinz) + (cosx * cosz);
            //p[6] = (cosx * siny * sinz) - (sinx * cosz);
            //p[8] = -siny;
            //p[9] = sinx * cosy;
            //p[10] = cosx * cosy;
            //p[15] = 1.0f;

            return m;
        }

        public void Translate(float x, float y, float z)
        {
            Matrix m = TranslationMatrix(x, y, z);
            Multiply(&m);
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
            Matrix dm = new Matrix();
            float* s1 = (float*)&m1, s2 = (float*)&m2, d = (float*)&dm;

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

        public override string ToString()
        {
            return String.Format("({0},{1},{2},{3})({4},{5},{6},{7})({8},{9},{10},{11})({12},{13},{14},{15})", this[0], this[1], this[2], this[3], this[4], this[5], this[6], this[7], this[8], this[9], this[10], this[11], this[12], this[13], this[14], this[15]);
        }


        internal void Rotate(float x, float y, float z)
        {
            Matrix m = RotationMatrix(x, y, z);
            this.Multiply(&m);
        }

        internal void Scale(float x, float y, float z)
        {
            Matrix m = ScaleMatrix(x, y, z);
            this.Multiply(&m);
        }
    }
}
