using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;
using System.Drawing;
using System.IO;
using BrawlLib.Imaging;
using BrawlLib.Wii.Graphics;
using BrawlLib.Wii.Models;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialNode : MDL0EntryNode
    {
        internal MDL0Material* Header { get { return (MDL0Material*)WorkingUncompressed.Address; } }

        internal List<MDL0PolygonNode> _polygons = new List<MDL0PolygonNode>();

        internal List<string> _part4Entries = new List<string>();

        internal byte _numTextures;
        internal byte _numLayers;
        internal int _unk1, _unk6, _unk3, _unk4;
        internal byte _flag3;
        internal byte _flag4;
        internal byte _flag5;
        internal byte _flag6;
        internal byte _flag7;
        internal byte _flag8;
        internal int _type;

        //In order of appearance in display list
        internal AlphaFunction _alphaFunc = AlphaFunction.Default;
        internal ZMode _zMode = ZMode.Default;
        //Mask, does not allow changing the dither/update bits
        internal BlendMode _blendMode = BlendMode.Default;
        internal ConstantAlpha _constantAlpha = ConstantAlpha.Default;
        //Pad 7
        //TEV Set 1
        //TEV Set 2
        //Indexed texture coordinate scale?
        //XF Texture matrix info

        #region Shader linkage
        internal MDL0ShaderNode _shader;
        [Browsable(false)]
        public MDL0ShaderNode ShaderNode
        {
            get { return _shader; }
            set
            {
                if (_shader == value)
                    return;
                if (_shader != null)
                    _shader._materials.Remove(this);
                if ((_shader = value) != null)
                    _shader._materials.Add(this);
                Model.SignalPropertyChange();
            }
        }
        public string Shader
        {
            get { return _shader == null ? null : _shader._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    ShaderNode = null;
                else
                {
                    MDL0ShaderNode node = Model.FindChild(String.Format("Shaders/{0}", value), false) as MDL0ShaderNode;
                    if (node != null)
                        ShaderNode = node;
                }
            }
        }
        #endregion


        //[Category("Material")]
        //public int TotalLen { get { return Header->_dataLen; } }
        //[Category("Material")]
        //public int MDL0Offset { get { return Header->_mdl0Offset; } }
        //[Category("Material")]
        //public int StringOffset { get { return Header->_stringOffset; } }
        //[Category("Material")]
        //public int ID { get { return Header->_index; } }

        public int Tex1Unk1 { get { return ((sbyte*)Header)[0x24C]; } }
        public int Tex1Unk2 { get { return ((sbyte*)Header)[0x24D]; } }
        public int Tex1Unk3 { get { return ((sbyte*)Header)[0x24E]; } }
        public int Tex1Unk4 { get { return ((sbyte*)Header)[0x24F]; } }
        public bMatrix43 Tex1Mtx { get { return *(bMatrix43*)((byte*)Header + 0x250); } }

        public int Tex2Unk1 { get { return ((sbyte*)Header)[0x280]; } }
        public int Tex2Unk2 { get { return ((sbyte*)Header)[0x281]; } }
        public int Tex2Unk3 { get { return ((sbyte*)Header)[0x282]; } }
        public int Tex2Unk4 { get { return ((sbyte*)Header)[0x283]; } }
        public bMatrix43 Tex2Mtx { get { return *(bMatrix43*)((byte*)Header + 0x284); } }

        public int Tex3Unk1 { get { return ((sbyte*)Header)[0x2B4]; } }
        public int Tex3Unk2 { get { return ((sbyte*)Header)[0x2B5]; } }
        public int Tex3Unk3 { get { return ((sbyte*)Header)[0x2B6]; } }
        public int Tex3Unk4 { get { return ((sbyte*)Header)[0x2B7]; } }
        public bMatrix43 Tex3Mtx { get { return *(bMatrix43*)((byte*)Header + 0x2B8); } }

        public int Tex4Unk1 { get { return ((sbyte*)Header)[0x2E8]; } }
        public int Tex4Unk2 { get { return ((sbyte*)Header)[0x2E9]; } }
        public int Tex4Unk3 { get { return ((sbyte*)Header)[0x2EA]; } }
        public int Tex4Unk4 { get { return ((sbyte*)Header)[0x2EB]; } }
        public bMatrix43 Tex4Mtx { get { return *(bMatrix43*)((byte*)Header + 0x2EC); } }

        public int Tex5Unk1 { get { return ((sbyte*)Header)[0x31C]; } }
        public int Tex5Unk2 { get { return ((sbyte*)Header)[0x31D]; } }
        public int Tex5Unk3 { get { return ((sbyte*)Header)[0x31E]; } }
        public int Tex5Unk4 { get { return ((sbyte*)Header)[0x31F]; } }
        public bMatrix43 Tex5Mtx { get { return *(bMatrix43*)((byte*)Header + 0x320); } }

        public int Tex6Unk1 { get { return ((sbyte*)Header)[0x350]; } }
        public int Tex6Unk2 { get { return ((sbyte*)Header)[0x351]; } }
        public int Tex6Unk3 { get { return ((sbyte*)Header)[0x352]; } }
        public int Tex6Unk4 { get { return ((sbyte*)Header)[0x353]; } }
        public bMatrix43 Tex6Mtx { get { return *(bMatrix43*)((byte*)Header + 0x354); } }

        public int Tex7Unk1 { get { return ((sbyte*)Header)[0x384]; } }
        public int Tex7Unk2 { get { return ((sbyte*)Header)[0x385]; } }
        public int Tex7Unk3 { get { return ((sbyte*)Header)[0x386]; } }
        public int Tex7Unk4 { get { return ((sbyte*)Header)[0x387]; } }
        public bMatrix43 Tex7Mtx { get { return *(bMatrix43*)((byte*)Header + 0x388); } }

        public int Tex8Unk1 { get { return ((sbyte*)Header)[0x3B8]; } }
        public int Tex8Unk2 { get { return ((sbyte*)Header)[0x3B9]; } }
        public int Tex8Unk3 { get { return ((sbyte*)Header)[0x3BA]; } }
        public int Tex8Unk4 { get { return ((sbyte*)Header)[0x3BB]; } }
        public bMatrix43 Tex8Mtx { get { return *(bMatrix43*)((byte*)Header + 0x3BC); } }

        [Category("Material")]
        public string Unknown1 { get { return _unk1.ToString("X"); } }
        [Category("Material")]
        public byte Textures { get { return _numTextures; } set { _numTextures = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Layers { get { return _numLayers; } set { _numLayers = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag3 { get { return _flag3; } set { _flag3 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag4 { get { return _flag4; } set { _flag4 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public int Type { get { return _type; } set { _type = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag5 { get { return _flag5; } set { _flag5 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag6 { get { return _flag6; } set { _flag6 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag7 { get { return _flag7; } set { _flag7 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag8 { get { return _flag8; } set { _flag8 = value; SignalPropertyChange(); } }

        [Category("Material")]
        public int Unknown3 { get { return _unk3; } }
        [Category("Material")]
        public int Unknown4 { get { return _unk4; } }

        //[Category("Material")]
        //public int ShaderOffset { get { return Header->_shaderOffset; } }

        //[Category("Material")]
        //public int NumTextures { get { return Header->_numTextures; } }
        //[Category("Material")]
        //public int Part3Offset { get { return Header->_part3Offset; } }
        //[Category("Material")]
        //public int Part4Offset { get { return Header->_part4Offset; } }
        //[Category("Material")]
        //public int Part5Offset { get { return Header->_part5Offset; } }

        [Category("Material")]
        public int Unknown6 { get { return _unk6; } }

        //[Category("Material Part3")]
        //public List<MDL0Data7Part3> Part3Entries { get { return _part3Entries; } }
        [Category("Material Part4")]
        public List<string> Part4Entries { get { return _part4Entries; } }

        protected override bool OnInitialize()
        {
            MDL0Material* header = Header;

            if ((_name == null) && (header->_stringOffset != 0))
                _name = header->ResourceString;

            _numTextures = header->_flag1;
            _numLayers = header->_numLayers;
            _unk1 = header->_unk1;
            _unk3 = header->_unk3;
            _unk4 = header->_unk4;
            _unk6 = header->_unk6;
            _flag3 = header->_flag3;
            _flag4 = header->_flag4;
            _flag5 = header->_flag5;
            _flag6 = header->_flag6;
            _flag7 = header->_flag7;
            _flag8 = header->_flag8;
            _type = header->_type;


            MDL0Data7Part4* part4 = header->Part4;
            if (part4 != null)
            {
                ResourceGroup* group = part4->Group;
                for (int i = 0; i < group->_numEntries; i++)
                    _part4Entries.Add(group->First[i].GetName());
            }

            OnPopulate();
            return true;
        }

        protected override void OnPopulate()
        {
            MDL0MatLayer* part3 = Header->Part3;
            for (int i = 0; i < Header->_numTextures; i++)
                new MDL0MaterialRefNode().Initialize(this, part3++, MDL0MatLayer.Size);
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _part4Entries)
                table.Add(s);

            //foreach (MDL0MaterialRefNode n in Children)
            //    n.GetStrings(table);
        }

        //protected override int OnCalculateSize(bool force)
        //{

        //}

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Material* header = (MDL0Material*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;

            header->_flag1 = _numTextures;
            header->_numLayers = _numLayers;
            header->_flag3 = _flag3;
            header->_flag4 = _flag4;
            header->_flag5 = _flag5;
            header->_flag6 = _flag6;
            header->_flag7 = _flag7;
            header->_flag8 = _flag8;
            header->_type = _type;

            MDL0Data7Part4* part4 = header->Part4;
            if (part4 != null)
            {
                ResourceGroup* group = part4->Group;
                group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
                ResourceEntry* rEntry = group->First;

                for (int i = 0, x = 1; i < group->_numEntries; i++)
                {
                    MDL0Data7Part4Entry* entry = (MDL0Data7Part4Entry*)((int)group + (rEntry++)->_dataOffset);
                    ResourceEntry.Build(group, x++, entry, (BRESString*)stringTable[_part4Entries[i]]);
                    entry->ResourceStringAddress = stringTable[_part4Entries[i]] + 4;
                }
            }

            MDL0MatLayer* part3 = header->Part3;
            foreach (MDL0MaterialRefNode n in Children)
                n.PostProcess(mdlAddress, part3++, stringTable);
        }

        internal override void Bind(GLContext ctx) { foreach (MDL0MaterialRefNode m in Children) m.Bind(ctx); }
        internal override void Unbind(GLContext ctx) { foreach (MDL0MaterialRefNode m in Children) m.Unbind(ctx); }
    }
}
