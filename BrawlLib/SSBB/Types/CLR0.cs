using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct CLR0
    {
        public const int Size = 0x24;
        public const int Tag = 0x30524C43;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _unk1;
        public bshort _len1;
        public bshort _len2;
        public bint _unk2;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct CLR0Entry
    {
        //public const int Size = 0x10;

        public bint _stringOffset;
        public bint _unk1; //1 Block count?
        public bint _unk2; //Base color?
        public bint _dataOffset;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + _dataOffset + 12; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }
}
