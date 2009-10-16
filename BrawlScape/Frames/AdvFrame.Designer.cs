namespace BrawlScape.Frames
{
    partial class AdvFrame
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
            this.sseAreaList = new BrawlScape.SSEAreaList();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sseStageList = new BrawlScape.SSEStageList();
            this.modelList = new BrawlScape.ModelList();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.modelControl = new BrawlScape.ModelControl();
            this.texturePanel = new BrawlScape.TexturePanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sseAreaList
            // 
            this.sseAreaList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.sseAreaList.Dock = System.Windows.Forms.DockStyle.Top;
            this.sseAreaList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.sseAreaList.HideSelection = false;
            this.sseAreaList.ImageSize = new System.Drawing.Size(72, 56);
            this.sseAreaList.Location = new System.Drawing.Point(0, 0);
            this.sseAreaList.Margin = new System.Windows.Forms.Padding(0);
            this.sseAreaList.MultiSelect = false;
            this.sseAreaList.Name = "sseAreaList";
            this.sseAreaList.Size = new System.Drawing.Size(113, 131);
            this.sseAreaList.TabIndex = 0;
            this.sseAreaList.UseCompatibleStateImageBehavior = false;
            this.sseAreaList.View = System.Windows.Forms.View.Details;
            this.sseAreaList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.AdvAreaDefinition>(this.sseAreaList_ResourceChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 90;
            // 
            // sseStageList
            // 
            this.sseStageList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.sseStageList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sseStageList.Dock = System.Windows.Forms.DockStyle.Top;
            this.sseStageList.HideSelection = false;
            this.sseStageList.ImageSize = new System.Drawing.Size(72, 56);
            this.sseStageList.Location = new System.Drawing.Point(0, 0);
            this.sseStageList.Margin = new System.Windows.Forms.Padding(0);
            this.sseStageList.MultiSelect = false;
            this.sseStageList.Name = "sseStageList";
            this.sseStageList.Size = new System.Drawing.Size(672, 110);
            this.sseStageList.TabIndex = 1;
            this.sseStageList.UseCompatibleStateImageBehavior = false;
            this.sseStageList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.AdvStageDefinition>(this.sseStageList_ResourceChanged);
            // 
            // modelList
            // 
            this.modelList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.modelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.modelList.HideSelection = false;
            this.modelList.ImageSize = new System.Drawing.Size(128, 128);
            this.modelList.Location = new System.Drawing.Point(0, 131);
            this.modelList.Margin = new System.Windows.Forms.Padding(0);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.Size = new System.Drawing.Size(113, 251);
            this.modelList.TabIndex = 2;
            this.modelList.UseCompatibleStateImageBehavior = false;
            this.modelList.View = System.Windows.Forms.View.Details;
            this.modelList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.ModelDefinition>(this.modelList_ResourceChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 100;
            // 
            // modelControl
            // 
            this.modelControl.CurrentModel = null;
            this.modelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelControl.InitialYFactor = -160;
            this.modelControl.InitialZoomFactor = -20;
            this.modelControl.Location = new System.Drawing.Point(113, 110);
            this.modelControl.Name = "modelControl";
            this.modelControl.RotationScale = 0.4F;
            this.modelControl.Size = new System.Drawing.Size(359, 382);
            this.modelControl.TabIndex = 3;
            this.modelControl.TranslationScale = 0.15F;
            this.modelControl.ZoomScale = 6F;
            // 
            // texturePanel
            // 
            this.texturePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.texturePanel.ImageSize = new System.Drawing.Size(128, 128);
            this.texturePanel.Location = new System.Drawing.Point(472, 110);
            this.texturePanel.Margin = new System.Windows.Forms.Padding(0);
            this.texturePanel.Name = "texturePanel";
            this.texturePanel.Size = new System.Drawing.Size(200, 382);
            this.texturePanel.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.modelList);
            this.panel1.Controls.Add(this.sseAreaList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 110);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(113, 382);
            this.panel1.TabIndex = 5;
            // 
            // AdvFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.modelControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.texturePanel);
            this.Controls.Add(this.sseStageList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AdvFrame";
            this.Size = new System.Drawing.Size(672, 492);
            this.Load += new System.EventHandler(this.SSEFrame_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SSEAreaList sseAreaList;
        private SSEStageList sseStageList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private ModelList modelList;
        private ModelControl modelControl;
        private TexturePanel texturePanel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel1;
    }
}
