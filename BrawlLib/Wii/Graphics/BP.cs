using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Graphics
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct BPCommand
    {
        public BPMemory Mem; 
        public fixed byte Data[3];
    }
    
    //Not reversed, can be used directly
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public struct AlphaFunction
    {
        public static readonly AlphaFunction Default = new AlphaFunction() { dat = 0x3F };
        //0000 0000 0000 0000 1111 1111   ref0
        //0000 0000 1111 1111 0000 0000   ref1
        //0000 0111 0000 0000 0000 0000   comp0
        //0011 1000 0000 0000 0000 0000   comp1
        //1100 0000 0000 0000 0000 0000   logic

        public byte dat;
        public byte ref1;
        public byte ref0;

        public AlphaCompare Comp0 { get { return (AlphaCompare)(dat & 7); } set { dat = (byte)((dat & 0xF8) | ((int)value & 7)); } }
        public AlphaCompare Comp1 { get { return (AlphaCompare)((dat >> 3) & 7); } set { dat = (byte)((dat & 0xC7) | (((int)value & 7) << 3)); } }
        public AlphaOp Logic { get { return (AlphaOp)((dat >> 6) & 3); } set { dat = (byte)((dat & 0x3F) | (((int)value & 3) << 6)); } }
    }

    public enum AlphaCompare
    {
        COMPARE_NEVER = 0,
        COMPARE_LESS,
        COMPARE_EQUAL,
        COMPARE_LEQUAL,
        COMPARE_GREATER,
        COMPARE_NEQUAL,
        COMPARE_GEQUAL,
        COMPARE_ALWAYS
    }

    public enum AlphaOp
    {
        ALPHAOP_AND = 0,
        ALPHAOP_OR,
        ALPHAOP_XOR,
        ALPHAOP_XNOR,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ZMode
    {
        public static readonly ZMode Default = new ZMode() { dat = 0x17 };

        public byte pad0, pad1, dat;

        public bool EnableDepthTest { get { return (dat & 1) != 0; } set { dat = (byte)((dat & 0xFE) | (value ? 1 : 0)); } }
        public bool EnableDepthUpdate { get { return (dat & 0x10) != 0; } set { dat = (byte)((dat & 0xEF) | (value ? 0x10 : 0));} }
        public GXCompare DepthFunction { get { return (GXCompare)((dat >> 1) & 7); } set { dat = (byte)((dat & 0xF1) | ((int)value << 1)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BlendMode
    {
        public static readonly BlendMode Default = new BlendMode() { dat1 = 0xA0, dat2 = 0x34 };

        //0000 0000 0000 0001    EnableBlend
        //0000 0000 0000 0010    EnableLogic
        //0000 0000 0000 0100    EnableDither
        //0000 0000 0000 1000    UpdateColor
        //0000 0000 0001 0000    UpdateAlpha
        //0000 0000 1110 0000    DstFactor
        //0000 0111 0000 0000    SrcFactor
        //0000 1000 0000 0000    Subtract
        //1111 0000 0000 0000    LogicOp

        public byte pad, dat2, dat1;

        public bool EnableBlend { get { return (dat1 & 1) != 0; } set { dat1 = (byte)((dat1 & 0xFE) | (value ? 1 : 0)); } }
        public bool EnableLogicOp { get { return (dat1 & 2) != 0; } set { dat1 = (byte)((dat1 & 0xFD) | (value ? 2 : 0)); } }
        public bool EnableDither { get { return (dat1 & 4) != 0; } set { dat1 = (byte)((dat1 & 0xFB) | (value ? 4 : 0)); } }
        public bool EnableColorUpdate { get { return (dat1 & 8) != 0; } set { dat1 = (byte)((dat1 & 0xF7) | (value ? 8 : 0)); } }
        public bool EnableAlphaUpdate { get { return (dat1 & 0x10) != 0; } set { dat1 = (byte)((dat1 & 0xEF) | (value ? 0x10 : 0)); } }
        public BlendFactor DstFactor { get { return (BlendFactor)(dat1 >> 5); } set { dat1 = (byte)((dat1 & 0x1F) | ((int)value << 5)); } }
        public BlendFactor SrcFactor { get { return (BlendFactor)(dat2 & 7); } set { dat1 = (byte)((dat2 & 0xF8) | (int)value); } }
        public bool Subtract { get { return (dat2 & 8) != 0; } set { dat2 = (byte)((dat2 & 0xF7) | (value ? 8 : 0)); } }
        public GXLogicOp LogicOp { get { return (GXLogicOp)(dat2 >> 4); } set { dat2 = (byte)((dat2 & 0xF) | ((int)value << 4)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorReg
    {
        public static readonly ColorReg Default = new ColorReg() { _dat0 = 0x80 };

        private byte _dat0, _dat1, _dat2;

        public short A { get { return (short)(((short)(_dat1 << 13) >> 5) | _dat2); } set { _dat1 = (byte)((_dat1 & 0xF8) | ((value >> 8) & 0x7)); _dat2 = (byte)(value & 0xFF); } }
        public short B { get { return (short)(((short)(_dat0 << 9) >> 5) | (_dat1 >> 4)); } set { _dat0 = (byte)((_dat0 & 0x80) | ((value >> 4) & 0x7F)); _dat1 = (byte)((_dat1 & 0xF) | (value << 4)); } }
        public bool Type { get { return (_dat0 & 0x80) != 0; } set { _dat0 = (byte)((_dat0 & 0x7F) | (value ? 0x80 : 0)); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ConstantAlpha
    {
        public static readonly ConstantAlpha Default = new ConstantAlpha();

        public byte Pad, Enable, Value;
    }

    public enum BlendFactor
    {
        GX_BL_ZERO,
        GX_BL_ONE,
        GX_BL_SRCCLR,
        GX_BL_INVSRCCLR,
        GX_BL_SRCALPHA,
        GX_BL_INVSRCALPHA,
        GX_BL_DSTALPHA,
        GX_BL_INVDSTALPHA,

        GX_BL_DSTCLR = GX_BL_SRCCLR,
        GX_BL_INVDSTCLR = GX_BL_INVSRCCLR

    }

    public enum BPMemory : byte
    {
        BPMEM_GENMODE = 0x00,
        BPMEM_DISPLAYCOPYFILER = 0x01, // =0x01 + 4
        BPMEM_IND_MTXA = 0x06, // =0x06 + (3 * 3)
        BPMEM_IND_MTXB = 0x07,// =0x07 + (3 * 3)
        BPMEM_IND_MTXC = 0x08,// =0x08 + (3 * 3)
        BPMEM_IND_IMASK = 0x0F,
        BPMEM_IND_CMD = 0x10, // =0x10 + 16
        BPMEM_SCISSORTL = 0x20,
        BPMEM_SCISSORBR = 0x21,
        BPMEM_LINEPTWIDTH = 0x22,
        BPMEM_PERF0_TRI = 0x23,
        BPMEM_PERF0_QUAD = 0x24,
        BPMEM_RAS1_SS0 = 0x25,
        BPMEM_RAS1_SS1 = 0x26,
        BPMEM_IREF = 0x27,
        BPMEM_TREF = 0x28,// =0x28 + 8
        BPMEM_SU_SSIZE = 0x30,// =0x30 + (2 * 8)
        BPMEM_SU_TSIZE = 0x31,// =0x31 + (2 * 8)
        BPMEM_ZMODE = 0x40,
        BPMEM_BLENDMODE = 0x41,
        BPMEM_CONSTANTALPHA = 0x42,
        BPMEM_ZCOMPARE = 0x43,
        BPMEM_FIELDMASK = 0x44,
        BPMEM_SETDRAWDONE = 0x45,
        BPMEM_BUSCLOCK0 = 0x46,
        BPMEM_PE_TOKEN_ID = 0x47,
        BPMEM_PE_TOKEN_INT_ID = 0x48,
        BPMEM_EFB_TL = 0x49,
        BPMEM_EFB_BR = 0x4A,
        BPMEM_EFB_ADDR = 0x4B,
        BPMEM_MIPMAP_STRIDE = 0x4D,
        BPMEM_COPYYSCALE = 0x4E,
        BPMEM_CLEAR_AR = 0x4F,
        BPMEM_CLEAR_GB = 0x50,
        BPMEM_CLEAR_Z = 0x51,
        BPMEM_TRIGGER_EFB_COPY = 0x52,
        BPMEM_COPYFILTER0 = 0x53,
        BPMEM_COPYFILTER1 = 0x54,
        BPMEM_CLEARBBOX1 = 0x55,
        BPMEM_CLEARBBOX2 = 0x56,
        BPMEM_UNKOWN_57 = 0x57,
        BPMEM_REVBITS = 0x58,
        BPMEM_SCISSOROFFSET = 0x59,
        BPMEM_UNKNOWN_60 = 0x60,
        BPMEM_UNKNOWN_61 = 0x61,
        BPMEM_UNKNOWN_62 = 0x62,
        BPMEM_TEXMODESYNC = 0x63,
        BPMEM_LOADTLUT0 = 0x64,
        BPMEM_LOADTLUT1 = 0x65,
        BPMEM_TEXINVALIDATE = 0x66,
        BPMEM_PERF1 = 0x67,
        BPMEM_FIELDMODE = 0x68,
        BPMEM_BUSCLOCK1 = 0x69,
        BPMEM_TX_SETMODE0 = 0x80,// =0x80 + 4
        BPMEM_TX_SETMODE1 = 0x84,// =0x84 + 4
        BPMEM_TX_SETIMAGE0 = 0x88,// =0x88 + 4
        BPMEM_TX_SETIMAGE1 = 0x8C,// =0x8C + 4
        BPMEM_TX_SETIMAGE2 = 0x90,// =0x90 + 4
        BPMEM_TX_SETIMAGE3 = 0x94,// =0x94 + 4
        BPMEM_TX_SETTLUT = 0x98,// =0x98 + 4
        BPMEM_TX_SETMODE0_4 = 0xA0,// =0xA0 + 4
        BPMEM_TX_SETMODE1_4 = 0xA4,// =0xA4 + 4
        BPMEM_TX_SETIMAGE0_4 = 0xA8,// =0xA8 + 4
        BPMEM_TX_SETIMAGE1_4 = 0xAC,// =0xA4 + 4
        BPMEM_TX_SETIMAGE2_4 = 0xB0,// =0xB0 + 4
        BPMEM_TX_SETIMAGE3_4 = 0xB4,// =0xB4 + 4
        BPMEM_TX_SETLUT_4 = 0xB8,// =0xB8 + 4
        BPMEM_TEV_COLOR_ENV = 0xC0,// =0xC0 + (2 * 16)
        BPMEM_TEV_ALPHA_ENV = 0xC1,// =0xC1 + (2 * 16)
        BPMEM_TEV_REGISTER_L = 0xE0,// =0xE0 + (2 * 4)
        BPMEM_TEV_REGISTER_H = 0xE1,// =0xE1 + (2 * 4)
        BPMEM_FOGRANGE = 0xE8,
        BPMEM_FOGPARAM0 = 0xEE,
        BPMEM_FOGBMAGNITUDE = 0xEF,
        BPMEM_FOGBEXPONENT = 0xF0,
        BPMEM_FOGPARAM3 = 0xF1,
        BPMEM_FOGCOLOR = 0xF2,
        BPMEM_ALPHACOMPARE = 0xF3,
        BPMEM_BIAS = 0xF4,
        BPMEM_ZTEX2 = 0xF5,
        BPMEM_TEV_KSEL = 0xF6, // =0xF6 + 8
        BPMEM_BP_MASK = 0xFE
    }
}
