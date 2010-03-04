using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public struct Face
    {
        public int I1, I2, I3;
        Vector3 Normal;
    }
}
