using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.CHR0)]
    class CHR0Wrapper : GenericWrapper
    {
        public override string ExportFilter { get { return ExportFilters.CHR0; } }
    }
}
