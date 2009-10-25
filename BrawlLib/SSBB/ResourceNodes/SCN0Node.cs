using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class SCN0Node : BRESEntryNode
    {
        internal SCN0* Header { get { return (SCN0*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;

            return Header->Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Header->Group;
            for (int i = 0; i < group->_numEntries; i++)
                new SCN0GroupNode(group->First[i].GetName()).Initialize(this, new DataSource(group->First[i].DataAddress, 0));
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SCN0GroupNode n in Children)
                n.GetStrings(table);
        }

        //To do
        //protected override int OnCalculateSize(bool force)
        //{
        //    int size = SCN0.Size + 0x18 + (Children.Count * 0x10);
        //    foreach (SCN0GroupNode n in Children)
        //        foreach(SCN0EntryNode e in n.Children)
        //            size += e.CalculateSize(force);
        //    return size;
        //}

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            SCN0* header = (SCN0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            ResourceGroup* group = header->Group;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SCN0GroupNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((SCN0*)address)->_header._tag == SCN0.Tag ? new SCN0Node() : null; }

    }

    public unsafe class SCN0GroupNode : ResourceNode
    {
        internal ResourceGroup* Group { get { return (ResourceGroup*)WorkingUncompressed.Address; } }

        public SCN0GroupNode() : base() { }
        public SCN0GroupNode(string name) : base() { _name = name; }

        internal void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (SCN0EntryNode n in Children)
                n.GetStrings(table);
        }

        protected override bool OnInitialize()
        {
            return Group->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group = Group;

            Type t = null;
            if (_name == "LightSet(NW4R)")
                t = typeof(SCN0LightSetNode);
            else if (_name == "AmbLights(NW4R)")
                t = typeof(SCN0AmbientLightNode);
            else if (_name == "Lights(NW4R)")
                t = typeof(SCN0LightNode);
            else if (_name == "Fogs(NW4R)")
                t = typeof(SCN0FogNode);
            else if (_name == "Cameras(NW4R)")
                t = typeof(SCN0CameraNode);
            else
                return;

            for (int i = 0; i < Group->_numEntries; i++)
                ((ResourceNode)Activator.CreateInstance(t)).Initialize(this, new DataSource(Group->First[i].DataAddress, 0));
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            ResourceGroup* group = (ResourceGroup*)dataAddress;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (SCN0EntryNode n in Children)
            {
                dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }
        }
    }

    public unsafe class SCN0EntryNode : ResourceNode
    {
        internal SCN0CommonHeader* Header { get { return (SCN0CommonHeader*)WorkingUncompressed.Address; } }

        internal virtual void GetStrings(StringTable table) { table.Add(Name); }

        //To do
        //protected override int OnCalculateSize(bool force)
        //{
        //    return base.OnCalculateSize(force);
        //}

        protected override bool OnInitialize()
        {
            if (Header->_stringOffset != 0)
                _name = Header->ResourceString;

            if (IsBranch)
                if (IsCompressed)
                    _replUncompSrc.Length = Header->_length;
                else
                    _replSrc.Length = _replUncompSrc.Length = Header->_length;
            else
                if (IsCompressed)
                    _uncompSource.Length = Header->_length;
                else
                    _origSource.Length = _uncompSource.Length = Header->_length;

            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            SCN0CommonHeader* header = (SCN0CommonHeader*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }
    }

    public unsafe class SCN0LightSetNode : SCN0EntryNode
    {
        internal SCN0LightSet* Data { get { return (SCN0LightSet*)WorkingUncompressed.Address; } }

        private string _ambientLight;
        private List<string> _entries = new List<string>();

        [Category("Light Set")]
        public short Magic { get { return Data->_magic; } }
        [Category("Light Set")]
        public byte NumLights { get { return Data->_numLights; } }
        [Category("Light Set")]
        public byte Unknown1 { get { return Data->_unk1; } }

        [Category("Light Set")]
        public string Ambience { get { return _ambientLight; } set { _ambientLight = value; } }
        [Category("Light Set")]
        public List<string> Entries { get { return _entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (Data->_stringOffset2 != 0)
                _ambientLight = Data->AmbientString;

            bint* strings = Data->StringOffsets;
            for (int i = 0; i < Data->_numLights; i++)
                _entries.Add(new String((sbyte*)strings + strings[i]));

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            if (_ambientLight != null)
                table.Add(_ambientLight);

            foreach (string s in _entries)
                table.Add(s);
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            base.PostProcess(dataAddress, stringTable);

            SCN0LightSet* header = (SCN0LightSet*)dataAddress;

            if (_ambientLight != null)
                header->AmbientStringAddress = stringTable[_ambientLight] + 4;
            else
                header->_stringOffset2 = 0;

            int i;
            bint* strings = header->StringOffsets;
            for (i = 0; i < _entries.Count; i++)
                strings[i] = (int)stringTable[_entries[i]] + 4 - (int)strings;
            while (i < 8)
                strings[i++] = 0;
        }
    }

    public unsafe class SCN0AmbientLightNode : SCN0EntryNode
    {
        internal SCN0AmbientLight* Data { get { return (SCN0AmbientLight*)WorkingUncompressed.Address; } }

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
        internal SCN0Part3* Data { get { return (SCN0Part3*)WorkingUncompressed.Address; } }

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
        internal SCN0Part4* Data { get { return (SCN0Part4*)WorkingUncompressed.Address; } }

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
        internal SCN0Part5* Data { get { return (SCN0Part5*)WorkingUncompressed.Address; } }

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
