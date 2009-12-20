using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDSoundNode : RWSDEntryNode
    {
        internal RWSD_WAVEEntry* Header { get { return (RWSD_WAVEEntry*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            if (_name == null)
                _name = string.Format("Audio[{0:X2}]", Index);

            return false;
        }
    }
}
