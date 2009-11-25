using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Modeling;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0TextureNode : MDL0EntryNode
    {
        internal MDL0Texture* Header { get { return (MDL0Texture*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return (Header->_numEntries * 8) + 4; } }

        internal MDL0TextureEntry[] _entries;

        [Category("Data10")]
        public int NumEntries { get { return Header->_numEntries; } }
        [Category("Data10")]
        public MDL0TextureEntry[] Entries { get { return _entries; } }

        internal TextureRef _textureReference;

        protected override bool OnInitialize()
        {
            //_name = Data->

            _entries = new MDL0TextureEntry[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                _entries[i] = Header->Entries[i];
            }

            return false;
        }
    }
}
