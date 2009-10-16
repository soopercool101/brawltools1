using System;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.BRES)]
    class BRESWrapper : GenericWrapper
    {
        [NodeAction("Extract All...")]
        public void ExportToFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((BRESNode)_resource).ExportToFolder(path);
        }

        [NodeAction("Replace All...")]
        public void ReplaceFromFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((BRESNode)_resource).ReplaceFromFolder(path);
        }
    }
}
