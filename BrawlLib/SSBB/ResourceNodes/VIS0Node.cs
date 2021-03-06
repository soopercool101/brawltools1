﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class VIS0Node : BRESEntryNode
    {
        internal VIS0* Header { get { return (VIS0*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.VIS0; } }

        internal int _frameCount;
        internal int _unk1, _unk2;

        [Category("Bone Visibility")]
        public int FrameCount 
        { 
            get { return _frameCount; } 
            set 
            {
                _frameCount = value;
                foreach (VIS0EntryNode e in Children)
                    e.EntryCount = _frameCount + 1;
                SignalPropertyChange();
            } 
        }

        [Category("Bone Visibility")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Bone Visibility")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }

        public unsafe VIS0EntryNode CreateEntry()
        {
            VIS0EntryNode entry = new VIS0EntryNode();
            entry._entryCount = -1;
            entry.EntryCount = _frameCount + 1;
            entry.Name = this.FindName(null);
            AddChild(entry);
            return entry;
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _frameCount = Header->_frameCount;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;

            return Header->Group->_numEntries > 0;
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = VIS0.Size + 0x18;
            foreach (ResourceNode e in Children)
            {
                size += 0x10;
                size += e.CalculateSize(force);
            }
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int count = Children.Count;

            VIS0* header = (VIS0*)address;
            *header = new VIS0(length, _frameCount, count, _unk1, _unk2);

            ResourceGroup* group = header->Group;
            *group = new ResourceGroup(count);

            ResourceEntry* entry = group->First;

            VoidPtr dataAddress = group->EndAddress;
            foreach (ResourceNode n in Children)
            {
                entry->_dataOffset = (int)dataAddress - (int)group;
                entry++;

                int len = n._calcSize;
                n.Rebuild(dataAddress, len, force);
                dataAddress += len;
            }
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

    public unsafe class VIS0EntryNode : ResourceNode
    {
        internal VIS0Entry* Header { get { return (VIS0Entry*)WorkingUncompressed.Address; } }

        internal byte[] _data = new byte[0];
        internal int _entryCount;
        internal VIS0Flags _flags;

        [Browsable(false)]
        public int EntryCount
        {
            get { return _entryCount; }
            set
            {
                if (_entryCount == 0)
                    return;

                _entryCount = value;
                int len = value.Align(32) / 8;

                if (_data.Length < len)
                {
                    byte[] newArr = new byte[len];
                    Array.Copy(_data, newArr, _data.Length);
                    _data = newArr;
                }
                SignalPropertyChange();
            }
        }

        [Category("VIS0 Entry")]
        public VIS0Flags Flags { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }

        protected override int OnCalculateSize(bool force)
        {
            if (_entryCount == 0)
                return 8;
            return _entryCount.Align(32) / 8 + 8;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            VIS0Entry* header = (VIS0Entry*)address;
            *header = new VIS0Entry(_flags);

            if (_entryCount != 0)
                Marshal.Copy(_data, 0, header->Data, length - 8);
        }

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _flags = Header->Flags;

            if ((_flags & VIS0Flags.Constant) == 0)
            {
                _entryCount = ((VIS0Node)_parent)._frameCount + 1;
                int numBytes = _entryCount.Align(32) / 8;

                SetSizeInternal(numBytes + 8);

                _data = new byte[numBytes];
                Marshal.Copy(Header->Data, _data, 0, numBytes);
            }
            else
            {
                _entryCount = 0;
                _data = new byte[0];
                SetSizeInternal(8);
            }

            return false;
        }

        public bool GetEntry(int index)
        {
            int i = index >> 3;
            int bit = 1 << (7 - (index & 0x7));
            return (_data[i] & bit) != 0;
        }
        public void SetEntry(int index, bool value)
        {
            int i = index >> 3;
            int bit = 1 << (7 - (index & 0x7));
            int mask = ~bit;
            _data[i] = (byte)((_data[i] & mask) | (value ? bit : 0));
            SignalPropertyChange();
        }

        public void MakeConstant(bool value)
        {
            _flags = VIS0Flags.Constant | (value ? VIS0Flags.Enabled : 0);
            _entryCount = 0;
            SignalPropertyChange();
        }
        public void MakeAnimated()
        {
            _flags = VIS0Flags.None;
            _entryCount = -1;
            EntryCount = ((VIS0Node)_parent)._frameCount + 1;
            SignalPropertyChange();
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            VIS0Entry* header = (VIS0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
