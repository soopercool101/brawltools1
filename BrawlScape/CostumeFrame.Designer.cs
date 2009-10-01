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
            this.textureImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._costumeList = new System.Windows.Forms.ListView();
            this.costumeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCostumeCSP = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostume = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.cspImages = new System.Windows.Forms.ImageList(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.picStock = new System.Windows.Forms.PictureBox();
            this.picGame = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.costumeContext.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGame)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            // textureImages
            // 
            this.textureImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.textureImages.ImageSize = new System.Drawing.Size(128, 128);
            this.textureImages.TransparentColor = System.Drawing.Color.Transparent;
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
            this.mnuCostumeCSP.Name = "mnuCostumeCSP";
            this.mnuCostumeCSP.Size = new System.Drawing.Size(122, 22);
            this.mnuCostumeCSP.Text = "Portrait";
            this.mnuCostumeCSP.DropDownOpening += new System.EventHandler(this.mnuCostumeCSP_DropDownOpening);
            // 
            // mnuCostume
            // 
            this.mnuCostume.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCostumeExport,
            this.mnuCostumeImport,
            this.mnuCostumeRestore});
            this.mnuCostume.Name = "mnuCostume";
            this.mnuCostume.Size = new System.Drawing.Size(122, 22);
            this.mnuCostume.Text = "Costume";
            // 
            // mnuCostumeExport
            // 
            this.mnuCostumeExport.Name = "mnuCostumeExport";
            this.mnuCostumeExport.Size = new System.Drawing.Size(119, 22);
            this.mnuCostumeExport.Text = "Export...";
            this.mnuCostumeExport.Click += new System.EventHandler(this.mnuCostumeExport_Click);
            // 
            // mnuCostumeImport
            // 
            this.mnuCostumeImport.Name = "mnuCostumeImport";
            this.mnuCostumeImport.Size = new System.Drawing.Size(119, 22);
            this.mnuCostumeImport.Text = "Import...";
            this.mnuCostumeImport.Click += new System.EventHandler(this.mnuCostumeImport_Click);
            // 
            // mnuCostumeRestore
            // 
            this.mnuCostumeRestore.Name = "mnuCostumeRestore";
            this.mnuCostumeRestore.Size = new System.Drawing.Size(119, 22);
            this.mnuCostumeRestore.Text = "Restore";
            this.mnuCostumeRestore.Click += new System.EventHandler(this.mnuCostumeRestore_Click);
            // 
            // cspImages
            // 
            this.cspImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.cspImages.ImageSize = new System.Drawing.Size(128, 160);
            this.cspImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBox3
            // 
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(201, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(303, 410);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Models";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.panel2);
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(120, 86);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Game/Stock Portrait";
            // 
            // picStock
            // 
            this.picStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStock.Location = new System.Drawing.Point(70, 13);
            this.picStock.Name = "picStock";
            this.picStock.Size = new System.Drawing.Size(32, 32);
            this.picStock.TabIndex = 2;
            this.picStock.TabStop = false;
            this.picStock.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picStock_MouseDown);
            // 
            // picGame
            // 
            this.picGame.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGame.Location = new System.Drawing.Point(13, 5);
            this.picGame.Name = "picGame";
            this.picGame.Size = new System.Drawing.Size(48, 56);
            this.picGame.TabIndex = 1;
            this.picGame.TabStop = false;
            this.picGame.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picGame_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(201, 410);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(303, 86);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.picGame);
            this.panel2.Controls.Add(this.picStock);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(114, 67);
            this.panel2.TabIndex = 1;
            // 
            // CostumeFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "CostumeFrame";
            this.Size = new System.Drawing.Size(697, 496);
            this.Load += new System.EventHandler(this.CostumeFrame_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.costumeContext.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGame)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem mnuCostume;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeExport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeImport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeRestore;
        private System.Windows.Forms.ImageList cspImages;
        private System.Windows.Forms.ImageList textureImages;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox picStock;
        private System.Windows.Forms.PictureBox picGame;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
