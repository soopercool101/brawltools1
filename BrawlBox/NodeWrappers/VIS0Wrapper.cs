using System;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox
{
    [NodeWrapper(ResourceType.VIS0)]
    class VIS0Wrapper : GenericWrapper
    {
        public override string ExportFilter { get { return ExportFilters.VIS0; } }
    }
}
