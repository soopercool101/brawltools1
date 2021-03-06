﻿using System;

namespace BrawlLib.Wii.Graphics
{
    public enum GXListCommand : byte
    {
        NOP = 0,
        LoadBPReg = 0x61,
        LoadCPReg = 0x08,
        LoadXFReg = 0x10,

        //LoadIndex - 32bit data
        //0000 0000 0000 0000 0000 1111 1111 1111   - XF Memory address
        //0000 0000 0000 0000 1111 0000 0000 0000   - Length (reads length + 1 uint values into XF memory)
        //1111 1111 1111 1111 0000 0000 0000 0000   - Index (for matrices, this is node index)
        LoadIndexA = 0x20, //Position matrices (4 x 3)
        LoadIndexB = 0x28, //Normal matrices (3 x 3)
        LoadIndexC = 0x30, //Post matrices (4 x 4)
        LoadIndexD = 0x38, //Lights
        CmdCallDL = 0x40,
        CmdUnknownMetrics = 0x44,
        CmdInvlVC = 0x48,
        DrawQuads = 0x80,
        DrawTriangles = 0x90,
        DrawTriangleStrip = 0x98,
        DrawTriangleFan = 0xA0,
        DrawLines = 0xA8,
        DrawLineStrip = 0xB0,
        DrawPoints = 0xB8
    }

    public enum GXCompare
    {
        GX_NEVER,
        GX_LESS,
        GX_EQUAL,
        GX_LEQUAL,
        GX_GREATER,
        GX_NEQUAL,
        GX_GEQUAL,
        GX_ALWAYS
    }

    public enum GXLogicOp
    {
        GX_LO_CLEAR,
        GX_LO_AND,
        GX_LO_REVAND,
        GX_LO_COPY,
        GX_LO_INVAND,
        GX_LO_NOOP,
        GX_LO_XOR,
        GX_LO_OR,
        GX_LO_NOR,
        GX_LO_EQUIV,
        GX_LO_INV,
        GX_LO_REVOR,
        GX_LO_INVCOPY,
        GX_LO_INVOR,
        GX_LO_NAND,
        GX_LO_SET
    }
}
