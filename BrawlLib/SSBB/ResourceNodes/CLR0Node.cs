using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CLR0Node : BRESEntryNode
    {
        internal CLR0* Header { get { return (CLR0*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.CLR0; } }

        internal int _numFrames, _unk1, _unk2;

        [Category("CLR0")]
        public uint FrameCount
        {
            get { return (uint)_numFrames; }
            set
            {
                _numFrames = (int)value;
                foreach (CLR0EntryNode n in Children)
                    n.NumEntries = _numFrames + 1;
                SignalPropertyChange();
            }
        }
        [Category("CLR0")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("CLR0")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _numFrames = Header->_frames;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            return Header->Group->_numEntries > 0;
        }

        public CLR0EntryNode CreateEntry()
        {
            CLR0EntryNode node = new CLR0EntryNode();
            node.NumEntries = _numFrames + 1;
            node.Name = this.FindName();
            this.AddChild(node);
            return node;
        }

        //To do
        protected override int OnCalculateSize(bool force)
        {
            int size = CLR0.Size + 0x18;// +(Children.Count * 0x10);
            size += Children.Count * ((_numFrames + 1) * 4 + 0x20);
            return size;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int count = Children.Count;
            int stride = (_numFrames + 1) * 4 * count;

            CLR0Entry* pEntry = (CLR0Entry*)(address + 0x3C + (count * 0x10));
            ABGRPixel* pData = (ABGRPixel*)(((int)pEntry + count * 0x10));

            CLR0* header = (CLR0*)address;
            *header = new CLR0(length, _unk1, _numFrames, count, _unk2);

            ResourceGroup* group = header->Group;
            *group = new ResourceGroup(count);

            ResourceEntry* entry = group->First;
            foreach (CLR0EntryNode n in Children)
            {
                entry->_dataOffset = (int)pEntry - (int)group;
                *pEntry = new CLR0Entry(n._flags, (ABGRPixel)n._colorMask, (int)pData - ((int)pEntry + 12));

                entry++;
                pEntry++;

                foreach (ARGBPixel p in n._colors)
                    *pData++ = (ABGRPixel)p;

                n._changed = false;
            }

            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = new DataSource(address, length);
        }

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

        internal static ResourceNode TryParse(DataSource source) { return ((CLR0*)source.Address)->_header._tag == CLR0.Tag ? new CLR0Node() : null; }

    }

    public unsafe class CLR0EntryNode : ResourceNode, IColorSource
    {
        internal CLR0Entry* Header { get { return (CLR0Entry*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.CLR0Entry; } }

        internal int _flags;
        [Category("CLR0 Entry")]
        public int Flags { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }

        internal ARGBPixel _colorMask;
        [Browsable(false)]
        public ARGBPixel ColorMask { get { return _colorMask; } set { _colorMask = value; SignalPropertyChange(); } }

        internal List<ARGBPixel> _colors = new List<ARGBPixel>();
        [Browsable(false)]
        public List<ARGBPixel> Colors { get { return _colors; } }

        internal int _numEntries;
        [Browsable(false)]
        internal int NumEntries
        {
            get { return _numEntries; }
            set
            {
                if (value > _numEntries)
                {
                    ARGBPixel p = _numEntries > 0 ? _colors[_numEntries - 1] : new ARGBPixel(255, 0, 0, 0);
                    for (int i = value - _numEntries; i-- > 0; )
                        _colors.Add(p);
                }
                else if (value < _colors.Count)
                    _colors.RemoveRange(value, _colors.Count - value);

                _numEntries = value;
            }
        }

        protected override bool OnInitialize()
        {
            _flags = Header->_flags;
            _colorMask = (ARGBPixel)Header->_colorMask;

            _numEntries = ((CLR0Node)_parent)._numFrames + 1;
            _colors.Clear();

            ABGRPixel* data = Header->Data;
            for (int i = 0; i < _numEntries; i++)
                _colors.Add((ARGBPixel)(*data++));

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            //Get size
            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CLR0Entry* header = (CLR0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        #region IColorSource Members

        [Browsable(false)]
        public bool HasPrimary { get { return true; } }
        [Browsable(false)]
        public ARGBPixel PrimaryColor
        {
            get { return _colorMask; }
            set { _colorMask = value; SignalPropertyChange(); }
        }
        [Browsable(false)]
        public string PrimaryColorName { get { return "Color Mask"; } }
        [Browsable(false)]
        public int ColorCount { get { return _numEntries; } }
        public ARGBPixel GetColor(int index) { return Colors[index]; }
        public void SetColor(int index, ARGBPixel color) { Colors[index] = color; SignalPropertyChange(); }

        #endregion
    }
}
