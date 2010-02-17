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

        //internal int _tableOffset, _groupOffset, _texTableOffset, _dataOffset;
        //internal ResourceNode[] _boneCache;
        private ModelLinker _linker;
        internal bool _hasTree, _hasMix, _hasOpa, _hasXlu;

        [Category("MDL0 Def")]
        public int Unknown1 { get { return _unk1; } }
        [Category("MDL0 Def")]
        public int Unknown2 { get { return _unk2; }}
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

        protected override int OnCalculateSize(bool force)
        {
            _linker = ModelLinker.Prepare(this);
            return ModelEncoder.CalcSize(_linker, force);
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

            if ((_name == null) && ((offset = header->StringOffset)!= 0))
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
            ModelDecoder.Decode(this);
        }

        //internal List<NodeRef> _nodeGroups = new List<NodeRef>();
        internal IMatrixProvider[] _nodes;
        //internal List<ResourceNode> _bones = new List<ResourceNode>();
        //internal List<TextureRef> _texRefs = new List<TextureRef>();

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (MDL0GroupNode n in Children)
                n.GetStrings(table);

            if (_hasTree) table.Add("NodeTree");
            if (_hasMix) table.Add("NodeMix");
            if (_hasOpa) table.Add("DrawOpa");
            if (_hasXlu) table.Add("DrawXlu");
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
            header->StringOffset = (stringTable[Name] + 4) - (byte*)header;

            //Post-process groups, using linker lists
            List<MDLResourceType> gList = ModelLinker.IndexBank[_version];
            foreach (MDL0GroupNode node in Children)
            {
                MDLResourceType type = (MDLResourceType)Enum.Parse(typeof(MDLResourceType), node.Name);
                if (((index = gList.IndexOf(type)) >= 0) && (type != MDLResourceType.Shaders))
                    node.PostProcess(dataAddress + offsets[index], stringTable);
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

            ResourceNode group;

            if (_renderPolygons)
            {
                ctx.glEnable(GLEnableCap.Lighting);
                ctx.glEnable(GLEnableCap.DepthTest);
                if (_renderPolygonsWireframe)
                    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
                else
                    ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

                if ((group = FindChild("Polygons", false)) != null)
                    foreach (MDL0PolygonNode poly in group.Children)
                        poly.Render(ctx);
            }

            if (_renderBones)
            {
                ctx.glDisable((uint)GLEnableCap.Lighting);
                ctx.glDisable((uint)GLEnableCap.DepthTest);
                ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);

                if ((group = FindChild("Bones", false)) != null)
                    foreach (MDL0BoneNode bone in group.Children)
                        bone.Render(ctx);
            }
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
            foreach (IMatrixProvider nr in _nodes)
                nr.CalcBase();
            foreach (IMatrixProvider nr in _nodes)
                nr.CalcWeighted();

            //Weight vertices
            if ((group = FindChild("Polygons", false)) != null)
                foreach (MDL0PolygonNode poly in group.Children)
                    poly.WeightVertices(_nodes);
        }

        #endregion

        internal static ResourceNode TryParse(DataSource source) { return ((MDL0Header*)source.Address)->_header._tag == MDL0Header.Tag ? new MDL0Node() : null; }
    }
}
