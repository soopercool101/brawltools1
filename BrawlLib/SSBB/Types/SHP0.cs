using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    unsafe struct SHP0
    {
        public const uint Tag = 0x30504853;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _data2Offset;
        public bint _stringOffset;
        public bint _unk2;
        public bshort _unk3;
        public bshort _unk4; //Entries?
        public bint _unk5; //0x01

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SHP0Entry
    {
        public bint _numParts; //entries + 1 index part
        public bint _stringOffset;
        public bint _numEntries;
        public bint _unk2;
        public bint _indeciesOffset;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public bshort* Indecies { get { return (bshort*)(Address + _indeciesOffset); } }

        public bint* EntryOffset { get { return (bint*)(Address + 0x14); } }
        public SHP0Part2* GetEntry(int index)
        {
            bint* ptr = &EntryOffset[index];
            return (SHP0Part2*)((VoidPtr)ptr + *ptr);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct SHP0Part2
    {
        public bshort _numEntries;
        public bshort _unk1;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public BVec3* Entries { get { return (BVec3*)(Address + 4); } }
    }
}
