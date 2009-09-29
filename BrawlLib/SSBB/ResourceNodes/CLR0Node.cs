using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CLR0Node : BRESEntryNode, IResourceGroupNode
    {
        internal CLR0* Header { get { return (CLR0*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Header->Group; } }

        private int _mPrev, _mNext;

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            ResourceGroup* group = Header->Group;
            _mPrev = group->_first._prev;
            _mNext = group->_first._next;

            return group->_numEntries > 0;
        }
        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new CLR0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, CLR0Entry.Size));
        }

        //protected override int OnCalculateSize(bool force) { return CLR0.Size + (Children.Count * CLR0Entry.Size) + 0x18; }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (CLR0EntryNode n in Children)
                strings[n.Name] = 0;
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((CLR0*)address)->_header._tag == CLR0.Tag ? new CLR0Node() : null; }

    }

    public unsafe class CLR0EntryNode : ResourceEntryNode
    {
        internal CLR0Entry* Header { get { return (CLR0Entry*)WorkingSource.Address; } }
    }
}
