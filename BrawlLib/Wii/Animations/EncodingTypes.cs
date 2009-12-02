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
        public bushort _unk1;
        public bfloat _unk2;

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
}
