using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class VIS0Node : BRESEntryNode
    {
        internal VIS0* Header { get { return (VIS0*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new VIS0EntryNode().Initialize(this, new DataSource((VoidPtr)group + group->First[i]._dataOffset, 0));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (VIS0EntryNode n in Children)
                table.Add(n.Name);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);
            VIS0* header = (VIS0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (VIS0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((VIS0*)source.Address)->_header._tag == VIS0.Tag ? new VIS0Node() : null; }
    }

    unsafe class VIS0EntryNode : ResourceNode
    {
        internal VIS0Entry* Header { get { return (VIS0Entry*)WorkingUncompressed.Address; } }

        [Category("VIS0 Entry")]
        public int Flags { get { return Header->_flags; } }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            VIS0Entry* header = (VIS0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
