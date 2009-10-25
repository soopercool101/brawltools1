using System;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox
{
    [NodeWrapper(ResourceType.BRESGroup)]
    class BRESGroupWrapper : GenericWrapper
    {
        public BRESGroupWrapper() { ContextMenuStrip = null; }
    }
}
