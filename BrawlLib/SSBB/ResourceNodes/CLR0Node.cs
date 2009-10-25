using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CLR0Node : BRESEntryNode
    {
        internal CLR0* Header { get { return (CLR0*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;

            return Header->Group->_numEntries > 0;
        }

        //To do
        //protected override int OnCalculateSize(bool force)
        //{
        //    int size = CLR0.Size + 0x18 + (Children.Count * 0x10);
        //    foreach (CHR0EntryNode n in Children)
        //        size += n.CalculateSize(force);
        //    return size;
        //}

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new CLR0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (CLR0EntryNode n in Children)
                table.Add(n.Name);
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            CLR0* header = (CLR0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (CLR0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((CLR0*)address)->_header._tag == CLR0.Tag ? new CLR0Node() : null; }

    }

    public unsafe class CLR0EntryNode : ResourceNode
    {
        internal CLR0Entry* Header { get { return (CLR0Entry*)WorkingUncompressed.Address; } }

        //To do
        //protected override int OnCalculateSize(bool force)
        //{
        //    return base.OnCalculateSize(force);
        //}

        protected override bool OnInitialize()
        {
            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;
            //Get size
            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CLR0Entry* header = (CLR0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
