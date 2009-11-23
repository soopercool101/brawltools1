using System;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0NormalNode : MDL0EntryNode
    {
        internal MDL0NormalData* Header { get { return (MDL0NormalData*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_dataLen; } }

        [Category("Normal Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("Normal Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Normal Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("Normal Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Normal Data")]
        public int ID { get { return Header->_index; } }
        [Category("Normal Data")]
        public bool HasBox { get { return Header->_hasbox != 0; } }
        [Category("Normal Data")]
        public int Format { get { return Header->_type; } }
        [Category("Normal Data")]
        public int Divisor { get { return Header->_divisor; } }
        [Category("Normal Data")]
        public int EntryStride { get { return Header->_entryStride; } }
        [Category("Normal Data")]
        public short NumEntries { get { return Header->_numVertices; } }

        private Vector3[] _normals;
        public Vector3[] Normals
        {
            get { return _normals == null ? _normals = ModelConverter.ExtractNormals(Header) : _normals; }
            set { _normals = value; SignalPropertyChange(); }
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0NormalData* header = (MDL0NormalData*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
