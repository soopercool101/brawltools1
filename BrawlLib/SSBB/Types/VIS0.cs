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
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct VIS0Entry
    {
        public bushort _part1;
        public bushort _part2;
        public buint _part3;
        public buint _part4;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        //A value of 0 is followed by mask data?
        public uint Value { get { return (_part1 == 0) ? _part3 : _part4; } }
        public string GetString() 
        {
            return new string((sbyte*)(Address + ((_part1 == 0) ? _part2 : (uint)_part3))); 
        }
    }
}
