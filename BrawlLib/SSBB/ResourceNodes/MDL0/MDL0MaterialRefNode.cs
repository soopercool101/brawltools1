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
using System.Drawing.Imaging;
using BrawlLib.Modeling;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialRefNode : MDL0EntryNode
    {
        internal MDL0MatLayer* Header { get { return (MDL0MatLayer*)_origSource.Address; } }

        internal int _unk2;
        internal int _unk3;
        internal int _unk4;
        internal int _unk5;
        internal int _layerId1;
        internal int _layerId2;
        internal int _unk8;
        internal int _unk9;
        internal int _unk10;
        internal int _unk11;
        internal float _float;

        #region Texture linkage
        internal MDL0TextureNode _texture;
        [Browsable(false)]
        public MDL0TextureNode TextureNode
        {
            get { return _texture; }
            set
            {
                if (_texture == value)
                    return;
                if (_texture != null)
                    _texture._texRefs.Remove(this);
                if ((_texture = value) != null)
                {
                    _texture._texRefs.Add(this);
                    Name = _texture._name;
                }
                Model.SignalPropertyChange();
            }
        }
        public string Texture
        {
            get { return _texture == null ? null : _texture._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    TextureNode = null;
                else
                {
                    MDL0TextureNode node = Model.FindChild(String.Format("Textures/{0}", value), false) as MDL0TextureNode;
                    if (node != null)
                        TextureNode = node;
                }
            }
        }
        #endregion

        #region Decal linkage
        internal MDL0TextureNode _decal;
        [Browsable(false)]
        public MDL0TextureNode DecalNode
        {
            get { return _decal; }
            set
            {
                if (_decal == value)
                    return;
                if (_decal != null)
                    _decal._decRefs.Remove(this);
                if ((_decal = value) != null)
                    _decal._decRefs.Add(this);
                Model.SignalPropertyChange();
            }
        }
        public string Decal
        {
            get { return _decal == null ? null : _decal._name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    DecalNode = null;
                else
                {
                    MDL0TextureNode node = Model.FindChild(String.Format("Textures/{0}", value), false) as MDL0TextureNode;
                    if (node != null)
                        DecalNode = node;
                }
            }
        }
        #endregion

        public override string Name
        {
            get { return _texture != null ? _texture.Name : base.Name; }
            set { base.Name = value; }
        }

        //[Category("Texture Reference")]
        //public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public string DecalTexture { get { return _decal == null ? null : _decal._name; } }//set { _secondaryTexture = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int LayerId1 { get { return _layerId1; } set { _layerId1 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int LayerId2 { get { return _layerId2; } set { _layerId2 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown8 { get { return _unk8; } set { _unk8 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown9 { get { return _unk9; } set { _unk9 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public float Float { get { return _float; } set { _float = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown10 { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown11 { get { return _unk11; } set { _unk11 = value; SignalPropertyChange(); } }


        protected override bool OnInitialize()
        {
            MDL0MatLayer* header = Header;

            _unk2 = header->_unk2;
            _unk3 = header->_unk3;
            _unk4 = header->_unk4;
            _unk5 = header->_unk5;
            _layerId1 = header->_layerId1;
            _layerId2 = header->_layerId2;
            _unk8 = header->_unk8;
            _unk9 = header->_unk9;
            _float = header->_float;
            _unk10 = header->_unk10;
            _unk11 = header->_unk11;

            if ((_name == null) && (header->_stringOffset != 0))
                _name = header->ResourceString;
            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            //if (!String.IsNullOrEmpty(_secondaryTexture))
            //    table.Add(_secondaryTexture);
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0MatLayer* header = (MDL0MatLayer*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            if (_decal != null)
                header->SecondaryTextureAddress = stringTable[_decal.Name] + 4;
            else
                header->_secondaryOffset = 0;

            //header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
            header->_unk5 = _unk5;
            header->_layerId1 = _layerId1;
            header->_layerId2 = _layerId2;
            header->_unk8 = _unk8;
            header->_unk9 = _unk9;
            header->_unk10 = _unk10;
            header->_unk11 = _unk11;
            header->_float = _float;
        }

        internal override void Bind(GLContext ctx)
        {
            if (_texture != null)
                _texture.Prepare(ctx);
        }

        internal override void Unbind(GLContext ctx)
        {
            if (_texture != null)
                _texture.Unbind(ctx);
        }
    }
}
