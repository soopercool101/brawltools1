using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    struct DataBlock
    {
        private VoidPtr _address;
        private uint _length;

        public VoidPtr Address { get { return _address; } set { _address = value; } }
        public uint Length { get { return _length; } set { _length = value; } }
        public VoidPtr EndAddress { get { return _address + _length; } }

        public DataBlock(VoidPtr address, uint length)
        {
            _address = address;
            _length = length;
        }
    }

    unsafe struct DataBlockCollection
    {
        private DataBlock _block;

        public DataBlockCollection(DataBlock block) { _block = block; }

        private buint* Data { get { return (buint*)_block.EndAddress; } }

        public DataBlock this[int index]
        {
            get { return new DataBlock(_block.Address + Data[index << 1], Data[(index << 1) + 1]); }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SSBBCommonHeader
    {
        public const uint Size = 0x10;

        public uint _tag;
        public short _endian;
        public short _version;
        public buint _length;
        public bushort _firstOffset;
        public bushort _numEntries;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public DataBlock DataBlock { get { return new DataBlock(Address, Size); } }

        public DataBlockCollection Entries { get { return new DataBlockCollection(DataBlock); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RuintList
    {
        public buint _numEntries;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ruint* Entries { get { return (ruint*)(Address + 4); } }

        public VoidPtr GetEntry(VoidPtr offset, int index) { return offset + Entries[index]; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RuintCollection
    {
        public ruint _first;

        public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ruint* Entries { get { return (ruint*)Address; } }

        public VoidPtr this[int index] { get { return Address + Entries[index]; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SSBBEntryHeader
    {
        public const uint Size = 0x08;

        public uint _tag;
        public buint _length;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public DataBlock DataBlock { get { return new DataBlock(Address, Size); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResourceGroup
    {
        public bint _totalSize;
        public bint _numEntries;
        public ResourceEntry _first;

        public ResourceGroup(int numEntries)
        {
            _totalSize = (numEntries * 0x10) + 0x18;
            _numEntries = numEntries;
            _first = new ResourceEntry(-1, (short)numEntries, 0, 0);
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceEntry* First { get { return (ResourceEntry*)(Address + 0x18); } }
        public VoidPtr EndAddress { get { return Address + _totalSize; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ResourceEntry
    {
        public const uint Size = 0x10;

        public bshort _id;
        public bshort _pad;
        public bshort _leftIndex;
        public bshort _rightIndex;
        public bint _stringOffset;
        public bint _dataOffset;

        public int CharIndex { get { return _id >> 3; } set { _id = (short)((value << 3) | (_id & 0x7)); } }
        public int CharShift { get { return _id & 0x7; } set { _id = (short)((value & 0x7) | (_id & 0xFFF8)); } }

        public ResourceEntry(short id, short prev, short next, int dataOffset)
        {
            _id = id;
            _pad = 0;
            _leftIndex = prev;
            _rightIndex = next;
            _stringOffset = 0;
            _dataOffset = dataOffset;
        }
        public ResourceEntry(short id, short prev, short next, int dataOffset, int stringOffset)
        {
            _id = id;
            _pad = 0;
            _leftIndex = prev;
            _rightIndex = next;
            _stringOffset = stringOffset;
            _dataOffset = dataOffset;
        }

        private ResourceEntry* Address { get { fixed (ResourceEntry* ptr = &this)return ptr; } }

        public VoidPtr DataAddress { get { return (VoidPtr)Parent + _dataOffset; } }
        public VoidPtr StringAddress { get { return (VoidPtr)Parent + _stringOffset; } set { _stringOffset = (int)value - (int)Parent; } }

        public string GetName() { return new String((sbyte*)StringAddress); }

        public ResourceGroup* Parent
        {
            get
            {
                ResourceEntry* entry = Address;
                while (entry->_id != -1) entry--;
                return (ResourceGroup*)((uint)entry - 8);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct AudioFormatInfo
    {
        public byte _encoding;
        public byte _looped;
        public byte _channels;
        public byte _unk;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct ADPCMInfo
    {
        public fixed short _coefs[16];

        public bushort _gain;
        public bshort _ps;
        public bshort _yn1;
        public bshort _yn2;
        public bshort _lps;
        public bshort _lyn1;
        public bshort _lyn2;

        public short[] Coefs
        {
            get
            {
                short[] arr = new short[16];
                fixed (short* ptr = _coefs)
                {
                    bshort* sPtr = (bshort*)ptr;
                    for (int i = 0; i < 16; i++)
                        arr[i] = sPtr[i];
                }
                return arr;
            }
        }
    }
}
