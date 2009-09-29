namespace BrawlScape
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCharacters = new System.Windows.Forms.ToolStripButton();
            this.btnItems = new System.Windows.Forms.ToolStripButton();
            this._framePanel = new System.Windows.Forms.Panel();
            this.btnProject = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnProject,
            this.btnCharacters,
            this.btnItems});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(677, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnCharacters
            // 
            this.btnCharacters.Image = ((System.Drawing.Image)(resources.GetObject("btnCharacters.Image")));
            this.btnCharacters.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCharacters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCharacters.Name = "btnCharacters";
            this.btnCharacters.Size = new System.Drawing.Size(83, 22);
            this.btnCharacters.Text = "Characters";
            this.btnCharacters.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCharacters.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // btnItems
            // 
            this.btnItems.Image = ((System.Drawing.Image)(resources.GetObject("btnItems.Image")));
            this.btnItems.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnItems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnItems.Name = "btnItems";
            this.btnItems.Size = new System.Drawing.Size(56, 22);
            this.btnItems.Text = "Items";
            this.btnItems.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnItems.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // _framePanel
            // 
            this._framePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._framePanel.Location = new System.Drawing.Point(0, 25);
            this._framePanel.Name = "_framePanel";
            this._framePanel.Size = new System.Drawing.Size(677, 487);
            this._framePanel.TabIndex = 2;
            // 
            // btnProject
            // 
            this.btnProject.Image = ((System.Drawing.Image)(resources.GetObject("btnProject.Image")));
            this.btnProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProject.Name = "btnProject";
            this.btnProject.Size = new System.Drawing.Size(64, 22);
            this.btnProject.Text = "Project";
            this.btnProject.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 512);
            this.Controls.Add(this._framePanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "BrawlScape";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCharacters;
        private System.Windows.Forms.Panel _framePanel;
        private System.Windows.Forms.ToolStripButton btnItems;
        private System.Windows.Forms.ToolStripButton btnProject;

    }
}

