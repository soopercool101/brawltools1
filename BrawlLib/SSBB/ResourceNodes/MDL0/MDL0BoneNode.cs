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

        internal MDL0Bone* Data { get { return (MDL0Bone*)WorkingSource.Address; } }

        [Category("Bone")]
        public string DataName { get { return Data->Name; } }

        [Category("Bone")]
        public int HeaderLen { get { return Data->_headerLen; } }
        [Category("Bone")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Bone")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Bone")]
        public int BoneIndex { get { return Data->_index; } }

        [Category("Bone")]
        public int NodeId { get { return Data->_nodeId; } }
        [Category("Bone")]
        public uint Flags { get { return Data->_flags; } }
        [Category("Bone")]
        public uint Pad1 { get { return Data->_pad1; } }
        [Category("Bone")]
        public uint Pad2 { get { return Data->_pad2; } }

        [Category("Bone")]
        public Vector3 Scale { get { return Data->_scale; } }
        [Category("Bone")]
        public Vector3 Rotation { get { return Data->_rotation; } }
        [Category("Bone")]
        public Vector3 Translation { get { return Data->_translation; } }
        [Category("Bone")]
        public Vector3 BoxMin { get { return Data->_boxMin; } }
        [Category("Bone")]
        public Vector3 BoxMax { get { return Data->_boxMax; } }

        [Category("Bone")]
        public int ParentOffset { get { return Data->_parentOffset / 0xD0; } }
        [Category("Bone")]
        public int FirstChildOffset { get { return Data->_unk1 / 0xD0; } }
        [Category("Bone")]
        public int NextOffset { get { return Data->_unk2 / 0xD0; } }
        [Category("Bone")]
        public int PrevOffset { get { return Data->_nextOffset / 0xD0; } }
        [Category("Bone")]
        public int Part2Offset { get { return Data->_part2Offset; } }

        [Category("Bone")]
        public bMatrix43 TransformMatrix { get { return Data->_transform; } }
        [Category("Bone")]
        public bMatrix43 TransformInverted { get { return Data->_transformInv; } }

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

            if (Data->_part2Offset != 0)
            {
                MDL0Data7Part4* part4 = Data->Part2;
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
    }
}
