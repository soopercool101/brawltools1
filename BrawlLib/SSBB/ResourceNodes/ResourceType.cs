using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.SSBB.ResourceNodes
{
    //Lower byte is resource type (used for icon index)
    //Upper byte is entry type/flags
    public enum ResourceType : int
    {
        //Base types
        Unknown = 0x0000,
        Container = 0x0001,

        ARC = 0x0202,
        BRES = 0x0203,
        MSBin = 0x0204,
        EFLS = 0x0215,
        CollisionDef = 0x0216,

        TEX0 = 0x0305,
        PLT0 = 0x0306,
        MDL0 = 0x0307,
        CHR0 = 0x0308,
        CLR0 = 0x0309,
        VIS0 = 0x030A,

        RSAR = 0x000B,
        RSTM = 0x000C,

        RSARFile = 0x0B0D,
        RSARGroup = 0x0B0E,
        RSARType = 0x0B0F,
        RSARBank = 0x0B10,

        RWSD = 0x0011,
        //RWSDDataEntry = 0x0800,
        //RWSDWaveEntry= 0x0800,

        RBNK = 0x0012,
        RSEQ = 0x0013,

        //Generic types
        ARCEntry = 0x0200,

        BRESEntry = 0x0300,
        BRESGroup = 0x0301,

        MDL0Group = 0x0701,
        MDL0Bone = 0x0714,

        CHR0Entry = 0x0800,
        CLR0Entry = 0x0900,

        RSARFolder = 0x0B01,

        RWSDGroup = 0x1101,
    }
}
