using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PAT0Node : BRESEntryNode
    {
        internal PAT0* Header { get { return (PAT0*)WorkingUncompressed.Address; } }

        internal List<string> _stringList1 = new List<string>();
        internal List<string> _stringList2 = new List<string>();

        [Category("Pattern")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("Pattern")]
        public short Unknown2 { get { return Header->_unk2; } }
        [Category("Pattern")]
        public short NumEntries { get { return Header->_numEntries; } }
        [Category("Pattern")]
        public short NumStrings1 { get { return Header->_numStrings1; } }
        [Category("Pattern")]
        public short NumStrings2 { get { return Header->_numStrings2; } }
        [Category("Pattern")]
        public int Unknown4 { get { return Header->_unk4; } }

        [Category("Pattern")]
        public List<string> StringEntries1 { get { return _stringList1; } }
        [Category("Pattern")]
        public List<string> StringEntries2 { get { return _stringList2; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            bint* strings = Header->StringOffsets1;
            for (int i = 0; i < Header->_numStrings1; )
                _stringList1.Add(new String((sbyte*)strings + strings[i++]));

            strings = Header->StringOffsets2;
            for (int i = 0; i < Header->_numStrings2; )
                _stringList2.Add(new String((sbyte*)strings + strings[i++]));

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

            foreach (string s in _stringList1)
                table.Add(s);

            foreach (string s in _stringList2)
                table.Add(s);

            foreach (PAT0EntryNode n in Children)
                table.Add(n.Name);
        }

        //To do
        //protected override int OnCalculateSize(bool force)
        //{
        //    int size = CLR0.Size + 0x18 + (Children.Count * 0x10);
        //    foreach (PAT0EntryNode n in Children)
        //        size += n.CalculateSize(force);
        //    return size;
        //}

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            PAT0* header = (PAT0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (PAT0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            bint* strings = header->StringOffsets1;
            for (int i = 0; i < _stringList1.Count; i++)
                strings[i] = (int)stringTable[_stringList1[i]] + 4 - (int)strings;

            strings = header->StringOffsets2;
            for (int i = 0; i < _stringList2.Count; i++)
                strings[i] = (int)stringTable[_stringList2[i]] + 4 - (int)strings;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((PAT0*)source.Address)->_header._tag == PAT0.Tag ? new PAT0Node() : null; }

    }

    public unsafe class PAT0EntryNode : ResourceNode
    {
        internal PAT0Entry* Header { get { return (PAT0Entry*)WorkingUncompressed.Address; } }

        //To do
        //protected override int OnCalculateSize(bool force)
        //{
        //    return base.OnCalculateSize(force);
        //}

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            //Get size
            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            PAT0Entry* header = (PAT0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
