using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SRT0Node : BRESEntryNode
    {
        internal SRT0* Header { get { return (SRT0*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;
            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new SRT0EntryNode().Initialize(this, new DataSource((VoidPtr)group + group->First[i]._dataOffset, 0));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SRT0EntryNode n in Children)
                table.Add(n.Name);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);
            SRT0* header = (SRT0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SRT0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((SRT0*)address)->_header._tag == SRT0.Tag ? new SRT0Node() : null; }
    }

    public unsafe class SRT0EntryNode : ResourceNode
    {
        internal SRT0Entry* Header { get { return (SRT0Entry*)WorkingUncompressed.Address; } }

        [Category("SRT0 Entry")]
        public int Type { get { return Header->_headerType; } }
        [Category("SRT0 Entry")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("SRT0 Entry")]
        public int Unknown2 { get { return Header->_unk2; } }

        protected override bool OnInitialize()
        {
            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;
            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            SRT0Entry* header = (SRT0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
