using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    unsafe class MDL0MaterialExtNode : MDL0EntryNode
    {
        internal MDL0Data8* Data { get { return (MDL0Data8*)WorkingSource.Address; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
                _origSource.Length = _uncompSource.Length = Data->_dataLength;

            return false;
        }
    }
}
