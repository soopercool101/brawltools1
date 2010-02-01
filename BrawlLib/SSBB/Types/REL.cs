using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RELHeader
    {
        public buint _id;
        public buint _linkNext; //0
        public buint _linkPrev; //0
        public buint _numSections;
        public buint _infoOffset;
        public buint _nameOffset;
        public buint _nameSize;
        public buint _version;

        public buint _bssSize;
        public buint _relOffset;
        public buint _impOffset;
        public buint _impSize;
        public byte _prologSection;
        public byte _epilogSection;
        public byte _unresolvedSection;
        public byte _bssSection;
        public buint _prologOffset;
        public buint _epilogOffset;
        public buint _unresolvedOffset;

        public buint _moduleAlign;
        public buint _bssAlign;
        public buint _fixSize;

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public RELSection* SectionInfo { get { return (RELSection*)(Address + _infoOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct RELSection
    {
        public buint _offset;
        public buint _size;

        public bool IsCodeSection { get { return (_offset & 1) != 0; } set { _offset = (uint)(_offset & ~1) | (uint)(value ? 1 : 0); } }
        public int Offset { get { return (int)_offset & ~1; } set { _offset = (uint)(value & ~1) | (_offset & 1); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct RELImport
    {
        public buint _moduleId;
        public buint _offset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct RELLink
    {
        public bushort _prevOffset;
        public byte _type;
        public byte _section;
        public buint _addEnd;
    }

    public enum RELLinkType : byte
    {
        NOP = 0xC9, //Increment offset6
        Section = 0xCA, //Set current section
        End = 0xCB,
        MrkRef = 0xCC
    }
}
