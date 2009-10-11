using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    unsafe class MDL0PolygonNode : MDL0EntryNode//, IPolygon
    {
        //private List<SSBBPrimitive> _primitives;
        internal MDL0Polygon* Data { get { return (MDL0Polygon*)WorkingSource.Address; } }

        [Category("Polygon Data")]
        public int TotalLen { get { return Data->_totalLength; } }
        [Category("Polygon Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Polygon Data")]
        public int NodeId { get { return Data->_nodeId; } }

        [Category("Polygon Data")]
        public MDL0ElementFlags ElementFlags { get { return Data->_flags; } }
        //[SSBBBrowsable, Category("Polygon Data"), TypeConverter(typeof(IntToHex))]
        //public int UnkFlags1 { get { return Data->_unkFlags1; } }
        [Category("Polygon Data")]
        public int UnkFlags2 { get { return Data->_unkFlags2; } }

        [Category("Polygon Data")]
        public int DefSize { get { return Data->_defSize; } }
        [Category("Polygon Data")]
        public int DefFlags { get { return Data->_defFlags; } }
        [Category("Polygon Data")]
        public int DefOffset { get { return Data->_defOffset; } }

        [Category("Polygon Data")]
        public int DataLength1 { get { return Data->_dataLen1; } }
        [Category("Polygon Data")]
        public int DataLength2 { get { return Data->_dataLen2; } }
        [Category("Polygon Data")]
        public int DataOffset { get { return Data->_dataOffset; } }

        [Category("Polygon Data")]
        public int Unknown2 { get { return Data->_unk2; } }
        [Category("Polygon Data")]
        public int Unknown3 { get { return Data->_unk3; } }
        [Category("Polygon Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Polygon Data")]
        public int ItemId { get { return Data->_index; } }
        [Category("Polygon Data")]
        public int Unknown4 { get { return Data->_unk4; } }
        [Category("Polygon Data")]
        public int Faces { get { return Data->_numFaces; } }

        [Category("Polygon Data")]
        public int VertexSet { get { return Data->_vertexId; } }
        [Category("Polygon Data")]
        public int NormalSet { get { return Data->_normalId; } }
        [Category("Polygon Data")]
        public int ColorSet1 { get { return Data->_colorId1; } }

        [Category("Polygon Data")]
        public int ColorSet2 { get { return Data->_colorId2; } }

        [Category("Polygon Data")]
        public int Part1Offset { get { return Data->_part1Offset; } }

        //private List<MDL0MaterialNode> _materialNodes = new List<MDL0MaterialNode>();
        //public List<MDL0MaterialNode> MaterialNodes
        //{
        //    get { return _materialNodes; }
        //}

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
                _origSource.Length = _uncompSource.Length = Data->_totalLength;

            return false;
        }

        //public MDL0VertexNode GetVertexNode() { return VertexSet >= 0 ? _parent._parent.FindChild("Vertices", false).Children[VertexSet] as MDL0VertexNode : null; }
        //public MDL0NormalNode GetNormalNode() { return NormalSet >= 0 ? _parent._parent.FindChild("Normals", false).Children[NormalSet] as MDL0NormalNode : null; }
        //public MDL0ColorNode GetColorNode(int index) { return Data->_colorId1 >= 0 ? _parent._parent.FindChild("Colors", false).Children[Data->_colorId1] as MDL0ColorNode : null; }
        //public MDL0UVNode GetUVNode(int index) { return Data->_ >= 0 ? _parent._parent.FindChild("Colors", false).Children[Data->_colorId1] as MDL0ColorNode : null; }
    }
}
