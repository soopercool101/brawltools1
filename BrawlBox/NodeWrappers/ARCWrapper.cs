using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlBox
{
    [NodeWrapper(ResourceType.ARC)]
    class ARCWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static ARCWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("ARChive", null, NewARCAction),
                new ToolStripMenuItem("Brres package", null, NewBRESAction)
                ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Delete));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewBRESAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewBRES(); }
        protected static void NewARCAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().NewARC(); }
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ExportAll(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<ARCWrapper>().ReplaceAll(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[6].Enabled = _menu.Items[8].Enabled = _menu.Items[9].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            ARCWrapper w = GetInstance<ARCWrapper>();
            if (w.Parent == null)
                _menu.Items[6].Enabled = _menu.Items[9].Enabled = false;
            else
                _menu.Items[6].Enabled = _menu.Items[9].Enabled = true;

            if ((w._resource.IsDirty) || (w._resource.IsBranch))
                _menu.Items[8].Enabled = true;
            else
                _menu.Items[8].Enabled = false;
        }
        #endregion

        public override string ExportFilter
        {
            get
            {
                return "PAC Archive (*.pac)|*.pac|" +
                    "Compressed PAC Archive (*.pcs)|*.pcs|" +
                    "Archive Pair (*.pair)|*.pair";
            }
        }

        public ARCWrapper() { ContextMenuStrip = _menu; }

        public void NewARC() { _resource.AddChild(new ARCNode() { Name = "NewARChive", FileType = ARCFileType.MiscData }); Expand(); }
        public void NewBRES() { _resource.AddChild(new BRESNode() { FileType = ARCFileType.TextureData }); Expand(); }

        public override void OnExport(string outPath, int filterIndex)
        {
            switch (filterIndex)
            {
                case 1: ((ARCNode)_resource).ExportPAC(outPath); break;
                case 2: ((ARCNode)_resource).ExportPCS(outPath); break;
                case 3: ((ARCNode)_resource).ExportPair(outPath); break;
            }
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((ARCNode)_resource).ExtractToFolder(path);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((ARCNode)_resource).ReplaceFromFolder(path);
        }
    }
}
