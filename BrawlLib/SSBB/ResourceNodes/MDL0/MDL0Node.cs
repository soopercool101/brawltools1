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
    public unsafe class MDL0Node : BRESEntryNode, IRenderedObject
    {
        internal MDL0Header* Header { get { return (MDL0Header*)WorkingUncompressed.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0; } }

        public override int DataAlign { get { return 0x20; } }

        //Changing the version will change the conversion. Should we rebuild all on a version change?
        internal int _version;
        internal int _unk1, _unk2, _unk3, _unk4, _unk5;
        internal int _numVertices, _numFaces, _numNodes;
        internal Vector3 _min, _max;

        internal ModelLinker _linker;
        internal AssetStorage _assets;
        internal bool _hasTree, _hasMix, _hasOpa, _hasXlu;

        [Category("MDL0 Def")]
        public int Unknown1 { get { return _unk1; } }
        [Category("MDL0 Def")]
        public int Unknown2 { get { return _unk2; } }
        [Category("MDL0 Def")]
        public int NumVertices { get { return _numVertices; } }
        [Category("MDL0 Def")]
        public int NumFaces { get { return _numFaces; } }
        [Category("MDL0 Def")]
        public int Unknown3 { get { return _unk3; } }
        [Category("MDL0 Def")]
        public int NumNodes { get { return _numNodes; } }
        [Category("MDL0 Def")]
        public int Unknown4 { get { return _unk4; } }
        [Category("MDL0 Def")]
        public int Unknown5 { get { return _unk5; } }
        [Category("MDL0 Def")]
        public Vector3 BoxMin { get { return _min; } }
        [Category("MDL0 Def")]
        public Vector3 BoxMax { get { return _max; } }

        //internal List<ResourceNode> _bones;
        //private List<MDL0PolygonNode> _polygons;
        //public T FindResource<T>(int index) where T : ResourceNode
        //{
        //    ResourceNode group = null;
        //    if (typeof(T) == typeof(MDL0PolygonNode))
        //        group = FindChild("Polygons", false) as ResourceNode;
        //    else if (typeof(T) == typeof(MDL0VertexNode))
        //        group = FindChild("Vertices", false) as ResourceNode;
        //    else if (typeof(T) == typeof(MDL0NormalNode))
        //        group = FindChild("Normals", false) as ResourceNode;
        //    else if (typeof(T) == typeof(MDL0UVNode))
        //        group = FindChild("UV Points", false) as ResourceNode;
        //    else if (typeof(T) == typeof(MDL0ColorNode))
        //        group = FindChild("Colors", false) as ResourceNode;
        //    else if (typeof(T) == typeof(MDL0BoneNode))
        //    {
        //        int count = -1;
        //        if ((group = FindChild("Bones", false) as MDL0GroupNode) != null)
        //            return FindBone(group, index, ref count) as T;
        //    }

        //    if (group == null)
        //        return null;

        //    if (group.Children.Count <= index)
        //        return null;

        //    return group.Children[index] as T;
        //}
        //private ResourceNode FindBone(ResourceNode node, int index, ref int count)
        //{
        //    if (count++ >= index)
        //        return node;

        //    foreach (ResourceNode n in node.Children)
        //        if ((node = FindBone(n, index, ref count)) != null)
        //            return node;

        //    return null;
        //}

        #region Immediate accessors
        internal MDL0GroupNode _boneGroup, _matGroup, _shadGroup, _polyGroup;//, _texGroup;
        internal List<ResourceNode> _boneList, _matList, _shadList, _polyList;//, _texList;
        [Browsable(false)]
        public ResourceNode BoneGroup { get { return _boneGroup; } }
        [Browsable(false)]
        public ResourceNode MaterialGroup { get { return _matGroup; } }
        [Browsable(false)]
        public ResourceNode ShaderGroup { get { return _shadGroup; } }
        [Browsable(false)]
        public ResourceNode PolygonGroup { get { return _polyGroup; } }
        //[Browsable(false)]
        //public ResourceNode TextureGroup { get { return _texGroup; } }
        [Browsable(false)]
        public List<ResourceNode> BoneList { get { return _boneList; } }
        [Browsable(false)]
        public List<ResourceNode> MaterialList { get { return _matList; } }
        [Browsable(false)]
        public List<ResourceNode> ShaderList { get { return _shadList; } }
        [Browsable(false)]
        public List<ResourceNode> PolygonList { get { return _polyList; } }
        //[Browsable(false)]
        //public List<ResourceNode> TextureList { get { return _texList; } }
        #endregion

        public TextureManager _textures = new TextureManager();

        //public MDL0TextureNode FindOrCreateTexture(string name)
        //{
        //    //texGroup will not be null during initialization, so AddChild will not occur!
        //    if (_texGroup == null)
        //        AddChild(_texGroup = new MDL0GroupNode(MDLResourceType.Textures), false);
        //    else
        //        foreach (MDL0TextureNode n in _texGroup._children)
        //            if (n._name == name)
        //                return n;

        //    MDL0TextureNode node = new MDL0TextureNode() { _name = name };
        //    _texGroup.AddChild(node, false);

        //    return node;
        //}
        public override void AddChild(ResourceNode child, bool change)
        {
            if (child is MDL0GroupNode)
                LinkGroup(child as MDL0GroupNode);
            base.AddChild(child, change);
        }
        private void LinkGroup(MDL0GroupNode group)
        {
            switch (group._type)
            {
                case MDLResourceType.Bones: { _boneGroup = group; _boneList = group._children; break; }
                case MDLResourceType.Materials: { _matGroup = group; _matList = group._children; break; }
                case MDLResourceType.Shaders: { _shadGroup = group; _shadList = group._children; break; }
                case MDLResourceType.Polygons: { _polyGroup = group; _polyList = group._children; break; }
                //case MDLResourceType.Textures: { _texGroup = group; _texList = group._children; break; }
            }
        }
        private void UnlinkGroup(MDL0GroupNode group)
        {
            switch (group._type)
            {
                case MDLResourceType.Bones: { _boneGroup = null; _boneList = null; break; }
                case MDLResourceType.Materials: { _matGroup = null; _matList = null; break; }
                case MDLResourceType.Shaders: { _shadGroup = null; _shadList = null; break; }
                case MDLResourceType.Polygons: { _polyGroup = null; _polyList = null; break; }
                //case MDLResourceType.Textures: { _texGroup = null; _texList = null; break; }
            }
        }
        internal void InitGroups()
        {
            LinkGroup(new MDL0GroupNode(MDLResourceType.Bones));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Materials));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Shaders));
            LinkGroup(new MDL0GroupNode(MDLResourceType.Polygons));
            //LinkGroup(new MDL0GroupNode(MDLResourceType.Textures));

            _boneGroup._parent = this;
            _matGroup._parent = this;
            _shadGroup._parent = this;
            _polyGroup._parent = this;
            //_texGroup._parent = this;
        }
        internal void CleanGroups()
        {
            if (_boneList.Count > 0)
                _children.Add(_boneGroup);
            else
                UnlinkGroup(_boneGroup);

            if (_matList.Count > 0)
                _children.Add(_matGroup);
            else
                UnlinkGroup(_matGroup);

            if (_shadList.Count > 0)
                _children.Add(_shadGroup);
            else
                UnlinkGroup(_shadGroup);

            if (_polyList.Count > 0)
                _children.Add(_polyGroup);
            else
                UnlinkGroup(_polyGroup);

            //if (_texList.Count > 0)
            //    _children.Add(_texGroup);
            //else
            //    UnlinkGroup(_texGroup);
        }

        public override void RemoveChild(ResourceNode child)
        {
            if (child is MDL0GroupNode)
                UnlinkGroup(child as MDL0GroupNode);
            base.RemoveChild(child);
        }

        protected override int OnCalculateSize(bool force)
        {
            //Clean and sort influence list
            _influences.Clean();
            _influences.Sort();

            //Clean and sort texture list
            _textures.Clean();
            _textures.Sort();

            _linker = ModelLinker.Prepare(this);
            return ModelEncoder.CalcSize(_linker);
        }
        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ModelEncoder.Build(_linker, (MDL0Header*)address, length, force);
            _linker = null;
            //ModelEncoder.Build(this, (MDL0Header*)address, length, force);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            MDL0Header* header = Header;
            int offset;

            if ((_name == null) && ((offset = header->StringOffset) != 0))
                _name = new String((sbyte*)header + offset);

            MDL0Props* props = header->Properties;

            _version = header->_header._version;
            _unk1 = props->_unk1;
            _unk2 = props->_unk2;
            _numVertices = props->_numVertices;
            _numFaces = props->_numFaces;
            _unk3 = props->_unk3;
            _numNodes = props->_numNodes;
            _unk4 = props->_unk4;
            _unk5 = props->_unk5;
            _min = props->_minExtents;
            _max = props->_maxExtents;

            return true;
        }

        protected override void OnPopulate()
        {
            InitGroups();
            _linker = new ModelLinker(Header);
            _assets = new AssetStorage(_linker);
            try
            {
                //Set def flags
                _hasMix = _hasOpa = _hasTree = _hasXlu = false;
                if (_linker.Defs != null)
                    foreach (ResourcePair p in *_linker.Defs)
                        if (p.Name == "NodeTree") _hasTree = true;
                        else if (p.Name == "NodeMix") _hasMix = true;
                        else if (p.Name == "DrawOpa") _hasOpa = true;
                        else if (p.Name == "DrawXlu") _hasXlu = true;

                _boneGroup.Parse(this);
                _matGroup.Parse(this);
                _shadGroup.Parse(this);
                _polyGroup.Parse(this);
                //Texture group doesn't need parsing
                //It's only used as a name reference/link and will be re-created on build.

                //Eliminate influences with no references?
            }
            finally
            {
                //Clean up!
                _assets.Dispose();
                _assets = null;
                _linker = null;
                CleanGroups();
            }
        }

        internal InfluenceManager _influences = new InfluenceManager();
        //internal List<NodeRef> _nodeGroups = new List<NodeRef>();
        //internal IMatrixProvider[] _nodes;
        //internal List<ResourceNode> _bones = new List<ResourceNode>();
        //internal List<TextureRef> _texRefs = new List<TextureRef>();

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (MDL0GroupNode n in Children)
                n.GetStrings(table);

            //Add def names
            if (_hasTree) table.Add("NodeTree");
            if (_hasMix) table.Add("NodeMix");
            if (_hasOpa) table.Add("DrawOpa");
            if (_hasXlu) table.Add("DrawXlu");

            //Add texture names (handled by materials?)
            //foreach (TextureRef t in _textures._textures)
            //    table.Add(t.Name);
        }

        public override unsafe void Export(string outPath)
        {
            if (outPath.EndsWith(".dae"))
            {
                //Model model = GetModel();
                Collada.Serialize(this, outPath);
            }
            else
                base.Export(outPath);
        }

        public static MDL0Node FromFile(string path)
        {
            string ext = Path.GetExtension(path);

            if (path.EndsWith(".mdl", StringComparison.OrdinalIgnoreCase))
                return NodeFactory.FromFile(null, path) as MDL0Node;
            if (path.EndsWith(".dae", StringComparison.OrdinalIgnoreCase))
                return Collada.ImportModel(path);
            //else if (string.Equals(ext, "fbx", StringComparison.OrdinalIgnoreCase))
            //{
            //}
            //else if (string.Equals(ext, "blender", StringComparison.OrdinalIgnoreCase))
            //{
            //}

            throw new NotSupportedException("The file extension specified is not of a supported model type.");
        }

        //protected override int OnCalculateSize(bool force)
        //{
        //    int size = 0;
        //    //Definitions

        //    foreach (ResourceNode node in Children)
        //    {
        //        size += node.CalculateSize(force);
        //    }

        //    return size;
        //}

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            MDL0Header* header = (MDL0Header*)dataAddress;
            ResourceGroup* pGroup, sGroup;
            ResourceEntry* pEntry, sEntry;
            bint* offsets = header->Offsets;
            int index, sIndex;

            //Model name
            header->StringOffset = (int)((byte*)stringTable[Name] + 4 - (byte*)header);

            //Post-process groups, using linker lists
            List<MDLResourceType> gList = ModelLinker.IndexBank[_version];
            foreach (MDL0GroupNode node in Children)
            {
                MDLResourceType type = (MDLResourceType)Enum.Parse(typeof(MDLResourceType), node.Name);
                if (((index = gList.IndexOf(type)) >= 0) && (type != MDLResourceType.Shaders))
                    node.PostProcess(dataAddress, dataAddress + offsets[index], stringTable);
            }

            //Post-process definitions
            index = gList.IndexOf(MDLResourceType.Defs);
            pGroup = (ResourceGroup*)(dataAddress + offsets[index]);
            pGroup->_first = new ResourceEntry(0xFFFF, 0, 0, 0);
            pEntry = &pGroup->_first + 1;
            index = 1;
            if (_hasTree)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["NodeTree"]);
            if (_hasMix)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["NodeMix"]);
            if (_hasOpa)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["DrawOpa"]);
            if (_hasXlu)
                ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (pEntry++)->_dataOffset, (BRESString*)stringTable["DrawXlu"]);

            //Link shader names using material list
            index = offsets[gList.IndexOf(MDLResourceType.Materials)];
            sIndex = offsets[gList.IndexOf(MDLResourceType.Shaders)];
            if ((index > 0) && (sIndex > 0))
            {
                pGroup = (ResourceGroup*)(dataAddress + index);
                sGroup = (ResourceGroup*)(dataAddress + sIndex);
                pEntry = &pGroup->_first + 1;
                sEntry = &sGroup->_first + 1;

                sGroup->_first = new ResourceEntry(0xFFFF, 0, 0, 0);
                index = pGroup->_numEntries;
                for (int i = 1; i <= index; i++)
                    ResourceEntry.Build(sGroup, i, (byte*)sGroup + (sEntry++)->_dataOffset, (BRESString*)((byte*)pGroup + (pEntry++)->_stringOffset - 4));
            }
        }

        #region Rendering

        internal bool _bound = false;
        internal bool _renderPolygons = true;
        internal bool _renderPolygonsWireframe = false;
        internal bool _renderBones = true;
        internal bool _visible = true;

        public void Attach(GLContext context)
        {
            _visible = true;
            ApplyCHR(null, 0);
            foreach (MDL0GroupNode g in Children)
                g.Bind(context);
        }

        public void Detach(GLContext context)
        {
            foreach (MDL0GroupNode g in Children)
                g.Unbind(context);
        }

        public void Refesh(GLContext context)
        {
            ResourceNode n = FindChild("Textures", false);
            if (n != null)
                foreach (MDL0TextureNode t in n.Children)
                    t.Reload();
        }

        public void Render(GLContext ctx)
        {
            if (!_visible)
                return;

            if (_renderPolygons)
            {
                ctx.glEnable(GLEnableCap.Lighting);
                ctx.glEnable(GLEnableCap.DepthTest);
                if (_renderPolygonsWireframe)
                    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
                else
                    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

                if (_polyList != null)
                    foreach (MDL0PolygonNode poly in _polyList)
                        poly.Render(ctx);
            }

            if (_renderBones)
            {
                ctx.glDisable((uint)GLEnableCap.Lighting);
                ctx.glDisable((uint)GLEnableCap.DepthTest);
                ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

                if (_boneList != null)
                    foreach (MDL0BoneNode bone in _boneList)
                        bone.Render(ctx);
            }
        }

        internal void ApplyCHR(CHR0Node node, int index)
        {
            //Transform bones
            if (_boneList != null)
            {
                foreach (MDL0BoneNode b in _boneList)
                    b.ApplyCHR0(node, index);
                foreach (MDL0BoneNode b in _boneList)
                    b.RecalcFrameState();
            }

            //Transform nodes
            //foreach (IMatrixProvider nr in _nodes)
            //    nr.CalcBase();
            //foreach (IMatrixProvider nr in _nodes)
            //    nr.CalcWeighted();

            foreach (Influence inf in _influences._influences)
                inf.CalcMatrix();

            if (_polyList != null)
                foreach (MDL0PolygonNode poly in _polyList)
                    poly.WeightVertices();

            //Weight vertices
            //if ((group = FindChild("Polygons", false)) != null)
            //    foreach (MDL0PolygonNode poly in group.Children)
            //        poly.WeightVertices(_nodes);
        }

        #endregion

        internal static ResourceNode TryParse(DataSource source) { return ((MDL0Header*)source.Address)->_header._tag == MDL0Header.Tag ? new MDL0Node() : null; }
    }
}
