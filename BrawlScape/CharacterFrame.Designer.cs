namespace BrawlScape
{
    partial class CharacterFrame
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
            this._charList = new System.Windows.Forms.ListView();
            this.csfList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._costumeList = new System.Windows.Forms.ListView();
            this.costumeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCostumeCSP = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSPReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSPExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSPResore = new System.Windows.Forms.ToolStripMenuItem();
            this.cspImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._textureList = new System.Windows.Forms.ListView();
            this.textureContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuTextureReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTextureExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTextureRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.textureImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mnuCostume = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.costumeContext.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.textureContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // _charList
            // 
            this._charList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this._charList.AutoArrange = false;
            this._charList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._charList.Dock = System.Windows.Forms.DockStyle.Top;
            this._charList.HideSelection = false;
            this._charList.LargeImageList = this.csfList;
            this._charList.Location = new System.Drawing.Point(0, 0);
            this._charList.Margin = new System.Windows.Forms.Padding(0, 0, 0, 18);
            this._charList.MultiSelect = false;
            this._charList.Name = "_charList";
            this._charList.Size = new System.Drawing.Size(560, 98);
            this._charList.TabIndex = 0;
            this._charList.UseCompatibleStateImageBehavior = false;
            this._charList.SelectedIndexChanged += new System.EventHandler(this._charList_SelectedIndexChanged);
            // 
            // csfList
            // 
            this.csfList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.csfList.ImageSize = new System.Drawing.Size(80, 56);
            this.csfList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._costumeList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 382);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Costumes";
            // 
            // _costumeList
            // 
            this._costumeList.AutoArrange = false;
            this._costumeList.BackColor = System.Drawing.Color.Gainsboro;
            this._costumeList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._costumeList.ContextMenuStrip = this.costumeContext;
            this._costumeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._costumeList.HideSelection = false;
            this._costumeList.LargeImageList = this.cspImages;
            this._costumeList.Location = new System.Drawing.Point(3, 16);
            this._costumeList.MultiSelect = false;
            this._costumeList.Name = "_costumeList";
            this._costumeList.ShowGroups = false;
            this._costumeList.Size = new System.Drawing.Size(195, 363);
            this._costumeList.TabIndex = 2;
            this._costumeList.UseCompatibleStateImageBehavior = false;
            this._costumeList.SelectedIndexChanged += new System.EventHandler(this._costumeList_SelectedIndexChanged);
            // 
            // costumeContext
            // 
            this.costumeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCostumeCSP,
            this.mnuCostume});
            this.costumeContext.Name = "costumeContext";
            this.costumeContext.Size = new System.Drawing.Size(153, 70);
            this.costumeContext.Opening += new System.ComponentModel.CancelEventHandler(this.costumeContext_Opening);
            // 
            // mnuCostumeCSP
            // 
            this.mnuCostumeCSP.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCSPReplace,
            this.mnuCSPExport,
            this.mnuCSPResore});
            this.mnuCostumeCSP.Name = "mnuCostumeCSP";
            this.mnuCostumeCSP.Size = new System.Drawing.Size(152, 22);
            this.mnuCostumeCSP.Text = "Portrait";
            // 
            // mnuCSPReplace
            // 
            this.mnuCSPReplace.Name = "mnuCSPReplace";
            this.mnuCSPReplace.Size = new System.Drawing.Size(152, 22);
            this.mnuCSPReplace.Text = "Replace...";
            this.mnuCSPReplace.Click += new System.EventHandler(this.mnuCSPReplace_Click);
            // 
            // mnuCSPExport
            // 
            this.mnuCSPExport.Name = "mnuCSPExport";
            this.mnuCSPExport.Size = new System.Drawing.Size(152, 22);
            this.mnuCSPExport.Text = "Export...";
            this.mnuCSPExport.Click += new System.EventHandler(this.mnuCSPExport_Click);
            // 
            // mnuCSPResore
            // 
            this.mnuCSPResore.Name = "mnuCSPResore";
            this.mnuCSPResore.Size = new System.Drawing.Size(152, 22);
            this.mnuCSPResore.Text = "Restore";
            this.mnuCSPResore.Click += new System.EventHandler(this.mnuCSPResore_Click);
            // 
            // cspImages
            // 
            this.cspImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.cspImages.ImageSize = new System.Drawing.Size(128, 160);
            this.cspImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._textureList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(367, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 382);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Textures";
            // 
            // _textureList
            // 
            this._textureList.AutoArrange = false;
            this._textureList.BackColor = System.Drawing.Color.Gainsboro;
            this._textureList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._textureList.ContextMenuStrip = this.textureContext;
            this._textureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textureList.HideSelection = false;
            this._textureList.LargeImageList = this.textureImages;
            this._textureList.Location = new System.Drawing.Point(3, 16);
            this._textureList.MultiSelect = false;
            this._textureList.Name = "_textureList";
            this._textureList.Size = new System.Drawing.Size(187, 363);
            this._textureList.TabIndex = 0;
            this._textureList.UseCompatibleStateImageBehavior = false;
            this._textureList.SelectedIndexChanged += new System.EventHandler(this._textureList_SelectedIndexChanged);
            // 
            // textureContext
            // 
            this.textureContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTextureReplace,
            this.mnuTextureExport,
            this.mnuTextureRestore});
            this.textureContext.Name = "textureContext";
            this.textureContext.Size = new System.Drawing.Size(125, 70);
            this.textureContext.Opening += new System.ComponentModel.CancelEventHandler(this.textureContext_Opening);
            // 
            // mnuTextureReplace
            // 
            this.mnuTextureReplace.Name = "mnuTextureReplace";
            this.mnuTextureReplace.Size = new System.Drawing.Size(124, 22);
            this.mnuTextureReplace.Text = "Replace...";
            this.mnuTextureReplace.Click += new System.EventHandler(this.mnuTextureReplace_Click);
            // 
            // mnuTextureExport
            // 
            this.mnuTextureExport.Name = "mnuTextureExport";
            this.mnuTextureExport.Size = new System.Drawing.Size(124, 22);
            this.mnuTextureExport.Text = "Export...";
            this.mnuTextureExport.Click += new System.EventHandler(this.mnuTextureExport_Click);
            // 
            // mnuTextureRestore
            // 
            this.mnuTextureRestore.Name = "mnuTextureRestore";
            this.mnuTextureRestore.Size = new System.Drawing.Size(124, 22);
            this.mnuTextureRestore.Text = "Restore";
            this.mnuTextureRestore.Click += new System.EventHandler(this.mnuTextureRestore_Click);
            // 
            // textureImages
            // 
            this.textureImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.textureImages.ImageSize = new System.Drawing.Size(128, 128);
            this.textureImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBox3
            // 
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(201, 98);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(166, 382);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Models";
            // 
            // mnuCostume
            // 
            this.mnuCostume.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCostumeExport,
            this.mnuCostumeImport,
            this.mnuCostumeRestore});
            this.mnuCostume.Name = "mnuCostume";
            this.mnuCostume.Size = new System.Drawing.Size(152, 22);
            this.mnuCostume.Text = "Costume";
            // 
            // mnuCostumeExport
            // 
            this.mnuCostumeExport.Name = "mnuCostumeExport";
            this.mnuCostumeExport.Size = new System.Drawing.Size(152, 22);
            this.mnuCostumeExport.Text = "Export...";
            this.mnuCostumeExport.Click += new System.EventHandler(this.mnuCostumeExport_Click);
            // 
            // mnuCostumeImport
            // 
            this.mnuCostumeImport.Name = "mnuCostumeImport";
            this.mnuCostumeImport.Size = new System.Drawing.Size(152, 22);
            this.mnuCostumeImport.Text = "Import...";
            this.mnuCostumeImport.Click += new System.EventHandler(this.mnuCostumeImport_Click);
            // 
            // mnuCostumeRestore
            // 
            this.mnuCostumeRestore.Name = "mnuCostumeRestore";
            this.mnuCostumeRestore.Size = new System.Drawing.Size(152, 22);
            this.mnuCostumeRestore.Text = "Restore";
            this.mnuCostumeRestore.Click += new System.EventHandler(this.mnuCostumeRestore_Click);
            // 
            // CharacterFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._charList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CharacterFrame";
            this.Size = new System.Drawing.Size(560, 480);
            this.Load += new System.EventHandler(this.CharacterFrame_Load);
            this.Enter += new System.EventHandler(this.CharacterFrame_Enter);
            this.groupBox1.ResumeLayout(false);
            this.costumeContext.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.textureContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _charList;
        private System.Windows.Forms.ImageList csfList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView _costumeList;
        private System.Windows.Forms.ImageList cspImages;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ContextMenuStrip costumeContext;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeCSP;
        private System.Windows.Forms.ToolStripMenuItem mnuCSPReplace;
        private System.Windows.Forms.ToolStripMenuItem mnuCSPExport;
        private System.Windows.Forms.ToolStripMenuItem mnuCSPResore;
        private System.Windows.Forms.ListView _textureList;
        private System.Windows.Forms.ImageList textureImages;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ContextMenuStrip textureContext;
        private System.Windows.Forms.ToolStripMenuItem mnuTextureReplace;
        private System.Windows.Forms.ToolStripMenuItem mnuTextureExport;
        private System.Windows.Forms.ToolStripMenuItem mnuTextureRestore;
        private System.Windows.Forms.ToolStripMenuItem mnuCostume;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeExport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeImport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeRestore;
    }
}
