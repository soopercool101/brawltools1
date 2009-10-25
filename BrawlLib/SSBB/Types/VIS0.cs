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
        public bshort _unk2;
        public bshort _numEntries;
        public bint _unk3;

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

        //public bushort _part1;
        //public bushort _part2;
        //public buint _part3;
        //public buint _part4;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        //public uint Value { get { return (_part1 == 0) ? _part3 : _part4; } }
        //public string GetString() 
        //{
        //    return new string((sbyte*)(Address + ((_part1 == 0) ? _part2 : (uint)_part3))); 
        //}
    }
}
