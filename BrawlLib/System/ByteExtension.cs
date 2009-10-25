using System;

namespace System
{
    public static class ByteExtension
    {
        public static int CompareBits(this Byte b1, byte b2)
        {
            for (int i = 8, b = 0x80; i-- != 0; b >>= 1)
                if ((b1 & b) != (b2 & b))
                    return i;
            return 0;
        }
    }
}
