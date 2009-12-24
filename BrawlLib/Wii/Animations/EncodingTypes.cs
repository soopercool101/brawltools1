using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    //Format	Keys	Total	Size

    //F3F	    100	    200	    1208
    //F1F	    200	    200	    800
    //F6B	    100	    200	    616
    //F4B       100	    200	    416
    //F1B	    200	    200	    208

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F3FHeader
    {
        public const int Size = 8;

        public bushort _numFrames;
        public bushort _unk;
        public bfloat _frameScale;

        public F3FHeader(int entries, float frameScale)
        {
            _numFrames = (ushort)entries;
            _unk = 0;
            _frameScale = frameScale;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
        public F3FEntry* Data { get { return (F3FEntry*)(Address + Size); } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F3FEntry
    {
        public const int Size = 12;

        public bfloat _index;
        public bfloat _value;
        public bfloat _unk;

        public F3FEntry(float index, float value, float unk)
        {
            _index = index;
            _value = value;
            _unk = unk;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F6BHeader
    {
        public const int Size = 16;

        public bushort _numFrames;
        public bushort _unk1;
        public bfloat _frameScale; // = 1 / num frames. Percent each animation frame takes up.
        public bfloat _step;
        public bfloat _base;

        public F6BHeader(int frames, float frameScale, float step, float floor)
        {
            _numFrames = (ushort)frames;
            _unk1 = 0;
            _frameScale = frameScale;
            _step = step;
            _base = floor;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
        public F6BEntry* Data { get { return (F6BEntry*)(Address + Size); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F6BEntry
    {
        public const int Size = 6;

        public bushort _data;
        public bushort _step;
        public bushort _unk;

        public F6BEntry(int index, int step, int unk)
        {
            _data = (ushort)(index << 5);
            _step = (ushort)step;
            _unk = (ushort)unk;
        }

        public int FrameIndex
        {
            get { return _data >> 5; }
            set { _data = (ushort)((_data & 0x1F) | (value << 5)); }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F4BHeader
    {
        public const int Size = 16;

        public bushort _entries;
        public bushort _unk;
        public bfloat _frameScale;
        public bfloat _step;
        public bfloat _base;

        public F4BHeader(int entries, float frameScale, float step, float floor)
        {
            _entries = (ushort)entries;
            _unk = 0;
            _frameScale = frameScale;
            _step = step;
            _base = floor;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
        public F4BEntry* Data { get { return (F4BEntry*)(Address + Size); } }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F4BEntry
    {
        public const int Size = 4;

        public buint _data;

        //Flags
        //0x3E0 = Loop frame?

        public F4BEntry(int index, int step, int flags)
        {
            _data = (uint)((index << 24) | ((step & 0xFFF) << 12) | (flags & 0xFFF));
        }

        public int FrameIndex { get { return (int)((uint)_data >> 24); } set { _data = (_data & 0xFFFFFF) | ((uint)value << 24); } }
        public int Step { get { return (int)(((uint)_data >> 12) & 0xFFF); } set { _data = ((uint)_data & 0xFF000FFF) | (((uint)value & 0xFFF) << 12); } }
        public int Flags { get { return (int)((uint)_data & 0xFFF); } set { _data = (_data & 0xFFFFF000) | ((uint)value & 0xFFF); } }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F1BHeader
    {
        public const int Size = 8;

        public bfloat _step;
        public bfloat _base;

        public F1BHeader(float step, float floor)
        {
            _step = step;
            _base = floor;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }
        public byte* Data { get { return (byte*)Address + Size; } }
    }
}
