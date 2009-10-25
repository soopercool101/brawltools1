using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct PAT0
    {
        public const uint Tag = 0x30544150;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _offset1; //String list 1
        public bint _offset2; //String list 2
        public bint _offset3;
        public bint _offset4;
        public bint _stringOffset;
        public bint _unk1; //0x00
        public bshort _unk2;
        public bshort _numEntries; //0x05, same as group entries
        public bshort _numStrings1; //0x07
        public bshort _numStrings2; //0
        public bint _unk4; //0x00

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public bint* StringOffsets1 { get { return (bint*)(Address + _offset1); } }
        public bint* StringOffsets2 { get { return (bint*)(Address + _offset2); } }

        //public string GetStringEntry(int index)
        //{
        //    return new String((sbyte*)((VoidPtr)StringOffsets + StringOffsets[index]));
        //}

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct PAT0Entry
    {
        public const int Size = 0x14;

        public bint _stringOffset;
        public bint _unk1; //0x07
        public bshort _unk2;
        public bshort _unk3; //Data offset
        public bshort _numEntries;
        public bshort _unk4;
        public bint _unk5; //Color? Float?

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }
}
