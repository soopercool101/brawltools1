using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Collections.Generic;
using BrawlLib.Wii.Compression;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BRESNode : ARCEntryNode, IResourceGroupNode
    {
        internal BRESHeader* Header { get { return (BRESHeader*)WorkingSource.Address; } }

        internal ROOTHeader* RootHeader { get { return Header->First; } }
        internal ResourceGroup* Group { get { return &RootHeader->_master; } }
        ResourceGroup* IResourceGroupNode.Group { get { return &RootHeader->_master; } }

        public override ResourceType ResourceType { get { return ResourceType.BRES; } }

        private short _gPrev, _gNext, _gId = -1;

        [Category("Resource Group")]
        public short GroupId { get { return _gId; } set { _gId = value; } }
        [Category("Resource Group")]
        public short GroupPrev { get { return _gPrev; } set { _gPrev = value; } }
        [Category("Resource Group")]
        public short GroupNext { get { return _gNext; } set { _gNext = value; } }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Group;
            for (int i = 0; i < group->_numEntries; i++)
                new BRESGroupNode().Initialize(this, group->First[i].DataAddress, 0);
        }
        protected override bool OnInitialize()
        {
            base.OnInitialize();

            ResourceGroup* group = &Header->First->_master;
            _gPrev = group->_first._prev;
            _gNext = group->_first._next;
            _gId = group->_first._id;

            return group->_numEntries != 0;
        }

        //public ResourceNode GetResource(string group, string name)
        //{
        //    foreach (ResourceNode n in Children)
        //        if (n.Name == group)
        //            foreach (ResourceNode c in n.Children)
        //                if (c.Name == name)
        //                    return c;
        //    return null;
        //}

        private int _numEntries, _strOffset, _rootSize;
        private SortedDictionary<string, VoidPtr> _stringTable = new SortedDictionary<string, VoidPtr>(StringComparer.Ordinal);
        protected override int OnCalculateSize(bool force)
        {
            int size = BRESHeader.Size;
            _rootSize = 0x20 + (Children.Count * 0x10);

            //Get entry count and data start
            _numEntries = 0;
            foreach (BRESGroupNode n in Children)
            {
                _rootSize += (n.Children.Count * 0x10) + 0x18;
                _numEntries += n.Children.Count;
            }
            size += _rootSize;

            //Get strings and advance entry offset
            _stringTable.Clear();
            foreach (BRESGroupNode n in Children)
            {
                _stringTable[n.Name] = 0;
                foreach (BRESEntryNode c in n.Children)
                {
                    size = size.Align(c.DataAlign) + c.CalculateSize(force);
                    c.GetStrings(_stringTable);
                }
            }
            _strOffset = size = size.Align(4);
            foreach (string s in _stringTable.Keys)
                size += (s.Length + 5).Align(4);

            return size.Align(0x80);
        }

        protected internal override void OnRebuild(VoidPtr address, int size, bool force)
        {
            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = new DataSource(address, size);

            BRESString* pStr = (BRESString*)(address + _strOffset);

            string[] strings = new string[_stringTable.Count];
            _stringTable.Keys.CopyTo(strings, 0);
            foreach (string s in strings)
            {
                _stringTable[s] = pStr->Data;
                pStr->Value = s;
                pStr = pStr->Next;
            }

            *Header = new BRESHeader(size, _numEntries + 1);
            *RootHeader = new ROOTHeader(_rootSize, Children.Count);

            VoidPtr dataAddr = (VoidPtr)RootHeader + _rootSize;

            ResourceGroup* pMaster = &RootHeader->_master;
            ResourceGroup* rGroup = (ResourceGroup*)pMaster->EndAddress;

            pMaster->_first = new ResourceEntry(_gId, _gPrev, _gNext, 0);

            ResourceEntry* gEntry = pMaster->First;
            foreach (BRESGroupNode g in Children)
            {
                //Set group entry
                *gEntry++ = new ResourceEntry(g.EntryId, g.EntryPrev, g.EntryNext, (int)rGroup - (int)pMaster, (int)_stringTable[g.Name] - (int)pMaster);

                //Initialize group and index entry
                *rGroup = new ResourceGroup(g.Children.Count);
                rGroup->_first = new ResourceEntry(g.GID, g.GPrev, g.GNext, 0);

                ResourceEntry* nEntry = rGroup->First;
                foreach (BRESEntryNode n in g.Children)
                {
                    //Align data
                    dataAddr = ((int)dataAddr).Align(n.DataAlign);

                    //Set entry data
                    *nEntry++ = new ResourceEntry(n.EntryId, n.EntryPrev, n.EntryNext, (int)dataAddr - (int)rGroup, (int)_stringTable[n.Name] - (int)rGroup);

                    //Rebuild entry
                    int len = n._calcSize;
                    n.Rebuild(dataAddr, len, force);
                    n.OnAfterRebuild(_stringTable);
                    dataAddr += len;
                }
                g.HasChanged = false;

                //Advance to next group
                rGroup = (ResourceGroup*)rGroup->EndAddress;
            }
            _stringTable.Clear();
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((BRESHeader*)address)->_tag == BRESHeader.Tag ? new BRESNode() : null; }

    }

    public unsafe class BRESGroupNode : ResourceEntryNode, IResourceGroupNode
    {
        internal ResourceGroup* Group { get { return (ResourceGroup*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return (ResourceGroup*)WorkingSource.Address; } }

        [Browsable(false)]
        public override BrawlLib.Wii.Compression.CompressionType Compression
        {
            get { return base.Compression; }
            set { base.Compression = value; }
        }

        short _gPrev, _gNext, _gId = -1;
        [Category("Resource Group")]
        public short GPrev { get { return _gPrev; } set { _gPrev = value; } }
        [Category("Resource Group")]
        public short GNext { get { return _gNext; } set { _gNext = value; } }
        [Category("Resource Group")]
        public short GID { get { return _gId; } set { _gId = value; } }

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

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            ResourceGroup* group = Group;
            _gPrev = group->_first._prev;
            _gNext = group->_first._next;
            _gId = group->_first._id;

            return group->_numEntries != 0;
        }

    }

    public unsafe class BRESEntryNode : ResourceEntryNode
    {
        internal BRESCommonHeader* CommonHeader { get { return (BRESCommonHeader*)WorkingRawSource.Address; } }
       // internal ResourceEntry* EntryData { get { return _parent != null ? (ResourceEntry*)(&((BRESGroupNode)_parent).GroupData->First[Index]) : null; } }

        [Browsable(false)]
        public virtual int DataAlign { get { return 4; } }

        [Browsable(false)]
        public override CompressionType Compression
        {
            get { return base.Compression;  }
            set { base.Compression = value;}
        }

        //short _prev, _next, _id;
        //[Category("BRES Entry")]
        //public short Prev { get { return _prev; } set { _prev = value; } }
        //[Category("BRES Entry")]
        //public short Next { get { return _next; } set { _next = value; } }
        //[Category("BRES Entry")]
        //public short ID { get { return _id; } set { _id = value; } }

        //protected override bool OnInitialize()
        //{
        //    if (_parent != null)
        //    {
        //        _name = EntryData->GetName();
        //        _prev = EntryData->_prev;
        //        _next = EntryData->_next;
        //        _id = EntryData->_id;
        //    }
        //    return false;
        //}

        internal virtual void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
        }

        internal protected virtual void OnAfterRebuild(IDictionary<string, VoidPtr> strings)
        {
            CommonHeader->_size = WorkingRawSource.Length;
            CommonHeader->_bresOffset = (int)_parent._parent.WorkingRawSource.Address - (int)CommonHeader;
        }
    }
}
