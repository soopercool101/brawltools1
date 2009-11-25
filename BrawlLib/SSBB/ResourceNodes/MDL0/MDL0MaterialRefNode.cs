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
    public unsafe class MDL0MaterialRefNode : ResourceNode
    {
        internal MDL0Data7Part3* Header { get { return (MDL0Data7Part3*)_origSource.Address; } }

        internal int _unk1;
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

        [Category("Texture Reference")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
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

        internal TextureRef _textureReference;

        protected override bool OnInitialize()
        {
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _unk5 = Header->_unk5;
            _layerId1 = Header->_layerId1;
            _layerId2 = Header->_layerId2;
            _unk8 = Header->_unk8;
            _unk9 = Header->_unk9;
            _float = Header->_float;
            _unk10 = Header->_unk10;
            _unk11 = Header->_unk11;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            return false;
        }

        internal unsafe void GetStrings(StringTable table)
        {
            table.Add(Name);
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Data7Part3* header = (MDL0Data7Part3*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            header->_unk1 = _unk1;
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


        internal void Bind(GLContext ctx)
        {
            if (_textureReference != null)
                _textureReference.Prepare(ctx);
        }

        internal void Unbind(GLContext ctx)
        {
            if (_textureReference != null)
                _textureReference.Unbind();
        }
    }
}
