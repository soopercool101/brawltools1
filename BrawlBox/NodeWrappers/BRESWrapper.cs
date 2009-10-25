using System;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlBox
{
    [NodeWrapper(ResourceType.BRES)]
    class BRESWrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static BRESWrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("Ne&w", null,
                new ToolStripMenuItem("Texture", null, NewTextureAction)
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
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void NewTextureAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewTexture(); }
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ExportAll(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ReplaceAll(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[6].Enabled = _menu.Items[8].Enabled = _menu.Items[9].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESWrapper w = GetInstance<BRESWrapper>();
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

        public override string ExportFilter { get { return "Brres Resource Pack (*.brres)|*.brres"; } }

        public BRESWrapper() { ContextMenuStrip = _menu; }

        public void NewTexture()
        {
            using (TextureConverterDialog dlg = new TextureConverterDialog()) { dlg.ShowDialog(MainForm.Instance, ResourceNode as BRESNode); }
        }

        public void ExportAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((BRESNode)_resource).ExportToFolder(path);
        }

        public void ReplaceAll()
        {
            string path = Program.ChooseFolder();
            if (path == null)
                return;

            ((BRESNode)_resource).ReplaceFromFolder(path);
        }
    }
}
