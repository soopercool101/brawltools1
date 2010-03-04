using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Modeling
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FacePoint
    {
        public uint VertexId;
        public Vector3 Normal; //Must be multiplied by vertex matrix
        public fixed uint Color[2];
        public fixed float UVs[16]; //Multiplied with pre-set texture matrix (set by material)
    }
}
