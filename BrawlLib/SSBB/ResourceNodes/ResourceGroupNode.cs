using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    internal interface IResourceGroupNode
    {
        unsafe ResourceGroup* Group { get; }
    }

    //public unsafe class ResourceGroupNode : ResourceNode, IResourceGroupNode
    //{
    //    ResourceGroup* IResourceGroupNode.Group { get { return (ResourceGroup*)WorkingSource.Address; } }


    //}
}
