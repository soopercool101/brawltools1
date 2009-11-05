using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialRefNode : ResourceNode
    {
        internal MDL0Data7Part3* Header { get { return (MDL0Data7Part3*)_origSource.Address; } }

        int _unk1;
        int _unk2;
        int _unk3;
        int _unk4;
        int _unk5;
        int _unk6;
        int _unk7;
        int _unk8;
        int _unk9;
        int _unk10;
        int _unk11;
        float _float;

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
        public int Unknown6 { get { return _unk6; } set { _unk6 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown7 { get { return _unk7; } set { _unk7 = value; SignalPropertyChange(); } }
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
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _unk5 = Header->_unk5;
            _unk6 = Header->_unk6;
            _unk7 = Header->_unk7;
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
            header->_unk6 = _unk6;
            header->_unk7 = _unk7;
            header->_unk8 = _unk8;
            header->_unk9 = _unk9;
            header->_unk10 = _unk10;
            header->_unk11 = _unk11;
            header->_float = _float;
        }
    }
}
