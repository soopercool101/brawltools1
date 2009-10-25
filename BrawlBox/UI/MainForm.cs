using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;
using System.Reflection;

namespace BrawlBox
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance { get { return _instance == null ? _instance = new MainForm() : _instance; } }

        private BaseWrapper _root;
        public BaseWrapper RootNode { get { return _root; } }

        public MainForm()
        {
            InitializeComponent();
            this.Text = Program.AssemblyTitle;
        }

        public void Reset()
        {
            _root = null;
            resourceTree.SelectedNode = null;
            resourceTree.Clear();

            //propertyGrid1.SelectedObject = null;
            //if (previewPanel1.Picture != null)
            //{
            //    previewPanel1.Picture.Dispose();
            //    previewPanel1.Picture = null;
            //}
            //audioPlaybackControl1.TargetObject = null;
            //splitContainer2.Panel2Collapsed = true;

            if (Program.RootNode != null)
            {
                this.Text = String.Format("{0} - {1}", Program.AssemblyTitle, Program.RootPath);

                _root = BaseWrapper.Wrap(Program.RootNode);
                resourceTree.BeginUpdate();
                resourceTree.Nodes.Add(_root);
                resourceTree.SelectedNode = _root;
                _root.Expand();
                resourceTree.EndUpdate();

                closeToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.Text = Program.AssemblyTitle;

                closeToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
            }
        }

        public void TargetResource(ResourceNode n)
        {
            if (_root != null)
                resourceTree.SelectedNode = _root.FindResource(n, true);
        }

        private void resourceTree_SelectionChanged(object sender, EventArgs e)
        {
            if (previewPanel1.Picture != null)
            {
                previewPanel1.Picture.Dispose();
                previewPanel1.Picture = null;
            }

            BaseWrapper w;
            ResourceNode node;
            if ((resourceTree.SelectedNode is BaseWrapper) && ((node = (w = resourceTree.SelectedNode as BaseWrapper).ResourceNode) != null))
            {
                propertyGrid1.SelectedObject = node;

                if (node is IImageSource)
                {
                    previewPanel1.Picture = ((IImageSource)node).GetImage(0);
                    previewPanel1.Visible = true;
                    splitContainer2.Panel2Collapsed = false;
                }
                else
                {
                    previewPanel1.Visible = false;
                    //if (e.Node is IAudioSource)
                    //{
                    //    audioPlaybackControl1.TargetObject = ((IAudioSource)e.Node).AudioStreams[0];
                    //    audioPlaybackControl1.Visible = true;
                    //    splitContainer2.Panel2Collapsed = false;
                    //}
                    //else
                    //{
                    //    audioPlaybackControl1.TargetObject = null;
                    //    audioPlaybackControl1.Visible = false;
                    splitContainer2.Panel2Collapsed = true;
                    //}
                }

                if ((editToolStripMenuItem.DropDown = w.ContextMenuStrip) != null)
                    editToolStripMenuItem.Enabled = true;
                else
                    editToolStripMenuItem.Enabled = false;
            }
            else
            {
                propertyGrid1.SelectedObject = null;
                previewPanel1.Visible = false;
                splitContainer2.Panel2Collapsed = true;

                editToolStripMenuItem.DropDown = null;
                editToolStripMenuItem.Enabled = false;
            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close()) e.Cancel = true;
            base.OnClosing(e);
        }



        private static string _inFilter = "All Supported Formats (*.pac,*.pcs,*.brres,*.plt0,*.tex0,*.brstm,*.brsar)|*.pac;*.pcs;*.brres;*.plt0;*.tex0;*.brstm;*.brsar|" +
                    "PAC File Archive (*.pac)|*.pac|" +
                    "Compressed File Package (*.pcs)|*.pcs|" +
                    "BRRES Resource Package (*.brres)|*.brres|" +
                    "PLT0 Raw Palette (*.plt0)|*.plt0|" +
                    "TEX0 Raw Texture (*.tex0)|*.tex0|" +
                    "BRSTM Audio Stream (*.brstm)|*.brstm|" +
                    "BRSAR Audio Package (*.brsar)|*.brsar";

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inFile;
            if (Program.OpenFile(_inFilter, out inFile) != 0)
                Program.Open(inFile);
        }

        #region File Menu
        private void aRCArchiveToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<ARCNode>(); }
        private void brresPackToolStripMenuItem_Click(object sender, EventArgs e) { Program.New<BRESNode>(); }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) { Program.Save(); }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) { Program.SaveAs(); }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e) { Program.Close(); }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }
        #endregion


        private void fileResizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (FileResizer res = new FileResizer())
            //    res.ShowDialog();
        }
        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { AboutForm.Instance.ShowDialog(this); }



    }
}
