using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.OpenGL
{
    public enum GLAccumOp : uint
    {
        Accum = 0x0100,
        Load = 0x0101,
        Return = 0x0102,
        Mult = 0x0103,
        Add = 0x0104
    }

    public enum GLAlphaFunc : uint
    {
        Never = 0x0200,
        Less = 0x0201,
        Equal = 0x0202,
        LEqual = 0x0203,
        Greater = 0x0204,
        NotEqual = 0x0205,
        GEqual = 0x0206,
        Always = 0x0207
    }

    public enum GLBeginMode : uint
    {
        Points = 0x0000,
        Lines = 0x0001,
        LineLoop = 0x0002,
        LineStrip = 0x0003,
        Triangles = 0x0004,
        TriangleStrip = 0x0005,
        TriangleFan = 0x0006,
        Quads = 0x0007,
        QuadStrip = 0x0008,
        Polygon = 0x0009
    }

    public enum GLClearMask : uint
    {
        DepthBuffer = 0x00000100,
        StencilBuffer = 0x00000400,
        ColorBuffer = 0x00004000
    }

    public enum GLMatrixMode : uint
    {
        ModelView = 0x1700,
        Projection = 0x1701,
        Texture = 0x1702
    }
}
