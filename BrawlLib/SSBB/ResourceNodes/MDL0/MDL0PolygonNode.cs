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
        //protected override int DataLength { get { return Header->_totalLength; } }

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
        public int Part1Offset { get { return Header->_part1Offset; } }

        #region Bone linkage
        internal MDL0BoneNode _singleBind;
        [Browsable(false)]
        public MDL0BoneNode SingleBind
        {
            get { return _singleBind; }
            set
            {
                if (_singleBind == value)
                    return;
                if (_singleBind != null)
                    _singleBind._polygons.Remove(this);
                if ((_singleBind = value) != null)
                    _singleBind._polygons.Add(this);
                Model.SignalPropertyChange();
            }
        }
        public string Bone
        {
            get { return _singleBind == null ? null : _singleBind._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SingleBind = null;
                else
                {
                    MDL0BoneNode bone = Model.FindChild("Bones", false).FindChild(value, true) as MDL0BoneNode;
                    if (bone != null)
                        SingleBind = bone;
                }
            }
        }
        #endregion

        #region Material linkage
        internal MDL0MaterialNode _material;
        [Browsable(false)]
        public MDL0MaterialNode MaterialNode
        {
            get { return _material; }
            set
            {
                if (_material == value)
                    return;
                if (_material != null)
                    _material._polygons.Remove(this);
                if ((_material = value) != null)
                    _material._polygons.Add(this);
                Model.SignalPropertyChange();
            }
        }
        public string Material
        {
            get { return _material == null ? null : _material._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    MaterialNode = null;
                else
                {
                    MDL0MaterialNode node = Model.FindChild(String.Format("Materials/{0}", value), false) as MDL0MaterialNode;
                    if (node != null)
                        MaterialNode = node;
                }
            }
        }
        #endregion

        #region Vertex Linkage
        internal MDL0VertexNode _vertexNode;
        [Browsable(false)]
        public MDL0VertexNode VertexNode
        {
            get { return _vertexNode; }
            set
            {
                if (_vertexNode == value)
                    return;
                if (_vertexNode != null)
                    _vertexNode._polygons.Remove(this);
                if ((_vertexNode = value) != null)
                    _vertexNode._polygons.Add(this);
                Model.SignalPropertyChange();
            }
        }
        public string Vertices
        {
            get { return _vertexNode == null ? null : _vertexNode._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    VertexNode = null;
                else
                {
                    MDL0VertexNode node = Model.FindChild(String.Format("Vertices/{0}", value), false) as MDL0VertexNode;
                    if (node != null)
                        VertexNode = node;
                }
            }
        }
        #endregion

        #region Normal linkage
        internal MDL0NormalNode _normalNode;
        [Browsable(false)]
        public MDL0NormalNode NormalNode
        {
            get { return _normalNode; }
            set
            {
                if (_normalNode == value)
                    return;
                if (_normalNode != null)
                    _normalNode._polygons.Remove(this);
                if ((_normalNode = value) != null)
                    _normalNode._polygons.Add(this);
                Model.SignalPropertyChange();
            }
        }
        public string Normals
        {
            get { return _normalNode == null ? null : _normalNode._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    NormalNode = null;
                else
                {
                    MDL0NormalNode node = Model.FindChild(String.Format("Normals/{0}", value), false) as MDL0NormalNode;
                    if (node != null)
                        NormalNode = node;
                }
            }
        }
        #endregion

        #region Color linkage
        internal MDL0ColorNode[] _colorSet = new MDL0ColorNode[2];
        public MDL0ColorNode GetColorNode(int index) { return _colorSet[index]; }
        public void SetColorNode(int index, string name)
        {
            if (String.IsNullOrEmpty(name))
                SetColorNode(index, (MDL0ColorNode)null);
            else
            {
                MDL0ColorNode node = Model.FindChild(String.Format("Colors/{0}", name), false) as MDL0ColorNode;
                if (node != null)
                    SetColorNode(index, node);
            }
        }
        public void SetColorNode(int index, MDL0ColorNode node)
        {
            MDL0ColorNode old = _colorSet[index];
            if (old == node)
                return;
            if (old != null)
                old._polygons.Remove(this);
            if ((_colorSet[index] = node) != null)
                node._polygons.Add(this);
            Model.SignalPropertyChange();
        }
        public string Color0 { get { return _colorSet[0] == null ? null : _colorSet[0]._name; } set { SetColorNode(0, value); } }
        public string Color1 { get { return _colorSet[1] == null ? null : _colorSet[1]._name; } set { SetColorNode(1, value); } }
        #endregion

        #region UV linkage
        internal MDL0UVNode[] _uvSet = new MDL0UVNode[8];
        public MDL0UVNode GetUVNode(int index) { return _uvSet[index]; }
        public void SetUVNode(int index, string name)
        {
            if (String.IsNullOrEmpty(name))
                SetUVNode(index, (MDL0UVNode)null);
            else
            {
                MDL0UVNode node = Model.FindChild(String.Format("UVs/{0}", name), false) as MDL0UVNode;
                if (node != null)
                    SetUVNode(index, node);
            }
        }
        public void SetUVNode(int index, MDL0UVNode node)
        {
            MDL0UVNode old = _uvSet[index];
            if (old == node)
                return;
            if (old != null)
                old._polygons.Remove(this);
            if ((_uvSet[index] = node) != null)
                node._polygons.Add(this);
            Model.SignalPropertyChange();
        }
        public string UV0 { get { return _uvSet[0] == null ? null : _uvSet[0]._name; } set { SetUVNode(0, value); } }
        public string UV1 { get { return _uvSet[1] == null ? null : _uvSet[1]._name; } set { SetUVNode(1, value); } }
        public string UV2 { get { return _uvSet[2] == null ? null : _uvSet[2]._name; } set { SetUVNode(2, value); } }
        public string UV3 { get { return _uvSet[3] == null ? null : _uvSet[3]._name; } set { SetUVNode(3, value); } }
        public string UV4 { get { return _uvSet[4] == null ? null : _uvSet[4]._name; } set { SetUVNode(4, value); } }
        public string UV5 { get { return _uvSet[5] == null ? null : _uvSet[5]._name; } set { SetUVNode(5, value); } }
        public string UV6 { get { return _uvSet[6] == null ? null : _uvSet[6]._name; } set { SetUVNode(6, value); } }
        public string UV7 { get { return _uvSet[7] == null ? null : _uvSet[7]._name; } set { SetUVNode(7, value); } }
        #endregion

        internal bool _render = true;

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
            //base.OnInitialize();

            MDL0Polygon* header = Header;

            if ((_name == null) && (header->_stringOffset != 0))
                _name = header->ResourceString;

            //Link nodes
            //if (header->_vertexId >= 0)
            //    _vertexNode = ((MDL0Node)_parent._parent).FindResource<MDL0VertexNode>(header->_vertexId);
            //if (header->_normalId >= 0)
            //    _normalNode = ((MDL0Node)_parent._parent).FindResource<MDL0NormalNode>(header->_normalId);

            //int id;
            //for (int i = 0; i < 2; i++)
            //    if ((id = header->ColorIds[i]) >= 0)
            //        _colorSet[i] = ((MDL0Node)_parent._parent).FindResource<MDL0ColorNode>(id);

            //for (int i = 0; i < 8; i++)
            //    if ((id = header->UVIds[i]) >= 0)
            //        _uvSet[i] = ((MDL0Node)_parent._parent).FindResource<MDL0UVNode>(id);


            return false;
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".obj"))
            {
                Wavefront.Serialize(outPath, this);
            }
            else
                base.Export(outPath);
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

            //if (_wireframe)
            //    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
            //else
            //    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

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
                if (_material.Children.Count == 0)
                {
                    ctx.glDisable((uint)GLEnableCap.Texture2D);
                    foreach (Primitive prim in Primitives)
                    {
                        prim.PreparePointers(ctx);
                        prim.Render(ctx, -1);
                    }
                }
                else
                {
                    ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                    ctx.glEnable(GLEnableCap.Texture2D);
                    foreach (MDL0MaterialRefNode mr in _material.Children)
                    {
                        //if ((mr._layerId1 == 0) || (!mr._textureReference.Enabled))
                        //    continue;
                        if (!mr._texture.Enabled)
                            continue;

                        mr.Bind(ctx);
                        foreach (Primitive prim in Primitives)
                        {
                            prim.PreparePointers(ctx);
                            prim.Render(ctx, 0);
                        }
                    }
                    ctx.glDisable((uint)GLEnableCap.Texture2D);
                    ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                }
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

        internal void WeightVertices(IMatrixProvider[] nodes)
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
