﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Animations;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CHR0Node : BRESEntryNode
    {
        internal CHR0* Header { get { return (CHR0*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.CHR0; } }

        internal int _layer = 0;
        [Category("Character Animation")]
        public int Layer
        {
            get { return _layer; }
            set { _layer = value; SignalPropertyChange(); }
        }

        internal int _numFrames = 1;
        [Category("Character Animation")]
        public int FrameCount
        {
            get { return _numFrames; }
            set
            {
                if ((_numFrames == value) || (value < 1))
                    return;

                _numFrames = value;
                foreach (CHR0EntryNode n in Children)
                    n.SetSize(_numFrames);

                SignalPropertyChange();
            }
        }

        public CHR0EntryNode CreateEntry()
        {
            CHR0EntryNode n = new CHR0EntryNode();
            n._numFrames = _numFrames;
            n._name = this.FindName();
            AddChild(n);
            return n;
        }

        public void InsertKeyframe(int index)
        {
            FrameCount++;
            foreach (CHR0EntryNode c in Children)
                c.Keyframes.Insert(index);
        }
        public void DeleteKeyframe(int index)
        {
            foreach (CHR0EntryNode c in Children)
                c.Keyframes.Delete(index);
            FrameCount--;
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _layer = Header->_layer;
            _numFrames = Header->_numFrames;

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

        protected override int OnCalculateSize(bool force)
        {
            int size = CHR0.Size + 0x18 + (Children.Count * 0x10);
            foreach (CHR0EntryNode n in Children)
                size += n.CalculateSize(true);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            CHR0* header = (CHR0*)address;
            *header = new CHR0(length, _numFrames, Children.Count, _layer);

            ResourceGroup* group = header->Group;
            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;

            foreach (CHR0EntryNode n in Children)
                dataAddress += n._entryLen;

            //VoidPtr dataAddr = group->EndAddress;
            //CHR0Entry* entry = (CHR0Entry*)group->EndAddress;

            ResourceEntry* rEntry = group->First;
            foreach (CHR0EntryNode n in Children)
            {
                rEntry->_dataOffset = (int)entryAddress - (int)group;
                rEntry++;

                n._dataAddr = dataAddress;
                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
                dataAddress += n._dataLen;
            }

            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = new DataSource(address, length);
        }

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

        internal static ResourceNode TryParse(DataSource source) { return ((CHR0*)source.Address)->_header._tag == CHR0.Tag ? new CHR0Node() : null; }

    }

    public unsafe class CHR0EntryNode : ResourceNode
    {
        internal CHR0Entry* Header { get { return (CHR0Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.CHR0Entry; } }

        internal int _numFrames;
        [Browsable(false)]
        public int FrameCount { get { return _numFrames; } }

        internal KeyframeCollection _keyframes;
        [Browsable(false)]
        public KeyframeCollection Keyframes 
        { 
            get 
            {
                if (_keyframes == null)
                {
                    if (Header != null)
                        _keyframes = AnimationConverter.DecodeKeyframes(Header, _numFrames);
                    else
                        _keyframes = new KeyframeCollection(_numFrames);
                }
                return _keyframes;
            } 
        }

        internal int _dataLen;
        internal int _entryLen;
        internal VoidPtr _dataAddr;
        protected override int OnCalculateSize(bool force)
        {
            _dataLen = AnimationConverter.CalculateSize(Keyframes, out _entryLen);
            return _dataLen + _entryLen;
        }

        protected override bool OnInitialize()
        {
            _keyframes = null;

            if (_parent is CHR0Node)
                _numFrames = ((CHR0Node)_parent)._numFrames;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return false;
        }

        public override unsafe void Export(string outPath)
        {
            StringTable table = new StringTable();
            table.Add(_name);

            int dataLen = OnCalculateSize(true);
            int totalLen = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(totalLen);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    AnimationConverter.EncodeKeyframes(Keyframes, map.Address, map.Address + _entryLen);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(map.Address, table);
                }
            }
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            AnimationConverter.EncodeKeyframes(_keyframes, address, _dataAddr);

            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = new DataSource(address, length);
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CHR0Entry* header = (CHR0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        internal void SetSize(int count)
        {
            if (_keyframes != null)
                Keyframes.SetSize(count);

            _numFrames = count;
            SignalPropertyChange();
        }
        public float GetKeyframe(KeyFrameMode mode, int index) { return Keyframes.GetKeyframe(mode, index); }
        public void SetKeyframe(KeyFrameMode mode, int index, float value)
        {
            Keyframes.SetKeyFrame(mode, index, value);
            SignalPropertyChange();
        }
        public void RemoveKeyframe(KeyFrameMode mode, int index)
        {
            Keyframes.RemoveKeyframe(mode, index);
            SignalPropertyChange();
        }

        public AnimationFrame GetAnimFrame(int index)
        {
            return Keyframes.AnimFrames[index];
        }
    }
}
