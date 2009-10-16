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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textureList = new BrawlScape.TexturePanel();
            this.itemList = new BrawlScape.ItemList();
            this.modelList = new BrawlScape.ModelList();
            this.modelPanel = new ModelControl();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textureList);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(357, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(195, 362);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Textures";
            // 
            // textureList
            // 
            this.textureList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureList.ImageSize = new System.Drawing.Size(128, 128);
            this.textureList.Location = new System.Drawing.Point(3, 16);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(189, 343);
            this.textureList.TabIndex = 0;
            // 
            // itemList
            // 
            this.itemList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.itemList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.itemList.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemList.HideSelection = false;
            this.itemList.ImageSize = new System.Drawing.Size(64, 64);
            this.itemList.Location = new System.Drawing.Point(0, 0);
            this.itemList.MultiSelect = false;
            this.itemList.Name = "itemList";
            this.itemList.Size = new System.Drawing.Size(552, 107);
            this.itemList.TabIndex = 4;
            this.itemList.UseCompatibleStateImageBehavior = false;
            this.itemList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.ItemDefinition>(this.itemList_ResourceChanged);
            // 
            // modelList
            // 
            this.modelList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modelList.Dock = System.Windows.Forms.DockStyle.Left;
            this.modelList.HideSelection = false;
            this.modelList.ImageSize = new System.Drawing.Size(128, 128);
            this.modelList.Location = new System.Drawing.Point(0, 107);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.Size = new System.Drawing.Size(139, 362);
            this.modelList.TabIndex = 5;
            this.modelList.UseCompatibleStateImageBehavior = false;
            this.modelList.View = System.Windows.Forms.View.List;
            this.modelList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.ModelDefinition>(this.modelList_ResourceChanged);
            // 
            // modelPanel
            // 
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.InitialYFactor = -20;
            this.modelPanel.InitialZoomFactor = -15;
            this.modelPanel.Location = new System.Drawing.Point(139, 107);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.RotationScale = 0.4F;
            this.modelPanel.Size = new System.Drawing.Size(218, 362);
            this.modelPanel.TabIndex = 6;
            this.modelPanel.TranslationScale = 0.05F;
            this.modelPanel.ZoomScale = 2.5F;
            // 
            // ItemFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.modelPanel);
            this.Controls.Add(this.modelList);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.itemList);
            this.Name = "ItemFrame";
            this.Size = new System.Drawing.Size(552, 469);
            this.Load += new System.EventHandler(this.ItemFrame_Load);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private BrawlScape.ItemList itemList;
        private TexturePanel textureList;
        private ModelList modelList;
        private ModelControl modelPanel;
    }
}
