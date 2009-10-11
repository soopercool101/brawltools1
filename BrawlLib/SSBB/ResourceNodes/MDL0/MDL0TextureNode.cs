using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Data10Node : MDL0EntryNode
    {
        internal MDL0Data10* Data { get { return (MDL0Data10*)WorkingSource.Address; } }
        internal MDL0Data10Entry[] _entries;

        [Category("Data10")]
        public int NumEntries { get { return Data->_numEntries; } }
        [Category("Data10")]
        public MDL0Data10Entry[] Entries { get { return _entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _entries = new MDL0Data10Entry[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                _entries[i] = Data->Entries[i];
            }

            return false;
        }
    }
}
