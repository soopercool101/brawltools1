using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Header
    {
        public const uint Size = 16;
        public const uint Tag = 0x304C444D;

        public BRESCommonHeader _header;

        //public buint _infoOffset;
        //public buint _boneOffset;
        //public buint _vertexDataOffset;
        //public buint _normalDataOffset;

        //public buint _colorDataOffset;
        //public buint _uvDataOffset;
        //public buint _materialOffset;
        //public buint _shaderOffset;
        //public buint _polygonOffset;
        //public buint _textureOffset;
        //public buint _decalOffset;

        //public bint _stringOffset;

        //public MDL0Props _modelDef;

        public MDL0Header(int length, int version)
        {
            _header._tag = Tag;
            _header._size = length;
            _header._version = version;
            _header._bresOffset = 0;
        }

        internal byte* Address { get { fixed (void* ptr = &this)return (byte*)ptr; } }

        public bint* Offsets { get { return (bint*)(Address + 0x10); } }

        public int StringOffset
        {
            get
            {
                switch (_header._version)
                {
                    case 0x09:
                        return *(bint*)(Address + 0x3C);
                    default:
                        return 0;
                }
            }
            set
            {
                switch (_header._version)
                {
                    case 0x09:
                        *(bint*)(Address + 0x3C) = value;
                        break;
                }
            }
        }

        public MDL0Props* Properties
        {
            get
            {
                switch (_header._version)
                {
                    case 0x09:
                        return (MDL0Props*)(Address + 0x40);
                    default:
                        return null;
                }
            }
        }

        //public string Name { get { return new String((sbyte*)Address + _stringOffset); } }
        //public ResourceGroup* GetEntry(int index) 
        //{
        //    uint offset = Offsets[index];
        //    if (offset == 0)
        //        return null;
        //    return (ResourceGroup*)(Address + offset);
        //}

        //public ResourceGroup* InfoGroup { get { return _infoOffset == 0 ? null : (ResourceGroup*)(Address + _infoOffset); } }
        //public ResourceGroup* BoneGroup { get { return _boneOffset == 0 ? null : (ResourceGroup*)(Address + _boneOffset); } }
        //public ResourceGroup* VertexGroup { get { return _vertexDataOffset == 0 ? null : (ResourceGroup*)(Address + _vertexDataOffset); } }
        //public ResourceGroup* NormalGroup { get { return _normalDataOffset == 0 ? null : (ResourceGroup*)(Address + _normalDataOffset); } }
        //public ResourceGroup* ColorGroup { get { return _colorDataOffset == 0 ? null : (ResourceGroup*)(Address + _colorDataOffset); } }
        //public ResourceGroup* UVGroup { get { return _uvDataOffset == 0 ? null :  (ResourceGroup*)(Address + _uvDataOffset); } }
        //public ResourceGroup* Material1Group { get { return _materialOffset == 0 ? null : (ResourceGroup*)(Address + _materialOffset); } }
        //public ResourceGroup* Material2Group { get { return _shaderOffset == 0 ? null : (ResourceGroup*)(Address + _shaderOffset); } }
        //public ResourceGroup* PolygonGroup { get { return _polygonOffset == 0 ? null : (ResourceGroup*)(Address + _polygonOffset); } }
        //public ResourceGroup* Data10Group { get { return _textureOffset == 0 ? null : (ResourceGroup*)(Address + _textureOffset); } }
        //public ResourceGroup* Data11Group { get { return _decalOffset == 0 ? null : (ResourceGroup*)(Address + _decalOffset); } }

        //public MDL0Part2* Part2 { get { return (MDL0Part2*)Address + 0x40; } }
    }

    //Immediately after header, separate entity
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Props
    {
        public const uint Size = 0x40;

        public buint _headerLen; //0x40
        public bint _mdl0Offset;
        public bint _unk1; //0x00 or 0x02
        public bint _unk2; //0x00
        public bint _numVertices; //Length/offset?
        public bint _numFaces; //Length/offset?
        public bint _unk3; //0x00
        public bint _numNodes;
        public bshort _unk4; //0x0101
        public bshort _unk5; //0x00
        public buint _dataOffset; //0x40
        public BVec3 _minExtents;
        public BVec3 _maxExtents;

        public MDL0Props(int vertices, int faces, int nodes, int unk1, int unk2, int unk3, int unk4, int unk5, Vector3 min, Vector3 max)
        {
            _headerLen = 0x40;
            _mdl0Offset = 0;
            _unk1 = unk1;
            _unk2 = unk2;
            _numVertices = vertices;
            _numFaces = faces;
            _unk3 = unk3;
            _numNodes = nodes;
            _unk4 = (short)unk4;
            _unk5 = (short)unk5;
            _dataOffset = 0x40;
            _minExtents = min;
            _maxExtents = max;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0Header* MDL0 { get { return (MDL0Header*)(Address + _mdl0Offset); } }

        public MDL0NodeTable* IndexTable { get { return (MDL0NodeTable*)((VoidPtr)Address + _dataOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0CommonHeader
    {
        public bint _size;
        public bint _mdlOffset;

        internal void* Address { get { fixed (void* p = &this)return p; } }
        public MDL0Header* MDL0Header { get { return (MDL0Header*)((byte*)Address + _mdlOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeTable
    {
        public bint _numEntries;

        private void* Address { get { fixed (void* ptr = &this)return ptr; } }

        public bint* First { get { return (bint*)Address + 1; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0DefEntry
    {
        public byte _type;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0NodeType2* Type2Data { get { return (MDL0NodeType2*)(Address + 1); } }
        public MDL0NodeType3* Type3Data { get { return (MDL0NodeType3*)(Address + 1); } }
        public MDL0NodeType4* Type4Data { get { return (MDL0NodeType4*)(Address + 1); } }
        public MDL0NodeType5* Type5Data { get { return (MDL0NodeType5*)(Address + 1); } }

        public MDL0DefEntry* Next
        {
            get
            {
                switch (_type)
                {
                    case 2: return (MDL0DefEntry*)(Type2Data + 1);
                    case 3: return (MDL0DefEntry*)(Type3Data + 1);
                    case 4: return (MDL0DefEntry*)(Type4Data + 1);
                    case 5: return (MDL0DefEntry*)(Type5Data + 1);
                }
                return null;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe abstract class MDL0NodeClass
    {
        public static object Create(ref VoidPtr addr)
        {
            object n = null;
            switch (*(byte*)addr++)
            {
                case 2: { n = Marshal.PtrToStructure(addr, typeof(MDL0Node2Class)); addr += MDL0Node2Class.Size; break; }
                case 3: { n = new MDL0Node3Class((MDL0NodeType3*)addr); addr += ((MDL0Node3Class)n).GetSize(); break; }
                case 4: { n = Marshal.PtrToStructure(addr, typeof(MDL0NodeType4)); addr += MDL0NodeType4.Size; break; }
                case 5: { n = Marshal.PtrToStructure(addr, typeof(MDL0NodeType5)); addr += MDL0NodeType5.Size; break; }
            }
            return n;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class MDL0Node2Class : MDL0NodeClass
    {
        public const uint Size = 0x04;

        public bushort _boneIndex;
        public bushort _parentNodeIndex;

        public ushort Id { get { return _boneIndex; } set { _boneIndex = value; } }
        public ushort Unknown { get { return _parentNodeIndex; } set { _parentNodeIndex = value; } }

        public override string ToString()
        {
            return string.Format("Type2 ({0},{1})", Id, Unknown);
        }
    }

    public unsafe class MDL0Node3Class
    {
        public bushort _id;
        public List<MDL0NodeType3Entry> _entries = new List<MDL0NodeType3Entry>();

        public unsafe MDL0Node3Class(MDL0NodeType3* ptr)
        {
            _id = ptr->_id;
            for (int i = 0; i < ptr->_numEntries; i++)
                _entries.Add(ptr->Entries[i]);
        }

        public ushort Id { get { return _id; } set { _id = value; } }
        public MDL0NodeType3Entry[] Entries { get { return _entries.ToArray(); } }

        public int GetSize() { return 3 + (_entries.Count * MDL0NodeType3Entry.Size); }

        public override string ToString()
        {
            return string.Format("Type3 ({0})", Id);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType2
    {
        public const int Size = 0x04;

        public bushort _index;
        public bushort _parentId;

        public ushort Index { get { return _index; } set { _index = value; } }
        public ushort ParentId { get { return _parentId; } set { _parentId = value; } }

        public override string ToString()
        {
            return string.Format("Type2 ({0},{1})", Index, ParentId);
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType3
    {
        public const int Size = 0x03;

        public bushort _id;
        public byte _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0NodeType3Entry* Entries { get { return (MDL0NodeType3Entry*)(Address + 3); } }

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType3Entry
    {
        public const int Size = 0x06;

        public bushort _id;
        public bfloat _value;

        public ushort Id { get { return _id; } set { _id = value; } }
        public float Value { get { return _value; } set { _value = value; } }

        public override string ToString()
        {
            return String.Format("Type3Entry ({0},{1})", Id, Value);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType4
    {
        public const uint Size = 0x07;

        public bushort _materialIndex;
        public bushort _polygonIndex;
        public bushort _boneIndex;
        public byte _val4;

        public ushort MaterialId { get { return _materialIndex; } set { _materialIndex = value; } }
        public ushort PolygonId { get { return _polygonIndex; } set { _polygonIndex = value; } }
        public ushort BoneIndex { get { return _boneIndex; } set { _boneIndex = value; } }
        public byte Val4 { get { return _val4; } set { _val4 = value; } }

        public override string ToString()
        {
            return string.Format("Type4 ({0},{1},{2},{3})", MaterialId, PolygonId, BoneIndex, Val4);
        }
    }

    //Links node IDs with indexes
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NodeType5
    {
        public const uint Size = 0x04;

        public bushort _id; //Node Id?
        public bushort _index; //Node Index?

        public int Id { get { return _id; } set { _id = (ushort)value; } }
        public int Index { get { return _index; } set { _index = (ushort)value; } }

        public override string ToString()
        {
            return string.Format("Type5 ({0},{1})", Id, Index);
        }
    }

    [Flags]
    public enum BoneFlags : uint
    {
        NoTransform = 0x0001,
        FixedTranslation = 0x0002,
        FixedRotation = 0x0004,
        FixedScale = 0x0008,
        Unk1 = 0x0010,
        Unk2 = 0x0100,
        HasGeometry = 0x0200
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Bone
    {
        public bint _headerLen;
        public bint _mdl0Offset;
        public bint _stringOffset;
        public bint _index;

        public bint _nodeId;
        public buint _flags;
        public buint _pad1;
        public buint _pad2;

        public BVec3 _scale;
        public BVec3 _rotation;
        public BVec3 _translation;
        public BVec3 _boxMin;
        public BVec3 _boxMax;

        public bint _parentOffset;
        public bint _firstChildOffset;
        public bint _nextOffset;
        public bint _prevOffset;
        public bint _part2Offset;

        public bMatrix43 _transform;
        public bMatrix43 _transformInv;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0Data7Part4* Part2 { get { return (MDL0Data7Part4*)(Address + _part2Offset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0VertexData
    {
        public bint _dataLen; //including header
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;
        public bint _index;
        public bint _isXYZ;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bshort _numVertices;
        public BVec3 _eMin;
        public BVec3 _eMax;
        public bint _pad1;
        public bint _pad2;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + _dataOffset; } }

        public WiiVertexComponentType Type { get { return (WiiVertexComponentType)(int)_type; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0NormalData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;
        public bint _index;
        public bint _hasbox;
        public bint _type;
        public byte _divisor;
        public byte _entryStride;
        public bshort _numVertices;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + _dataOffset; } }

        public WiiVertexComponentType Type { get { return (WiiVertexComponentType)(int)_type; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0ColorData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x20
        public bint _stringOffset;
        public bint _index;
        public bint _isRGBA;
        public bint _format;
        public byte _entryStride;
        public byte _unk3;
        public bshort _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + _dataOffset; } }

        public WiiColorComponentType Type { get { return (WiiColorComponentType)(int)_format; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0UVData
    {
        public bint _dataLen; //includes header/padding
        public bint _mdl0Offset;
        public bint _dataOffset; //0x40
        public bint _stringOffset;
        public bint _index;
        public bint _unk1;
        public bint _format;
        public byte _divisor;
        public byte _entryStride;
        public bshort _numEntries;
        public BVec2 _min;
        public BVec2 _max;
        public bint _pad1;
        public bint _pad2;
        public bint _pad3;
        public bint _pad4;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public BVec2* Entries { get { return (BVec2*)(Address + _dataOffset); } }

        public WiiVertexComponentType Type { get { return (WiiVertexComponentType)(int)_format; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Material
    {
        public const int Size = 64;

        public bint _dataLen;
        public bint _mdl0Offset;
        public bint _stringOffset;
        public bint _index;
        public bint _unk1; //0x00
        public byte _flag1;
        public byte _numLayers;
        public byte _flag3;
        public byte _flag4;
        public bint _type; //0x02
        public byte _flag5;
        public byte _flag6;
        public byte _flag7;
        public byte _flag8;
        public bint _unk3;
        public bint _unk4; //0xFFFFFFFF
        public bint _shaderOffset;
        public bint _numTextures;
        public bint _part3Offset;
        public bint _part4Offset;
        public bint _part5Offset;
        public bint _unk6;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public MDL0MatLayer* Part3 { get { return (_part3Offset != 0) ? (MDL0MatLayer*)(Address + _part3Offset) : null; } }
        public MDL0Data7Part4* Part4 { get { return (_part4Offset != 0) ? (MDL0Data7Part4*)(Address + _part4Offset) : null; } }
        public void* Part5 { get { return (_part5Offset != 0) ? Address + _part5Offset : null; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0MatLayer
    {
        public const int Size = 52;

        public bint _stringOffset;
        public bint _secondaryOffset;
        public bint _unk2;
        public bint _unk3;
        public bint _unk4;
        public bint _unk5;
        public bint _layerId1;
        public bint _layerId2;
        public bint _unk8;
        public bint _unk9;
        public bfloat _float;
        public bint _unk10;
        public bint _unk11;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public string SecondaryTexture { get { return (_secondaryOffset == 0) ? null : new String((sbyte*)this.SecondaryTextureAddress); } }
        public VoidPtr SecondaryTextureAddress
        {
            get { return Address + _secondaryOffset; }
            set { _secondaryOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Data7Part4
    {
        public bint _totalLen; //including group + all entries
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + 4); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Data7Part4Entry
    {
        public bint _totalLen;
        public bint _unk1; //0x18, length without first four?
        public bint _unk2; //0x01, num entries?
        public bint _unk3; //0x00
        public bint _stringOffset; //same as entry
        public bint _unk4; //0x00
        public bint _unk5; //0x00

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Shader
    {
        public bint _dataLength;
        public bint _mdl0Offset;
        public bint _index;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Polygon
    {
        public bint _totalLength;
        public bint _mdl0Offset;
        public bint _nodeId; //Linkage?

        public MDL0ElementFlags _flags;

        //public bint _elementFlags; //Specifies number/size of elements (also in def block)
        //public bint _unkFlags1; //3 means extra element? (also in def block)
        public bint _unkFlags2; //0x15 (also in def block)

        public bint _defSize; //Size of def block including padding. Always 0xE0?
        public bint _defFlags; //0x80
        public bint _defOffset; //Relative to defSize field

        public bint _dataLen1;
        public bint _dataLen2; //Same as previous
        public bint _dataOffset; //Relative to dataLen1
        public bint _unk2; //0x2E00 flags?
        public bint _unk3; //0
        public bint _stringOffset;
        public bint _index;
        public bint _unk4;
        public bint _numFaces;
        public bshort _vertexId;
        public bshort _normalId;
        fixed short _colorIds[2];
        //public bshort _colorId1;
        //public bshort _colorId2;
        fixed short _uids[8];
        public bint _part1Offset;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public MDL0Header* Parent { get { return (MDL0Header*)(Address + _mdl0Offset); } }

        //public MDL0VertexData* VertexData { get { return (MDL0VertexData*)Parent->VertexGroup->First[_vertexId].DataAddress; } }
        //public MDL0NormalData* NormalData { get { return (MDL0NormalData*)Parent->NormalGroup->First[_normalId].DataAddress; } }
        //public MDL0ColorData* ColorData1 { get { return (MDL0ColorData*)Parent->ColorGroup->First[_colorId1].DataAddress; } }
        //public MDL0ColorData* ColorData2 { get { return (MDL0ColorData*)Parent->ColorGroup->First[_colorId2].DataAddress; } }
        //public MDL0UVData* GetUVData(int index)
        //{
        //    fixed (short* p = _uids)
        //    {
        //        bshort* ptr = (bshort*)p;
        //        return ptr[index] == -1 ? null : (MDL0UVData*)Parent->UVGroup->First[ptr[index]].DataAddress;
        //    }
        //}

        public bshort* ColorIds { get { return (bshort*)(Address + 0x4C); } }
        public bshort* UVIds { get { return (bshort*)(Address + 0x50); } }

        public bushort* WeightIndices { get { return (bushort*)(Address + _part1Offset); } }

        public VoidPtr PrimitiveData { get { return Address + 0x24 + _dataOffset; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    //public struct EntrySize
    //{
    //    public int _extraLen;
    //    public int _vertexLen;
    //    public int _normalLen;
    //    public int _colorLen;
    //    public int _uvEntries;
    //    public int[] _uvLen;
    //    public int _uvTotal;
    //    public int _totalLen;

    //    public VertexFormats _format;
    //    public int _stride;

    //    public EntrySize(MDL0ElementFlags flags)
    //    {
    //        _extraLen = flags.ExtraLength;
    //        _vertexLen = flags.VertexEntryLength;
    //        _normalLen = flags.NormalEntryLength;
    //        _colorLen = flags.ColorEntryLength;

    //        _uvEntries = _uvTotal = 0;
    //        _uvLen = new int[8];
    //        for (int i = 0; i < 8; _uvTotal += _uvLen[i++])
    //            if ((_uvLen[i] = flags.UVLength(i)) != 0)
    //                _uvEntries++;

    //        _totalLen = _extraLen + _vertexLen + _normalLen + _colorLen + _uvTotal;

    //        _format = VertexFormats.None;
    //        _stride = 0;
    //        if (_vertexLen != 0) { _format |= VertexFormats.Position; _stride += 12; }
    //        if (_normalLen != 0) { _format |= VertexFormats.Normal; _stride += 12; }
    //        if (_colorLen != 0) { _format |= VertexFormats.Diffuse; _stride += 4; }
    //        //if (_uvEntries != 0) { _format |= (VertexFormats)(_uvEntries << 8); _stride += 8 * _uvEntries; }
    //    }
    //}

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0ElementFlags
    {
        private buint _data1;
        private buint _data2;

        //Data1
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

        //Data2
        //0000 0000 0000 0000 0000 0000 0000 0011     - Tex0 format
        //0000 0000 0000 0000 0000 0000 0000 1100     - Tex1 format
        //0000 0000 0000 0000 0000 0000 0011 0000     - Tex2 format
        //0000 0000 0000 0000 0000 0000 1100 0000     - Tex3 format
        //0000 0000 0000 0000 0000 0011 0000 0000     - Tex4 format
        //0000 0000 0000 0000 0000 1100 0000 0000     - Tex5 format
        //0000 0000 0000 0000 0011 0000 0000 0000     - Tex6 format
        //0000 0000 0000 0000 1100 0000 0000 0000     - Tex7 format

        public bool HasVertexData { get { return (_data1 & 0x400) != 0; } }
        public int VertexEntryLength { get { return (HasVertexData) ? (((_data1 & 0x200) != 0) ? 2 : 1) : 0; } }
        public bool HasNormalData { get { return (_data1 & 0x1000) != 0; } }
        public int NormalEntryLength { get { return (HasNormalData) ? (((_data1 & 0x800) != 0) ? 2 : 1) : 0; } }

        //public bool HasColorData { get { return (_data1 & 0x4000) != 0; } }
        //public int ColorEntryLength { get { return (HasColorData) ? (((_data1 & 0x2000) != 0) ? 2 : 1) : 0; } }

        public bool HasColor(int index) { return (_data1 & (0x4000 << (index * 2))) != 0; }
        public int ColorLength(int index) { return HasColor(index) ? (((_data1 & (0x2000 << (index * 2))) != 0) ? 2 : 1) : 0; }
        public int ColorTotalLength { get { int len = 0; for (int i = 0; i < 2; )len += ColorLength(i++); return len; } }

        public bool HasUV(int index) { return (_data2 & (2 << (index * 2))) != 0; }
        public int UVLength(int index) { return HasUV(index) ? (((_data2 & (1 << (index * 2))) != 0) ? 2 : 1) : 0; }
        public int UVTotalLength { get { int len = 0; for (int i = 0; i < 8; )len += UVLength(i++); return len; } }

        public bool HasExtra(int index) { return (_data1 & (1 << index)) != 0; }
        public int ExtraLength { get { int len = 0; for (int i = 0; i < 8; ) if (HasExtra(i++))len++; return len; } }

        //public bool HasWeights { get { return (_data1 & 0xFF) != 0; } }
        //public int WeightLength { get { return ExtraLength; } }
    }


    //

    //Part2
    //0x0850 = bytes per data

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0Texture
    {
        public bint _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public MDL0TextureEntry* Entries { get { return (MDL0TextureEntry*)(Address + 4); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MDL0TextureEntry
    {
        public bint _x;
        public bint _y;

        public override string ToString()
        {
            return String.Format("(X:{0:X}, Y:{1:X})", (int)_x, (int)_y);
        }
    }

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public unsafe struct UVPoint
    //{
    //    public bfloat U;
    //    public bfloat V;

    //    public override string ToString()
    //    {
    //        return String.Format("U:{0}, V:{1}", (float)U, (float)V);
    //    }
    //}

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public unsafe struct RGBAPixel
    //{
    //    public byte R;
    //    public byte G;
    //    public byte B;
    //    public byte A;

    //    public static explicit operator RGBAPixel(uint val) { return *((RGBAPixel*)&val); }
    //    public static explicit operator uint(RGBAPixel p) { return *((uint*)&p); }

    //    public static explicit operator RGBAPixel(ARGBPixel p) { return new RGBAPixel() { R = p.R, G = p.G, B = p.G, A = p.A }; }
    //    public static explicit operator ARGBPixel(RGBAPixel p) { return new ARGBPixel() { A = p.A, R = p.R, G = p.G, B = p.G }; }

    //    public override string ToString()
    //    {
    //        return String.Format("R:{0:X}, G:{1:X}, B:{2:X}, A:{3:X}", R, G, B, A);
    //    }
    //}
}
