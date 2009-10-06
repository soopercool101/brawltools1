using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Models
{

    public struct ModelEntrySize
    {
        public int _extraLen;
        public int _vertexLen;
        public int _normalLen;
        public int[] _colorLen;
        public int _colorEntries;
        public int _colorTotal;
        public int _uvEntries;
        public int[] _uvLen;
        public int _uvTotal;
        public int _totalLen;

        public ModelEntrySize(MDL0ElementFlags flags)
        {
            _extraLen = flags.ExtraLength;
            _vertexLen = flags.VertexEntryLength;
            _normalLen = flags.NormalEntryLength;

            _colorEntries = _colorTotal = 0;
            _colorLen = new int[2];
            for (int i = 0; i < 2; _colorTotal += _colorLen[i++])
                if ((_colorLen[i] = flags.ColorLength(i)) != 0)
                    _colorEntries++;

            _uvEntries = _uvTotal = 0;
            _uvLen = new int[8];
            for (int i = 0; i < 8; _uvTotal += _uvLen[i++])
                if ((_uvLen[i] = flags.UVLength(i)) != 0)
                    _uvEntries++;

            _totalLen = _extraLen + _vertexLen + _normalLen + _colorTotal + _uvTotal;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PrimitiveHeader
    {
        public WiiPrimitiveType Type;
        public bushort Entries;

        internal VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + 3; } }
    }
}
