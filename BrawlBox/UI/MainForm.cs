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
            //propertyGrid1.BrowsableAttributes = new AttributeCollection(new SSBBBrowsableAttribute());
            this.Text = Program.AssemblyTitle;
        }

        public void Reset()
        {
            _root = null;
            resourceTree.Clear();
            propertyGrid1.SelectedObject = null;
            previewPanel1.RenderingTarget = null;
            //audioPlaybackControl1.TargetObject = null;
            splitContainer2.Panel2Collapsed = true;

            if (Program.RootNode != null)
            {
                this.Text = String.Format("{0} - {1}", Program.RootPath, Program.AssemblyTitle);

                _root = BaseWrapper.Wrap(Program.RootNode);
                resourceTree.BeginUpdate();
                resourceTree.Nodes.Add(_root);
                resourceTree.SelectedNode = _root;
                _root.Expand();
                resourceTree.EndUpdate();
            }
            else
                this.Text = Program.AssemblyTitle;
        }

        public void TargetResource(ResourceNode n)
        {
            if (_root != null)
                resourceTree.SelectedNode = _root.FindResource(n, true);
        }

        private void resourceTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            editToolStripMenuItem.DropDownItems.Clear();

            if (e.Node is BaseWrapper)
            {
                BaseWrapper w = e.Node as BaseWrapper;
                ResourceNode node = w.ResourceNode;
                propertyGrid1.SelectedObject = node;

                if (node is IImageSource)
                {
                    previewPanel1.RenderingTarget = node;
                    previewPanel1.Visible = true;
                    splitContainer2.Panel2Collapsed = false;
                }
                else
                {
                    previewPanel1.RenderingTarget = null;
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
                contextMenuStrip1.Items.AddRange(ActionFactory.Build(w));

                editToolStripMenuItem.DropDownItems.AddRange(ActionFactory.Build(w));
                editToolStripMenuItem.DropDownItems.Add(toolStripMenuItem2);
                editToolStripMenuItem.DropDownItems.Add(settingsToolStripMenuItem);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close()) e.Cancel = true;
            base.OnClosing(e);
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (propertyGrid1.SelectedObject is ResourceNode)
                ((ResourceNode)propertyGrid1.SelectedObject).HasChanged = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.Save())
                this.Text = this.Text.Replace("*", "");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.OpenDialog.Filter =
                    "All Supported Formats (*.pac,*.pcs,*.brres,*.plt0,*.tex0,*.brstm,*.brsar)|*.pac;*.pcs;*.brres;*.plt0;*.tex0;*.brstm;*.brsar|" +
                    "PAC File Archive (*.pac)|*.pac|" +
                    "Compressed File Package (*.pcs)|*.pcs|" +
                    "BRRES Resource Package (*.brres)|*.brres|" +
                    "PLT0 Raw Palette (*.plt0)|*.plt0|" +
                    "TEX0 Raw Texture (*.tex0)|*.tex0|" +
                    "BRSTM Audio Stream (*.brstm)|*.brstm|" +
                    "BRSAR Audio Package (*.brsar)|*.brsar";
            if (Program.OpenDialog.ShowDialog(this) == DialogResult.OK)
                Program.Open(Program.OpenDialog.FileName);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Program.ConfigForm.ShowDialog(this);
        }

        private void resourceTree_AfterDeselect(object sender, EventArgs e)
        {
            contextMenuStrip1.Items.Clear();

            editToolStripMenuItem.DropDownItems.Clear();
            editToolStripMenuItem.DropDownItems.Add(settingsToolStripMenuItem);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Program.SaveAs();
        }

        private void fileResizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (FileResizer res = new FileResizer())
            //    res.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm.Instance.ShowDialog(this);
        }
    }
}
