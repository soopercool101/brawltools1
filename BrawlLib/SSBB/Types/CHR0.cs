﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Animations;

namespace BrawlLib.SSBBTypes
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    public unsafe struct CHR0
    {
        public const int Size = 0x28;
        public const uint Tag = 0x30524843;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _stringOffset;
        public bint _unk1;
        public bushort _numFrames;
        public bushort _numEntries;
        public bint _unk2;
        public bint _unk3;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }

        public CHR0(int size, int frames, int entries, int unk1, int unk2, int unk3)
        {
            _header._tag = Tag;
            _header._size = size;
            _header._version = 4; //what is it? version?
            _header._bresOffset = 0;

            _dataOffset = Size;
            _stringOffset = 0;
            _unk1 = unk1;
            _numFrames = (ushort)frames;
            _numEntries = (ushort)entries;
            _unk2 = unk2;
            _unk3 = unk3;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CHR0Entry
    {
        public bint _stringOffset;
        public buint _code;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public AnimationCode Code { get { return new AnimationCode() { _data = _code }; } set { _code = value._data; } }

        public VoidPtr Data { get { return Address + 8; } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }
}
