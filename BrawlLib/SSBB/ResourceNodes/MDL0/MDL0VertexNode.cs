using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0VertexNode : MDL0EntryNode
    {
        internal MDL0VertexData* Data { get { return (MDL0VertexData*)WorkingSource.Address; } }

        [Category("Vertex Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Vertex Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Vertex Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("Vertex Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Vertex Data")]
        public int ID { get { return Data->_index; } }
        [Category("Vertex Data")]
        public bool IsXYZ { get { return Data->_isXYZ != 0; } }
        [Category("Vertex Data")]
        public int DataType { get { return Data->_type; } }
        [Category("Vertex Data")]
        public byte Divisor { get { return Data->_divisor; } }
        [Category("Vertex Data")]
        public byte EntryStride { get { return Data->_entryStride; } }
        [Category("Vertex Data")]
        public short NumVertices { get { return Data->_numVertices; } }
        [Category("Vertex Data")]
        public Vector3 EMin { get { return Data->_eMin; } }
        [Category("Vertex Data")]
        public Vector3 EMax { get { return Data->_eMax; } }
        [Category("Vertex Data")]
        public int Pad1 { get { return Data->_pad1; } }
        [Category("Vertex Data")]
        public int Pad2 { get { return Data->_pad2; } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
                _origSource.Length = _uncompSource.Length = TotalLen;

            return false;
        }
    }
}
