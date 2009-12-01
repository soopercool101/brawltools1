using System;

namespace System.Windows.Forms
{
    class ModelPlaybackPanel : UserControl
    {
        #region Designer

        private Button btnPlay;
        private NumericUpDown numTotalFrames;
        private NumericUpDown numFPS;
        private Label label14;
        private CheckBox chkLoop;
        private NumericUpDown numFrameIndex;
        private Button btnPrevFrame;
        private Button btnNextFrame;
        private Label label15;
        private Label lblFrameCount;
        private Button btnFirst;
        private Button btnLast;
    
        private void InitializeComponent()
        {
            this.btnPlay = new System.Windows.Forms.Button();
            this.numTotalFrames = new System.Windows.Forms.NumericUpDown();
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.numFrameIndex = new System.Windows.Forms.NumericUpDown();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(133, 0);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(1);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(236, 20);
            this.btnPlay.TabIndex = 14;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            // 
            // numTotalFrames
            // 
            this.numTotalFrames.Enabled = false;
            this.numTotalFrames.Location = new System.Drawing.Point(105, 23);
            this.numTotalFrames.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numTotalFrames.Name = "numTotalFrames";
            this.numTotalFrames.Size = new System.Drawing.Size(52, 20);
            this.numTotalFrames.TabIndex = 19;
            // 
            // numFPS
            // 
            this.numFPS.Location = new System.Drawing.Point(43, 0);
            this.numFPS.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFPS.Name = "numFPS";
            this.numFPS.Size = new System.Drawing.Size(35, 20);
            this.numFPS.TabIndex = 15;
            this.numFPS.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(3, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 20);
            this.label14.TabIndex = 17;
            this.label14.Text = "Speed:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLoop
            // 
            this.chkLoop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLoop.Location = new System.Drawing.Point(423, 1);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 20);
            this.chkLoop.TabIndex = 16;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            // 
            // numFrameIndex
            // 
            this.numFrameIndex.Location = new System.Drawing.Point(43, 23);
            this.numFrameIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFrameIndex.Name = "numFrameIndex";
            this.numFrameIndex.Size = new System.Drawing.Size(52, 20);
            this.numFrameIndex.TabIndex = 12;
            this.numFrameIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Enabled = false;
            this.btnPrevFrame.Location = new System.Drawing.Point(108, 0);
            this.btnPrevFrame.Margin = new System.Windows.Forms.Padding(1);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(23, 20);
            this.btnPrevFrame.TabIndex = 11;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextFrame.Enabled = false;
            this.btnNextFrame.Location = new System.Drawing.Point(371, 0);
            this.btnNextFrame.Margin = new System.Windows.Forms.Padding(1);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(23, 20);
            this.btnNextFrame.TabIndex = 10;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(3, 23);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 20);
            this.label15.TabIndex = 18;
            this.label15.Text = "Frame:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Location = new System.Drawing.Point(80, 23);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(40, 20);
            this.lblFrameCount.TabIndex = 13;
            this.lblFrameCount.Text = "/";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFirst
            // 
            this.btnFirst.Enabled = false;
            this.btnFirst.Location = new System.Drawing.Point(83, 0);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(1);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(23, 20);
            this.btnFirst.TabIndex = 20;
            this.btnFirst.Text = "|<";
            this.btnFirst.UseVisualStyleBackColor = true;
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.Enabled = false;
            this.btnLast.Location = new System.Drawing.Point(396, 0);
            this.btnLast.Margin = new System.Windows.Forms.Padding(1);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(23, 20);
            this.btnLast.TabIndex = 21;
            this.btnLast.Text = ">|";
            this.btnLast.UseVisualStyleBackColor = true;
            // 
            // ModelPlaybackPanel
            // 
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.numTotalFrames);
            this.Controls.Add(this.numFPS);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.chkLoop);
            this.Controls.Add(this.numFrameIndex);
            this.Controls.Add(this.btnPrevFrame);
            this.Controls.Add(this.btnNextFrame);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lblFrameCount);
            this.Name = "ModelPlaybackPanel";
            this.Size = new System.Drawing.Size(473, 45);
            this.ResumeLayout(false);

        }

        #endregion

        public ModelPlaybackPanel() { InitializeComponent(); }
    }
}
