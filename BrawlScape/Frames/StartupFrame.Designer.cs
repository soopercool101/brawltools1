namespace BrawlScape
{
    partial class StartupFrame
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
            this.btnDataBrowse = new System.Windows.Forms.Button();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProjectPath = new System.Windows.Forms.TextBox();
            this.btnProjectBrowse = new System.Windows.Forms.Button();
            this.rdoProjMode = new System.Windows.Forms.RadioButton();
            this.rdoRawMode = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDataBrowse
            // 
            this.btnDataBrowse.Location = new System.Drawing.Point(378, 66);
            this.btnDataBrowse.Name = "btnDataBrowse";
            this.btnDataBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnDataBrowse.TabIndex = 5;
            this.btnDataBrowse.Text = "Browse...";
            this.btnDataBrowse.UseVisualStyleBackColor = true;
            this.btnDataBrowse.Click += new System.EventHandler(this.btnDataBrowse_Click);
            // 
            // txtDataPath
            // 
            this.txtDataPath.Enabled = false;
            this.txtDataPath.Location = new System.Drawing.Point(113, 68);
            this.txtDataPath.Name = "txtDataPath";
            this.txtDataPath.Size = new System.Drawing.Size(259, 20);
            this.txtDataPath.TabIndex = 4;
            this.txtDataPath.TextChanged += new System.EventHandler(this.txtDataPath_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(27, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Data Folder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Project Folder";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtProjectPath
            // 
            this.txtProjectPath.Enabled = false;
            this.txtProjectPath.Location = new System.Drawing.Point(113, 94);
            this.txtProjectPath.Name = "txtProjectPath";
            this.txtProjectPath.Size = new System.Drawing.Size(259, 20);
            this.txtProjectPath.TabIndex = 7;
            this.txtProjectPath.TextChanged += new System.EventHandler(this.txtProjectPath_TextChanged);
            // 
            // btnProjectBrowse
            // 
            this.btnProjectBrowse.Location = new System.Drawing.Point(378, 92);
            this.btnProjectBrowse.Name = "btnProjectBrowse";
            this.btnProjectBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnProjectBrowse.TabIndex = 8;
            this.btnProjectBrowse.Text = "Browse...";
            this.btnProjectBrowse.UseVisualStyleBackColor = true;
            this.btnProjectBrowse.Click += new System.EventHandler(this.btnProjectBrowse_Click);
            // 
            // rdoProjMode
            // 
            this.rdoProjMode.Checked = true;
            this.rdoProjMode.Location = new System.Drawing.Point(113, 42);
            this.rdoProjMode.Name = "rdoProjMode";
            this.rdoProjMode.Size = new System.Drawing.Size(93, 20);
            this.rdoProjMode.TabIndex = 9;
            this.rdoProjMode.TabStop = true;
            this.rdoProjMode.Text = "Project Mode";
            this.rdoProjMode.UseVisualStyleBackColor = true;
            this.rdoProjMode.CheckedChanged += new System.EventHandler(this.rdoProjMode_CheckedChanged);
            // 
            // rdoRawMode
            // 
            this.rdoRawMode.Location = new System.Drawing.Point(212, 42);
            this.rdoRawMode.Name = "rdoRawMode";
            this.rdoRawMode.Size = new System.Drawing.Size(96, 20);
            this.rdoRawMode.TabIndex = 10;
            this.rdoRawMode.Text = "Direct Mode";
            this.rdoRawMode.UseVisualStyleBackColor = true;
            this.rdoRawMode.CheckedChanged += new System.EventHandler(this.rdoRawMode_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "Mode";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(248, 231);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(158, 231);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 13;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // StartupFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rdoRawMode);
            this.Controls.Add(this.rdoProjMode);
            this.Controls.Add(this.btnProjectBrowse);
            this.Controls.Add(this.txtProjectPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDataBrowse);
            this.Controls.Add(this.txtDataPath);
            this.Controls.Add(this.label1);
            this.Name = "StartupFrame";
            this.Size = new System.Drawing.Size(578, 491);
            this.Load += new System.EventHandler(this.StartupFrame_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDataBrowse;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProjectPath;
        private System.Windows.Forms.Button btnProjectBrowse;
        private System.Windows.Forms.RadioButton rdoProjMode;
        private System.Windows.Forms.RadioButton rdoRawMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReset;
    }
}
