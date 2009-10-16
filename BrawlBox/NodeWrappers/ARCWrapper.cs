using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox
{
    [NodeWrapper(ResourceType.ARC)]
    class ARCWrapper : GenericWrapper
    {
        [NodeAction("Extract All...")]
        public void ExportToFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((ARCNode)_resource).ExtractToFolder(path);
        }

        [NodeAction("Replace All...")]
        public void ReplaceFromFolder()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((ARCNode)_resource).ReplaceFromFolder(path);
        }
    }
}
