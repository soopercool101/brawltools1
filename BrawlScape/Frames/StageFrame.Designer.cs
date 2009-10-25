using System.Windows.Forms;
namespace BrawlScape
{
    partial class StageFrame
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
            this.stageList = new BrawlScape.StageList();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.modelPanel = new ModelPanel();
            this.textureList = new BrawlScape.TexturePanel();
            this.modelList = new BrawlScape.ModelList();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stageList
            // 
            this.stageList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.stageList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stageList.Dock = System.Windows.Forms.DockStyle.Top;
            this.stageList.HideSelection = false;
            this.stageList.ImageSize = new System.Drawing.Size(64, 56);
            this.stageList.Location = new System.Drawing.Point(0, 0);
            this.stageList.Margin = new System.Windows.Forms.Padding(0);
            this.stageList.MultiSelect = false;
            this.stageList.Name = "stageList";
            this.stageList.Size = new System.Drawing.Size(573, 110);
            this.stageList.TabIndex = 0;
            this.stageList.UseCompatibleStateImageBehavior = false;
            this.stageList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.StageDefinition>(this.stageList_ResourceChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 110);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(573, 322);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.modelPanel);
            this.tabPage1.Controls.Add(this.textureList);
            this.tabPage1.Controls.Add(this.modelList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(565, 296);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Objects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // modelPanel
            // 
            this.modelPanel.CurrentModel = null;
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.InitialYFactor = -100;
            this.modelPanel.InitialZoomFactor = -20;
            this.modelPanel.Location = new System.Drawing.Point(152, 0);
            this.modelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.RotationScale = 0.3F;
            this.modelPanel.Size = new System.Drawing.Size(220, 296);
            this.modelPanel.TabIndex = 2;
            this.modelPanel.TranslationScale = 0.2F;
            this.modelPanel.ZoomScale = 7F;
            // 
            // textureList
            // 
            this.textureList.Dock = System.Windows.Forms.DockStyle.Right;
            this.textureList.ImageSize = new System.Drawing.Size(128, 128);
            this.textureList.Location = new System.Drawing.Point(372, 0);
            this.textureList.Margin = new System.Windows.Forms.Padding(0);
            this.textureList.Name = "textureList";
            this.textureList.Size = new System.Drawing.Size(193, 296);
            this.textureList.TabIndex = 1;
            // 
            // modelList
            // 
            this.modelList.AutoArrange = false;
            this.modelList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modelList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName});
            this.modelList.Dock = System.Windows.Forms.DockStyle.Left;
            this.modelList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.modelList.HideSelection = false;
            this.modelList.ImageSize = new System.Drawing.Size(128, 128);
            this.modelList.Location = new System.Drawing.Point(0, 0);
            this.modelList.Margin = new System.Windows.Forms.Padding(0);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.Size = new System.Drawing.Size(152, 296);
            this.modelList.TabIndex = 0;
            this.modelList.UseCompatibleStateImageBehavior = false;
            this.modelList.View = System.Windows.Forms.View.Details;
            this.modelList.ResourceChanged += new BrawlScape.ResourceChangeEvent<BrawlScape.ModelDefinition>(this.modelList_ResourceChanged);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 151;
            // 
            // StageFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.stageList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StageFrame";
            this.Size = new System.Drawing.Size(573, 432);
            this.Load += new System.EventHandler(this.StageFrame_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BrawlScape.StageList stageList;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private TexturePanel textureList;
        private ModelList modelList;
        private ModelPanel modelPanel;
        private System.Windows.Forms.ColumnHeader columnName;
    }
}
