using System.Windows.Forms;
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
            this.characterContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCharIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNameStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.modelPanel = new ModelPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stockPortrait = new BrawlScape.ReferencedPictureBox();
            this.modelList = new BrawlScape.ModelList();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gamePortrait = new BrawlScape.ReferencedPictureBox();
            this.textureList = new BrawlScape.TexturePanel();
            this.costumeList = new BrawlScape.CostumeList();
            this.costumeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuCostumeCSP = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostume = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeImport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCostumeRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.charList = new BrawlScape.CharacterList();
            this.characterContext.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stockPortrait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gamePortrait)).BeginInit();
            this.costumeContext.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // characterContext
            // 
            this.characterContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCharIcon,
            this.mnuNameStrip});
            this.characterContext.Name = "characterContext";
            this.characterContext.Size = new System.Drawing.Size(152, 48);
            this.characterContext.Opening += new System.ComponentModel.CancelEventHandler(this.characterContext_Opening);
            // 
            // mnuCharIcon
            // 
            this.mnuCharIcon.Name = "mnuCharIcon";
            this.mnuCharIcon.Size = new System.Drawing.Size(151, 22);
            this.mnuCharIcon.Text = "Character Icon";
            // 
            // mnuNameStrip
            // 
            this.mnuNameStrip.Name = "mnuNameStrip";
            this.mnuNameStrip.Size = new System.Drawing.Size(151, 22);
            this.mnuNameStrip.Text = "Name Strip";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.modelPanel);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.textureList);
            this.tabPage1.Controls.Add(this.costumeList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(552, 357);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Costumes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // modelPanel
            // 
            this.modelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.modelPanel.CurrentModel = null;
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.InitialYFactor = -175;
            this.modelPanel.InitialZoomFactor = -17;
            this.modelPanel.Location = new System.Drawing.Point(190, 63);
            this.modelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.RotationScale = 0.4F;
            this.modelPanel.Size = new System.Drawing.Size(172, 294);
            this.modelPanel.TabIndex = 7;
            this.modelPanel.TranslationScale = 0.05F;
            this.modelPanel.ZoomScale = 2.5F;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stockPortrait);
            this.panel1.Controls.Add(this.modelList);
            this.panel1.Controls.Add(this.gamePortrait);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(190, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(172, 63);
            this.panel1.TabIndex = 8;
            // 
            // stockPortrait
            // 
            this.stockPortrait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stockPortrait.Location = new System.Drawing.Point(53, 28);
            this.stockPortrait.Name = "stockPortrait";
            this.stockPortrait.Reference = null;
            this.stockPortrait.Size = new System.Drawing.Size(32, 32);
            this.stockPortrait.TabIndex = 1;
            this.stockPortrait.TabStop = false;
            // 
            // modelList
            // 
            this.modelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.modelList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modelList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.modelList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.modelList.HideSelection = false;
            this.modelList.ImageSize = new System.Drawing.Size(128, 128);
            this.modelList.Location = new System.Drawing.Point(87, 0);
            this.modelList.Margin = new System.Windows.Forms.Padding(0);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.ShowGroups = false;
            this.modelList.Size = new System.Drawing.Size(85, 63);
            this.modelList.TabIndex = 6;
            this.modelList.UseCompatibleStateImageBehavior = false;
            this.modelList.View = System.Windows.Forms.View.Details;
            this.modelList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.ModelDefinition>(this.modelList_ResourceChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Models";
            this.columnHeader1.Width = 138;
            // 
            // gamePortrait
            // 
            this.gamePortrait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gamePortrait.Location = new System.Drawing.Point(3, 4);
            this.gamePortrait.Name = "gamePortrait";
            this.gamePortrait.Reference = null;
            this.gamePortrait.Size = new System.Drawing.Size(48, 56);
            this.gamePortrait.TabIndex = 0;
            this.gamePortrait.TabStop = false;
            // 
            // textureList
            // 
            this.textureList.Dock = System.Windows.Forms.DockStyle.Right;
            this.textureList.ImageSize = new System.Drawing.Size(128, 128);
            this.textureList.Location = new System.Drawing.Point(362, 0);
            this.textureList.Margin = new System.Windows.Forms.Padding(0);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(190, 357);
            this.textureList.TabIndex = 5;
            // 
            // costumeList
            // 
            this.costumeList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.costumeList.ContextMenuStrip = this.costumeContext;
            this.costumeList.Dock = System.Windows.Forms.DockStyle.Left;
            this.costumeList.HideSelection = false;
            this.costumeList.ImageSize = new System.Drawing.Size(128, 160);
            this.costumeList.Location = new System.Drawing.Point(0, 0);
            this.costumeList.Margin = new System.Windows.Forms.Padding(0);
            this.costumeList.MultiSelect = false;
            this.costumeList.Name = "costumeList";
            this.costumeList.Size = new System.Drawing.Size(190, 357);
            this.costumeList.TabIndex = 4;
            this.costumeList.UseCompatibleStateImageBehavior = false;
            this.costumeList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.CostumeDefinition>(this.costumeList_ResourceChanged);
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
            this.mnuCostumeCSP.Name = "mnuCostumeCSP";
            this.mnuCostumeCSP.Size = new System.Drawing.Size(152, 22);
            this.mnuCostumeCSP.Text = "Portrait";
            // 
            // mnuCostume
            // 
            this.mnuCostume.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCostumeExport,
            this.mnuCostumeImport,
            this.mnuCostumeRestore,
            this.toolStripMenuItem1,
            this.exportAllToolStripMenuItem});
            this.mnuCostume.Name = "mnuCostume";
            this.mnuCostume.Size = new System.Drawing.Size(152, 22);
            this.mnuCostume.Text = "Costume";
            // 
            // mnuCostumeExport
            // 
            this.mnuCostumeExport.Name = "mnuCostumeExport";
            this.mnuCostumeExport.Size = new System.Drawing.Size(133, 22);
            this.mnuCostumeExport.Text = "Export...";
            this.mnuCostumeExport.Click += new System.EventHandler(this.mnuCostumeExport_Click);
            // 
            // mnuCostumeImport
            // 
            this.mnuCostumeImport.Name = "mnuCostumeImport";
            this.mnuCostumeImport.Size = new System.Drawing.Size(133, 22);
            this.mnuCostumeImport.Text = "Import...";
            this.mnuCostumeImport.Click += new System.EventHandler(this.mnuCostumeImport_Click);
            // 
            // mnuCostumeRestore
            // 
            this.mnuCostumeRestore.Name = "mnuCostumeRestore";
            this.mnuCostumeRestore.Size = new System.Drawing.Size(133, 22);
            this.mnuCostumeRestore.Text = "Restore";
            this.mnuCostumeRestore.Click += new System.EventHandler(this.mnuCostumeRestore_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(130, 6);
            // 
            // exportAllToolStripMenuItem
            // 
            this.exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
            this.exportAllToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.exportAllToolStripMenuItem.Text = "Export All...";
            this.exportAllToolStripMenuItem.Click += new System.EventHandler(this.exportAllToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 97);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 383);
            this.tabControl1.TabIndex = 2;
            // 
            // charList
            // 
            this.charList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.charList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.charList.ContextMenuStrip = this.characterContext;
            this.charList.Dock = System.Windows.Forms.DockStyle.Top;
            this.charList.HideSelection = false;
            this.charList.ImageSize = new System.Drawing.Size(80, 56);
            this.charList.Location = new System.Drawing.Point(0, 0);
            this.charList.MultiSelect = false;
            this.charList.Name = "charList";
            this.charList.Size = new System.Drawing.Size(560, 97);
            this.charList.TabIndex = 3;
            this.charList.UseCompatibleStateImageBehavior = false;
            this.charList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.CharacterDefinition>(this.charList_ResourceChanged);
            // 
            // CharacterFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.charList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CharacterFrame";
            this.Size = new System.Drawing.Size(560, 480);
            this.Load += new System.EventHandler(this.CharacterFrame_Load);
            this.characterContext.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stockPortrait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gamePortrait)).EndInit();
            this.costumeContext.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ContextMenuStrip characterContext;
        private System.Windows.Forms.ToolStripMenuItem mnuCharIcon;
        private System.Windows.Forms.ToolStripMenuItem mnuNameStrip;
        private BrawlScape.CharacterList charList;
        private BrawlScape.CostumeList costumeList;
        private System.Windows.Forms.ContextMenuStrip costumeContext;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeCSP;
        private System.Windows.Forms.ToolStripMenuItem mnuCostume;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeExport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeImport;
        private System.Windows.Forms.ToolStripMenuItem mnuCostumeRestore;
        private BrawlScape.TexturePanel textureList;
        private BrawlScape.ModelList modelList;
        private ModelPanel modelPanel;
        private System.Windows.Forms.Panel panel1;
        private BrawlScape.ReferencedPictureBox stockPortrait;
        private BrawlScape.ReferencedPictureBox gamePortrait;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
