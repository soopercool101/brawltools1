using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CHR0Node : BRESEntryNode
    {
        internal CHR0* Header { get { return (CHR0*)WorkingUncompressed.Address; } }

        private int _len1, _len2;


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _len1 = Header->_len1;
            _len2 = Header->_len2;

            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new CHR0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (CHR0EntryNode n in Children)
                table.Add(n.Name);
        }

        //protected override int OnCalculateSize(bool force)
        //{
        //    int size = CHR0.Size + 0x18 + (Children.Count * 0x10);
        //    foreach (CHR0EntryNode n in Children)
        //        size += n.CalculateSize(force);
        //    return size;
        //}

        //protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        //{
        //    _replSrc = _replUncompSrc = new DataSource(address, length);

        //    CHR0* header = (CHR0*)address;
        //    *header = new CHR0(length, Children.Count, _len1, _len2);

        //    ResourceGroup* group = header->Group;
        //    *group = new ResourceGroup(Children.Count);

        //    CHR0Entry* entry = (CHR0Entry*)group->EndAddress;

        //    ResourceEntry* rEntry = group->First;
        //    foreach (CHR0EntryNode n in Children)
        //    {
        //        rEntry->_dataOffset = (int)entry - (int)group;
        //        rEntry++;

        //        int size = n._calcSize;
        //        n.Rebuild(entry, size, force);
        //        entry += size;
        //    }
        //}

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            CHR0* header = (CHR0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (CHR0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((CHR0*)address)->_header._tag == CHR0.Tag ? new CHR0Node() : null; }
    }

    public unsafe class CHR0EntryNode : ResourceNode
    {
        internal CHR0Entry* Header { get { return (CHR0Entry*)WorkingUncompressed.Address; } }

        private byte _b1, _b2, _b3, _b4;

        [Category("CHR0 Data")]
        public byte B1 { get { return _b1; } set { _b1 = value; } }
        [Category("CHR0 Data")]
        public byte B2 { get { return _b2; } set { _b2 = value; } }
        [Category("CHR0 Data")]
        public byte B3 { get { return _b3; } set { _b3 = value; } }
        [Category("CHR0 Data")]
        public byte B4 { get { return _b4; } set { _b4 = value; } }


        //protected override int OnCalculateSize(bool force)
        //{
        //}

        //protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        //{
        //    _replSrc = _replUncompSrc = new DataSource(address, length);

        //    CHR0Entry* header = (CHR0Entry*)address;
        //    *header = new CHR0Entry(_b1, _b2, _b3, _b4);
        //}

        protected override bool OnInitialize()
        {
            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;

            //Get size

            _b1 = Header->_b1;
            _b2 = Header->_b2;
            _b3 = Header->_b3;
            _b4 = Header->_b4;

            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CHR0Entry* header = (CHR0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
