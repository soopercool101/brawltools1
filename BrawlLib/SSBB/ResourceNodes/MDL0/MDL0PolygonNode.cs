using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;
using System.Collections.Generic;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0PolygonNode : MDL0EntryNode//, IPolygon
    {
        //private List<SSBBPrimitive> _primitives;
        internal MDL0Polygon* Header { get { return (MDL0Polygon*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_totalLength; } }

        [Category("Polygon Data")]
        public int TotalLen { get { return Header->_totalLength; } }
        [Category("Polygon Data")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Polygon Data")]
        public int NodeId { get { return Header->_nodeId; } }

        [Category("Polygon Data")]
        public MDL0ElementFlags ElementFlags { get { return Header->_flags; } }
        //[SSBBBrowsable, Category("Polygon Data"), TypeConverter(typeof(IntToHex))]
        //public int UnkFlags1 { get { return Data->_unkFlags1; } }
        [Category("Polygon Data")]
        public int UnkFlags2 { get { return Header->_unkFlags2; } }

        [Category("Polygon Data")]
        public int DefSize { get { return Header->_defSize; } }
        [Category("Polygon Data")]
        public int DefFlags { get { return Header->_defFlags; } }
        [Category("Polygon Data")]
        public int DefOffset { get { return Header->_defOffset; } }

        [Category("Polygon Data")]
        public int DataLength1 { get { return Header->_dataLen1; } }
        [Category("Polygon Data")]
        public int DataLength2 { get { return Header->_dataLen2; } }
        [Category("Polygon Data")]
        public int DataOffset { get { return Header->_dataOffset; } }

        [Category("Polygon Data")]
        public int Unknown2 { get { return Header->_unk2; } }
        [Category("Polygon Data")]
        public int Unknown3 { get { return Header->_unk3; } }
        [Category("Polygon Data")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Polygon Data")]
        public int ItemId { get { return Header->_index; } }
        [Category("Polygon Data")]
        public int Unknown4 { get { return Header->_unk4; } }
        [Category("Polygon Data")]
        public int Faces { get { return Header->_numFaces; } }

        [Category("Polygon Data")]
        public int VertexSet { get { return Header->_vertexId; } }
        [Category("Polygon Data")]
        public int NormalSet { get { return Header->_normalId; } }
        //[Category("Polygon Data")]
        //public int ColorSet1 { get { return Header->_colorId1; } }

        //[Category("Polygon Data")]
        //public int ColorSet2 { get { return Header->_colorId2; } }

        [Category("Polygon Data")]
        public int Part1Offset { get { return Header->_part1Offset; } }

        //private List<MDL0MaterialNode> _materialNodes = new List<MDL0MaterialNode>();
        //public List<MDL0MaterialNode> MaterialNodes
        //{
        //    get { return _materialNodes; }
        //}

        internal IMatrixProvider _singleBind;

        internal MDL0MaterialNode _material;

        internal MDL0VertexNode _vertexNode;
        internal MDL0NormalNode _normalNode;
        internal MDL0ColorNode[] _colorSet = new MDL0ColorNode[2];
        internal MDL0UVNode[] _uvSet = new MDL0UVNode[8];

        //public MDL0VertexNode VertexNode { get { return _vertexNode; } }

        internal bool _render = true;
        internal bool _wireframe = false;

        private List<Primitive> _primitives;
        public List<Primitive> Primitives
        {
            get { return _primitives == null ? _primitives = ModelConverter.ExtractPrimitives(Header) : _primitives; }
            set { _primitives = value; SignalPropertyChange(); }
        }

        public override void Dispose()
        {
            if (_primitives != null)
                foreach (Primitive prim in _primitives)
                    prim.Dispose();
            base.Dispose();
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            //Link nodes
            if (Header->_vertexId >= 0)
                _vertexNode = ((MDL0Node)_parent._parent).FindResource<MDL0VertexNode>(Header->_vertexId);
            if (Header->_normalId >= 0)
                _normalNode = ((MDL0Node)_parent._parent).FindResource<MDL0NormalNode>(Header->_normalId);

            int id;
            for (int i = 0; i < 2; i++)
                if ((id = Header->ColorIds[i]) >= 0)
                    _colorSet[i] = ((MDL0Node)_parent._parent).FindResource<MDL0ColorNode>(id);

            for (int i = 0; i < 8; i++)
                if ((id = Header->UVIds[i]) >= 0)
                    _uvSet[i] = ((MDL0Node)_parent._parent).FindResource<MDL0UVNode>(id);


            return false;
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Polygon* header = (MDL0Polygon*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        #region Rendering
        internal void Render(GLContext ctx)
        {
            if (!_render)
                return;

            if (_wireframe)
                ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
            else
                ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

            ctx.glPushMatrix();

            //Enable arrays
            ctx.glEnableClientState(GLArrayType.VERTEX_ARRAY);

            if (_normalNode != null)
                ctx.glEnableClientState(GLArrayType.NORMAL_ARRAY);

            if (_colorSet[0] != null)
                ctx.glEnableClientState(GLArrayType.COLOR_ARRAY);

            if (_singleBind != null)
            {
                Matrix m = _singleBind.FrameMatrix;
                ctx.glMultMatrix((float*)&m);
            }

            if (_material != null)
            {
                ctx.glEnable(GLEnableCap.Texture2D);
                ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                foreach (MDL0MaterialRefNode mr in _material.Children)
                {
                    if ((mr._layerId1 == 0) || (!mr._textureReference.Enabled))
                        continue;

                    mr.Bind(ctx);
                    foreach (Primitive prim in Primitives)
                    {
                        prim.PreparePointers(ctx);
                        prim.Render(ctx, mr._layerId1);
                    }
                }
                ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                ctx.glDisable((uint)GLEnableCap.Texture2D);
            }
            else
            {
                foreach (Primitive prim in Primitives)
                {
                    prim.PreparePointers(ctx);
                    prim.Render(ctx, 0);
                }
            }


            if (_normalNode != null)
                ctx.glDisableClientState(GLArrayType.NORMAL_ARRAY);

            if (_colorSet[0] != null)
                ctx.glDisableClientState(GLArrayType.COLOR_ARRAY);

            ctx.glDisableClientState(GLArrayType.VERTEX_ARRAY);

            ctx.glPopMatrix();
        }

        //internal void SetFrame(CHR0EntryNode n, int index)
        //{
        //}

        internal void WeightVertices(List<IMatrixProvider> nodes)
        {
            foreach (Primitive prim in Primitives)
                prim.Precalc(this, nodes);
        }

        internal override void Bind(GLContext ctx)
        {
            _render = true;
        }
        internal override void Unbind(GLContext ctx)
        {
            foreach (Primitive prim in _primitives)
                prim.Dispose();
        }

        #endregion
    }
}
