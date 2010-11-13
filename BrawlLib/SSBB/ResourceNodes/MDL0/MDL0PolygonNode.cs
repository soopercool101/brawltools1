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

        //[Category("Polygon Data")]
        //public int TotalLen { get { return Header->_totalLength; } }
        //[Category("Polygon Data")]
        //public int MDL0Offset { get { return Header->_mdl0Offset; } }
        //[Category("Polygon Data")]
        //public int NodeId { get { return Header->_nodeId; } }

        //[Category("Polygon Data")]
        //public MDL0ElementFlags ElementFlags { get { return Header->_flags; } }
        //[SSBBBrowsable, Category("Polygon Data"), TypeConverter(typeof(IntToHex))]
        //public int UnkFlags1 { get { return Data->_unkFlags1; } }
        //[Category("Polygon Data")]
        //public int UnkFlags2 { get { return Header->_unkFlags2; } }

        //[Category("Polygon Data")]
        //public int DefSize { get { return Header->_defSize; } }
        //[Category("Polygon Data")]
        //public int DefFlags { get { return Header->_defFlags; } }
        //[Category("Polygon Data")]
        //public int DefOffset { get { return Header->_defOffset; } }

        //[Category("Polygon Data")]
        //public int DataLength1 { get { return Header->_dataLen1; } }
        //[Category("Polygon Data")]
        //public int DataLength2 { get { return Header->_dataLen2; } }
        //[Category("Polygon Data")]
        //public int DataOffset { get { return Header->_dataOffset; } }

        //[Category("Polygon Data")]
        //public int Unknown2 { get { return Header->_unk2; } }
        //[Category("Polygon Data")]
        //public int Unknown3 { get { return Header->_unk3; } }
        //[Category("Polygon Data")]
        //public int StringOffset { get { return Header->_stringOffset; } }
        //[Category("Polygon Data")]
        //public int ItemId { get { return Header->_index; } }
        //[Category("Polygon Data")]
        //public int NumVertices { get { return Header->_numVertices; } }
        //[Category("Polygon Data")]
        //public int Faces { get { return Header->_numFaces; } }

        //[Category("Polygon Data")]
        //public int Part1Offset { get { return Header->_part1Offset; } }

        //Cannot use MDL0BoneNode because weighted nodes are valid targets!
        //#region Bone linkage
        //internal MDL0BoneNode _singleBind;
        //[Browsable(false)]
        //public MDL0BoneNode SingleBind
        //{
        //    get { return _singleBind; }
        //    set
        //    {
        //        if (_singleBind == value)
        //            return;
        //        if (_singleBind != null)
        //            _singleBind._polygons.Remove(this);
        //        if ((_singleBind = value) != null)
        //            _singleBind._polygons.Add(this);
        //        Model.SignalPropertyChange();
        //    }
        //}
        //public string Bone
        //{
        //    get { return _singleBind == null ? null : _singleBind._name; }
        //    set
        //    {
        //        if (String.IsNullOrEmpty(value))
        //            SingleBind = null;
        //        else
        //        {
        //            MDL0BoneNode bone = Model.FindChild("Bones", false).FindChild(value, true) as MDL0BoneNode;
        //            if (bone != null)
        //                SingleBind = bone;
        //        }
        //    }
        //}
        //#endregion
        internal short[] _elementIndices = new short[16];

        internal IMatrixNode _singleBind;

        public IMatrixNode Influence
        {
            get { return _singleBind; }
            set
            {
                if (_singleBind == value)
                    return;

                if (_singleBind != null)
                    _singleBind.ReferenceCount--;

                if ((_singleBind = value) != null)
                    _singleBind.ReferenceCount++;
            }
        }

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


        #region Node linkages

        internal List<Influence> _nodeList = new List<Influence>();

        #endregion

        internal bool _render = true;
        internal PrimitiveManager _manager;

        public override void Dispose()
        {
            if (_manager != null)
            { _manager.Dispose(); _manager = null; }

            base.Dispose();
        }

        protected override bool OnInitialize()
        {
            MDL0Polygon* header = Header;
            int nodeId = header->_nodeId;

            ModelLinker linker = Model._linker;

            //Attach single bind. Doesn't have to be bone node.
            if (nodeId >= 0)
                Influence = linker.NodeCache[nodeId];

            if (header->_defFlags != 0x80)
            {
                Console.WriteLine("OMG!");
            }
            if (header->_defSize != 0xE0)
            {
                Console.WriteLine("OMG!");
            }
            if (header->_dataLen1 != header->_dataLen2)
            {
                Console.WriteLine("DataLen deviation!");
            }
            if (header->_unk3 != 0)
            {
                Console.WriteLine("OMG!");
            }

            if (header != null)
            {
                //Conditional name assignment
                if ((_name == null) && (header->_stringOffset != 0))
                    _name = header->ResourceString;

                //Create primitive manager
                if (_parent != null)
                    _manager = new PrimitiveManager(header, Model._assets, linker.NodeCache);

            }

            return false;
        }

        private int[] _nodeCache;
        //This should be done after node indices have been asssigned
        protected override int OnCalculateSize(bool force)
        {
            int size = (int)MDL0Polygon.Size;

            //May be best to ensure this is done after assets have been merged.

            //Create node table
            HashSet<int> nodes = new HashSet<int>();
            foreach (Vertex3 v in _manager._vertices)
                nodes.Add(v._influence.NodeIndex);

            //Copy to array and sort
            _nodeCache = new int[nodes.Count];
            nodes.CopyTo(_nodeCache);
            Array.Sort(_nodeCache);

            //Add table length
            size += _nodeCache.Length * 4 + 4;
            size.Align(0x20);

            //Add def length
            size += 0xE0;

            //Combine primitives
            //Merge vertices
            //Group faces based on shared sides

            //Build display list

            //Texture matrices (0x30) start at 0x78, max 11
            //Pos matrices (0x20) start at 0x00, max 10
            //Normal matrices (0x28) start at 0x400, max 10

            return base.OnCalculateSize(force);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            base.OnRebuild(address, length, force);
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".obj"))
            {
                //Wavefront.Serialize(outPath, this);
            }
            else
                base.Export(outPath);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Polygon* header = (MDL0Polygon*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;
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


            _manager.PrepareStream(ctx);

            if (_singleBind != null)
            {
                ctx.glPushMatrix();
                Matrix m = _singleBind.Matrix;
                ctx.glMultMatrix((float*)&m);
            }

            if ((_material != null) && (_material._children.Count > 0))
            {
                foreach (MDL0MaterialRefNode mr in _material._children)
                {
                    if (!mr._texture.Enabled)
                        continue;

                    mr.Bind(ctx);
                    _manager.Render(ctx, 0);
                }
            }
            else
                _manager.Render(ctx, -1);

            _manager.DetachStreams(ctx);

            if (_singleBind != null)
                ctx.glPopMatrix();
        }

        //internal void SetFrame(CHR0EntryNode n, int index)
        //{
        //}

        internal void WeightVertices()
        {
            _manager.Weight();
        }

        internal override void Bind(GLContext ctx)
        {
            _render = true;
        }
        internal override void Unbind(GLContext ctx)
        {
            //foreach (Primitive prim in _primitives)
            //    prim.Dispose();
        }

        #endregion
    }
}
