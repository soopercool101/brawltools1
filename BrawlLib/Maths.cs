using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib
{
    public static unsafe class Maths
    {
        private static double _double2fixmagic = 68719476736.0f * 1.5f;
        public static unsafe Int32 ToInt32(this Single value)
        {
            double v = value + _double2fixmagic;
            return *((int*)&v) >> 16;
        }

        public static void FFloor3(float* v)
        {
            double d;
            int* p = (int*)&d;
            int i = 3;
            while (i-- > 0)
            {
                d = v[i] + _double2fixmagic;
                v[i] = *p >> 16;
            }
        }
        public static void FMult3(float* l, float* r)
        {
            *l++ *= *r++;
            *l++ *= *r++;
            *l++ *= *r++;
            //for (int i = 3; i-- > 0; )
            //    l[i] *= r[i];
            //return l;
        }
        public static void FMult3(float* l, float r)
        {
            *l++ *= r;
            *l++ *= r;
            *l++ *= r;
            //for (int i = 3; i-- > 0; )
            //    l[i] *= r;
            //return l;
        }
        public static void FAdd3(float* l, float* r)
        {
            *l++ += *r++;
            *l++ += *r++;
            *l++ += *r++;
            //for (int i = 3; i-- > 0; )
            //    l[i] += r[i];
            //return l;
        }
        public static void FAdd3(float* l, float r)
        {
            *l++ += r;
            *l++ += r;
            *l++ += r;
            //for (int i = 3; i-- > 0; )
            //    l[i] += r;
            //return l;
        }
        public static void FSub3(float* l, float* r)
        {
            *l++ -= *r++;
            *l++ -= *r++;
            *l++ -= *r++;
            //for (int i = 3; i-- > 0; )
            //    l[i] -= r[i];
            //return l;
        }
    }
}
