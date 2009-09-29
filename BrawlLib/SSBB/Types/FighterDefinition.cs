using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    //File contains anmation data?
    //Filled with float values separated by uint values.
    //End of file contains incrementing uint values, frame counters?

    [StructLayout( LayoutKind.Sequential, Pack=1)]
    unsafe struct FighterDefinitionHeader
    {
        buint _unk; //0x10000 ?
        buint _dataLength; //file size minus header
        buint spacer; //0x0000FFFF
        fixed byte _padding[20];
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct DefHeader1
    {
        buint _dataLength;
        buint _unkLength;
        buint _unkLength2;
        buint _unkLength3; //when added together, these two numbers equal the number of string entries at the end of the file
        buint _unkLength4; //they may denote numbers of data entries
        fixed byte _padding[12];
    }
}
