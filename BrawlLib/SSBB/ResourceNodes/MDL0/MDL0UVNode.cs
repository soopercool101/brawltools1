using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0UVNode : MDL0EntryNode
    {
        internal MDL0UVData* Header { get { return (MDL0UVData*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_dataLen; } }

        [Category("UV Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("UV Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("UV Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("UV Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("UV Data")]
        public int ID { get { return Header->_index; } }
        [Category("UV Data")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("UV Data")]
        public int Format { get { return Header->_format; } }
        [Category("UV Data")]
        public byte Divisor { get { return Header->_divisor; } }
        [Category("UV Data")]
        public byte EntryStride { get { return Header->_entryStride; } }
        [Category("UV Data")]
        public short NumEntries { get { return Header->_numEntries; } }

        [Category("UV Data")]
        public Vector2 Min { get { return Header->_min; } }
        [Category("UV Data")]
        public Vector2 Max { get { return Header->_max; } }

        [Category("UV Data")]
        public int Pad1 { get { return Header->_pad1; } }
        [Category("UV Data")]
        public int Pad2 { get { return Header->_pad2; } }
        [Category("UV Data")]
        public int Pad3 { get { return Header->_pad3; } }
        [Category("UV Data")]
        public int Pad4 { get { return Header->_pad4; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0UVData* header = (MDL0UVData*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
