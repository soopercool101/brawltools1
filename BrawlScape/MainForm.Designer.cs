﻿namespace BrawlScape
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.startupFrame1 = new BrawlScape.StartupFrame();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.characterFrame1 = new BrawlScape.CharacterFrame();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.itemFrame1 = new BrawlScape.ItemFrame();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(677, 512);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.startupFrame1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(669, 486);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Project";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // startupFrame1
            // 
            this.startupFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startupFrame1.Location = new System.Drawing.Point(3, 3);
            this.startupFrame1.Name = "startupFrame1";
            this.startupFrame1.Size = new System.Drawing.Size(663, 480);
            this.startupFrame1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.characterFrame1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(669, 486);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Characters";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // characterFrame1
            // 
            this.characterFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.characterFrame1.Location = new System.Drawing.Point(3, 3);
            this.characterFrame1.Margin = new System.Windows.Forms.Padding(0);
            this.characterFrame1.Name = "characterFrame1";
            this.characterFrame1.SelectedCharacter = null;
            this.characterFrame1.Size = new System.Drawing.Size(663, 480);
            this.characterFrame1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.itemFrame1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(669, 486);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Items";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // itemFrame1
            // 
            this.itemFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemFrame1.Location = new System.Drawing.Point(3, 3);
            this.itemFrame1.Name = "itemFrame1";
            this.itemFrame1.SelectedTexture = null;
            this.itemFrame1.Size = new System.Drawing.Size(663, 480);
            this.itemFrame1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 512);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "BrawlScape";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private StartupFrame startupFrame1;
        private CharacterFrame characterFrame1;
        private ItemFrame itemFrame1;

    }
}

