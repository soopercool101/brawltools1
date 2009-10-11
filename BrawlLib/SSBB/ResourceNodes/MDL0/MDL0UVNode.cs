using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0UVNode : MDL0EntryNode
    {
        internal MDL0UVData* Data { get { return (MDL0UVData*)WorkingSource.Address; } }

        [Category("UV Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("UV Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("UV Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("UV Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("UV Data")]
        public int ID { get { return Data->_index; } }
        [Category("UV Data")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("UV Data")]
        public int Format { get { return Data->_format; } }
        [Category("UV Data")]
        public byte Divisor { get { return Data->_divisor; } }
        [Category("UV Data")]
        public byte EntryStride { get { return Data->_entryStride; } }
        [Category("UV Data")]
        public short NumEntries { get { return Data->_numEntries; } }

        [Category("UV Data")]
        public Vector2 Min { get { return Data->_min; } }
        [Category("UV Data")]
        public Vector2 Max { get { return Data->_max; } }

        [Category("UV Data")]
        public int Pad1 { get { return Data->_pad1; } }
        [Category("UV Data")]
        public int Pad2 { get { return Data->_pad2; } }
        [Category("UV Data")]
        public int Pad3 { get { return Data->_pad3; } }
        [Category("UV Data")]
        public int Pad4 { get { return Data->_pad4; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
                _origSource.Length = _uncompSource.Length = TotalLen;

            return false;
        }
    }
}
