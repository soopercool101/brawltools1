using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct ruint
    {
        public uint _control;
        public buint _data;

        public VoidPtr Offset(VoidPtr baseAddr) { return baseAddr + _data; }

        public static implicit operator ruint(uint r) { return new ruint() { _control = 1, _data = r }; }
        public static implicit operator uint(ruint r) { return r._data; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSARHeader
    {
        public const uint Tag = 0x52415352;

        public SSBBCommonHeader _commonHeader;

        //public uint _tag;
        //public buint _version;
        //public buint _fileSize;
        //public bushort _unk1; //0x40
        //public bushort _unk2; //0x03
        //public buint _symbOffset;
        //public buint _symbLength;
        //public buint _infoOffset;
        //public buint _infoLength;
        //public buint _fileOffset;
        //public buint _fileLength;

        private VoidPtr Address { get { fixed (RSARHeader* ptr = &this)return ptr; } }

        public SYMBHeader* SYMBBlock { get { return (SYMBHeader*)_commonHeader.Entries[0].Address; } }
        public INFOHeader* INFOBlock { get { return (INFOHeader*)_commonHeader.Entries[1].Address; } }
        public FILEHeader* FILEBlock { get { return (FILEHeader*)_commonHeader.Entries[2].Address; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SYMBHeader
    {
        public const uint Tag = 0x424D5953;

        public buint _tag;
        public bint _length;
        public bint _stringOffset;

        public bint _maskOffset1; //For sounds
        public bint _maskOffset2; //For types
        public bint _maskOffset3; //For groups
        public bint _maskOffset4; //For banks

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        //public VoidPtr StringData { get { return Address + 8 + _stringOffset; } }
        public SYMBMaskHeader* MaskData1 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset1); } }
        public SYMBMaskHeader* MaskData2 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset2); } }
        public SYMBMaskHeader* MaskData3 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset3); } }
        public SYMBMaskHeader* MaskData4 { get { return (SYMBMaskHeader*)(Address + 8 + _maskOffset4); } }

        public uint StringCount { get { return StringOffsets[-1]; } }
        public buint* StringOffsets { get { return (buint*)(Address + 8 + _stringOffset + 4); } }

        public string GetStringEntry(int index)
        {
            return new String((sbyte*)(Address + 8 + StringOffsets[index]));
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SYMBMaskHeader
    {
        public buint _entrySize; //unknown
        public buint _entryNum; //number of entries in block
        //entries are 0x14 bytes each

        private VoidPtr Address { get { fixed (SYMBMaskHeader* ptr = &this)return ptr; } }
        public SYMBMaskEntry* Entries { get { return (SYMBMaskEntry*)(Address + 8); } }


    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SYMBMaskEntry
    {
        public buint _unk1;
        public buint _unk2;
        public buint _unk3;
        public buint _unk4;
        public buint _unk5;

        //[TypeConverter(typeof(UIntToHex))]
        //public uint Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Unknown2 { get { return _unk2; } set { _unk2 = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Unknown3 { get { return _unk3; } set { _unk3 = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Unknown4 { get { return _unk4; } set { _unk4 = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Unknown5 { get { return _unk5; } set { _unk5 = value; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOHeader
    {
        public const uint Tag = 0x6F464E49;

        public SSBBEntryHeader _entryHeader;
        public RuintCollection _collection;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr OffsetAddress { get { return _collection.Address; } }

        public ruint* GroupList { get { return (ruint*)(Address + 8); } }

        //Sounds
        public uint InfoCount { get { return *((buint*)_collection[0]); } }
        public ruint* InfoOffsets { get { return (ruint*)(_collection[0] + 4); } }
        public INFOData1Part1* GetInfoAddress(int index) { return (INFOData1Part1*)(_collection.Address + InfoOffsets[index]); }

        //Banks
        public uint Info2Count { get { return *((buint*)_collection[1]); } }
        public ruint* Info2Offsets { get { return (ruint*)(_collection[1] + 4); } }
        public INFOData2* GetInfo2Address(int index) { return (INFOData2*)(_collection.Address + Info2Offsets[index]); }

        //Types
        public uint Info3Count { get { return *((buint*)_collection[2]); } }
        public ruint* Info3Offsets { get { return (ruint*)(_collection[2] + 4); } }
        public INFOData3* GetInfo3Address(int index) { return (INFOData3*)(_collection.Address + Info3Offsets[index]); }

        //Sets
        public uint Info4Count { get { return *((buint*)_collection[3]); } }
        public ruint* Info4Offsets { get { return (ruint*)(_collection[3] + 4); } }
        public INFOData4* GetInfo4Address(int index) { return (INFOData4*)(_collection.Address + Info4Offsets[index]); }

        //Groups
        public uint Info5Count { get { return *((buint*)_collection[4]); } }
        public ruint* Info5Offsets { get { return (ruint*)(_collection[4] + 4); } }
        public INFOData5* GetInfo5Address(int index) { return (INFOData5*)(_collection.Address + Info5Offsets[index]); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData1Part1
    {
        public bint _stringId;
        public bint _groupId;
        public buint _unk1;
        public ruint _offset1;
        public byte _flag1;
        public byte _flag2;
        public byte _flag3;
        public byte _flag4;
        public ruint _offset2;
        public buint _unk2;
        public buint _unk3;
        public buint _unk4;

        public INFOData1Part2* GetPart2(VoidPtr baseAddr) { return (INFOData1Part2*)(baseAddr + _offset2); }
        public INFOData1Part3* GetPart3(VoidPtr baseAddr) { return (INFOData1Part3*)(baseAddr + _offset1); }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData1Part2
    {
        public buint _groupIndex;
        public buint _unk2;
        public buint _unk3;
        public buint _unk4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData1Part3
    {
        public buint _unk1;
        public buint _unk2;
        public buint _unk3;
    }

    //Bank info
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData2
    {
        public bint _stringId; //string id
        public bint _collectionId;
        public bint _padding;
    }

    //Type flags?
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData3
    {
        public bint _typeId;
        public uint _flags;
        public buint _unk1; //always 0
        public buint _unk2; //always 0
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData4
    {
        public buint _length;
        public buint _unk1;
        public buint _magic;
        public ruint _stringOffset;
        public ruint _part2Offset;

        public INFOData4Part2* GetPart2(VoidPtr baseAddr) { return (INFOData4Part2*)(baseAddr + _part2Offset); }
        public string GetPath(VoidPtr baseAddr) { return (_stringOffset == 0) ? null : new String((sbyte*)(baseAddr + _stringOffset)); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData4Part2
    {
        public buint _entryCount;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ruint* Entries { get { return (ruint*)(Address + 4); } }

        public INFOData4Part3* GetEntry(VoidPtr baseAddr, int index) { return (INFOData4Part3*)(baseAddr + Entries[index]); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData4Part3
    {
        public buint _groupId;
        public buint _index;

        public uint GroupId { get { return _groupId; } set { _groupId = value; } }
        public uint Index { get { return _index; } set { _index = value; } }

        public override string ToString()
        {
            return String.Format("[{0}, {1}]", GroupId, Index);
        }
    }

    //Sound groups
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData5
    {
        public bint _id; //string id
        public bint _magic; //always -1
        public bint _unk1; //always 0
        public bint _unk2; //always 0
        public bint _data1Offset;
        public bint _data1Length;
        public bint _data2Offset;
        public bint _data2Length;
        public ruint _offset;

        public RuintList* GetCollection(VoidPtr offset) { return (RuintList*)(offset + _offset); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct INFOData5Entry
    {
        public buint _groupId;
        public buint _data1Offset;
        public buint _data1Length;
        public buint _data2Offset;
        public buint _data2Length;
        public buint _unk;

        //[TypeConverter(typeof(UIntToHex))]
        //public uint GroupId { get { return _groupId; } set { _groupId = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Data1Offset { get { return _data1Offset; } set { _data1Offset = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Data1Length { get { return _data1Length; } set { _data1Length = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Data2Offset { get { return _data2Offset; } set { _data2Offset = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Data2Length { get { return _data2Length; } set { _data2Length = value; } }
        //[TypeConverter(typeof(UIntToHex))]
        //public uint Unknown { get { return _unk; } set { _unk = value; } }

        public override string ToString()
        {
            return String.Format("[{0:X}]", (uint)_groupId);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FILEHeader
    {
        public const uint Tag = 0x454C4946;

        public uint _tag;
        public bint _length;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RWSDHeader
    {
        public const uint Tag = 0x44535752;

        public SSBBCommonHeader _header;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public DATAHeader* DataHeader { get { return (DATAHeader*)(_header.Entries[0].Address); } }
        public WAVEHeader* WAVEHeader { get { return (WAVEHeader*)(_header.Entries[1].Address); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct DATAHeader
    {
        public const uint Tag = 0x41514144;

        public uint _tag;
        public uint _length;
        public uint _numEntries;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr OffsetAddress { get { return Address + 8; } }

        public ruint* Entries { get { return (ruint*)(Address + 12); } }

        public DATAEntry* GetEntry(int index)
        {
            return (DATAEntry*)(OffsetAddress + Entries[index]._data);
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct DATAEntry
    {
        public ruint _part1Offset;
        public ruint _part2Offset;
        public ruint _part3Offset;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public DATAEntryPart1* GetPart1(VoidPtr offset) { return (DATAEntryPart1*)_part1Offset.Offset(offset); }
        public RuintList* GetPart2(VoidPtr offset) { return (RuintList*)_part2Offset.Offset(offset); }
        public RuintList* GetPart3(VoidPtr offset) { return (RuintList*)_part3Offset.Offset(offset); }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct DATAEntryPart1
    {
        public bfloat _unk1;
        public bfloat _unk2;
        public bushort _unk3;
        public bushort _unk4;
        public buint _unk5;
        public buint _unk6;
        public buint _unk7;
        public buint _unk8;
        public buint _unk9;
    }

    //These entries are embedded in a list of lists, using RuintList
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct DATAEntryPart2Entry
    {
        public bint _unk1; //0
        public bint _unk2; //0
        public bint _unk3; //0
        public bint _unk4; //0

        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown2 { get { return _unk2; } set { _unk2 = value; } }
        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown3 { get { return _unk3; } set { _unk3 = value; } }
        //[TypeConverter(typeof(IntToHex))]
        //public int Unknown4 { get { return _unk4; } set { _unk4 = value; } }
    }

    //These entries are embedded in a list, using RuintList
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct DATAEntryPart3Entry
    {
        public bint _index;
        public buint _magic; //0x7F7F7F7F
        public bint _unk1; //0
        public bfloat _unk2; //0.01557922
        public bfloat _unk3; //1.0
        public bint _unk4; //0
        public bint _unk5; //0
        public bint _unk6; //0
        public bint _unk7; //0
        public bint _unk8; //0
        public bint _unk9; //0
        public bint _unk10; //0

        public int Index { get { return _index; } set { _index = value; } }
        public uint Magic { get { return _magic; } set { _magic = value; } }
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        public float Float1 { get { return _unk2; } set { _unk2 = value; } }
        public float Float2 { get { return _unk3; } set { _unk3 = value; } }
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; } }
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; } }
        public int Unknown6 { get { return _unk6; } set { _unk6 = value; } }
        public int Unknown7 { get { return _unk7; } set { _unk7 = value; } }
        public int Unknown8 { get { return _unk8; } set { _unk8 = value; } }
        public int Unknown9 { get { return _unk9; } set { _unk9 = value; } }
        public int Unknown10 { get { return _unk10; } set { _unk10 = value; } }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct WAVEHeader
    {
        public const uint Tag = 0x45564157;

        public buint _tag;
        public buint _length;
        public buint _numEntries;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public buint* Entries { get { return (buint*)(Address + 12); } }
        public WAVEEntry* GetEntry(int index)
        {
            return (WAVEEntry*)(Address + Entries[index]);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct WAVEEntry
    {
        public AudioFormatInfo _format;

        public bushort _frequency;
        public bushort _unk3; //0x00
        public buint _flags;
        public bint _nibbles; //Includes ALL data, not just samples
        public bint _unk4; //0x1C
        public bint _offset;
        public bint _unk5; //0
        public bint _unk6; //0x20
        public bint _unk7; //0
        public bint _unk8; //0x3C
        public int _unk9; //1
        public int _unk10; //1
        public int _unk11; //1
        public int _unk12; //1
        public int _unk13; //0

        public ADPCMInfo _adpcInfo;

        public int NumSamples { get { return (_nibbles / 16 * 14) + ((_nibbles % 16) - 2); } }
    }
}
