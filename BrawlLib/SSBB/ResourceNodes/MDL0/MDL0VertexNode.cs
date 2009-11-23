using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0VertexNode : MDL0EntryNode
    {
        internal MDL0VertexData* Header { get { return (MDL0VertexData*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_dataLen; } }

        [Category("Vertex Data")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("Vertex Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Vertex Data")]
        public int DataOffset { get { return Header->_dataOffset; } }
        [Category("Vertex Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Vertex Data")]
        public int ID { get { return Header->_index; } }
        [Category("Vertex Data")]
        public bool IsXYZ { get { return Header->_isXYZ != 0; } }
        [Category("Vertex Data")]
        public int DataType { get { return Header->_type; } }
        [Category("Vertex Data")]
        public byte Divisor { get { return Header->_divisor; } }
        [Category("Vertex Data")]
        public byte EntryStride { get { return Header->_entryStride; } }
        [Category("Vertex Data")]
        public short NumVertices { get { return Header->_numVertices; } }
        [Category("Vertex Data")]
        public Vector3 EMin { get { return Header->_eMin; } }
        [Category("Vertex Data")]
        public Vector3 EMax { get { return Header->_eMax; } }
        [Category("Vertex Data")]
        public int Pad1 { get { return Header->_pad1; } }
        [Category("Vertex Data")]
        public int Pad2 { get { return Header->_pad2; } }

        private Vector3[] _vertices;
        public Vector3[] Vertices
        {
            get { return _vertices == null ? _vertices = ModelConverter.ExtractVertices(Header) : _vertices; }
            set { _vertices = value; SignalPropertyChange(); }
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
            MDL0UVData* header = (MDL0UVData*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        //public VertexList GetVertices() { return new VertexList() { _name = Name, _vertices = ModelConverter.ExtractVertices(Header) }; }
    }
}
