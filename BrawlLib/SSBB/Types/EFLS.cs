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

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public EFLSEntry* Entries { get { return (EFLSEntry*)(Address + 0x10); } }

        public string GetString(int index)
        {
            EFLSEntry* entry = &Entries[index];
            if (entry->_stringOffset == 0)
                return "";
            return new String((sbyte*)Address + entry->_stringOffset);
        }
    }

    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct EFLSEntry
    {
        public const int Size = 0x10;

        public bshort _flags;
        public bshort _unk1; //0
        public bint _stringOffset; //From file, 0 is empty string
        public bint _unk2; //0
        public bint _unk3; //0
    }
}
