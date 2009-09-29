using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class VIS0Node : BRESEntryNode, IResourceGroupNode
    {
        internal VIS0* Header { get { return (VIS0*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Header->Group; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new VIS0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (VIS0EntryNode n in Children)
                strings[n.Name] = 0;
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((VIS0*)address)->_header._tag == VIS0.Tag ? new VIS0Node() : null; }
    }

    unsafe class VIS0EntryNode : ResourceEntryNode
    {
        internal VIS0Entry* Data { get { return (VIS0Entry*)WorkingSource.Address; } }

        [Category("VIS0 Entry")]
        public uint Type { get { return Data->Value; } }
    }
}
