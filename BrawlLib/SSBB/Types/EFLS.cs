using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public unsafe struct EFLSHeader
    {
        public const uint Tag = 0x534C4645;
        public const int Size = 0x10;

        public uint _tag;
        public bshort _numEntries;
        public bshort _unk1; //0
        public bint _unk2; //0
        public bint _unk3; //0

        public EFLSHeader(int entries, int unk1, int unk2, int unk3)
        {
            _tag = Tag;
            _numEntries = (short)entries;
            _unk1 = (short)unk1;
            _unk2 = unk2;
            _unk3 = unk3;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        //First entry is always empty?
        public EFLSEntry* Entries { get { return (EFLSEntry*)(Address + 0x10); } }

        public string GetString(int index)
        {
            EFLSEntry* entry = &Entries[index];
            if (entry->_stringOffset == 0)
                return "<null>";
            return new String((sbyte*)Address + entry->_stringOffset);
        }
    }

    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct EFLSEntry
    {
        public const int Size = 0x10;

        public bshort _unk1; //-1
        public bshort _unk2; //0
        public bint _stringOffset; //From file, 0 is empty string?
        public bint _unk3; //0
        public bint _unk4; //0

        public EFLSEntry(int unk1, int unk2, int stringOffset, int unk3, int unk4)
        {
            _unk1 = (short)unk1;
            _unk2 = (short)unk2;
            _stringOffset = stringOffset;
            _unk3 = unk3;
            _unk4 = unk4;
        }
    }
}
