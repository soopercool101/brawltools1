using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    unsafe struct SRT0
    {
        public const uint Tag = 0x30545253;

        public BRESCommonHeader _header;

        public bint _dataOffset;
        public bint _stringOffset;
        public bint _unk1;
        public bshort _unk2;
        public bshort _numEntries;
        public bint _unk3;
        public bint _unk4;

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
    unsafe struct SRT0Entry
    {
        public bint _stringOffset;
        public bint _headerType; //0x01 or 0x03
        public bint _unk1; //0x00
        public bint _entryOffset; //0x10 or 0x14
        public bint _unk2; //0x20, only available when type = 3

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public SRT0Data2* Entries { get { return (SRT0Data2*)(Address + _entryOffset); } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SRT0Data2
    {
        public bint _type; //0x01f7 or 0x02f7;
        public bint _offset1;
        public bint _offset2;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public SRT0Part2* Entry1 { get { return (SRT0Part2*)(Address + 4 + _offset1); } }
        public SRT0Part2* Entry2 { get { return (SRT0Part2*)(Address + 8 + _offset2); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SRT0Part2
    {
        public bshort _numEntries;
        public bshort _unk1; //0x00
        public bint _unk2;
        
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public BVec3* Entries { get { return (BVec3*)(Address + 8); } }
    }
}
