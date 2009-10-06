using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Wii.Models
{
    public enum WiiPrimitiveType : byte
    {
        BoneDef1 = 0x20,
        BoneDef2 = 0x28,
        BoneDef3 = 0x30,
        Quads= 0x80,
        Triangles = 0x90,
        TriangleStrip = 0x98,
        TriangleFan = 0xA0,
        Lines = 0xA8,
        LineStrip = 0xB0,
        Points = 0xB8
    }

    public enum WiiVertexElements : byte
    {
        XY = 0,
        XYZ = 1
    }

    public enum WiiVertexComponentType : byte
    {
        UInt8 = 0,
        Int8 = 1,
        UInt16 = 2,
        Int16 = 3,
        Float = 4
    }

    public enum WiiColorComponentType : byte
    {
        RGB565 = 0,
        RGB8 = 1,
        RGBX8 = 2,
        RGBA4 = 3,
        RGBA6 = 4,
        RGBA8 = 5
    }
}
