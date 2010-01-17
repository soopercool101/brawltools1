using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct VIS0
    {
        public const uint Tag = 0x30534956;

        public BRESCommonHeader _header;
        public bint _dataOffset; //0x24
        public bint _stringOffset;
        public bint _unk1;
        public bshort _frameCount;
        public bshort _numEntries;
        public bint _unk2; //0

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct VIS0Entry
    {
        public bint _stringOffset;
        public bint _flags;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public VoidPtr Data { get { return Address + 8; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public VIS0Flags Flags { get { return (VIS0Flags)(int)_flags; } set { _flags = (int)value; } }
    }

    public enum VIS0Flags : int
    {
        None = 0x00,
        Enabled = 0x01,
        Constant = 0x02
    }
}
