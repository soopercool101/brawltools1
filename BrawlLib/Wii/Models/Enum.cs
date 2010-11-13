using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Wii.Models
{
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
