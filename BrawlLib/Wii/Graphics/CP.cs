using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Wii.Graphics
{
    public enum CPCommand : byte
    {
        SetMatricesA = 3,
        SetMatricesB = 4,
        SetVertexDescLo = 5,
        SetVertexDescHi = 6,
        SetUVATGroup0 = 7,
        SetUVATGroup1 = 8,
        SetUVATGroup2 = 9
    }

    //UVAT Group 0
    //0000 0000 0000 0000 0000 0000 0000 0001   - Pos elements
    //0000 0000 0000 0000 0000 0000 0000 1110   - Pos format
    //0000 0000 0000 0000 0000 0001 1111 0000   - Pos frac
    //0000 0000 0000 0000 0000 0010 0000 0000   - Normal elements
    //0000 0000 0000 0000 0001 1100 0000 0000   - Normal format
    //0000 0000 0000 0000 0010 0000 0000 0000   - Color0 elements
    //0000 0000 0000 0001 1100 0000 0000 0000   - Color0 Comp
    //0000 0000 0000 0010 0000 0000 0000 0000   - Color1 elements
    //0000 0000 0001 1100 0000 0000 0000 0000   - Color1 Comp
    //0000 0000 0010 0000 0000 0000 0000 0000   - Tex0 coord elements
    //0000 0001 1100 0000 0000 0000 0000 0000   - Tex0 coord format
    //0011 1110 0000 0000 0000 0000 0000 0000   - Tex0 coord frac
    //0100 0000 0000 0000 0000 0000 0000 0000   - Byte dequant
    //1000 0000 0000 0000 0000 0000 0000 0000   - Normal index 3


    //UVAT Group1
    //0000 0000 0000 0000 0000 0000 0000 0001   - Tex1 coord elements
    //0000 0000 0000 0000 0000 0000 0000 1110   - Tex1 coord format
    //0000 0000 0000 0000 0000 0001 1111 0000   - Tex1 coord frac
    //0000 0000 0000 0000 0000 0010 0000 0000   - Tex2 coord elements
    //0000 0000 0000 0000 0001 1100 0000 0000   - Tex2 coord format
    //0000 0000 0000 0011 1110 0000 0000 0000   - Tex2 coord frac
    //0000 0000 0000 0100 0000 0000 0000 0000   - Tex3 coord elements
    //0000 0000 0011 1000 0000 0000 0000 0000   - Tex3 coord format
    //0000 0111 1100 0000 0000 0000 0000 0000   - Tex3 coord frac
    //0000 1000 0000 0000 0000 0000 0000 0000   - Tex4 coord elements
    //0111 0000 0000 0000 0000 0000 0000 0000   - Tex4 coord format
    
    //UVAT Group1
    //0000 0000 0000 0000 0000 0000 0001 1111   - Tex4 coord frac
    //0000 0000 0000 0000 0000 0000 0010 0000   - Tex5 coord elements
    //0000 0000 0000 0000 0000 0001 1100 0000   - Tex5 coord format
    //0000 0000 0000 0000 0011 1110 0000 0000   - Tex5 coord frac
    //0000 0000 0000 0000 0100 0000 0000 0000   - Tex6 coord elements
    //0000 0000 0000 0011 1000 0000 0000 0000   - Tex6 coord format
    //0000 0000 0111 1100 0000 0000 0000 0000   - Tex6 coord frac
    //0000 0000 1000 0000 0000 0000 0000 0000   - Tex7 coord elements
    //0000 0111 0000 0000 0000 0000 0000 0000   - Tex7 coord format
    //1111 1000 0000 0000 0000 0000 0000 0000   - Tex7 coord frac

    class CP
    {
    }
}
