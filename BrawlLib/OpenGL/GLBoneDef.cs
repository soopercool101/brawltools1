using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Models;

namespace BrawlLib.OpenGL
{
    public unsafe class GLBoneDef : GLPrimitive
    {
        public int _index;
        public int _id;

        public GLBoneDef(PrimitiveHeader* header)
        {
            _index = header->Entries;
            _id = *(bushort*)header->Data;
        }
    }
}
