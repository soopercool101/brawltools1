using System;
using BrawlLib.SSBB.ResourceNodes;

namespace System.Windows.Forms
{
    public class SoundPackControl : UserControl
    {
        #region Designer

        public ListView lstSets;
        private ColumnHeader clmIndex;
        private ColumnHeader clmName;
        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem pathToolStripMenuItem;
        private ColumnHeader clmPath;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.clmIndex = new System.Windows.Forms.ColumnHeader();
            this.clmName = new System.Windows.Forms.ColumnHeader();
            this.clmPath = new System.Windows.Forms.ColumnHeader();
            this.lstSets = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clmIndex
            // 
            this.clmIndex.Text = "Index";
            this.clmIndex.Width = 38;
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 40;
            // 
            // clmPath
            // 
            this.clmPath.Text = "Path";
            this.clmPath.Width = 336;
            // 
            // lstSets
            // 
            this.lstSets.AutoArrange = false;
            this.lstSets.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstSets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmIndex,
            this.clmName,
            this.clmPath});
            this.lstSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSets.FullRowSelect = true;
            this.lstSets.GridLines = true;
            this.lstSets.HideSelection = false;
            this.lstSets.LabelWrap = false;
            this.lstSets.Location = new System.Drawing.Point(0, 0);
            this.lstSets.MultiSelect = false;
            this.lstSets.Name = "lstSets";
            this.lstSets.Size = new System.Drawing.Size(414, 240);
            this.lstSets.TabIndex = 0;
            this.lstSets.UseCompatibleStateImageBehavior = false;
            this.lstSets.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.pathToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 92);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            // 
            // pathToolStripMenuItem
            // 
            this.pathToolStripMenuItem.Name = "pathToolStripMenuItem";
            this.pathToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pathToolStripMenuItem.Text = "Path...";
            // 
            // SoundPackControl
            // 
            this.Controls.Add(this.lstSets);
            this.Name = "SoundPackControl";
            this.Size = new System.Drawing.Size(414, 240);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RSARNode _targetNode;
        public RSARNode TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; NodeChanged(); }
        }

        public SoundPackControl() { InitializeComponent(); }

        private void NodeChanged()
        {
            lstSets.BeginUpdate();

            lstSets.Items.Clear();
            if (_targetNode != null)
                foreach (RSARFileNode file in _targetNode.Files)
                    lstSets.Items.Add(new SoundPackItem(file));

            lstSets.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            lstSets.EndUpdate();
        }
    }

    public class SoundPackItem : ListViewItem
    {

        public SoundPackItem(RSARFileNode file)
        {
            ImageIndex = (byte)file.ResourceType;

            Text = file.FileIndex.ToString();
            SubItems.Add(file.Name);

            if (file is RSARExtFileNode)
                SubItems.Add(((RSARExtFileNode)file).ExtPath);
        }
    }
}
