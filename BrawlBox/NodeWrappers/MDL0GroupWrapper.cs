using System;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Group)]
    class MDL0GroupWrapper : GenericWrapper
    {
        public MDL0GroupWrapper() { ContextMenuStrip = null; }
    }
}
