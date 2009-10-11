using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ColorNode : MDL0EntryNode
    {
        internal MDL0ColorData* Data { get { return (MDL0ColorData*)WorkingSource.Address; } }

        [Category("Color Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Color Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Color Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("Color Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Color Data")]
        public int ID { get { return Data->_index; } }
        [Category("Color Data")]
        public int IsRGBA { get { return Data->_isRGBA; } }
        [Category("Color Data")]
        public int Format { get { return Data->_format; } }
        [Category("Color Data")]
        public byte EntryStride { get { return Data->_entryStride; } }
        [Category("Color Data")]
        public byte Unknown3 { get { return Data->_unk3; } }
        [Category("Color Data")]
        public short NumEntries { get { return Data->_numEntries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
                _origSource.Length = _uncompSource.Length = TotalLen;

            return false;
        }
    }
}
