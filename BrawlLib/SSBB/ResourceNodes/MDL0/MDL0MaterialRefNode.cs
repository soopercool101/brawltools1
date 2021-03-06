﻿using System;
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
        internal TextureRef _texture;
        [Browsable(false)]
        public TextureRef TextureNode
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
                    Name = _texture.Name;
                }
                Model.SignalPropertyChange();
            }
        }
        public string Texture
        {
            get { return _texture == null ? null : _texture.Name; }
            set { TextureNode = String.IsNullOrEmpty(value) ? null : Model._textures.FindOrCreate(value); }
        }
        #endregion

        #region Decal linkage
        internal TextureRef _decal;
        [Browsable(false)]
        public TextureRef DecalNode
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
            get { return _decal == null ? null : _decal.Name; }
            set { DecalNode = String.IsNullOrEmpty(value) ? null : Model._textures.FindOrCreate(value); }
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
        public string DecalTexture { get { return _decal == null ? null : _decal.Name; } }//set { _secondaryTexture = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Index1 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Index2 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
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

            if (header->_stringOffset != 0)
            {
                _name = header->ResourceString;
                _texture = Model._textures.FindOrCreate(_name);
                _texture._texRefs.Add(this);
            }
            if (header->_secondaryOffset != 0)
            {
                string name = header->SecondaryTexture;
                _decal = Model._textures.FindOrCreate(name);
                _decal._decRefs.Add(this);
            }

            return false;
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            if (_decal != null)
                table.Add(_decal.Name);
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0MatLayer* header = (MDL0MatLayer*)dataAddress;
            header->_stringOffset = (int)stringTable[Name] + 4 - (int)dataAddress;

            if (_decal != null)
                header->_secondaryOffset = (int)stringTable[_decal.Name] + 4 - (int)dataAddress;
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
                _texture.Unbind();
        }
    }
}
