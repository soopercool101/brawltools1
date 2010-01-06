using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;

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
        public bshort _frames;
        public bshort _entries;
        public bint _unk2;

        public CLR0(int size, int unk1, int frames, int entries, int unk2)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._bresOffset = 0;
            _header._numResources = 3;

            _dataOffset = Size;
            _stringOffset = 0;
            _unk1 = unk1;
            _frames = (short)frames;
            _entries = (short)entries;
            _unk2 = unk2;
        }

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
        public const int Size = 0x10;

        public bint _stringOffset;
        public bint _flags; //1 Block count?
        public ABGRPixel _colorMask; //Used as a mask for source color before applying frames
        public bint _dataOffset; //Offset from itself

        public CLR0Entry(int flags, ABGRPixel mask, int dataOffset)
        {
            _stringOffset = 0;
            _flags = flags;
            _colorMask = mask;
            _dataOffset = dataOffset;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ABGRPixel* Data { get { return (ABGRPixel*)(Address + _dataOffset + 12); } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }
}
