using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB
{
    public unsafe class SHP0Node : BRESEntryNode, IResourceGroupNode
    {
        internal SHP0* Header { get { return (SHP0*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Header->Group; } }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (SHP0EntryNode n in Children)
                strings[n.Name] = 0;
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new SHP0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((SHP0*)address)->_header._tag == SHP0.Tag ? new SHP0Node() : null; }

    }

    public unsafe class SHP0EntryNode : ResourceEntryNode
    {
        internal SHP0Entry* Header { get { return (SHP0Entry*)WorkingSource.Address; } }
    }
}
