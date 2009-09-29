using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class SingleExtension
    {
        public static unsafe Single Reverse(this Single value)
        {
            *(uint*)(&value) = ((uint*)&value)->Reverse();
            return value;
        }
    }
}
