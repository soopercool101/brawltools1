using System;
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

        internal int _numFrames;
        public int FrameCount { get { return _numFrames; } set { _numFrames = value; } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

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

        internal static ResourceNode TryParse(DataSource source) { return ((CHR0*)source.Address)->_header._tag == CHR0.Tag ? new CHR0Node() : null; }
    }

    public unsafe class CHR0EntryNode : ResourceNode
    {
        internal CHR0Entry* Header { get { return (CHR0Entry*)WorkingUncompressed.Address; } }

        //internal AnimationFrame[] _keyframes = new AnimationFrame[] { };
        internal AnimationKeyframe[] _animFrames;// = new AnimationKeyframe[] { };

        //public AnimationFrame[] Frames { get { return _animFrames; } }

        public int FrameCount
        {
            get
            {
                return ((CHR0Node)Parent)._numFrames;

                //if (_animFrames == null)
                //    _animFrames = AnimationConverter.DecodeSequence(Header, ((CHR0Node)Parent)._numFrames);
                //return _animFrames.Length;
            }
        }
        public AnimationKeyframe this[int index]
        {
            get
            {
                if (_animFrames == null)
                    _animFrames = AnimationConverter.DecodeSequence(Header, ((CHR0Node)Parent)._numFrames);
                return _animFrames[index];
            }
            set { _animFrames[index] = value; SignalPropertyChange(); }
        }

        internal void SetLength(int length)
        {
            int oldLen = _animFrames.Length;
            if (oldLen == length)
                return;

            AnimationKeyframe[] newFrames = new AnimationKeyframe[length];

            Array.Copy(_animFrames, newFrames, Math.Min(oldLen, length));
            if (length > oldLen)
            {
                AnimationKeyframe f = (oldLen == 0) ? AnimationKeyframe.Neutral : newFrames[oldLen - 1];
                for (int i = oldLen; i < length; )
                    newFrames[i++] = f;
            }

            _animFrames = newFrames;
        }


        //AnimationCode _buildCode;
        //protected override int OnCalculateSize(bool force)
        //{
        //    _buildCode = new AnimationCode();

        //    //Iterate through frames, building the code
        //    //Build data too?

        //    int numFrames = _animFrames.Length;

        //    float* pDiff = stackalloc float[numFrames];
        //    ushort* pKeys = stackalloc ushort[numFrames];

        //    int xKeyCount, yKeyCount, zKeyCount;
        //    Vector3 lastValue;

        //    fixed (AnimationFrame* pFrame = _animFrames)
        //    {
        //        bool isotropic = true;
        //        float* pFloat = (float*)pFrame;
        //        for (int i = 0; i < numFrames; i++, pFloat += 9)
        //            if (pFloat[0] != pFloat[1] || pFloat[1] != pFloat[2])
        //            {
        //                isotropic = false;
        //                break;
        //            }

        //        if (isotropic)
        //        {
        //        }
        //        else
        //        {
        //        }
        //    }
        //}

        //private int EncodeTransformation(float* list, int count, float* diff, int* keys, float defaultValue)
        //{
        //    int keyCount = 0;
        //    float current, last = defaultValue;

        //    for (int i = 0; i < count; i++, list += 9)
        //    {
        //        current = *list;
        //        if (last != current)
        //        {
        //            keys[keyCount] = i;
        //            diff[keyCount++] = current - last;
        //            last = current;
        //        }
        //    }

        //    return keyCount;
        //}

        //protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        //{
        //    _replSrc = _replUncompSrc = new DataSource(address, length);

        //    CHR0Entry* header = (CHR0Entry*)address;
        //    *header = new CHR0Entry(_b1, _b2, _b3, _b4);
        //}

        protected override bool OnInitialize()
        {
            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            //_animFrames = AnimationConverter.DecodeSequence(Header, ((CHR0Node)Parent)._numFrames);

            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CHR0Entry* header = (CHR0Entry*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }
}
