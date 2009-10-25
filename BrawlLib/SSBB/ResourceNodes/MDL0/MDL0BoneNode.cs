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

        [Category("Bone")]
        public Vector3 Scale { get { return Header->_scale; } }
        [Category("Bone")]
        public Vector3 Rotation { get { return Header->_rotation; } }
        [Category("Bone")]
        public Vector3 Translation { get { return Header->_translation; } }
        [Category("Bone")]
        public Vector3 BoxMin { get { return Header->_boxMin; } }
        [Category("Bone")]
        public Vector3 BoxMax { get { return Header->_boxMax; } }

        [Category("Bone")]
        public int ParentOffset { get { return Header->_parentOffset / 0xD0; } }
        [Category("Bone")]
        public int FirstChildOffset { get { return Header->_unk1 / 0xD0; } }
        [Category("Bone")]
        public int NextOffset { get { return Header->_unk2 / 0xD0; } }
        [Category("Bone")]
        public int PrevOffset { get { return Header->_nextOffset / 0xD0; } }
        [Category("Bone")]
        public int Part2Offset { get { return Header->_part2Offset; } }

        [Category("Bone")]
        public bMatrix43 TransformMatrix { get { return Header->_transform; } }
        [Category("Bone")]
        public bMatrix43 TransformInverted { get { return Header->_transformInv; } }

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

            if (Header->_stringOffset != 0)
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
