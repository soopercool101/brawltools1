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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.stageFrame1 = new BrawlScape.StageFrame();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.sseFrame1 = new BrawlScape.Frames.AdvFrame();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
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
            this.itemFrame1.Size = new System.Drawing.Size(663, 480);
            this.itemFrame1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.stageFrame1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(669, 486);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Melee Stages";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // stageFrame1
            // 
            this.stageFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stageFrame1.Location = new System.Drawing.Point(3, 3);
            this.stageFrame1.Margin = new System.Windows.Forms.Padding(0);
            this.stageFrame1.Name = "stageFrame1";
            this.stageFrame1.Size = new System.Drawing.Size(663, 480);
            this.stageFrame1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.sseFrame1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(669, 486);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Adv Stages";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // sseFrame1
            // 
            this.sseFrame1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sseFrame1.Location = new System.Drawing.Point(3, 3);
            this.sseFrame1.Margin = new System.Windows.Forms.Padding(0);
            this.sseFrame1.Name = "sseFrame1";
            this.sseFrame1.Size = new System.Drawing.Size(663, 480);
            this.sseFrame1.TabIndex = 0;
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
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabPage4;
        private StageFrame stageFrame1;
        private System.Windows.Forms.TabPage tabPage5;
        private BrawlScape.Frames.AdvFrame sseFrame1;

    }
}

