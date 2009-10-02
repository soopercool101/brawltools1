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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.costumeFrame1 = new BrawlScape.CostumeFrame();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.costumeFrame1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(552, 356);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Costumes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 98);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 382);
            this.tabControl1.TabIndex = 2;
            // 
            // costumeFrame1
            // 
            this.costumeFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.costumeFrame1.Location = new System.Drawing.Point(3, 3);
            this.costumeFrame1.Name = "costumeFrame1";
            this.costumeFrame1.Size = new System.Drawing.Size(546, 350);
            this.costumeFrame1.TabIndex = 0;
            // 
            // CharacterFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this._charList);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CharacterFrame";
            this.Size = new System.Drawing.Size(560, 480);
            this.Load += new System.EventHandler(this.CharacterFrame_Load);
            this.Enter += new System.EventHandler(this.CharacterFrame_Enter);
            this.tabPage1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _charList;
        private System.Windows.Forms.ImageList csfList;
        private System.Windows.Forms.TabPage tabPage1;
        private CostumeFrame costumeFrame1;
        private System.Windows.Forms.TabControl tabControl1;
    }
}
