using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Collections.Generic;
using BrawlLib.Wii.Compression;
using System.IO;
using System.Drawing;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BRESNode : ARCEntryNode
    {
        internal BRESHeader* Header { get { return (BRESHeader*)WorkingUncompressed.Address; } }

        internal ROOTHeader* RootHeader { get { return Header->First; } }
        internal ResourceGroup* Group { get { return &RootHeader->_master; } }

        public override ResourceType ResourceType { get { return ResourceType.BRES; } }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Group;
            for (int i = 0; i < group->_numEntries; i++)
                new BRESGroupNode(new String((sbyte*)group + group->First[i]._stringOffset)).Initialize(this, (VoidPtr)group + group->First[i]._dataOffset, 0);
        }
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            return Group->_numEntries > 0;
        }

        public T CreateResource<T>() where T : BRESEntryNode
        {
            string groupName;
            if (typeof(T) == typeof(TEX0Node))
                groupName = "Textures(NW4R)";
            else if (typeof(T) == typeof(PLT0Node))
                groupName = "Palettes(NW4R)";
            else if (typeof(T) == typeof(MDL0Node))
                groupName = "3DModels(NW4R)";
            else if (typeof(T) == typeof(CHR0Node))
                groupName = "AnmChr(NW4R)";
            else if (typeof(T) == typeof(CLR0Node))
                groupName = "AnmClr(NW4R)";
            else if (typeof(T) == typeof(SRT0Node))
                groupName = "AnmTexSrt(NW4R)";
            else if (typeof(T) == typeof(SHP0Node))
                groupName = "AnmShp(NW4R)";
            else
                return null;

            BRESGroupNode group = null;
            foreach (BRESGroupNode node in Children)
                if (node.Name == groupName)
                {
                    group = node;
                    break;
                }

            if (group == null)
                AddChild(group = new BRESGroupNode(groupName));

            T n = Activator.CreateInstance<T>();
            n.Name = group.FindName();
            group.AddChild(n);

            return n;
        }
        public void ExportToFolder(string outFolder)
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            foreach (BRESGroupNode group in Children)
            {
                foreach (BRESEntryNode entry in group.Children)
                {
                    if (entry is TEX0Node)
                        entry.Export(Path.Combine(outFolder, entry.Name + ".png"));
                    else if (entry is MDL0Node)
                        entry.Export(Path.Combine(outFolder, entry.Name + ".mdl0"));
                }
            }
        }
        public void ReplaceFromFolder(string inFolder)
        {
            DirectoryInfo dir = new DirectoryInfo(inFolder);
            FileInfo[] files;
            foreach (BRESGroupNode group in Children)
            {
                foreach (BRESEntryNode entry in group.Children)
                {
                    //Find file name for entry
                    files = dir.GetFiles(entry.Name + ".*");
                    foreach (FileInfo info in files)
                    {
                        entry.Replace(info.FullName);
                        break;
                    }
                }
            }
        }

        private int _numEntries, _strOffset, _rootSize;
        StringTable _stringTable = new StringTable();
        protected override int OnCalculateSize(bool force)
        {
            int size = BRESHeader.Size;
            _rootSize = 0x20 + (Children.Count * 0x10);

            //Get entry count and data start
            _numEntries = 0;
            //Children.Sort(NodeComparer.Instance);
            foreach (BRESGroupNode n in Children)
            {
                //n.Children.Sort(NodeComparer.Instance);
                _rootSize += (n.Children.Count * 0x10) + 0x18;
                _numEntries += n.Children.Count;
            }
            size += _rootSize;

            //Get strings and advance entry offset
            _stringTable.Clear();
            foreach (BRESGroupNode n in Children)
            {
                _stringTable.Add(n.Name);
                foreach (BRESEntryNode c in n.Children)
                {
                    size = size.Align(c.DataAlign) + c.CalculateSize(force);
                    c.GetStrings(_stringTable);
                }
            }
            _strOffset = size = size.Align(4);

            size += _stringTable.GetTotalSize();

            return size.Align(0x80);
        }

        protected internal override void OnRebuild(VoidPtr address, int size, bool force)
        {
            BRESHeader* header = (BRESHeader*)address;
            *header = new BRESHeader(size, _numEntries + 1);

            ROOTHeader* rootHeader = header->First;
            *rootHeader = new ROOTHeader(_rootSize, Children.Count);

            ResourceGroup* pMaster = &rootHeader->_master;
            ResourceGroup* rGroup = (ResourceGroup*)pMaster->EndAddress;

            //Write string table
            _stringTable.WriteTable(address + _strOffset);

            VoidPtr dataAddr = (VoidPtr)rootHeader + _rootSize;

            int gIndex = 1;
            foreach (BRESGroupNode g in Children)
            {
                ResourceEntry.Build(pMaster, gIndex++, rGroup, (BRESString*)_stringTable[g.Name]);

                *rGroup = new ResourceGroup(g.Children.Count);
                ResourceEntry* nEntry = rGroup->First;

                int rIndex = 1;
                foreach (BRESEntryNode n in g.Children)
                {
                    //Align data
                    dataAddr = ((int)dataAddr).Align(n.DataAlign);

                    ResourceEntry.Build(rGroup, rIndex++, dataAddr, (BRESString*)_stringTable[n.Name]);

                    //Rebuild entry
                    int len = n._calcSize;
                    n.Rebuild(dataAddr, len, force);
                    n.PostProcess(address, dataAddr, len, _stringTable);
                    dataAddr += len;
                }
                g._changed = false;

                //Advance to next group
                rGroup = (ResourceGroup*)rGroup->EndAddress;
            }
            _stringTable.Clear();
        }

        internal static ResourceNode TryParse(DataSource source) { return ((BRESHeader*)source.Address)->_tag == BRESHeader.Tag ? new BRESNode() : null; }

    }

    public unsafe class BRESGroupNode : ResourceNode
    {
        internal ResourceGroup* Group { get { return (ResourceGroup*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.BRESGroup; } }

        [Browsable(false)]
        public override BrawlLib.Wii.Compression.CompressionType Compression
        {
            get { return base.Compression; }
            set { base.Compression = value; }
        }

        public BRESGroupNode() : base() { }
        public BRESGroupNode(string name) : base() { _name = name; }

        public override void RemoveChild(ResourceNode child)
        {
            if ((Children.Count == 1) && (Children.Contains(child)))
                Parent.RemoveChild(this);
            else
                base.RemoveChild(child);
        }

        protected override bool OnInitialize()
        {
            return Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                BRESCommonHeader* hdr = (BRESCommonHeader*)group->First[i].DataAddress;
                if (NodeFactory.FromAddress(this, hdr, hdr->_size) == null)
                    new BRESEntryNode().Initialize(this, hdr, hdr->_size);
            }
        }

    }

    public unsafe class BRESEntryNode : ResourceNode
    {
        internal BRESCommonHeader* CommonHeader { get { return (BRESCommonHeader*)WorkingSource.Address; } }

        [Browsable(false)]
        public virtual int DataAlign { get { return 4; } }

        [Browsable(false)]
        public BRESNode BRESNode { get { return ((_parent != null) && (_parent._parent is BRESNode)) ? _parent._parent as BRESNode : null; } }

        protected override bool OnInitialize()
        {
            SetSizeInternal(CommonHeader->_size);
            return false;
        }

        internal virtual void GetStrings(StringTable strings)
        {
            strings.Add(Name);
        }

        public override unsafe void Export(string outPath)
        {
            Rebuild();

            StringTable table = new StringTable();
            GetStrings(table);

            int dataLen = WorkingUncompressed.Length.Align(4);
            int size = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(size);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Memory.Move(map.Address, WorkingUncompressed.Address, (uint)WorkingUncompressed.Length);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(null, map.Address, WorkingUncompressed.Length, table);
                }
            }
            table.Clear();
        }

        internal protected virtual void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            BRESCommonHeader* header = (BRESCommonHeader*)dataAddress;

            if (bresAddress)
                header->_bresOffset = (int)bresAddress - (int)header;

            header->_size = dataLength;
        }
    }
}
