using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Imaging;
using BrawlLib.Modeling;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Node : BRESEntryNode
    {
        internal MDL0* Header { get { return (MDL0*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0; } }

        public override int DataAlign { get { return 0x20; } }

        private int _unk1, _unk2, _unk3, _unk4, _version;
        private int _numVertices, _numFaces, _numNodes;
        private Vector3 _min, _max;

        [Category("MDL0 Def")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        [Category("MDL0 Def")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; } }
        [Category("MDL0 Def")]
        public int NumVertices { get { return _numVertices; } set { _numVertices = value; } }
        [Category("MDL0 Def")]
        public int NumFaces { get { return _numFaces; } set { _numFaces = value; } }
        [Category("MDL0 Def")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; } }
        [Category("MDL0 Def")]
        public int NumNodes { get { return _numNodes; } set { _numNodes = value; } }
        [Category("MDL0 Def")]
        public int Version { get { return _version; } set { _version = value; } }
        [Category("MDL0 Def")]
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; } }
        [Category("MDL0 Def")]
        public BVec3 BoxMin { get { return _min; } set { _min = value; } }
        [Category("MDL0 Def")]
        public BVec3 BoxMax { get { return _max; } set { _max = value; } }

        //internal List<ResourceNode> _bones;
        //private List<MDL0PolygonNode> _polygons;
        public T FindResource<T>(int index) where T : ResourceNode
        {
            ResourceNode group = null;
            if (typeof(T) == typeof(MDL0PolygonNode))
                group = FindChild("Polygons", false) as ResourceNode;
            else if (typeof(T) == typeof(MDL0VertexNode))
                group = FindChild("Vertices", false) as ResourceNode;
            else if (typeof(T) == typeof(MDL0NormalNode))
                group = FindChild("Normals", false) as ResourceNode;
            else if (typeof(T) == typeof(MDL0UVNode))
                group = FindChild("UV Points", false) as ResourceNode;
            else if (typeof(T) == typeof(MDL0ColorNode))
                group = FindChild("Colors", false) as ResourceNode;
            else if (typeof(T) == typeof(MDL0BoneNode))
            {
                int count = -1;
                if ((group = FindChild("Bones", false) as MDL0GroupNode) != null)
                    return FindBone(group, index, ref count) as T;
            }

            if (group == null)
                return null;

            if (group.Children.Count <= index)
                return null;

            return group.Children[index] as T;
        }
        private ResourceNode FindBone(ResourceNode node, int index, ref int count)
        {
            if (count++ >= index)
                return node;

            foreach (ResourceNode n in node.Children)
                if ((node = FindBone(n, index, ref count)) != null)
                    return node;

            return null;
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _unk1 = Header->_modelDef._unk1;
            _unk2 = Header->_modelDef._unk2;
            _numVertices = Header->_modelDef._numVertices;
            _numFaces = Header->_modelDef._numFaces;
            _unk3 = Header->_modelDef._unk3;
            _numNodes = Header->_modelDef._numNodes;
            _version = Header->_modelDef._version;
            _unk4 = Header->_modelDef._unk4;
            _min = Header->_modelDef._minExtents;
            _max = Header->_modelDef._maxExtents;

            return true;
        }

        private List<IMatrixProvider> _nodes = new List<IMatrixProvider>();

        internal List<ResourceNode> _bones = new List<ResourceNode>();

        protected override void OnPopulate()
        {
            ResourceGroup* group;
            for (int i = 0; i < 11; i++)
                if ((group = Header->GetEntry(i)) != null)
                    new MDL0GroupNode().Initialize(this, new DataSource(group, 0), i);

            MDL0GroupNode bNode= FindChild("Bones", false) as MDL0GroupNode;
            MDL0GroupNode pNode = FindChild("Polygons", false) as MDL0GroupNode;
            MDL0GroupNode mNode = FindChild("Materials1", false) as MDL0GroupNode;
            MDL0DefNode nodeMix = FindChild("Definitions/NodeMix", false) as MDL0DefNode;
            MDL0DefNode drawOpa = FindChild("Definitions/DrawOpa", false) as MDL0DefNode;
            MDL0DefNode drawXlu = FindChild("Definitions/DrawXlu", false) as MDL0DefNode;

            IMatrixProvider[] nodeCache = new IMatrixProvider[NumNodes];

            //Pull out bones
            //foreach (MDL0BoneNode bone in _boneCache)
            //    nodeCache[bone.NodeId] = bone;
            if (bNode != null)
            {
                _bones = bNode.Children;
                foreach (MDL0BoneNode b in bNode._nodeCache)
                    nodeCache[b.NodeId] = new NodeRef(b);

                //Pull out node groups
                if (nodeMix != null)
                    foreach (object o in nodeMix.Items)
                        if (o is MDL0Node3Class)
                        {
                            MDL0Node3Class d = o as MDL0Node3Class;
                            NodeRef nref = new NodeRef();

                            foreach (MDL0NodeType3Entry e in d._entries)
                                nref._entries.Add(new NodeWeight(nodeCache[e._id], e._value));

                            nodeCache[d._id] = nref;
                        }

                //Attach opaque textures
                if (drawOpa != null)
                {
                    foreach (object o in drawOpa.Items)
                        if (o is MDL0NodeType4)
                        {
                            MDL0NodeType4 d = (MDL0NodeType4)o;
                            MDL0PolygonNode poly = pNode.Children[d._polygonIndex] as MDL0PolygonNode;
                            poly._material = mNode.Children[d._materialIndex] as MDL0MaterialNode;
                        }
                }

                //Attach transparent textures
                //Parse entry 4?
                if (drawXlu != null)
                {
                    foreach (object o in drawXlu.Items)
                        if (o is MDL0NodeType4)
                        {
                            MDL0NodeType4 d = (MDL0NodeType4)o;
                            MDL0PolygonNode poly = pNode.Children[d._polygonIndex] as MDL0PolygonNode;
                            poly._material = mNode.Children[d._materialIndex] as MDL0MaterialNode;
                        }
                }

                bNode._nodeCache = null;
            }


            //Link polygons to nodes
            if ((bNode = FindChild("Polygons", false) as MDL0GroupNode) != null)
                foreach (MDL0PolygonNode n in bNode.Children)
                    if (n.NodeId >= 0)
                        n._singleBind = nodeCache[n.NodeId];


            //Clear caches
            //_boneCache.Clear();

            _nodes = new List<IMatrixProvider>(nodeCache);
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (MDL0GroupNode n in Children)
                n.GetStrings(table);
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".dae"))
            {
                //Model model = GetModel();
                //Collada.Serialize(new object[] { model }, outPath);
            }
            else
                base.Export(outPath);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            MDL0* header = (MDL0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            foreach (MDL0GroupNode node in Children)
            {
                VoidPtr addr = dataAddress + header->Offsets[node._index];
                node.PostProcess(addr, stringTable);
            }
        }

        #region Rendering

        internal void Render(GLContext ctx)
        {
            ResourceNode group;

            ctx.glDisable((uint)GLEnableCap.Lighting);
            ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);

            //render bones
            if ((group = FindChild("Bones", false)) != null)
                foreach (MDL0BoneNode bone in group.Children)
                    bone.Render(ctx);

            ctx.glEnable(GLEnableCap.Lighting);
            ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);
            ctx.glColor(1.0f, 1.0f, 1.0f, 1.0f);

            //render polygons
            if ((group = FindChild("Polygons", false)) != null)
                foreach (MDL0PolygonNode poly in group.Children)
                    poly.Render(ctx);
        }

        internal void ApplyCHR(CHR0Node node, int index)
        {
            //Transform bones
            ResourceNode group = FindChild("Bones", false);
            if (group != null)
            {
                foreach (MDL0BoneNode b in group.Children)
                    b.ApplyCHR0(node, index);
            }

            //Transform nodes
            foreach (NodeRef nr in _nodes)
                nr.CalcBase();
            foreach (NodeRef nr in _nodes)
                nr.CalcWeighted();

            //Weight vertices
            if ((group = FindChild("Polygons", false)) != null)
                foreach (MDL0PolygonNode poly in group.Children)
                    poly.WeightVertices(_nodes);
        }

        #endregion

        internal static ResourceNode TryParse(DataSource source) { return ((MDL0*)source.Address)->_entry._tag == MDL0.Tag ? new MDL0Node() : null; }
    }
}
