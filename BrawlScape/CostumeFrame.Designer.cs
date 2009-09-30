namespace BrawlScape
{
    partial class CostumeFrame
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._textureList = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._costumeList = new System.Windows.Forms.ListView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.costumeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCostumeCSP = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSPReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSPExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCSPResore = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostume = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.textureContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuTextureReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTextureExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTextureRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.cspImages = new System.Windows.Forms.ImageList(this.components);
            this.textureImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.costumeContext.SuspendLayout();
            this.textureContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._textureList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(504, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 496);
            this.groupBox2.TabIndex = 3;
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
            this._textureList.Size = new System.Drawing.Size(187, 477);
            this._textureList.TabIndex = 0;
            this._textureList.UseCompatibleStateImageBehavior = false;
            this._textureList.SelectedIndexChanged += new System.EventHandler(this._textureList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._costumeList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 496);
            this.groupBox1.TabIndex = 4;
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
            this._costumeList.Size = new System.Drawing.Size(195, 477);
            this._costumeList.TabIndex = 2;
            this._costumeList.UseCompatibleStateImageBehavior = false;
            this._costumeList.SelectedIndexChanged += new System.EventHandler(this._costumeList_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(201, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(303, 496);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Models";
            // 
            // costumeContext
            // 
            this.costumeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCostumeCSP,
            this.mnuCostume});
            this.costumeContext.Name = "costumeContext";
            this.costumeContext.Size = new System.Drawing.Size(123, 48);
            // 
            // mnuCostumeCSP
            // 
            this.mnuCostumeCSP.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCSPReplace,
            this.mnuCSPExport,
            this.mnuCSPResore});
            this.mnuCostumeCSP.Name = "mnuCostumeCSP";
            this.mnuCostumeCSP.Size = new System.Drawing.Size(122, 22);
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
            // textureContext
            // 
            this.textureContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTextureReplace,
            this.mnuTextureExport,
            this.mnuTextureRestore});
            this.textureContext.Name = "textureContext";
            this.textureContext.Size = new System.Drawing.Size(125, 70);
            // 
            // mnuTextureReplace
            // 
            this.mnuTextureReplace.Name = "mnuTextureReplace";
            this.mnuTextureReplace.Size = new System.Drawing.Size(152, 22);
            this.mnuTextureReplace.Text = "Replace...";
            this.mnuTextureReplace.Click += new System.EventHandler(this.mnuTextureReplace_Click);
            // 
            // mnuTextureExport
            // 
            this.mnuTextureExport.Name = "mnuTextureExport";
            this.mnuTextureExport.Size = new System.Drawing.Size(152, 22);
            this.mnuTextureExport.Text = "Export...";
            this.mnuTextureExport.Click += new System.EventHandler(this.mnuTextureExport_Click);
            // 
            // mnuTextureRestore
            // 
            this.mnuTextureRestore.Name = "mnuTextureRestore";
            this.mnuTextureRestore.Size = new System.Drawing.Size(152, 22);
            this.mnuTextureRestore.Text = "Restore";
            this.mnuTextureRestore.Click += new System.EventHandler(this.mnuTextureRestore_Click);
            // 
            // cspImages
            // 
            this.cspImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.cspImages.ImageSize = new System.Drawing.Size(128, 160);
            this.cspImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // textureImages
            // 
            this.textureImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.textureImages.ImageSize = new System.Drawing.Size(128, 128);
            this.textureImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CostumeFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "CostumeFrame";
            this.Size = new System.Drawing.Size(697, 496);
            this.Load += new System.EventHandler(this.CostumeFrame_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.costumeContext.ResumeLayout(false);
            this.textureContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView _textureList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView _costumeList;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ContextMenuStrip costumeContext;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeCSP;
        private System.Windows.Forms.ToolStripMenuItem mnuCSPReplace;
        private System.Windows.Forms.ToolStripMenuItem mnuCSPExport;
        private System.Windows.Forms.ToolStripMenuItem mnuCSPResore;
        private System.Windows.Forms.ToolStripMenuItem mnuCostume;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeExport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeImport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeRestore;
        private System.Windows.Forms.ContextMenuStrip textureContext;
        private System.Windows.Forms.ToolStripMenuItem mnuTextureReplace;
        private System.Windows.Forms.ToolStripMenuItem mnuTextureExport;
        private System.Windows.Forms.ToolStripMenuItem mnuTextureRestore;
        private System.Windows.Forms.ImageList cspImages;
        private System.Windows.Forms.ImageList textureImages;
    }
}
