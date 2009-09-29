using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SRT0Node : BRESEntryNode, IResourceGroupNode
    {
        internal SRT0* Header { get { return (SRT0*)WorkingSource.Address; } }
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
                new SRT0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (SRT0EntryNode n in Children)
                strings[n.Name] = 0;
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((SRT0*)address)->_header._tag == SRT0.Tag ? new SRT0Node() : null; }
    }

    public unsafe class SRT0EntryNode : ResourceEntryNode
    {
        internal SRT0Entry* Header { get { return (SRT0Entry*)WorkingSource.Address; } }

        [Category("SRT0 Entry")]
        public int Type { get { return Header->_headerType; } }
        [Category("SRT0 Entry")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("SRT0 Entry")]
        public int Unknown2 { get { return Header->_unk2; } }
    }
}
