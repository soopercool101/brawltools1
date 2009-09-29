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
        internal PLT0* Header { get { return (PLT0*)WorkingSource.Address; } }

        public override int DataAlign { get { return 0x10; } }

        [Category("Palette")]
        public int Colors { get { return Header->_numEntries; } }
        [Category("Palette")]
        public WiiPaletteFormat Format { get { return Header->PaletteFormat; } }

        protected internal override void OnAfterRebuild(IDictionary<string, VoidPtr> strings)
        {
            base.OnAfterRebuild(strings);
            Header->ResourceStringAddress = strings[Name];
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((PLT0*)address)->_bresEntry._tag == PLT0.Tag ? new PLT0Node() : null; }
    }
}
