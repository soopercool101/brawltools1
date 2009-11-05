using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARBankNode : RSAREntryNode
    {
        internal INFOData2* Header { get { return (INFOData2*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_stringId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARBank; } }
    }
}
