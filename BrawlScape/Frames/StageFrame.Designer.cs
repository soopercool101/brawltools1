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
            this.components = new System.ComponentModel.Container();
            this.stageList = new System.Windows.Forms.ListView();
            this.stageIcons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // stageList
            // 
            this.stageList.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.stageList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stageList.Dock = System.Windows.Forms.DockStyle.Top;
            this.stageList.LargeImageList = this.stageIcons;
            this.stageList.Location = new System.Drawing.Point(0, 0);
            this.stageList.MultiSelect = false;
            this.stageList.Name = "stageList";
            this.stageList.Size = new System.Drawing.Size(573, 97);
            this.stageList.TabIndex = 0;
            this.stageList.UseCompatibleStateImageBehavior = false;
            this.stageList.SelectedIndexChanged += new System.EventHandler(this.stageList_SelectedIndexChanged);
            // 
            // stageIcons
            // 
            this.stageIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.stageIcons.ImageSize = new System.Drawing.Size(64, 56);
            this.stageIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // StageFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stageList);
            this.Name = "StageFrame";
            this.Size = new System.Drawing.Size(573, 432);
            this.Load += new System.EventHandler(this.StageFrame_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView stageList;
        private System.Windows.Forms.ImageList stageIcons;
    }
}
