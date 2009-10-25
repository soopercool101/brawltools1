using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    unsafe class MDL0MaterialExtNode : MDL0EntryNode
    {
        internal MDL0Data8* Header { get { return (MDL0Data8*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_dataLength; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            return false;
        }
    }
}
