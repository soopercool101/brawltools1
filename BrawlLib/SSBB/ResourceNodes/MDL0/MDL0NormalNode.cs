using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0NormalNode : MDL0EntryNode
    {
        internal MDL0NormalData* Data { get { return (MDL0NormalData*)WorkingSource.Address; } }

        [Category("Normal Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Normal Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Normal Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("Normal Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Normal Data")]
        public int ID { get { return Data->_index; } }
        [Category("Normal Data")]
        public bool HasBox { get { return Data->_hasbox != 0; } }
        [Category("Normal Data")]
        public int Format { get { return Data->_type; } }
        [Category("Normal Data")]
        public int Divisor { get { return Data->_divisor; } }
        [Category("Normal Data")]
        public int EntryStride { get { return Data->_entryStride; } }
        [Category("Normal Data")]
        public short NumEntries { get { return Data->_numVertices; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
                _origSource.Length = _uncompSource.Length = TotalLen;

            return false;
        }
    }
}
