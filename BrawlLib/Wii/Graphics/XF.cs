using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Wii.Graphics
{
    public struct XFCommand
    {
        public bushort _length; //Reads one 32bit value for each (length + 1)
        public bushort _address;
    }

    public enum XFMemoryAddr : ushort
    {
        Size = 0x8000,
        Error = 0x1000,
        Diag = 0x1001,
        State0 = 0x1002,
        State1 = 0x1003,
        Clock = 0x1004,
        ClipDisable = 0x1005,
        SetGPMetric = 0x1006,

        //VTXSpecs
        //0000 0000 0000 0011   - Num colors
        //0000 0000 0000 1100   - Normal type (1 = normals, 2 = normals + binormals)
        //0000 0000 1111 0000   - Num textures
        VTXSpecs = 0x1008,
        SetNumChan = 0x1009,
        SetChan0AmbColor = 0x100A,
        SetChan1AmbColor = 0x100B,
        SetChan0MatColor = 0x100C,
        SetChan1MatColor = 0x100D,
        SetChan0Color = 0x100E,
        SetChan1Color = 0x100F,
        SetChan0Alpha = 0x1010,
        SetChan1Alpha = 0x1011,
        DualTex = 0x1012,
        SetMatrixIndA = 0x1018,
        SetMatrixIndB = 0x1019,
        SetViewport = 0x101A,
        SetZScale = 0x101C,
        SetZOffset = 0x101F,
        SetProjection = 0x1020,
        SetNumTexGens = 0x103F,
        SetTexMtxInfo = 0x1040,
        SetPosMtxInfo = 0x1050

    }

    public enum XFDataFormat : byte
    {
        None = 0,
        Direct = 1,
        Index8 = 2,
        Index16 = 3
    }

    //Vertex Format Lo (lower 17 bits of vtx format register)
    //0000 0000 0000 0000 0000 0000 0000 0001     - Vertex/Normal matrix index
    //0000 0000 0000 0000 0000 0000 0000 0010     - Texture Matrix 0
    //0000 0000 0000 0000 0000 0000 0000 0100     - Texture Matrix 1
    //0000 0000 0000 0000 0000 0000 0000 1000     - Texture Matrix 2
    //0000 0000 0000 0000 0000 0000 0001 0000     - Texture Matrix 3
    //0000 0000 0000 0000 0000 0000 0010 0000     - Texture Matrix 4
    //0000 0000 0000 0000 0000 0000 0100 0000     - Texture Matrix 5
    //0000 0000 0000 0000 0000 0000 1000 0000     - Texture Matrix 6
    //0000 0000 0000 0000 0000 0001 0000 0000     - Texture Matrix 7
    //0000 0000 0000 0000 0000 0110 0000 0000     - Vertex format
    //0000 0000 0000 0000 0001 1000 0000 0000     - Normal format
    //0000 0000 0000 0000 0110 0000 0000 0000     - Color0 format
    //0000 0000 0000 0001 1000 0000 0000 0000     - Color1 format

    //Vertex Format Hi (shifted left 17 bits before storing)
    //0000 0000 0000 0000 0110 0000 0000 0000 0000     - Tex0 format
    //0000 0000 0000 0001 1000 0000 0000 0000 0000     - Tex1 format
    //0000 0000 0000 0110 0000 0000 0000 0000 0000     - Tex2 format
    //0000 0000 0001 1000 0000 0000 0000 0000 0000     - Tex3 format
    //0000 0000 0110 0000 0000 0000 0000 0000 0000     - Tex4 format
    //0000 0001 1000 0000 0000 0000 0000 0000 0000     - Tex5 format
    //0000 0110 0000 0000 0000 0000 0000 0000 0000     - Tex6 format
    //0001 1000 0000 0000 0000 0000 0000 0000 0000     - Tex7 format

    class XF
    {
    }
}
