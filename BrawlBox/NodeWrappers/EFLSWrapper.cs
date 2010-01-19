using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;

namespace BrawlBox
{
    [NodeWrapper(ResourceType.EFLS)]
    class EFLSWrapper : GenericWrapper
    {
        public override string ExportFilter { get { return ExportFilters.EFLS; } }
    }
}
