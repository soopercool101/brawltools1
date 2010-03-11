using System;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib;

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
                //new ToolStripMenuItem("Texture", null, NewTextureAction),
                new ToolStripMenuItem("Model", null, NewModelAction),
                new ToolStripMenuItem("Character Animation", null, NewChrAction)
                ));
            _menu.Items.Add(new ToolStripMenuItem("&Import", null,
                new ToolStripMenuItem("Texture", null, ImportTextureAction),
                new ToolStripMenuItem("Model", null, ImportModelAction),
                new ToolStripMenuItem("Character Animation", null, ImportChrAction)
                ));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Export All", null, ExportAllAction));
            _menu.Items.Add(new ToolStripMenuItem("Replace All", null, ReplaceAllAction));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Move &Up", null, MoveUpAction, Keys.Control | Keys.Up));
            _menu.Items.Add(new ToolStripMenuItem("Move D&own", null, MoveDownAction, Keys.Control | Keys.Down));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Control | Keys.Delete));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void ImportTextureAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportTexture(); }
        protected static void ImportModelAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportModel(); }
        protected static void ImportChrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ImportChr(); }
        protected static void NewModelAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewModel(); }
        protected static void NewChrAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().NewChr(); }
        protected static void ExportAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ExportAll(); }
        protected static void ReplaceAllAction(object sender, EventArgs e) { GetInstance<BRESWrapper>().ReplaceAll(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[7].Enabled = _menu.Items[8].Enabled = _menu.Items[10].Enabled = _menu.Items[11].Enabled = _menu.Items[13].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            BRESWrapper w = GetInstance<BRESWrapper>();

            _menu.Items[7].Enabled = _menu.Items[13].Enabled = w.Parent != null;
            _menu.Items[8].Enabled = ((w._resource.IsDirty) || (w._resource.IsBranch));
            _menu.Items[10].Enabled = w.PrevNode != null;
            _menu.Items[11].Enabled = w.NextNode != null;
        }

        #endregion

        public override string ExportFilter { get { return ExportFilters.BRES; } }

        public BRESWrapper() { ContextMenuStrip = _menu; }

        public void ImportTexture()
        {
            string path;
            int index = Program.OpenFile(ExportFilters.TEX0, out path);
            if (index == 8)
            {
                TEX0Node node = NodeFactory.FromFile(null, path) as TEX0Node;
                ((BRESNode)_resource).GetOrCreateFolder<TEX0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
            else if (index > 0)
                using (TextureConverterDialog dlg = new TextureConverterDialog())
                {
                    dlg.ImageSource = path;
                    if (dlg.ShowDialog(MainForm.Instance, ResourceNode as BRESNode) == DialogResult.OK)
                    {
                        BaseWrapper w = this.FindResource(dlg.TextureNode, true);
                        w.EnsureVisible();
                        w.TreeView.SelectedNode = w;
                    }
                }
        }
        public void ImportModel()
        {
            string path;
            if (Program.OpenFile(ExportFilters.MDL0, out path) > 0)
            {
                MDL0Node node = MDL0Node.FromFile(path);
                if (node != null)
                {
                    ((BRESNode)_resource).GetOrCreateFolder<MDL0Node>().AddChild(node);

                    BaseWrapper w = this.FindResource(node, true);
                    w.EnsureVisible();
                    w.TreeView.SelectedNode = w;
                }
            }
        }
        public void ImportChr()
        {
            string path;
            if (Program.OpenFile(ExportFilters.CHR0, out path) > 0)
            {
                CHR0Node node = NodeFactory.FromFile(null, path) as CHR0Node;
                ((BRESNode)_resource).GetOrCreateFolder<CHR0Node>().AddChild(node);

                BaseWrapper w = this.FindResource(node, true);
                w.EnsureVisible();
                w.TreeView.SelectedNode = w;
            }
        }
        public void NewChr()
        {
            CHR0Node node = ((BRESNode)_resource).CreateResource<CHR0Node>("NewCHR");
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
        }
        public void NewModel()
        {
            MDL0Node node = ((BRESNode)_resource).CreateResource<MDL0Node>("NewModel");
            BaseWrapper res = this.FindResource(node, true);
            res = res.FindResource(node, false);
            res.EnsureVisible();
            res.TreeView.SelectedNode = res;
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
