﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB
{
    public unsafe class SHP0Node : BRESEntryNode
    {
        internal SHP0* Header { get { return (SHP0*)WorkingUncompressed.Address; } }

        internal List<string> _strings = new List<string>();

        public List<string> StringEntries { get { return _strings; } }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _strings)
                table.Add(s);

            foreach (SHP0EntryNode n in Children)
                table.Add(n.Name);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            bint* stringOffset = Header->StringEntries;
            for (int i = 0; i < Header->_numItems; i++)
                _strings.Add(new String((sbyte*)stringOffset + stringOffset[i]));

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new SHP0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);
            SHP0* header = (SHP0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            bint* stringPtr = header->StringEntries;
            for (int i = 0; i < header->_numItems; i++)
                stringPtr[i] = ((int)stringTable[_strings[i]] + 4) - (int)stringPtr;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SHP0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source) { return ((SHP0*)source.Address)->_header._tag == SHP0.Tag ? new SHP0Node() : null; }

    }

    public unsafe class SHP0EntryNode : ResourceNode
    {
        internal SHP0Entry* Header { get { return (SHP0Entry*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            SHP0Entry* header = (SHP0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
