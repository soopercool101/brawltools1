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

        [Category("Texture Reference")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("Texture Reference")]
        public int Unknown2 { get { return Header->_unk2; } }
        [Category("Texture Reference")]
        public int Unknown3 { get { return Header->_unk3; } }
        [Category("Texture Reference")]
        public int Unknown4 { get { return Header->_unk4; } }
        [Category("Texture Reference")]
        public int Unknown5 { get { return Header->_unk5; } }
        [Category("Texture Reference")]
        public int Unknown6 { get { return Header->_unk6; } }
        [Category("Texture Reference")]
        public int Unknown7 { get { return Header->_unk7; } }
        [Category("Texture Reference")]
        public int Unknown8 { get { return Header->_unk8; } }
        [Category("Texture Reference")]
        public int Unknown9 { get { return Header->_unk9; } }
        [Category("Texture Reference")]
        public float Float { get { return Header->_float; } }
        [Category("Texture Reference")]
        public int Unknown10 { get { return Header->_unk10; } }
        [Category("Texture Reference")]
        public int Unknown11 { get { return Header->_unk11; } }

        protected override bool OnInitialize()
        {
            Name = Header->TextureName;
            return false;
        }
    }
}
