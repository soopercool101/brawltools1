using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0Node : BRESEntryNode, IResourceGroupNode
    {
        internal SCN0* Header { get { return (SCN0*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Header->Group; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new SCN0GroupNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0x10));
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (SCN0GroupNode n in Children)
                n.GetStrings(strings);
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((SCN0*)address)->_header._tag == SCN0.Tag ? new SCN0Node() : null; }

    }

    public unsafe class SCN0GroupNode : ResourceEntryNode, IResourceGroupNode
    {
        internal ResourceGroup* Group { get { return (ResourceGroup*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Group; } }

        //internal ResourceEntry* EntryData { get { return _parent != null ? (ResourceEntry*)(&((SCN0Node)_parent).Header->Group->First[Index]) : null; } }

        //private int _id;

        internal void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (SCN0EntryNode n in Children)
                n.GetStrings(strings);
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            return Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Group;

            Type t = null;
            switch (_id)
            {
                case 109: { t = typeof(SCN0LightSetNode); break; }
                case 117: { t = typeof(SCN0AmbientLightNode); break; }
                case 93: { t = typeof(SCN0LightNode); break; }
                case 77: { t = typeof(SCN0FogNode); break; }
                case 101: { t = typeof(SCN0CameraNode); break; }
            }

            for (int i = 0; i < Group->_numEntries; i++)
                ((ResourceNode)Activator.CreateInstance(t)).Initialize(this, new DataSource(Group->First[i].DataAddress, 0x10));
        }
    }

    public unsafe class SCN0EntryNode : ResourceEntryNode
    {
        internal virtual void GetStrings(IDictionary<string, VoidPtr> strings) { strings[Name] = 0; }
        //internal ResourceEntry* EntryData { get { return _parent != null ? (ResourceEntry*)(&((SCN0GroupNode)_parent).Group->First[Index]) : null; } }

        //protected override bool OnInitialize()
        //{
        //    Name = EntryData->GetName();
        //    return false;
        //}
    }

    public unsafe class SCN0LightSetNode : SCN0EntryNode
    {
        internal SCN0Part1* Data { get { return (SCN0Part1*)WorkingSource.Address; } }
        private List<string> _entries = new List<string>();

        [Category("Light Set")]
        public string String2 { get { return Data->GetString(); } }
        [Category("Light Set")]
        public short Magic { get { return Data->_magic; } }
        [Category("Light Set")]
        public byte NumEntries { get { return Data->_numEntries; } }
        [Category("Light Set")]
        public byte Unknown1 { get { return Data->_unk1; } }
        [Category("Light Set")]
        public List<string> Entries { get { return _entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            for (int i = 0; i < NumEntries; i++)
                _entries.Add(Data->GetEntry(i));

            return false;
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (string s in _entries)
                strings[s] = 0;
        }
    }

    public unsafe class SCN0AmbientLightNode : SCN0EntryNode
    {
        internal SCN0Part2* Data { get { return (SCN0Part2*)WorkingSource.Address; } }

        [Category("Ambient Light")]
        public byte Unknown1 { get { return Data->_unk1; } }
        [Category("Ambient Light")]
        public byte Unknown2 { get { return Data->_unk2; } }
        [Category("Ambient Light")]
        public byte Unknown3 { get { return Data->_unk3; } }
        [Category("Ambient Light")]
        public byte Unknown4 { get { return Data->_unk4; } }
        [Category("Ambient Light")]
        public byte B1 { get { return Data->_b1; } }
        [Category("Ambient Light")]
        public byte B2 { get { return Data->_b2; } }
        [Category("Ambient Light")]
        public byte B3 { get { return Data->_b3; } }
        [Category("Ambient Light")]
        public byte B4 { get { return Data->_b4; } }
    }

    public unsafe class SCN0LightNode : SCN0EntryNode
    {
        internal SCN0Part3* Data { get { return (SCN0Part3*)WorkingSource.Address; } }

        [Category("Light")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("Light")]
        public int Unknown2 { get { return Data->_unk2; } }
        [Category("Light")]
        public short Unknown3 { get { return Data->_unk3; } }
        [Category("Light")]
        public short Unknown4 { get { return Data->_unk4; } }
        [Category("Light")]
        public int Unknown5 { get { return Data->_unk5; } }
        [Category("Light")]
        public BVec3 Vec1 { get { return Data->_vec1; } }
        [Category("Light")]
        public byte B1 { get { return Data->_b1; } }
        [Category("Light")]
        public byte B2 { get { return Data->_b2; } }
        [Category("Light")]
        public byte B3 { get { return Data->_b3; } }
        [Category("Light")]
        public byte B4 { get { return Data->_b4; } }
        [Category("Light")]
        public BVec3 Vec2 { get { return Data->_vec2; } }
        [Category("Light")]
        public int Unknown6 { get { return Data->_unk6; } }
        [Category("Light")]
        public int Unknown7 { get { return Data->_unk7; } }
        [Category("Light")]
        public int Unknown8 { get { return Data->_unk8; } }
        [Category("Light")]
        public int Unknown9 { get { return Data->_unk9; } }
        [Category("Light")]
        public int Unknown10 { get { return Data->_unk10; } }
        [Category("Light")]
        public int Unknown11 { get { return Data->_unk11; } }
        [Category("Light")]
        public float Unknown12 { get { return Data->_unk12; } }
    }

    public unsafe class SCN0FogNode : SCN0EntryNode
    {
        internal SCN0Part4* Data { get { return (SCN0Part4*)WorkingSource.Address; } }

        [Category("Fog")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("Fog")]
        public int Unknown2 { get { return Data->_unk2; } }
        [Category("Fog")]
        public float Float1 { get { return Data->_float1; } }
        [Category("Fog")]
        public float Float2 { get { return Data->_float2; } }
        [Category("Fog")]
        public byte B1 { get { return Data->_b1; } }
        [Category("Fog")]
        public byte B2 { get { return Data->_b2; } }
        [Category("Fog")]
        public byte B3 { get { return Data->_b3; } }
        [Category("Fog")]
        public byte B4 { get { return Data->_b4; } }

    }

    public unsafe class SCN0CameraNode : SCN0EntryNode
    {
        internal SCN0Part5* Data { get { return (SCN0Part5*)WorkingSource.Address; } }

        [Category("Camera")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("Camera")]
        public short Unknown2 { get { return Data->_unk2; } }
        [Category("Camera")]
        public short Unknown3 { get { return Data->_unk3; } }
        [Category("Camera")]
        public int Unknown4 { get { return Data->_unk4; } }
        [Category("Camera")]
        public BVec3 Vec1 { get { return Data->_vec1; } }
        [Category("Camera")]
        public BVec3 Vec2 { get { return Data->_vec2; } }
        [Category("Camera")]
        public BVec3 Vec3 { get { return Data->_vec3; } }
        [Category("Camera")]
        public BVec3 Vec4 { get { return Data->_vec4; } }
        [Category("Camera")]
        public BVec3 Vec5 { get { return Data->_vec5; } }
    }
}
