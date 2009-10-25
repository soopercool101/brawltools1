using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ColorNode : MDL0EntryNode
    {
        internal MDL0ColorData* Header { get { return (MDL0ColorData*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_dataLen; } }

        [Category("Color Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("Color Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Color Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("Color Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Color Data")]
        public int ID { get { return Header->_index; } }
        [Category("Color Data")]
        public int IsRGBA { get { return Header->_isRGBA; } }
        [Category("Color Data")]
        public int Format { get { return Header->_format; } }
        [Category("Color Data")]
        public byte EntryStride { get { return Header->_entryStride; } }
        [Category("Color Data")]
        public byte Unknown3 { get { return Header->_unk3; } }
        [Category("Color Data")]
        public short NumEntries { get { return Header->_numEntries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;
            return false;
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0ColorData* header = (MDL0ColorData*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
