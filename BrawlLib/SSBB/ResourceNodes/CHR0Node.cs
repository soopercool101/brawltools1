using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CHR0Node : BRESEntryNode, IResourceGroupNode
    {
        internal CHR0* Header { get { return (CHR0*)WorkingSource.Address; } }
        unsafe ResourceGroup* IResourceGroupNode.Group { get { return Header->Group; } }

        private int _len1, _len2;
        private short _mPrev, _mNext;


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _len1 = Header->_len1;
            _len2 = Header->_len2;

            ResourceGroup* group = Header->Group;
            _mPrev = group->_first._prev;
            _mNext = group->_first._next;

            return group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new CHR0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 8));
        }

        //protected override int OnCalculateSize(bool force) { return 0x40 + (Children.Count * 0x18); }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (CHR0EntryNode n in Children)
                strings[n.Name] = 0;
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((CHR0*)address)->_header._tag == CHR0.Tag ? new CHR0Node() : null; }

    }

    public unsafe class CHR0EntryNode : ResourceEntryNode
    {
        internal CHR0Entry* Header { get { return (CHR0Entry*)WorkingSource.Address; } }

        [Category("CHR0 Data")]
        public byte B1 { get { return Header->_b1; } set { Header->_b1 = value; } }
        [Category("CHR0 Data")]
        public byte B2 { get { return Header->_b2; } set { Header->_b2 = value; } }
        [Category("CHR0 Data")]
        public byte B3 { get { return Header->_b3; } set { Header->_b3 = value; } }
        [Category("CHR0 Data")]
        public byte B4 { get { return Header->_b4; } set { Header->_b4 = value; } }
    }
}
