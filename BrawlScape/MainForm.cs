using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlScape
{
    public partial class MainForm : Form
    {
        public static TextureReference _currentTextureNode;
        public static ContextMenuStrip _textureContext;

        public MainForm()
        {
            InitializeComponent();
            Text = Program.AssemblyTitle;
            _textureContext = textureContext;
        }

        private void textureContext_Opening(object sender, CancelEventArgs e)
        {
            if (_currentTextureNode == null)
                e.Cancel = true;
            else
            {
                TEX0Node node = (TEX0Node)_currentTextureNode.Node;

                mnuFileSize.Text = String.Format("File Size: {0}", node.WorkingSource.Length - 0x40);
                mnuFormat.Text = String.Format("Format: {0}", node.Format);
                mnuLOD.Text = String.Format("LOD: {0}", node.LevelOfDetail);
                mnuSize.Text = String.Format("Size: {0} x {1}", node.Width, node.Height);

                PLT0Node pNode = node.GetPaletteNode();
                if (pNode == null)
                    mnuPalette.Text = "Palette: None";
                else
                    mnuPalette.Text = String.Format("Palette: {0}, {1} colors", pNode.Format, pNode.Colors);
            }
        }

        private void mnuReplace_Click(object sender, EventArgs e) { _currentTextureNode.Replace(); }
        private void mnuExport_Click(object sender, EventArgs e) { _currentTextureNode.Export(); }
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e) { _currentTextureNode.Restore(); }
    }
}
