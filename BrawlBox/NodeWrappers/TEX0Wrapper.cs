using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.TEX0)]
    class TEX0Wrapper : GenericWrapper
    {
        public override string ExportFilter { get { return ExportFilters.TEX0; } }
    }
}
