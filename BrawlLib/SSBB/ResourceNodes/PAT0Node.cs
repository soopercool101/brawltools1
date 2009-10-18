using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PAT0Node : BRESEntryNode, IResourceGroupNode
    {
        internal PAT0* Header { get { return (PAT0*)WorkingSource.Address; } }
        unsafe ResourceGroup* IResourceGroupNode.Group        {            get { return Header->Group; }        }

        internal List<string> _stringList = new List<string>();

        [Category("Pattern")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("Pattern")]
        public short Unknown2 { get { return Header->_unk2; } }
        [Category("Pattern")]
        public short NumEntries { get { return Header->_numEntries; } }
        [Category("Pattern")]
        public short NumStrings { get { return Header->_numEntries2; } }
        [Category("Pattern")]
        public short Unknown3 { get { return Header->_unk3; } }
        [Category("Pattern")]
        public int Unknown4 { get { return Header->_unk4; } }

        [Category("Pattern")]
        public List<string> StringEntries { get { return _stringList; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            for (int i = 0; i < NumStrings; )
                _stringList.Add(Header->GetStringEntry(i++));

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new PAT0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, PAT0Entry.Size));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _stringList)
                table.Add(s);

            foreach (PAT0EntryNode n in Children)
                table.Add(n.Name);
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((PAT0*)address)->_header._tag == PAT0.Tag ? new PAT0Node() : null; }

    }

    public unsafe class PAT0EntryNode : ResourceEntryNode
    {
        internal PAT0Entry* Header { get { return (PAT0Entry*)WorkingSource.Address; } }
    }
}
