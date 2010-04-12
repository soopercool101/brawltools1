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

        [Category("Alpha Function")]
        public int Ref0 { get { return _alphaFunc.ref0; } }
        [Category("Alpha Function")]
        public AlphaCompare Comp0 { get { return _alphaFunc.Comp0; } }
        [Category("Alpha Function")]
        public AlphaOp Logic { get { return _alphaFunc.Logic; } }
        [Category("Alpha Function")]
        public int Ref1 { get { return _alphaFunc.ref1; } }
        [Category("Alpha Function")]
        public AlphaCompare Comp1 { get { return _alphaFunc.Comp1; } }

        [Category("Z Mode")]
        public bool EnableDepthTest { get { return _zMode.EnableDepthTest; } }
        [Category("Z Mode")]
        public bool EnableDepthUpdate { get { return _zMode.EnableDepthUpdate; } }
        [Category("Z Mode")]
        public GXCompare DepthFunction { get { return _zMode.DepthFunction; } }

        [Category("Blend Mode")]
        public bool EnableBlend { get { return _blendMode.EnableBlend; } }
        [Category("Blend Mode")]
        public bool EnableBlendLogic { get { return _blendMode.EnableLogicOp; } }
        //These are disabled via mask
        //[Category("Blend Mode")]
        //public bool EnableDither { get { return _blendMode.EnableDither; } }
        //[Category("Blend Mode")]
        //public bool EnableColorUpdate { get { return _blendMode.EnableColorUpdate; } }
        //[Category("Blend Mode")]
        //public bool EnableAlphaUpdate { get { return _blendMode.EnableAlphaUpdate; } }

        [Category("Blend Mode")]
        public BlendFactor SrcFactor { get { return _blendMode.SrcFactor; } }
        [Category("Blend Mode")]
        public GXLogicOp BlendLogicOp { get { return _blendMode.LogicOp; } }
        [Category("Blend Mode")]
        public BlendFactor DstFactor { get { return _blendMode.DstFactor; } }

        [Category("Blend Mode")]
        public bool Subtract { get { return _blendMode.Subtract; } }

        //[Category("Material")]
        //public int TotalLen { get { return Header->_dataLen; } }
        //[Category("Material")]
        //public int MDL0Offset { get { return Header->_mdl0Offset; } }
        //[Category("Material")]
        //public int StringOffset { get { return Header->_stringOffset; } }
        //[Category("Material")]
        //public int ID { get { return Header->_index; } }

        //Usage flags? Each set of 4 bits represents one texture layer. They are either on or off (0/F).
        public int UnkFlags1 { get { return ((bint*)Header)[105]; } }
        public int UnkFlags2 { get { return ((bint*)Header)[106]; } }

        //public float Tex1X { get { return ((bfloat*)Header)[107]; } }
        //public float Tex1Y { get { return ((bfloat*)Header)[108]; } }
        //public float Tex1Z { get { return ((bfloat*)Header)[109]; } }
        //public float Tex1A { get { return ((bfloat*)Header)[110]; } }
        //public float Tex1B { get { return ((bfloat*)Header)[111]; } }

        //public float Tex2X { get { return ((bfloat*)Header)[112]; } }
        //public float Tex2Y { get { return ((bfloat*)Header)[113]; } }
        //public float Tex2Z { get { return ((bfloat*)Header)[114]; } }
        //public float Tex2A { get { return ((bfloat*)Header)[115]; } }
        //public float Tex2B { get { return ((bfloat*)Header)[116]; } }

        //public float Tex3X { get { return ((bfloat*)Header)[117]; } }
        //public float Tex3Y { get { return ((bfloat*)Header)[118]; } }
        //public float Tex3Z { get { return ((bfloat*)Header)[119]; } }
        //public float Tex3A { get { return ((bfloat*)Header)[120]; } }
        //public float Tex3B { get { return ((bfloat*)Header)[121]; } }

        //public float Tex4X { get { return ((bfloat*)Header)[122]; } }
        //public float Tex4Y { get { return ((bfloat*)Header)[123]; } }
        //public float Tex4Z { get { return ((bfloat*)Header)[124]; } }
        //public float Tex4A { get { return ((bfloat*)Header)[125]; } }
        //public float Tex4B { get { return ((bfloat*)Header)[126]; } }

        //public float Tex5X { get { return ((bfloat*)Header)[127]; } }
        //public float Tex5Y { get { return ((bfloat*)Header)[128]; } }
        //public float Tex5Z { get { return ((bfloat*)Header)[129]; } }
        //public float Tex5A { get { return ((bfloat*)Header)[130]; } }
        //public float Tex5B { get { return ((bfloat*)Header)[131]; } }

        //public float Tex6X { get { return ((bfloat*)Header)[132]; } }
        //public float Tex6Y { get { return ((bfloat*)Header)[133]; } }
        //public float Tex6Z { get { return ((bfloat*)Header)[134]; } }
        //public float Tex6A { get { return ((bfloat*)Header)[135]; } }
        //public float Tex6B { get { return ((bfloat*)Header)[136]; } }

        //public float Tex7X { get { return ((bfloat*)Header)[137]; } }
        //public float Tex7Y { get { return ((bfloat*)Header)[138]; } }
        //public float Tex7Z { get { return ((bfloat*)Header)[139]; } }
        //public float Tex7A { get { return ((bfloat*)Header)[140]; } }
        //public float Tex7B { get { return ((bfloat*)Header)[141]; } }

        //public float Tex8X { get { return ((bfloat*)Header)[142]; } }
        //public float Tex8Y { get { return ((bfloat*)Header)[143]; } }
        //public float Tex8Z { get { return ((bfloat*)Header)[144]; } }
        //public float Tex8A { get { return ((bfloat*)Header)[145]; } }
        //public float Tex8B { get { return ((bfloat*)Header)[146]; } }

        //public int Tex1Unk1 { get { return ((sbyte*)Header)[0x24C]; } }
        //public int Tex1Unk2 { get { return ((sbyte*)Header)[0x24D]; } }
        //public int Tex1Unk3 { get { return ((sbyte*)Header)[0x24E]; } }
        //public int Tex1Unk4 { get { return ((sbyte*)Header)[0x24F]; } }
        //public bMatrix43 Tex1Mtx { get { return *(bMatrix43*)((byte*)Header + 0x250); } }

        //public int Tex2Unk1 { get { return ((sbyte*)Header)[0x280]; } }
        //public int Tex2Unk2 { get { return ((sbyte*)Header)[0x281]; } }
        //public int Tex2Unk3 { get { return ((sbyte*)Header)[0x282]; } }
        //public int Tex2Unk4 { get { return ((sbyte*)Header)[0x283]; } }
        //public bMatrix43 Tex2Mtx { get { return *(bMatrix43*)((byte*)Header + 0x284); } }

        //public int Tex3Unk1 { get { return ((sbyte*)Header)[0x2B4]; } }
        //public int Tex3Unk2 { get { return ((sbyte*)Header)[0x2B5]; } }
        //public int Tex3Unk3 { get { return ((sbyte*)Header)[0x2B6]; } }
        //public int Tex3Unk4 { get { return ((sbyte*)Header)[0x2B7]; } }
        //public bMatrix43 Tex3Mtx { get { return *(bMatrix43*)((byte*)Header + 0x2B8); } }

        //public int Tex4Unk1 { get { return ((sbyte*)Header)[0x2E8]; } }
        //public int Tex4Unk2 { get { return ((sbyte*)Header)[0x2E9]; } }
        //public int Tex4Unk3 { get { return ((sbyte*)Header)[0x2EA]; } }
        //public int Tex4Unk4 { get { return ((sbyte*)Header)[0x2EB]; } }
        //public bMatrix43 Tex4Mtx { get { return *(bMatrix43*)((byte*)Header + 0x2EC); } }

        //public int Tex5Unk1 { get { return ((sbyte*)Header)[0x31C]; } }
        //public int Tex5Unk2 { get { return ((sbyte*)Header)[0x31D]; } }
        //public int Tex5Unk3 { get { return ((sbyte*)Header)[0x31E]; } }
        //public int Tex5Unk4 { get { return ((sbyte*)Header)[0x31F]; } }
        //public bMatrix43 Tex5Mtx { get { return *(bMatrix43*)((byte*)Header + 0x320); } }

        //public int Tex6Unk1 { get { return ((sbyte*)Header)[0x350]; } }
        //public int Tex6Unk2 { get { return ((sbyte*)Header)[0x351]; } }
        //public int Tex6Unk3 { get { return ((sbyte*)Header)[0x352]; } }
        //public int Tex6Unk4 { get { return ((sbyte*)Header)[0x353]; } }
        //public bMatrix43 Tex6Mtx { get { return *(bMatrix43*)((byte*)Header + 0x354); } }

        //public int Tex7Unk1 { get { return ((sbyte*)Header)[0x384]; } }
        //public int Tex7Unk2 { get { return ((sbyte*)Header)[0x385]; } }
        //public int Tex7Unk3 { get { return ((sbyte*)Header)[0x386]; } }
        //public int Tex7Unk4 { get { return ((sbyte*)Header)[0x387]; } }
        //public bMatrix43 Tex7Mtx { get { return *(bMatrix43*)((byte*)Header + 0x388); } }

        //public int Tex8Unk1 { get { return ((sbyte*)Header)[0x3B8]; } }
        //public int Tex8Unk2 { get { return ((sbyte*)Header)[0x3B9]; } }
        //public int Tex8Unk3 { get { return ((sbyte*)Header)[0x3BA]; } }
        //public int Tex8Unk4 { get { return ((sbyte*)Header)[0x3BB]; } }
        //public bMatrix43 Tex8Mtx { get { return *(bMatrix43*)((byte*)Header + 0x3BC); } }

        public RGBAPixel Col00 { get { return ((RGBAPixel*)Header)[0xFB]; } }
        public RGBAPixel Col01 { get { return ((RGBAPixel*)Header)[0xFC]; } }
        public RGBAPixel Col02 { get { return ((RGBAPixel*)Header)[0xFD]; } }
        public RGBAPixel Col03 { get { return ((RGBAPixel*)Header)[0xFE]; } }
        public RGBAPixel Col04 { get { return ((RGBAPixel*)Header)[0xFF]; } }
        public RGBAPixel Col10 { get { return ((RGBAPixel*)Header)[0x100]; } }
        public RGBAPixel Col11 { get { return ((RGBAPixel*)Header)[0x101]; } }
        public RGBAPixel Col12 { get { return ((RGBAPixel*)Header)[0x102]; } }
        public RGBAPixel Col13 { get { return ((RGBAPixel*)Header)[0x103]; } }
        public RGBAPixel Col14 { get { return ((RGBAPixel*)Header)[0x104]; } }

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

            MatModeBlock* mode = header->DisplayLists;
            _alphaFunc = mode->AlphaFunction;
            _zMode = mode->ZMode;
            _blendMode = mode->BlendMode;
            _constantAlpha = mode->ConstantAlpha;

            MDL0Data7Part4* part4 = header->Part4;
            if (part4 != null)
            {
                ResourceGroup* group = part4->Group;
                for (int i = 0; i < group->_numEntries; i++)
                    _part4Entries.Add(group->First[i].GetName());
            }

            _children = new List<ResourceNode>();
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
