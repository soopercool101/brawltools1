using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Node : BRESEntryNode
    {
        //private SortedDictionary<int, Vec3[]> _vertexGroups;

        internal MDL0* Header { get { return (MDL0*)WorkingSource.Address; } }

        public override int DataAlign { get { return 0x20; } }
        //public override uint DataPad { get { return 0x4; } }

        [Category("MDL0 Data")]
        public int NumMeshes { get { return Header->_entry._numResources; } }
        [Category("MDL0 Def")]
        public uint Unknown1 { get { return Header->_modelDef._unk1; } }
        [Category("MDL0 Def")]
        public uint Unknown2 { get { return Header->_modelDef._unk2; } }
        [Category("MDL0 Def")]
        public uint Unknown3 { get { return Header->_modelDef._unk3; } }
        [Category("MDL0 Def")]
        public uint Unknown4 { get { return Header->_modelDef._unk4; } }
        [Category("MDL0 Def")]
        public uint Unknown5 { get { return Header->_modelDef._unk5; } }
        [Category("MDL0 Def")]
        public uint NumNodes { get { return Header->_modelDef._numNodes; } }
        [Category("MDL0 Def")]
        public uint Unknown7 { get { return Header->_modelDef._version; } }
        [Category("MDL0 Def")]
        public uint Unknown8 { get { return Header->_modelDef._unk6; } }
        [Category("MDL0 Def")]
        public uint DataOffset { get { return Header->_modelDef._dataOffset; } }
        [Category("MDL0 Def")]
        public BVec3 BoxMin { get { return Header->_modelDef._minExtents; } }
        [Category("MDL0 Def")]
        public BVec3 BoxMax { get { return Header->_modelDef._maxExtents; } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            return true;
        }

        protected override void OnPopulate()
        {
            for (int i = 0; i < 11; i++)
            {
                ResourceGroup* gPtr = Header->GetEntry(i);
                if (gPtr != null)
                    new MDL0GroupNode().Initialize(this, new DataSource(gPtr, 0), i);
            }
        }

        //public override void OnInit()
        //{
        //    base.OnInit();

        //    MDL0* header = _memoryBlock.Address;

        //    for (int i = 0; i < 11; i++)
        //    {
        //        ResourceGroup* gPtr = header->GetEntry(i);
        //        if (gPtr != null)
        //            new MDL0GroupNode().Initialize(this, gPtr, i);
        //    }

        //    //new MDL0OganizedNode().Initialize(this, null, "Organized");
        //}

        //public Vec3[] GetVertexGroup(int index)
        //{
        //    if (!_vertexGroups.ContainsKey(index))
        //        _vertexGroups[index] = VertexConverter.Decode((MDL0VertexData*)MDL0Header->VertexGroup->First[index].DataAddress);
        //    return _vertexGroups[index];
        //}

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (MDL0GroupNode n in Children)
                n.GetStrings(strings);
        }

        //public override void Write(FileStream stream, uint bresOffset, uint dataOffset, IDictionary<string, uint> strings)
        //{
        //    VoidPtr baseAddr = Marshal.AllocHGlobal((int)_memoryBlock.Length);
        //    try
        //    {
        //        Util.MoveMemory(baseAddr, _memoryBlock.Address, _memoryBlock.Length);

        //        MDL0* header = baseAddr;
        //        header->_entry._bresOffset = (int)bresOffset - (int)dataOffset;
        //        header->_stringOffset = strings[this.Text] - dataOffset;

        //        int index = 0;
        //        for (int i = 0; i < 11; i++)
        //        {
        //            //if (header->GroupOffsets[i] != 0)
        //            //{
        //            //    MDL0GroupNode n = Nodes[index++] as MDL0GroupNode;
        //            //    ResourceGroup* group = header->GetGroupAddress(i);
        //            //    uint gOffset = (uint)group - (uint)baseAddr + dataOffset;
        //            //    for (int x = 0, c = n.GetNodeCount(false); x < c; x++)
        //            //    {
        //            //        group->First[x]._stringOffset = strings[n.Nodes[x].Text] - gOffset;
        //            //    }
        //            //}
        //        }

        //        stream.Write(baseAddr, _memoryBlock.Length);
        //    }
        //    finally { Marshal.FreeHGlobal(baseAddr); }
        //}

        internal static ResourceNode TryParse(VoidPtr address) { return ((MDL0*)address)->_entry._tag == MDL0.Tag ? new MDL0Node() : null; }
    }

    //unsafe class MDL0OganizedNode : BaseNode
    //{

    //    public override void Initialize(object parent, FileView view, string name)
    //    {
    //        base.Initialize(parent, view, name);
    //        OnInit();
    //    }

    //    public override void OnInit()
    //    {
    //        MDL0* header = ((MDL0Node)Parent).MDL0Header;

    //        ResourceGroup* defGroup = header->InfoGroup;
    //        ResourceGroup* nodeGroup = header->BoneGroup;

    //        //find node definition
    //        int treeIndex = -1;
    //        while ((++treeIndex < defGroup->_numEntries) && (defGroup->First[treeIndex].GetName() != "NodeTree")) ;
    //        if (treeIndex >= defGroup->_numEntries)
    //            return;


    //        //Build tree
    //        MDL0BoneNode currentNode = null;
    //        for (MDL0DefEntry* entry = (MDL0DefEntry*)defGroup->First[treeIndex].DataAddress; entry->_type == 2; entry = entry->Next)
    //        {
    //            MDL0NodeType2* nPtr = entry->Type2Data;
    //            BaseNode parent;
    //            if ((currentNode == null) || ((parent = currentNode.FindParent(nPtr->_parentId)) == null))
    //                parent = this;

    //            currentNode = new MDL0BoneNode();
    //            currentNode.Initialize(parent, new MemoryBlock(&nodeGroup->First[nPtr->_index], 0), null);
    //        }
    //    }
    //}

    public unsafe class MDL0GroupNode : ResourceNode, IResourceGroupNode
    {
        private int _index;
        private int _mPrev, _mNext;

        internal ResourceGroup* Header { get { return (ResourceGroup*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Header; } }

        [Category("MDL0 Group")]
        public int MLeft { get { return _mPrev; } }
        [Category("MDL0 Group")]
        public int MRight { get { return _mNext; } }

        internal void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            foreach (MDL0EntryNode n in Children)
                n.GetStrings(strings);
        }

        internal void Initialize(ResourceNode parent, DataSource source, int index)
        {
            _index = index;
            base.Initialize(parent, source);
        }

        protected override bool OnInitialize()
        {
            switch (_index)
            {
                case 0: Name = "Definitions"; break;
                case 1: Name = "Bones"; break;
                case 2: Name = "Vertices"; break;
                case 3: Name = "Normals"; break;
                case 4: Name = "Colors"; break;
                case 5: Name = "UV Points"; break;
                case 6: Name = "Materials1"; break;
                case 7: Name = "Materials2"; break;
                case 8: Name = "Polygons"; break;
                case 9: Name = "Textures1"; break;
                case 10: Name = "Textures2"; break;
            }
            return Header->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            Type t;
            switch (_index)
            {
                case 0: { t = typeof(MDL0NodeEntry); break; }

                case 1:
                    {
                        MDL0* header = ((MDL0Node)Parent).Header;
                        ResourceGroup* defGroup = header->InfoGroup;
                        ResourceGroup* nodeGroup = header->BoneGroup;

                        //find node definition
                        int treeIndex = -1;
                        while ((++treeIndex < defGroup->_numEntries) && (defGroup->First[treeIndex].GetName() != "NodeTree")) ;
                        if (treeIndex >= defGroup->_numEntries)
                            return;


                        //Build tree
                        MDL0BoneNode currentNode = null;
                        for (MDL0DefEntry* entry = (MDL0DefEntry*)defGroup->First[treeIndex].DataAddress; entry->_type == 2; entry = entry->Next)
                        {
                            MDL0NodeType2* nPtr = entry->Type2Data;
                            ResourceNode parent;
                            if ((currentNode == null) || ((parent = currentNode.FindParent(nPtr->_parentId)) == null))
                                parent = this;

                            currentNode = new MDL0BoneNode();
                            currentNode.Initialize(parent, new DataSource(nodeGroup->First[nPtr->_index].DataAddress, 0), nPtr->_index);
                        }
                        return;

                        //t = typeof(MDL0BoneNode);
                        //break;
                    }
                case 2: { t = typeof(MDL0VertexNode); break; }
                case 3: { t = typeof(MDL0NormalNode); break; }
                case 4: { t = typeof(MDL0ColorNode); break; }
                case 5: { t = typeof(MDL0UVNode); break; }
                case 6: { t = typeof(MDL0Data7Node); break; }
                case 7: { t = typeof(MDL0Data8Node); break; }
                case 8: { t = typeof(MDL0PolygonNode); break; }
                case 9: { t = typeof(MDL0Data10Node); break; }
                case 10: { t = typeof(MDL0Data10Node); break; }
                default: return;
            }

            ResourceGroup* group = Header;
            for (int i = 0; i < group->_numEntries; i++)
                ((MDL0EntryNode)Activator.CreateInstance(t)).Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }
    }

    unsafe class MDL0EntryNode : ResourceEntryNode
    {
        internal virtual void GetStrings(IDictionary<string, VoidPtr> strings) { strings[Name] = 0; }
    }


    unsafe class MDL0NodeEntry : MDL0EntryNode
    {
        private List<object> _items = new List<object>();

        [Category("MDL0 Nodes")]//, Editor(typeof(EasyCollectionEditor), typeof(UITypeEditor))]
        public List<object> Items { get { return _items; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            VoidPtr addr = WorkingSource.Address;
            object n = null;
            while ((n = MDL0NodeClass.Create(ref addr)) != null)
                _items.Add(n);

            return false;
        }
    }

    unsafe class MDL0BoneNode : MDL0EntryNode, IResourceGroupNode
    {
        private List<string> _entries = new List<string>();

        internal MDL0Data2* Data { get { return (MDL0Data2*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return ((IResourceGroupNode)_parent).Group; } }

        private int _nIndex;

        [Category("Data2")]
        public string DataName { get { return Data->Name; } }

        [Category("Data2")]
        public uint HeaderLen { get { return Data->_headerLen; } }
        [Category("Data2")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Data2")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Data2")]
        public uint NodeIndex { get { return Data->_index; } }

        [Category("Data2")]
        public uint NodeId { get { return Data->_nodeId; } }
        [Category("Data2")]
        public uint Flags { get { return Data->_flags; } }
        [Category("Data2")]
        public uint Pad1 { get { return Data->_pad1; } }
        [Category("Data2")]
        public uint Pad2 { get { return Data->_pad2; } }

        [Category("Data2")]
        public BVec3 Scale { get { return Data->_scale; } }
        [Category("Data2")]
        public BVec3 Rotation { get { return Data->_rotation; } }
        [Category("Data2")]
        public BVec3 Translation { get { return Data->_translation; } }
        [Category("Data2")]
        public BVec3 BoxMin { get { return Data->_boxMin; } }
        [Category("Data2")]
        public BVec3 BoxMax { get { return Data->_boxMax; } }

        [Category("Data2")]
        public int ParentOffset { get { return Data->_parentOffset / 0xD0; } }
        [Category("Data2")]
        public int FirstChildOffset { get { return Data->_unk1 / 0xD0; } }
        [Category("Data2")]
        public int NextOffset { get { return Data->_unk2 / 0xD0; } }
        [Category("Data2")]
        public int PrevOffset { get { return Data->_nextOffset / 0xD0; } }
        [Category("Data2")]
        public int Part2Offset { get { return Data->_part2Offset; } }

        [Category("Data2")]
        public BVec3 V6 { get { return Data->_v6; } }
        [Category("Data2")]
        public BVec3 V7 { get { return Data->_v7; } }
        [Category("Data2")]
        public BVec3 V8 { get { return Data->_v8; } }
        [Category("Data2")]
        public BVec3 V9 { get { return Data->_v9; } }
        [Category("Data2")]
        public BVec3 VA { get { return Data->_va; } }
        [Category("Data2")]
        public BVec3 VB { get { return Data->_vb; } }
        [Category("Data2")]
        public BVec3 VC { get { return Data->_vc; } }
        [Category("Data2")]
        public BVec3 VD { get { return Data->_vd; } }

        [Category("Data2 Part2")]
        public List<string> Entries { get { return _entries; } }

        internal void Initialize(ResourceNode parent, DataSource source, int index)
        {
            _nIndex = index;
            base.Initialize(parent, source);
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (MDL0BoneNode n in Children)
                n.GetStrings(strings);
            foreach (string s in _entries)
                strings[s] = 0;
        }


        public MDL0BoneNode FindParent(int id)
        {
            if (NodeId == id)
                return this;

            int i = Index;
            if (i > 0)
                return ((MDL0BoneNode)_parent.Children[i - 1]).FindParent(id);

            if (_parent is MDL0BoneNode)
                return ((MDL0BoneNode)_parent).FindParent(id);

            return null;
        }

        protected override bool OnInitialize()
        {
            ResourceEntry* entry = &((IResourceGroupNode)_parent).Group->First[_nIndex];

            Name = entry->GetName();
            _id = entry->_id;
            _prev = entry->_prev;
            _next = entry->_next;

            if (Data->_part2Offset != 0)
            {
                MDL0Data7Part4* part4 = Data->Part2;
                if (part4 != null)
                {
                    ResourceGroup* group = part4->Group;
                    for (int i = 0; i < group->_numEntries; i++)
                    {
                        _entries.Add(group->First[i].GetName());
                    }
                }
            }
            return false;
        }
    }


    unsafe class MDL0VertexNode : MDL0EntryNode
    {
        internal MDL0VertexData* Data { get { return (MDL0VertexData*)WorkingSource.Address; } }
        private BVec3[] _vertices;

        [Category("Data3")]
        public uint TotalLen { get { return Data->_dataLen; } }
        [Category("Data3")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Data3")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("Data3")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Data3")]
        public int ID { get { return Data->_index; } }
        [Category("Data3")]
        public bool HasBoundingBox { get { return Data->_hasbox != 0; } }
        [Category("Data3")]
        public int DataType { get { return Data->_type; } }
        [Category("Data3")]
        public byte Divisor { get { return Data->_divisor; } }
        [Category("Data3")]
        public byte EntryStride { get { return Data->_entryStride; } }
        [Category("Data3")]
        public short NumVertices { get { return Data->_numVertices; } }
        [Category("Data3")]
        public BVec3 EMin { get { return Data->_eMin; } }
        [Category("Data3")]
        public BVec3 EMax { get { return Data->_eMax; } }
        [Category("Data3")]
        public int Pad1 { get { return Data->_pad1; } }
        [Category("Data3")]
        public int Pad2 { get { return Data->_pad2; } }

        //[SSBBBrowsable, Category("Vertex Data")]
        //public BVec3[] Vertices { get { return _vertices; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            //_vertices = new BVec3[NumVertices];
            //for (int i = 0; i < NumVertices; i++)
            //{
            //    _vertices[i] = Data->Entries[i];
            //}

            return false;
        }


        //[NodeAction("&Export")]
        //public override bool OnExport()
        //{
        //    using (SaveFileDialog dlg = new SaveFileDialog())
        //    {
        //        dlg.Filter = "OBJ file (*.obj)|*.obj";
        //        dlg.FileName = this.Name;
        //        if (dlg.ShowDialog() == DialogResult.OK)
        //        {
        //            using (FileStream stream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, FileOptions.RandomAccess))
        //            using (StreamWriter writer = new StreamWriter(stream))
        //            {
        //                writer.WriteLine("#OBJ definition for " + this.Name);
        //                writer.WriteLine("#Vertices");

        //                for (int i = 0; i < NumVertices; i++)
        //                    writer.WriteLine(String.Format("v {0} {1} {2}", (float)_vertices[i]._x, (float)_vertices[i]._y, (float)_vertices[i]._z));

        //                writer.WriteLine("#Faces");
        //                for (int i = 1; i <= NumVertices;)
        //                    writer.WriteLine(String.Format("f {0} {1} {2}", i++, i++, i++));

        //            }
        //        }
        //    }
        //    return false;
        //}

    }

    unsafe class MDL0NormalNode : MDL0EntryNode
    {
        internal MDL0NormalData* Data { get { return (MDL0NormalData*)WorkingSource.Address; } }
        private BVec3[] _normals;

        [Category("Normal Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Normal Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Normal Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("Normal Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Normal Data")]
        public int ID { get { return Data->_index; } }
        [Category("Normal Data")]
        public bool HasBox { get { return Data->_hasbox != 0; } }
        [ Category("Normal Data")]
        public int Format { get { return Data->_type; } }
        [Category("Normal Data")]
        public int Divisor { get { return Data->_divisor; } }
        [Category("Normal Data")]
        public int EntryStride { get { return Data->_entryStride; } }
        [Category("Normal Data")]
        public short NumEntries { get { return Data->_numVertices; } }

        //[SSBBBrowsable, Category("Normal Data")]
        //public BVec3[] Normals { get { return _normals; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            //_normals = new BVec3[NumEntries];
            //for (int i = 0; i < NumEntries; i++)
            //{
            //    _normals[i] = Data->Entries[i];
            //}
            return false;
        }
    }

    unsafe class MDL0ColorNode : MDL0EntryNode
    {
        internal MDL0ColorData* Data { get { return (MDL0ColorData*)WorkingSource.Address; } }
        private RGBAPixel[] _colors;

        [Category("Color Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Color Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Color Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("Color Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Color Data")]
        public int ID { get { return Data->_index; } }
        [Category("Color Data")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("Color Data")]
        public int Format { get { return Data->_format; } }
        [Category("Color Data")]
        public byte Unknown2 { get { return Data->_unk2; } }
        [Category("Color Data")]
        public byte Unknown3 { get { return Data->_unk3; } }
        [Category("Color Data")]
        public short NumEntries { get { return Data->_numEntries; } }

        //[SSBBBrowsable, Category("Color Data")]
        //public RGBAPixel[] Colors { get { return _colors; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            //_colors = new RGBAPixel[NumEntries];
            //for (int i = 0; i < NumEntries; i++)
            //{
            //    _colors[i] = (RGBAPixel)Data->Entries[i];
            //}
            return false;
        }
    }

    unsafe class MDL0UVNode : MDL0EntryNode
    {
        internal MDL0UVData* Data { get { return (MDL0UVData*)WorkingSource.Address; } }
        private UVPoint[] _uvPoints;

        [Category("UV Data")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("UV Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("UV Data")]
        public int DataOffset { get { return Data->_dataOffset; } }
        [Category("UV Data")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("UV Data")]
        public int ID { get { return Data->_index; } }
        [Category("UV Data")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("UV Data")]
        public int Format { get { return Data->_format; } }
        [Category("UV Data")]
        public byte Unknown2 { get { return Data->_unk2; } }
        [Category("UV Data")]
        public byte Unknown3 { get { return Data->_unk3; } }
        [Category("UV Data")]
        public short NumEntries { get { return Data->_numEntries; } }

        [Category("UV Data")]
        public UVPoint Min { get { return Data->_min; } }
        [Category("UV Data")]
        public UVPoint Max { get { return Data->_max; } }

        [Category("UV Data")]
        public int Pad1 { get { return Data->_pad1; } }
        [Category("UV Data")]
        public int Pad2 { get { return Data->_pad2; } }
        [Category("UV Data")]
        public int Pad3 { get { return Data->_pad3; } }
        [Category("UV Data")]
        public int Pad4 { get { return Data->_pad4; } }

        //[SSBBBrowsable, Category("UV Data")]
        //public UVPoint[] UVPoints { get { return _uvPoints; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            //_uvPoints = new UVPoint[NumEntries];
            //for (int i = 0; i < NumEntries; i++)
            //{
            //    _uvPoints[i] = Data->Entries[i];
            //}
            return false;
        }
    }

    unsafe class MDL0Data7Node : MDL0EntryNode
    {
        internal MDL0Data7* Data { get { return (MDL0Data7*)WorkingSource.Address; } }
        internal List<string> _part4Entries = new List<string>();

        [Category("Data7")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Data7")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Data7")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Data7")]
        public int ID { get { return Data->_index; } }
        [Category("Data7")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("Data7")]
        public byte Flag1 { get { return Data->_flag1; } }
        [Category("Data7")]
        public byte Flag2 { get { return Data->_flag2; } }
        [Category("Data7")]
        public byte Flag3 { get { return Data->_flag3; } }
        [Category("Data7")]
        public byte Flag4 { get { return Data->_flag4; } }
        [Category("Data7")]
        public int Type { get { return Data->_type; } }
        [Category("Data7")]
        public byte Flag5 { get { return Data->_flag5; } }
        [Category("Data7")]
        public byte Flag6 { get { return Data->_flag6; } }

        [Category("Data7")]
        public short Unknown2 { get { return Data->_unk2; } }
        [Category("Data7")]
        public int Unknown3 { get { return Data->_unk3; } }
        [Category("Data7")]
        public int Unknown4 { get { return Data->_unk4; } }

        [Category("Data7")]
        public int Part2Offset { get { return Data->_part2Offset; } }

        [Category("Data7")]
        public int NumLinks { get { return Data->_numLinks; } }
        [Category("Data7")]
        public int Part3Offset { get { return Data->_part3Offset; } }
        [Category("Data7")]
        public int Part4Offset { get { return Data->_part4Offset; } }
        [Category("Data7")]
        public int Part5Offset { get { return Data->_part5Offset; } }

        [Category("Data7")]
        public int Unknown6 { get { return Data->_unk6; } }

        [Category("Data7 Part4")]
        public List<string> Part4Entries { get { return _part4Entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            MDL0Data7Part4* part4 = Data->Part4;
            if (part4 != null)
            {
                ResourceGroup* group = part4->Group;
                for (int i = 0; i < group->_numEntries; i++)
                {
                    _part4Entries.Add(group->First[i].GetName());
                }
            }
            return false;
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (string s in _part4Entries)
                strings[s] = 0;
        }
    }


    unsafe class MDL0Data8Node : MDL0EntryNode
    {
        internal MDL0Data8* Data { get { return (MDL0Data8*)WorkingSource.Address; } }

    }


    unsafe class MDL0PolygonNode : MDL0EntryNode//, IPolygon
    {
        //private List<SSBBPrimitive> _primitives;
        internal MDL0Polygon* Data { get { return (MDL0Polygon*)WorkingSource.Address; } }

        [Category("Polygon Data")]
        public int TotalLen { get { return Data->_totalLength; } }
        [Category("Polygon Data")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Polygon Data")]
        public int Unknown1 { get { return Data->_unk1; } }

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

        //[NodeAction("&View")]
        //public virtual void OnView()
        //{
        //    using (PolygonViewer viewer = new PolygonViewer(this))
        //    {
        //        viewer.ShowDialog();
        //    }
        //}

        //[NodeAction("&Export")]
        //public override bool OnExport()
        //{
        //    using (SaveFileDialog dlg = new SaveFileDialog())
        //    {
        //        dlg.Filter = "OBJ file (*.obj)|*.obj";
        //        dlg.FileName = this.Name;
        //        if (dlg.ShowDialog() == DialogResult.OK)
        //        {
        //        }
        //    }
        //    return false;
        //}

        //private void ExtractData()
        //{
        //    //Get vertices
        //    Vector3[] vertices = VertexConverter.Decode(Data->VertexData);
        //    //Get normals
        //    Vector3[] normals = VertexConverter.DecodeNormals(Data->NormalData);
        //    //Get colors
        //    ARGBPixel[] colors1 = (Data->_colorId1 >= 0) ? VertexConverter.DecodeColors(Data->ColorData1) : null;
        //    ARGBPixel[] colors2 = (Data->_colorId2 >= 0) ? VertexConverter.DecodeColors(Data->ColorData2) : null;
        //    //Get uvs

        //    EntrySize es = new EntrySize(Data->_flags);


        //    VoidPtr ptr = Data->PrimitiveData;
        //    //Parse data
        //    int type;
        //    SSBBPrimitive prim;
        //    _primitives = new List<SSBBPrimitive>();
        //    while ((type = *(byte*)ptr) != 0)
        //    {
        //        if (type == 0x30 || type == 0x28 || type == 0x20)
        //        {
        //            ptr += 5;
        //            continue;
        //        }

        //        if((prim = SSBBPrimitive.FromData(ref ptr, es, vertices, normals, colors1)) == null)
        //            break;
        //        _primitives.Add(prim);
        //    }
        //}

        #region IPolygon Members

        //public int NumPrimitives
        //{
        //    get 
        //    {
        //        if (_primitives == null)
        //            ExtractData();
        //        return _primitives.Count; 
        //    }
        //}

        //public ICollection<SSBBPrimitive> Primitives
        //{
        //    get 
        //    {
        //        if (_primitives == null)
        //            ExtractData();
        //        return _primitives; 
        //    }
        //}

        #endregion
    }


    unsafe class MDL0Data10Node : MDL0EntryNode
    {
        internal MDL0Data10* Data { get { return (MDL0Data10*)WorkingSource.Address; } }
        internal MDL0Data10Entry[] _entries;

        [Category("Data10")]
        public int NumEntries { get { return Data->_numEntries; } }
        [Category("Data10")]
        public MDL0Data10Entry[] Entries { get { return _entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _entries = new MDL0Data10Entry[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                _entries[i] = Data->Entries[i];
            }

            return false;
        }
    }
}
