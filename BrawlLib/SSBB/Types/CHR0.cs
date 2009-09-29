using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct CHR0
    {
        public const int Size = 16;
        public const uint Tag = 0x30524843;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _unk1;
        public bshort _len1;
        public bshort _len2;
        public bint _unk2;
        public bint _unk3;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public CHR0(int size, int entries, short len1, short len2 )
        {
            _header._tag = Tag;
            _header._size = size;
            _header._numResources = entries;
            _header._bresOffset = 0;

            _dataOffset = 0x28;
            _stringOffset = 0;
            _unk1 = 0;
            _len1 = len1;
            _len2 = len2;
            _unk2 = 0;
            _unk3 = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CHR0Entry
    {
        public bint _stringOffset;
        public byte _b1;
        public byte _b2;
        public byte _b3;
        public byte _b4;
    }
}
