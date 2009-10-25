using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlScape
{
    public class TextureContextMenuStrip : ContextMenuStrip
    {
        private ToolStripMenuItem _mnuReplace;
        private ToolStripMenuItem _mnuExport;
        private ToolStripMenuItem _mnuRestore;
        private ToolStripSeparator _sep1;
        private ToolStripMenuItem _mnuSize;
        private ToolStripMenuItem _mnuFormat;
        private ToolStripMenuItem _mnuPalette;
        private ToolStripMenuItem _mnuLOD;
        private ToolStripMenuItem _mnuFileSize;

        private TextureReference _ref;
        public TextureReference TextureReference
        {
            get { return _ref; }
            set { _ref = value; }
        }

        public TextureContextMenuStrip():base()
        {
            this.SuspendLayout();

            Items.Add(_mnuReplace = new ToolStripMenuItem("Replace...", null, OnReplace));
            Items.Add(_mnuExport = new ToolStripMenuItem("Export...", null, OnExport));
            Items.Add(_mnuRestore = new ToolStripMenuItem("Restore", null, OnRestore));
            Items.Add(_sep1 = new ToolStripSeparator());
            Items.Add(_mnuSize = new ToolStripMenuItem());
            Items.Add(_mnuFormat = new ToolStripMenuItem());
            Items.Add(_mnuPalette = new ToolStripMenuItem());
            Items.Add(_mnuLOD = new ToolStripMenuItem());
            Items.Add(_mnuFileSize = new ToolStripMenuItem());

            this.ResumeLayout();
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if ((_ref == null) || (_ref.Node == null))
                e.Cancel = true;
            else
            {
                TEX0Node node = (TEX0Node)_ref.Node;

                _mnuSize.Text = String.Format("Size: {0} x {1}", node.Width, node.Height);
                _mnuFormat.Text = String.Format("Format: {0}", node.Format);

                PLT0Node pNode = node.GetPaletteNode();
                if (pNode == null)
                    _mnuPalette.Text = "Palette: None";
                else
                    _mnuPalette.Text = String.Format("Palette: {0}, {1} colors", pNode.Format, pNode.Colors);

                _mnuLOD.Text = String.Format("LOD: {0}", node.LevelOfDetail);
                _mnuFileSize.Text = String.Format("File Size: {0}", node.WorkingUncompressed.Length - 0x40);
            }
        }

        protected virtual void OnReplace(object s, EventArgs e) { _ref.Replace(); }
        protected virtual void OnExport(object s, EventArgs e) { _ref.Export(); }
        protected virtual void OnRestore(object s, EventArgs e) { _ref.Restore(); }
    }
}
