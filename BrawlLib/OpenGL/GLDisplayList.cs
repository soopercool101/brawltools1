using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.OpenGL
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public class GLDisplayList
    {
        public uint _id;

        public GLDisplayList(uint id) { _id = id; }
    }
}
