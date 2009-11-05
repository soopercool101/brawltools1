using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.Wii.Models;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0BoneNode : MDL0EntryNode
    {
        private List<string> _entries = new List<string>();

        internal MDL0Bone* Header { get { return (MDL0Bone*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_headerLen; } }

        private Vector3 _scale, _rot, _trans;
        private Vector3 _bMin, _bMax;
        private Matrix43 _transform, _transformInvert;

        [Category("Bone")]
        public int HeaderLen { get { return Header->_headerLen; } }
        [Category("Bone")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Bone")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Bone")]
        public int BoneIndex { get { return Header->_index; } }

        [Category("Bone")]
        public int NodeId { get { return Header->_nodeId; } }
        [Category("Bone")]
        public uint Flags { get { return Header->_flags; } }
        [Category("Bone")]
        public uint Pad1 { get { return Header->_pad1; } }
        [Category("Bone")]
        public uint Pad2 { get { return Header->_pad2; } }

        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Scale { get { return _scale; } set { _scale = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotation { get { return _rot; } set { _rot = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Translation { get { return _trans; } set { _trans = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMin { get { return _bMin; } set { _bMin = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMax { get { return _bMax; } set { _bMax = value; SignalPropertyChange(); } }

        [Category("Bone")]
        public int ParentOffset { get { return Header->_parentOffset / 0xD0; } }
        [Category("Bone")]
        public int FirstChildOffset { get { return Header->_firstChildOffset / 0xD0; } }
        [Category("Bone")]
        public int NextOffset { get { return Header->_nextOffset / 0xD0; } }
        [Category("Bone")]
        public int PrevOffset { get { return Header->_prevOffset / 0xD0; } }
        [Category("Bone")]
        public int Part2Offset { get { return Header->_part2Offset; } }

        [Category("Bone"), TypeConverter(typeof(Matrix43StringConverter))]
        public Matrix43 TransformMatrix { get { return _transform; } set { _transform = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Matrix43StringConverter))]
        public Matrix43 TransformInverted { get { return _transformInvert; } set { _transformInvert = value; SignalPropertyChange(); } }

        [Category("Data2 Part2")]
        public List<string> Entries { get { return _entries; } }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (MDL0BoneNode n in Children)
                n.GetStrings(table);

            foreach (string s in _entries)
                table.Add(s);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _scale = Header->_scale;
            _rot = Header->_rotation;
            _trans = Header->_translation;

            _bMin = Header->_boxMin;
            _bMax = Header->_boxMax;

            _transform = Header->_transform;
            _transformInvert = Header->_transformInv;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            if (Header->_part2Offset != 0)
            {
                MDL0Data7Part4* part4 = Header->Part2;
                if (part4 != null)
                {
                    ResourceGroup* group = part4->Group;
                    for (int i = 0; i < group->_numEntries; i++)
                    {
                        _entries.Add(group->First[i].GetName());
                    }
                }
            }
            return false;
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Bone* header = (MDL0Bone*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            header->_boxMin = _bMin;
            header->_boxMax = _bMax;
            header->_scale = _scale;
            header->_rotation = _rot;
            header->_translation = _trans;
            header->_transform = _transform;
            header->_transformInv = _transformInvert;

            MDL0Data7Part4* part4;
            if ((header->_part2Offset != 0) && ((part4 = header->Part2) != null))
            {
                ResourceGroup* group = part4->Group;
                group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
                ResourceEntry* rEntry = group->First;

                for (int i = 0, x = 1; i < group->_numEntries; )
                {
                    ResourceEntry.Build(group, x++, (VoidPtr)group + (rEntry++)->_dataOffset, (BRESString*)stringTable[_entries[i++]]);
                }
            }
        }
    }
}
