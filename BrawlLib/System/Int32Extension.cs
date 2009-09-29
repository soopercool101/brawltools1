using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class Int32Extension
    {
        public static Int32 Reverse(this Int32 value)
        {
            return ((value >> 24) & 0xFF) | (value << 24) | ((value >> 8) & 0xFF00) | ((value & 0xFF00) << 8);
        }
        public static Int32 Align(this Int32 value, int align)
        {
            if (value < 0) return 0;
            if (align <= 1) return value;
            int temp = value % align;
            if (temp != 0) value += align - temp;
            return value;
        }
        public static Int32 Clamp(this Int32 value, int min, int max)
        {
            if (value <= min) return min;
            if (value >= max) return max;
            return value;
        }
    }
}
