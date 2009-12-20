using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct F3FHeader
    {
        public const int Size = 8;

        public bushort _numFrames;
        public bushort _unk;
        public bfloat _frequency;

        public F3FHeader(int entries, float frequency)
        {
            _numFrames = (ushort)entries;
            _unk = 0;
            _frequency = frequency;
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
        public bfloat _error;
        public bfloat _step;
        public bfloat _base;

        public F6BHeader(int frames, float error, float step, float floor)
        {
            _numFrames = (ushort)frames;
            _unk1 = 0;
            _error = error;
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

        public F6BEntry(int index, int step)
        {
            _data = (ushort)(index << 5);
            _step = (ushort)step;
            _unk = 0;
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
        public bfloat _frequency;
        public bfloat _step;
        public bfloat _base;

        public F4BHeader(int entries, float frequency, float step, float floor)
        {
            _entries = (ushort)entries;
            _unk = 0;
            _frequency = frequency;
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
