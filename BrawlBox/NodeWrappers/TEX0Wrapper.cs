using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib;
using System.Windows.Forms;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.TEX0)]
    class TEX0Wrapper : GenericWrapper
    {
        public override string ExportFilter { get { return ExportFilters.TEX0; } }

        public override void Replace()
        {
            using (TextureConverterDialog dlg = new TextureConverterDialog())
            {
                dlg.ShowDialog(MainForm.Instance, ResourceNode as TEX0Node);
            }
        }
    }
}
