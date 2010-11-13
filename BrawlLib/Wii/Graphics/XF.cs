using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Graphics
{
    public struct XFCommand
    {
        public bushort _length; //Reads one 32bit value for each (length + 1)
        public bushort _address;
    }

    //TexMtxInfo
    //0000 0000 0000 0001   Unk
    //0000 0000 0000 0010   Projection

    public enum XFMemoryAddr : ushort
    {
        PosMatrices = 0x0000,

        Size = 0x8000,
        Error = 0x1000,
        Diag = 0x1001,
        State0 = 0x1002,
        State1 = 0x1003,
        Clock = 0x1004,
        ClipDisable = 0x1005,
        SetGPMetric = 0x1006,

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

    public enum XFNormalFormat
    {
        Invalid = 0, //Normals are always used!
        XYZ = 1,
        NBT = 2
    }

    //VTXSpecs
    //0000 0000 0000 0011   - Num colors
    //0000 0000 0000 1100   - Normal type (1 = normals, 2 = normals + binormals)
    //0000 0000 1111 0000   - Num textures
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XFVertexSpecs
    {
        internal buint _data;

        public int ColorCount { get { return (int)(_data & 3); } set { _data = _data & 0xFFFFFFFC | ((uint)value & 3); } }
        public int TextureCount { get { return (int)(_data >> 4 & 0xF); } set { _data = _data & 0xFFFFFF0F | (((uint)value & 0xF) << 4); } }
        public XFNormalFormat NormalFormat { get { return (XFNormalFormat)(_data >> 2 & 3); } set { _data = _data & 0xFFFFFFF3 | (((uint)value & 3) << 2); } }

        public XFVertexSpecs(uint raw) { _data = raw; }
        public XFVertexSpecs(int colors, int textures, XFNormalFormat normalFormat)
        { _data = (((uint)textures & 0xF) << 4) | (((uint)normalFormat & 3) << 2) | ((uint)colors & 3); }
    }

    //This is used by polygons to enable element arrays (I believe)
    //There doesn't seem to be a native spec for this
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct XFArrayFlags
    {
        internal buint _data;

        //0000 0000 0000 0000 0000 0001 Pos Matrix
        //0000 0000 0000 0000 0000 0010 TexMtx0
        //0000 0000 0000 0000 0000 0100 TexMtx1
        //0000 0000 0000 0000 0000 1000 TexMtx2
        //0000 0000 0000 0000 0001 0000 TexMtx3
        //0000 0000 0000 0000 0010 0000 TexMtx4
        //0000 0000 0000 0000 0100 0000 TexMtx5
        //0000 0000 0000 0000 1000 0000 TexMtx6
        //0000 0000 0000 0001 0000 0000 TexMtx7
        //0000 0000 0000 0010 0000 0000 Positions
        //0000 0000 0000 0100 0000 0000 Normals
        //0000 0000 0000 1000 0000 0000 Color0
        //0000 0000 0001 0000 0000 0000 Color1
        //0000 0000 0010 0000 0000 0000 Tex0
        //0000 0000 0100 0000 0000 0000 Tex1
        //0000 0000 1000 0000 0000 0000 Tex2
        //0000 0001 0000 0000 0000 0000 Tex3
        //0000 0010 0000 0000 0000 0000 Tex4
        //0000 0100 0000 0000 0000 0000 Tex5
        //0000 1000 0000 0000 0000 0000 Tex6
        //0001 0000 0000 0000 0000 0000 Tex7

        public bool HasPosMatrix { get { return (_data & 1) != 0; } set { _data = _data & 0xFFFFFFFE | (uint)(value ? 1 : 0); } }
        public bool HasPositions { get { return (_data & 0x200) != 0; } set { _data = _data & 0xFFFFFDFF | (uint)(value ? 0x200 : 0); } }
        public bool HasNormals { get { return (_data & 0x400) != 0; } set { _data = _data & 0xFFFFFBFF | (uint)(value ? 0x400 : 0); } }

        public bool GetHasTexMatrix(int index) { return (_data & 2 << index) != 0; }
        public void SetHasTexMatrix(int index, bool exists) { _data = _data & ~((uint)2 << index) | ((uint)(exists ? 2 : 0) << index); }

        public bool GetHasColor(int index) { return (_data & 0x800 << index) != 0; }
        public void SetHasColor(int index, bool exists) { _data = _data & ~((uint)0x800 << index) | ((uint)(exists ? 0x800 : 0) << index); }

        public bool GetHasUVs(int index) { return (_data & 0x2000 << index) != 0; }
        public void SetHasUVs(int index, bool exists) { _data = _data & ~((uint)0x2000 << index) | ((uint)(exists ? 0x2000 : 0) << index); }
    }


    public enum XFDataFormat : byte
    {
        None = 0,
        Direct = 1,
        Index8 = 2,
        Index16 = 3
    }


}
