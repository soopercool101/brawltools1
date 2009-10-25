using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Data10Node : MDL0EntryNode
    {
        internal MDL0Data10* Header { get { return (MDL0Data10*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return (Header->_numEntries * 8) + 4; } }

        internal MDL0Data10Entry[] _entries;

        [Category("Data10")]
        public int NumEntries { get { return Header->_numEntries; } }
        [Category("Data10")]
        public MDL0Data10Entry[] Entries { get { return _entries; } }

        protected override bool OnInitialize()
        {
            //_name = Data->

            _entries = new MDL0Data10Entry[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                _entries[i] = Header->Entries[i];
            }

            return false;
        }
    }
}
