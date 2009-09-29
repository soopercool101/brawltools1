namespace BrawlScape
{
    partial class ItemFrame
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._iconList = new System.Windows.Forms.ImageList(this.components);
            this._itemList = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._textureList = new System.Windows.Forms.ListView();
            this.ctxTexture = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSize = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPalette = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLOD = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSize = new System.Windows.Forms.ToolStripMenuItem();
            this._textureImageList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2.SuspendLayout();
            this.ctxTexture.SuspendLayout();
            this.SuspendLayout();
            // 
            // _iconList
            // 
            this._iconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._iconList.ImageSize = new System.Drawing.Size(32, 32);
            this._iconList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _itemList
            // 
            this._itemList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this._itemList.AutoArrange = false;
            this._itemList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._itemList.Dock = System.Windows.Forms.DockStyle.Top;
            this._itemList.HideSelection = false;
            this._itemList.LargeImageList = this._iconList;
            this._itemList.Location = new System.Drawing.Point(0, 0);
            this._itemList.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this._itemList.MultiSelect = false;
            this._itemList.Name = "_itemList";
            this._itemList.ShowGroups = false;
            this._itemList.Size = new System.Drawing.Size(540, 88);
            this._itemList.SmallImageList = this._iconList;
            this._itemList.TabIndex = 0;
            this._itemList.TileSize = new System.Drawing.Size(100, 68);
            this._itemList.UseCompatibleStateImageBehavior = false;
            this._itemList.SelectedIndexChanged += new System.EventHandler(this._itemList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 364);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Models";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._textureList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(337, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(203, 364);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Textures";
            // 
            // _textureList
            // 
            this._textureList.AutoArrange = false;
            this._textureList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textureList.ContextMenuStrip = this.ctxTexture;
            this._textureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textureList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._textureList.HideSelection = false;
            this._textureList.LargeImageList = this._textureImageList;
            this._textureList.Location = new System.Drawing.Point(3, 16);
            this._textureList.MultiSelect = false;
            this._textureList.Name = "_textureList";
            this._textureList.ShowGroups = false;
            this._textureList.Size = new System.Drawing.Size(197, 345);
            this._textureList.TabIndex = 0;
            this._textureList.UseCompatibleStateImageBehavior = false;
            this._textureList.SelectedIndexChanged += new System.EventHandler(this._textureList_SelectedIndexChanged);
            // 
            // ctxTexture
            // 
            this.ctxTexture.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuReplace,
            this.mnuExport,
            this.restoreToolStripMenuItem,
            this.toolStripSeparator1,
            this.mnuSize,
            this.mnuFormat,
            this.mnuPalette,
            this.mnuLOD,
            this.mnuFileSize});
            this.ctxTexture.Name = "ctxTexture";
            this.ctxTexture.Size = new System.Drawing.Size(125, 186);
            this.ctxTexture.Opening += new System.ComponentModel.CancelEventHandler(this.ctxTexture_Opening);
            // 
            // mnuReplace
            // 
            this.mnuReplace.Name = "mnuReplace";
            this.mnuReplace.Size = new System.Drawing.Size(152, 22);
            this.mnuReplace.Text = "Replace...";
            this.mnuReplace.Click += new System.EventHandler(this.mnuReplace_Click);
            // 
            // mnuExport
            // 
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.Size = new System.Drawing.Size(152, 22);
            this.mnuExport.Text = "Export...";
            this.mnuExport.Click += new System.EventHandler(this.mnuExport_Click);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuSize
            // 
            this.mnuSize.Enabled = false;
            this.mnuSize.Name = "mnuSize";
            this.mnuSize.Size = new System.Drawing.Size(152, 22);
            this.mnuSize.Text = "Size:";
            // 
            // mnuFormat
            // 
            this.mnuFormat.Enabled = false;
            this.mnuFormat.Name = "mnuFormat";
            this.mnuFormat.Size = new System.Drawing.Size(152, 22);
            this.mnuFormat.Text = "Format:";
            // 
            // mnuPalette
            // 
            this.mnuPalette.Enabled = false;
            this.mnuPalette.Name = "mnuPalette";
            this.mnuPalette.Size = new System.Drawing.Size(152, 22);
            this.mnuPalette.Text = "Palette:";
            // 
            // mnuLOD
            // 
            this.mnuLOD.Enabled = false;
            this.mnuLOD.Name = "mnuLOD";
            this.mnuLOD.Size = new System.Drawing.Size(152, 22);
            this.mnuLOD.Text = "LOD:";
            // 
            // mnuFileSize
            // 
            this.mnuFileSize.Enabled = false;
            this.mnuFileSize.Name = "mnuFileSize";
            this.mnuFileSize.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSize.Text = "File Size:";
            // 
            // _textureImageList
            // 
            this._textureImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._textureImageList.ImageSize = new System.Drawing.Size(128, 128);
            this._textureImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ItemFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._itemList);
            this.Name = "ItemFrame";
            this.Size = new System.Drawing.Size(540, 452);
            this.Load += new System.EventHandler(this.ItemFrame_Load);
            this.Enter += new System.EventHandler(this.ItemFrame_Enter);
            this.groupBox2.ResumeLayout(false);
            this.ctxTexture.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList _iconList;
        private System.Windows.Forms.ListView _itemList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView _textureList;
        private System.Windows.Forms.ImageList _textureImageList;
        private System.Windows.Forms.ContextMenuStrip ctxTexture;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSize;
        private System.Windows.Forms.ToolStripMenuItem mnuReplace;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuSize;
        private System.Windows.Forms.ToolStripMenuItem mnuFormat;
        private System.Windows.Forms.ToolStripMenuItem mnuLOD;
        private System.Windows.Forms.ToolStripMenuItem mnuExport;
        private System.Windows.Forms.ToolStripMenuItem mnuPalette;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
    }
}
