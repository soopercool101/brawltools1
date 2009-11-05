using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARSoundNode : RSAREntryNode
    {
        internal INFOData1Part1* Header { get { return (INFOData1Part1*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_stringId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARSound; } }
    }
}
