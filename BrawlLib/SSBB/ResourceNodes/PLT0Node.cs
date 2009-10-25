using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Textures;
using System.Collections.Generic;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class PLT0Node : BRESEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.PLT0; } }
        internal PLT0* Header { get { return (PLT0*)WorkingUncompressed.Address; } }

        public override int DataAlign { get { return 0x10; } }

        private int _numColors;
        private WiiPaletteFormat _format;

        [Category("Palette")]
        public int Colors { get { return _numColors; } set { _numColors = value; } }
        [Category("Palette")]
        public WiiPaletteFormat Format { get { return _format; } set { _format = value; } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if(Header->_stringOffset != 0)
                _name = Header->ResourceString;

            _numColors = Header->_numEntries;
            _format = Header->PaletteFormat;

            return false;
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength, StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            PLT0* header = (PLT0*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((PLT0*)address)->_bresEntry._tag == PLT0.Tag ? new PLT0Node() : null; }
    }
}
