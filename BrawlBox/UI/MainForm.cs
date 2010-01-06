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
using BrawlLib.IO;
using System.Audio;
using BrawlLib.Wii.Audio;

namespace BrawlBox
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        public static MainForm Instance { get { return _instance == null ? _instance = new MainForm() : _instance; } }

        private BaseWrapper _root;
        public BaseWrapper RootNode { get { return _root; } }

        private SettingsDialog _settings;
        private SettingsDialog Settings { get { return _settings == null ? _settings = new SettingsDialog() : _settings; } }

        public MainForm()
        {
            InitializeComponent();
            this.Text = Program.AssemblyTitle;
            previewPanel1.Dock = DockStyle.Fill;
            msBinEditor1.Dock = DockStyle.Fill;
            animEditControl.Dock = DockStyle.Fill;
            soundPackControl1.Dock = DockStyle.Fill;
            soundPackControl1.lstSets.SmallImageList = ResourceTree.Images;
            audioPlaybackPanel1.Dock = DockStyle.Fill;
            clrControl.Dock = DockStyle.Fill;
        }

        public void Reset()
        {
            _root = null;
            resourceTree.SelectedNode = null;
            resourceTree.Clear();

            if (Program.RootNode != null)
            {
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
                closeToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
            }

            UpdateName();
        }

        public void UpdateName()
        {
            if (Program.RootNode != null)
                this.Text = String.Format("{0} - {1}", Program.AssemblyTitle, Program.RootPath);
            else
                this.Text = Program.AssemblyTitle;
        }

        public void TargetResource(ResourceNode n)
        {
            if (_root != null)
                resourceTree.SelectedNode = _root.FindResource(n, true);
        }

        private Control _currentControl = null;
        private void resourceTree_SelectionChanged(object sender, EventArgs e)
        {
            Image img = previewPanel1.Picture;
            if (img != null)
            {
                previewPanel1.Picture = null;
                img.Dispose();
            }

            audioPlaybackPanel1.TargetSource = null;
            animEditControl.TargetSequence = null;
            msBinEditor1.CurrentNode = null;
            soundPackControl1.TargetNode = null;
            clrControl.ColorSource = null;

            Control newControl = null;

            BaseWrapper w;
            ResourceNode node;
            if ((resourceTree.SelectedNode is BaseWrapper) && ((node = (w = resourceTree.SelectedNode as BaseWrapper).ResourceNode) != null))
            {
                propertyGrid1.SelectedObject = node;

                if (node is IImageSource)
                {
                    previewPanel1.Picture = ((IImageSource)node).GetImage(0);
                    newControl = previewPanel1;
                }
                else if (node is MSBinNode)
                {
                    msBinEditor1.CurrentNode = node as MSBinNode;
                    newControl = msBinEditor1;
                }
                else if (node is CHR0EntryNode)
                {
                    animEditControl.TargetSequence = node as CHR0EntryNode;
                    newControl = animEditControl;
                }
                else if (node is RSARNode)
                {
                    soundPackControl1.TargetNode = node as RSARNode;
                    newControl = soundPackControl1;
                }
                else if (node is IAudioSource)
                {
                    audioPlaybackPanel1.TargetSource = node as IAudioSource;
                    newControl = audioPlaybackPanel1;
                }
                else if (node is IColorSource)
                {
                    clrControl.ColorSource = node as IColorSource;
                    newControl = clrControl;
                }

                if ((editToolStripMenuItem.DropDown = w.ContextMenuStrip) != null)
                    editToolStripMenuItem.Enabled = true;
                else
                    editToolStripMenuItem.Enabled = false;
            }
            else
            {
                propertyGrid1.SelectedObject = null;

                editToolStripMenuItem.DropDown = null;
                editToolStripMenuItem.Enabled = false;
            }

            if (_currentControl != newControl)
            {
                if (_currentControl != null)
                    _currentControl.Visible = false;
                if (newControl != null)
                    newControl.Visible = true;
                _currentControl = newControl;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Program.Close()) e.Cancel = true;
            base.OnClosing(e);
        }



        private static string _inFilter = "All Supported Formats |*.pac;*.pcs;*.brres;*.plt0;*.tex0;*.mdl0;*.chr0;*.brstm;*.brsar;*.msbin;*.rwsd;*.rseq;*.rbnk;*.clr0;*.vis0|" +
                    "PAC File Archive (*.pac)|*.pac|" +
                    "Compressed File Package (*.pcs)|*.pcs|" +
                    "BRRES Resource Package (*.brres)|*.brres|" +
                    "PLT0 Raw Palette (*.plt0)|*.plt0|" +
                    "TEX0 Raw Texture (*.tex0)|*.tex0|" +
                    "MDL0 Raw Model (*.mdl0)|*.mdl0|" +
                    "CHR0 Raw Animation (*.chr0)|*.chr0|" +
                    "BRSTM Audio Stream (*.brstm)|*.brstm|" +
                    "BRSAR Audio Package (*.brsar)|*.brsar|" +
                    "MSBin Message Pack (*.msbin)|*.msbin|" +
                    "Raw Sound Pack (*.rwsd)|*.rwsd|" +
                    "Raw Sound Sequence (*.rseq)|*.rseq|" +
                    "Raw Sound Bank (*.rbnk)|*.rbnk|" +
                    "Color Sequence (*.clr0)|*.clr0|" + 
                    "VIS Sequence (*.vis0)|*.vis0";

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
            Settings.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) { AboutForm.Instance.ShowDialog(this); }

        private void bRStmAudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path;
            if (Program.OpenFile("PCM Audio (*.wav)|*.wav", out path) > 0)
            {
                if (Program.New<RSTMNode>())
                {
                    using (BrstmConverterDialog dlg = new BrstmConverterDialog())
                    {
                        dlg.AudioSource = path;
                        if (dlg.ShowDialog(this) == DialogResult.OK)
                        {
                            Program.RootNode.Name = Path.GetFileNameWithoutExtension(dlg.AudioSource);
                            Program.RootNode.ReplaceRaw(dlg.AudioData);
                        }
                        else
                            Program.Close(true);
                    }
                }
            }
        }



    }
}
