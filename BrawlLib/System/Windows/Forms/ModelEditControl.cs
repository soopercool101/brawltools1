using System;
using BrawlLib.OpenGL;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.Modeling;
using System.Drawing;
using BrawlLib.Wii.Animations;

namespace System.Windows.Forms
{
    public class ModelEditControl : UserControl
    {
        #region Designer

        private ModelPanel modelPanel1;
        private Panel pnlAssets;
        private Panel panel1;
        private Label label2;
        private Label label1;
        private ColorDialog dlgColor;
        private Panel pnlAnim;
        private Button btnOptionToggle;
        private Button btnAnimToggle;
        private System.ComponentModel.IContainer components;
        private ListView listAnims;
        private Panel pnlPlayback;
        private ColumnHeader nameColumn;
        private GroupBox grpExt;
        private TextBox txtExtPath;
        private Button btnClose;
        private Button btnOpen;
        private Button btnSave;
        private OpenFileDialog dlgOpen;
        private Button btnPrevFrame;
        private Button btnNextFrame;
        private Button btnFrames;
        private NumericUpDown numFrameIndex;
        private NumericInputBox numScaleZ;
        private NumericInputBox numScaleY;
        private NumericInputBox numScaleX;
        private NumericInputBox numRotZ;
        private NumericInputBox numRotY;
        private NumericInputBox numRotX;
        private NumericInputBox numTransZ;
        private NumericInputBox numTransY;
        private Label label13;
        private Label label12;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private NumericInputBox numTransX;
        private GroupBox grpTransform;
        private Button btnPlay;
        private Timer animTimer;
        private NumericUpDown numFPS;
        private CheckBox chkLoop;
        private Label lblFrameCount;
        private Label label14;
        private ModelAssetPanel modelAssetPanel1;
        private Splitter spltAssets;
        private Label label15;

        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Animations", System.Windows.Forms.HorizontalAlignment.Left);
            this.pnlAssets = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.pnlAnim = new System.Windows.Forms.Panel();
            this.listAnims = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpTransform = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.grpExt = new System.Windows.Forms.GroupBox();
            this.txtExtPath = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOptionToggle = new System.Windows.Forms.Button();
            this.btnAnimToggle = new System.Windows.Forms.Button();
            this.pnlPlayback = new System.Windows.Forms.Panel();
            this.numFPS = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lblFrameCount = new System.Windows.Forms.Label();
            this.numFrameIndex = new System.Windows.Forms.NumericUpDown();
            this.btnPrevFrame = new System.Windows.Forms.Button();
            this.btnNextFrame = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.btnFrames = new System.Windows.Forms.Button();
            this.animTimer = new System.Windows.Forms.Timer();
            this.spltAssets = new System.Windows.Forms.Splitter();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.modelAssetPanel1 = new System.Windows.Forms.ModelAssetPanel();
            this.pnlAssets.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlAnim.SuspendLayout();
            this.grpTransform.SuspendLayout();
            this.grpExt.SuspendLayout();
            this.pnlPlayback.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAssets
            // 
            this.pnlAssets.BackColor = System.Drawing.Color.White;
            this.pnlAssets.Controls.Add(this.modelAssetPanel1);
            this.pnlAssets.Controls.Add(this.panel1);
            this.pnlAssets.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAssets.Location = new System.Drawing.Point(0, 0);
            this.pnlAssets.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAssets.Name = "pnlAssets";
            this.pnlAssets.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.pnlAssets.Size = new System.Drawing.Size(130, 546);
            this.pnlAssets.TabIndex = 2;
            this.pnlAssets.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 30);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(67, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 18);
            this.label2.TabIndex = 2;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Back Color:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // pnlAnim
            // 
            this.pnlAnim.BackColor = System.Drawing.Color.White;
            this.pnlAnim.Controls.Add(this.listAnims);
            this.pnlAnim.Controls.Add(this.grpTransform);
            this.pnlAnim.Controls.Add(this.grpExt);
            this.pnlAnim.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlAnim.Location = new System.Drawing.Point(581, 0);
            this.pnlAnim.Name = "pnlAnim";
            this.pnlAnim.Size = new System.Drawing.Size(173, 546);
            this.pnlAnim.TabIndex = 4;
            this.pnlAnim.Visible = false;
            // 
            // listAnims
            // 
            this.listAnims.AutoArrange = false;
            this.listAnims.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.listAnims.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "Animations";
            listViewGroup1.Name = "grpAnims";
            this.listAnims.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.listAnims.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listAnims.HideSelection = false;
            this.listAnims.Location = new System.Drawing.Point(0, 69);
            this.listAnims.MultiSelect = false;
            this.listAnims.Name = "listAnims";
            this.listAnims.Size = new System.Drawing.Size(173, 257);
            this.listAnims.TabIndex = 0;
            this.listAnims.UseCompatibleStateImageBehavior = false;
            this.listAnims.View = System.Windows.Forms.View.Details;
            this.listAnims.SelectedIndexChanged += new System.EventHandler(this.listAnims_SelectedIndexChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 160;
            // 
            // grpTransform
            // 
            this.grpTransform.Controls.Add(this.label5);
            this.grpTransform.Controls.Add(this.numScaleZ);
            this.grpTransform.Controls.Add(this.numTransX);
            this.grpTransform.Controls.Add(this.numScaleY);
            this.grpTransform.Controls.Add(this.label6);
            this.grpTransform.Controls.Add(this.numScaleX);
            this.grpTransform.Controls.Add(this.label7);
            this.grpTransform.Controls.Add(this.numRotZ);
            this.grpTransform.Controls.Add(this.label8);
            this.grpTransform.Controls.Add(this.numRotY);
            this.grpTransform.Controls.Add(this.label9);
            this.grpTransform.Controls.Add(this.numRotX);
            this.grpTransform.Controls.Add(this.label10);
            this.grpTransform.Controls.Add(this.numTransZ);
            this.grpTransform.Controls.Add(this.label11);
            this.grpTransform.Controls.Add(this.numTransY);
            this.grpTransform.Controls.Add(this.label12);
            this.grpTransform.Controls.Add(this.label13);
            this.grpTransform.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTransform.Location = new System.Drawing.Point(0, 326);
            this.grpTransform.Name = "grpTransform";
            this.grpTransform.Size = new System.Drawing.Size(173, 220);
            this.grpTransform.TabIndex = 21;
            this.grpTransform.TabStop = false;
            this.grpTransform.Text = "Transform";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Translation X:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Translation Y:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Translation Z:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Rotation X:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Rotation Y:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Rotation Z:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 20);
            this.label11.TabIndex = 10;
            this.label11.Text = "Scale X:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 192);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 20);
            this.label12.TabIndex = 11;
            this.label12.Text = "Scale Z:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(6, 172);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 20);
            this.label13.TabIndex = 12;
            this.label13.Text = "Scale Y:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpExt
            // 
            this.grpExt.Controls.Add(this.txtExtPath);
            this.grpExt.Controls.Add(this.btnClose);
            this.grpExt.Controls.Add(this.btnOpen);
            this.grpExt.Controls.Add(this.btnSave);
            this.grpExt.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpExt.Location = new System.Drawing.Point(0, 0);
            this.grpExt.Name = "grpExt";
            this.grpExt.Padding = new System.Windows.Forms.Padding(6, 4, 6, 3);
            this.grpExt.Size = new System.Drawing.Size(173, 69);
            this.grpExt.TabIndex = 2;
            this.grpExt.TabStop = false;
            this.grpExt.Text = "External File";
            // 
            // txtExtPath
            // 
            this.txtExtPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtExtPath.Location = new System.Drawing.Point(6, 17);
            this.txtExtPath.Name = "txtExtPath";
            this.txtExtPath.ReadOnly = true;
            this.txtExtPath.Size = new System.Drawing.Size(161, 20);
            this.txtExtPath.TabIndex = 3;
            this.txtExtPath.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClose.Location = new System.Drawing.Point(116, 42);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 20);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpen.Location = new System.Drawing.Point(8, 42);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(50, 20);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Load";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSave.Location = new System.Drawing.Point(62, 42);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(50, 20);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOptionToggle
            // 
            this.btnOptionToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOptionToggle.Location = new System.Drawing.Point(134, 0);
            this.btnOptionToggle.Name = "btnOptionToggle";
            this.btnOptionToggle.Size = new System.Drawing.Size(15, 546);
            this.btnOptionToggle.TabIndex = 5;
            this.btnOptionToggle.TabStop = false;
            this.btnOptionToggle.Text = ">";
            this.btnOptionToggle.UseVisualStyleBackColor = false;
            this.btnOptionToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // btnAnimToggle
            // 
            this.btnAnimToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAnimToggle.Location = new System.Drawing.Point(566, 0);
            this.btnAnimToggle.Name = "btnAnimToggle";
            this.btnAnimToggle.Size = new System.Drawing.Size(15, 546);
            this.btnAnimToggle.TabIndex = 6;
            this.btnAnimToggle.TabStop = false;
            this.btnAnimToggle.Text = "<";
            this.btnAnimToggle.UseVisualStyleBackColor = false;
            this.btnAnimToggle.Click += new System.EventHandler(this.btnAnimToggle_Click);
            // 
            // pnlPlayback
            // 
            this.pnlPlayback.BackColor = System.Drawing.Color.White;
            this.pnlPlayback.Controls.Add(this.numFPS);
            this.pnlPlayback.Controls.Add(this.label14);
            this.pnlPlayback.Controls.Add(this.chkLoop);
            this.pnlPlayback.Controls.Add(this.btnPlay);
            this.pnlPlayback.Controls.Add(this.lblFrameCount);
            this.pnlPlayback.Controls.Add(this.numFrameIndex);
            this.pnlPlayback.Controls.Add(this.btnPrevFrame);
            this.pnlPlayback.Controls.Add(this.btnNextFrame);
            this.pnlPlayback.Controls.Add(this.label15);
            this.pnlPlayback.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPlayback.Location = new System.Drawing.Point(149, 509);
            this.pnlPlayback.Name = "pnlPlayback";
            this.pnlPlayback.Size = new System.Drawing.Size(417, 37);
            this.pnlPlayback.TabIndex = 7;
            this.pnlPlayback.Visible = false;
            // 
            // numFPS
            // 
            this.numFPS.Location = new System.Drawing.Point(50, 10);
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
            this.numFPS.Size = new System.Drawing.Size(40, 20);
            this.numFPS.TabIndex = 5;
            this.numFPS.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFPS.ValueChanged += new System.EventHandler(this.numFPS_ValueChanged);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.Location = new System.Drawing.Point(6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 37);
            this.label14.TabIndex = 7;
            this.label14.Text = "Speed:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLoop
            // 
            this.chkLoop.AutoSize = true;
            this.chkLoop.Location = new System.Drawing.Point(96, 12);
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.Size = new System.Drawing.Size(50, 17);
            this.chkLoop.TabIndex = 6;
            this.chkLoop.Text = "Loop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.Location = new System.Drawing.Point(150, 7);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(65, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lblFrameCount
            // 
            this.lblFrameCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrameCount.Location = new System.Drawing.Point(312, 6);
            this.lblFrameCount.Name = "lblFrameCount";
            this.lblFrameCount.Size = new System.Drawing.Size(40, 24);
            this.lblFrameCount.TabIndex = 3;
            this.lblFrameCount.Text = "/ 0";
            this.lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numFrameIndex
            // 
            this.numFrameIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrameIndex.Location = new System.Drawing.Point(261, 9);
            this.numFrameIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFrameIndex.Name = "numFrameIndex";
            this.numFrameIndex.Size = new System.Drawing.Size(47, 20);
            this.numFrameIndex.TabIndex = 2;
            this.numFrameIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFrameIndex.ValueChanged += new System.EventHandler(this.numFrameIndex_ValueChanged);
            // 
            // btnPrevFrame
            // 
            this.btnPrevFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrevFrame.Enabled = false;
            this.btnPrevFrame.Location = new System.Drawing.Point(357, 6);
            this.btnPrevFrame.Name = "btnPrevFrame";
            this.btnPrevFrame.Size = new System.Drawing.Size(24, 24);
            this.btnPrevFrame.TabIndex = 1;
            this.btnPrevFrame.Text = "<";
            this.btnPrevFrame.UseVisualStyleBackColor = true;
            this.btnPrevFrame.Click += new System.EventHandler(this.btnPrevFrame_Click);
            // 
            // btnNextFrame
            // 
            this.btnNextFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextFrame.Enabled = false;
            this.btnNextFrame.Location = new System.Drawing.Point(387, 6);
            this.btnNextFrame.Name = "btnNextFrame";
            this.btnNextFrame.Size = new System.Drawing.Size(24, 24);
            this.btnNextFrame.TabIndex = 0;
            this.btnNextFrame.Text = ">";
            this.btnNextFrame.UseVisualStyleBackColor = true;
            this.btnNextFrame.Click += new System.EventHandler(this.btnNextFrame_Click);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.Location = new System.Drawing.Point(222, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 37);
            this.label15.TabIndex = 8;
            this.label15.Text = "Frame:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "All Files (*.*)|*.*";
            // 
            // btnFrames
            // 
            this.btnFrames.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnFrames.Location = new System.Drawing.Point(149, 494);
            this.btnFrames.Name = "btnFrames";
            this.btnFrames.Size = new System.Drawing.Size(417, 15);
            this.btnFrames.TabIndex = 8;
            this.btnFrames.TabStop = false;
            this.btnFrames.UseVisualStyleBackColor = false;
            this.btnFrames.Click += new System.EventHandler(this.btnFrames_Click);
            // 
            // animTimer
            // 
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // spltAssets
            // 
            this.spltAssets.Location = new System.Drawing.Point(130, 0);
            this.spltAssets.Name = "spltAssets";
            this.spltAssets.Size = new System.Drawing.Size(4, 546);
            this.spltAssets.TabIndex = 9;
            this.spltAssets.TabStop = false;
            this.spltAssets.Visible = false;
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.InitialYFactor = -100;
            this.modelPanel1.InitialZoomFactor = -5;
            this.modelPanel1.Location = new System.Drawing.Point(130, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(436, 546);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            // 
            // numScaleZ
            // 
            this.numScaleZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleZ.Location = new System.Drawing.Point(86, 192);
            this.numScaleZ.Name = "numScaleZ";
            this.numScaleZ.Size = new System.Drawing.Size(82, 20);
            this.numScaleZ.TabIndex = 20;
            this.numScaleZ.Text = "0";
            this.numScaleZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransX
            // 
            this.numTransX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransX.Location = new System.Drawing.Point(86, 16);
            this.numTransX.Name = "numTransX";
            this.numTransX.Size = new System.Drawing.Size(82, 20);
            this.numTransX.TabIndex = 3;
            this.numTransX.Text = "0";
            this.numTransX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numScaleY
            // 
            this.numScaleY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleY.Location = new System.Drawing.Point(86, 172);
            this.numScaleY.Name = "numScaleY";
            this.numScaleY.Size = new System.Drawing.Size(82, 20);
            this.numScaleY.TabIndex = 19;
            this.numScaleY.Text = "0";
            this.numScaleY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numScaleX
            // 
            this.numScaleX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numScaleX.Location = new System.Drawing.Point(86, 152);
            this.numScaleX.Name = "numScaleX";
            this.numScaleX.Size = new System.Drawing.Size(82, 20);
            this.numScaleX.TabIndex = 18;
            this.numScaleX.Text = "0";
            this.numScaleX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotZ
            // 
            this.numRotZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotZ.Location = new System.Drawing.Point(86, 124);
            this.numRotZ.Name = "numRotZ";
            this.numRotZ.Size = new System.Drawing.Size(82, 20);
            this.numRotZ.TabIndex = 17;
            this.numRotZ.Text = "0";
            this.numRotZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotY
            // 
            this.numRotY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotY.Location = new System.Drawing.Point(86, 104);
            this.numRotY.Name = "numRotY";
            this.numRotY.Size = new System.Drawing.Size(82, 20);
            this.numRotY.TabIndex = 16;
            this.numRotY.Text = "0";
            this.numRotY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numRotX
            // 
            this.numRotX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numRotX.Location = new System.Drawing.Point(86, 84);
            this.numRotX.Name = "numRotX";
            this.numRotX.Size = new System.Drawing.Size(82, 20);
            this.numRotX.TabIndex = 15;
            this.numRotX.Text = "0";
            this.numRotX.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransZ
            // 
            this.numTransZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransZ.Location = new System.Drawing.Point(86, 56);
            this.numTransZ.Name = "numTransZ";
            this.numTransZ.Size = new System.Drawing.Size(82, 20);
            this.numTransZ.TabIndex = 14;
            this.numTransZ.Text = "0";
            this.numTransZ.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // numTransY
            // 
            this.numTransY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numTransY.Location = new System.Drawing.Point(86, 36);
            this.numTransY.Name = "numTransY";
            this.numTransY.Size = new System.Drawing.Size(82, 20);
            this.numTransY.TabIndex = 13;
            this.numTransY.Text = "0";
            this.numTransY.ValueChanged += new System.EventHandler(this.BoxChanged);
            // 
            // modelAssetPanel1
            // 
            this.modelAssetPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelAssetPanel1.Location = new System.Drawing.Point(2, 30);
            this.modelAssetPanel1.Name = "modelAssetPanel1";
            this.modelAssetPanel1.Size = new System.Drawing.Size(128, 516);
            this.modelAssetPanel1.TabIndex = 4;
            this.modelAssetPanel1.TargetObject = null;
            this.modelAssetPanel1.TargetChanged += new System.EventHandler(this.modelAssetPanel1_TargetChanged);
            this.modelAssetPanel1.RenderStateChanged += new System.EventHandler(this.modelAssetPanel1_RenderStateChanged);
            // 
            // ModelEditControl
            // 
            this.Controls.Add(this.btnFrames);
            this.Controls.Add(this.pnlPlayback);
            this.Controls.Add(this.btnOptionToggle);
            this.Controls.Add(this.spltAssets);
            this.Controls.Add(this.modelPanel1);
            this.Controls.Add(this.btnAnimToggle);
            this.Controls.Add(this.pnlAnim);
            this.Controls.Add(this.pnlAssets);
            this.Name = "ModelEditControl";
            this.Size = new System.Drawing.Size(754, 546);
            this.Load += new System.EventHandler(this.ModelEditControl_Load);
            this.pnlAssets.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlAnim.ResumeLayout(false);
            this.grpTransform.ResumeLayout(false);
            this.grpTransform.PerformLayout();
            this.grpExt.ResumeLayout(false);
            this.grpExt.PerformLayout();
            this.pnlPlayback.ResumeLayout(false);
            this.pnlPlayback.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private int _animFrame, _maxFrame;
        private bool _updating, _loop;
        private ResourceNode _externalNode;
        private object _transformObject;
        private ListViewGroup _CHRGroup = new ListViewGroup("Character Animations");
        private MDL0BoneNode _selectedBone;
        private CHR0Node _selectedAnim;
        private NumericInputBox[] _transBoxes = new NumericInputBox[9];

        private MDL0Node _targetModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set
            {
                if (_targetModel == value)
                    return;

                _targetModel = value;
                ModelChanged();
            }
        }

        public ModelEditControl()
        {
            InitializeComponent();
            listAnims.Groups.Add(_CHRGroup);
            _transBoxes[0] = numScaleX; numScaleX.Tag = 0;
            _transBoxes[1] = numScaleY; numScaleY.Tag = 1;
            _transBoxes[2] = numScaleZ; numScaleZ.Tag = 2;
            _transBoxes[3] = numRotX; numRotX.Tag = 3;
            _transBoxes[4] = numRotY; numRotY.Tag = 4;
            _transBoxes[5] = numRotZ; numRotZ.Tag = 5;
            _transBoxes[6] = numTransX; numTransX.Tag = 6;
            _transBoxes[7] = numTransY; numTransY.Tag = 7;
            _transBoxes[8] = numTransZ; numTransZ.Tag = 8;
        }

        private void ModelChanged()
        {
            if (_externalNode != null)
            {
                _externalNode.Dispose();
                _externalNode = null;
            }

            modelPanel1.TargetModel = _targetModel;
            modelAssetPanel1.Attach(_targetModel);

            UpdateReferences();

            _animFrame = -1;
            SetFrame(0);
        }

        private bool UpdateReferences()
        {
            listAnims.BeginUpdate();
            listAnims.Items.Clear();

            if (_targetModel != null)
                LoadAnims(_targetModel.RootNode);

            int count = listAnims.Items.Count;

            if (_externalNode != null)
                LoadAnims(_externalNode.RootNode);

            if ((_selectedAnim != null) && (listAnims.SelectedItems.Count == 0))
            {
                _selectedAnim = null;
                AnimChanged();
            }

            listAnims.EndUpdate();

            return count != listAnims.Items.Count;
        }

        private void LoadAnims(ResourceNode root)
        {
            foreach (ResourceNode n in root.Children)
            {
                switch (n.ResourceType)
                {
                    case ResourceType.ARC:
                    case ResourceType.BRES:
                    case ResourceType.BRESGroup:
                        LoadAnims(n);
                        break;

                    case ResourceType.CHR0:
                        listAnims.Items.Add(new ListViewItem(n.Name, (int)n.ResourceType, _CHRGroup) { Tag = n });
                        break;
                }
            }
        }

        #region AnimationControls

        private void AnimChanged()
        {
            if (_selectedAnim == null)
            {
                numFrameIndex.Maximum = _maxFrame = 0;
                SetFrame(0);
            }
            else
            {
                if (_selectedAnim._numFrames < _maxFrame)
                {
                    SetFrame(1);
                    numFrameIndex.Maximum = _maxFrame = _selectedAnim._numFrames;
                }
                else
                {
                    numFrameIndex.Maximum = _maxFrame = _selectedAnim._numFrames;
                    SetFrame(1);
                }
            }

            lblFrameCount.Text = String.Format("/ {0}", _maxFrame);
        }

        private void SetFrame(int index)
        {
            if (_animFrame == index)
                return;

            if (_targetModel != null)
            {
                if (_selectedAnim != null)
                    _targetModel.ApplyCHR(_selectedAnim, index);
                else
                    _targetModel.ApplyCHR(null, 0);
            }
            else
                index = 0;

            _animFrame = index;

            btnNextFrame.Enabled = index < _maxFrame;
            btnPrevFrame.Enabled = index > 0;

            numFrameIndex.Value = index;

            UpdatePropDisplay();

            modelPanel1.Invalidate();
        }

        private void PlayAnim()
        {
            if (_selectedAnim == null)
                return;

            grpTransform.Enabled = false;

            if (_animFrame >= _maxFrame) //Reset anim
                SetFrame(1);

            if (_animFrame < _maxFrame)
                animTimer.Start();

            btnPlay.Text = "Stop";
        }
        private void StopAnim()
        {
            animTimer.Stop();
            btnPlay.Text = "Play";

            grpTransform.Enabled = _transformObject != null;
        }

        private void UpdatePropDisplay()
        {
            if (_transformObject == null)
            {
                grpTransform.Enabled = false;
            }
            else
            {
                grpTransform.Enabled = true;
                for (int i = 0; i < 9; i++)
                    ResetBox(i);
            }
        }

        private unsafe void ResetBox(int index)
        {
            NumericInputBox box = _transBoxes[index];

            if (_transformObject is MDL0BoneNode)
            {

                MDL0BoneNode bone = _transformObject as MDL0BoneNode;
                CHR0EntryNode entry;
                if ((_selectedAnim != null) && (_animFrame > 0) && ((entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode) != null))
                {
                    float val = entry.GetKeyframe((KeyFrameMode)index, _animFrame - 1);
                    if (float.IsNaN(val))
                    {
                        box.Value = entry.GetAnimFrame(_animFrame - 1)[index];
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        box.Value = val;
                        box.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    FrameState state = bone._bindState;
                    box.Value = ((float*)&state)[index];
                    box.BackColor = Color.White;
                }
            }
            else
            {
                box.Value = 0;
                box.BackColor = Color.White;
            }
        }
        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            if (_transformObject == null)
                return;

            NumericInputBox box = sender as NumericInputBox;
            int index = (int)box.Tag;

            if (_transformObject is MDL0BoneNode)
            {
                MDL0BoneNode bone = _transformObject as MDL0BoneNode;

                if ((_selectedAnim != null) && (_animFrame > 0))
                {
                    //Find bone anim and change transform
                    CHR0EntryNode entry = _selectedAnim.FindChild(bone.Name, false) as CHR0EntryNode;
                    if (entry == null) //Create new bone animation
                    {
                        if (!float.IsNaN(box.Value))
                        {
                            entry = _selectedAnim.CreateEntry();
                            entry._name = bone.Name;

                            //Set initial values
                            FrameState state = bone._bindState;
                            float* p = (float*)&state;
                            for (int i = 0; i < 3; i++)
                                if (p[i] != 1.0f)
                                    entry.SetKeyframe((KeyFrameMode)i, 0, p[i]);
                            for (int i = 3; i < 9; i++)
                                if (p[i] != 0.0f)
                                    entry.SetKeyframe((KeyFrameMode)i, 0, p[i]);

                            entry.SetKeyframe((KeyFrameMode)index, _animFrame - 1, box.Value);
                        }
                    }
                    else //Set existing 
                    {
                        if (float.IsNaN(box.Value))
                            entry.RemoveKeyframe((KeyFrameMode)index, _animFrame - 1);
                        else
                            entry.SetKeyframe((KeyFrameMode)index, _animFrame - 1, box.Value);
                    }
                }
                else
                {
                    //Change base transform
                    FrameState state = bone._bindState;
                    float* p = (float*)&state;
                    p[index] = float.IsNaN(box.Value) ? 0.0f : box.Value;
                    state.CalcTransforms();
                    bone._bindState = state;
                    bone.RecalcBindState();
                    bone.SignalPropertyChange();
                }

                //bone.ApplyCHR0(_selectedAnim, _animFrame);
                _targetModel.ApplyCHR(_selectedAnim, _animFrame);
                ResetBox(index);
                modelPanel1.Invalidate();
            }
        }

        private bool LoadExternal()
        {
            int count;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                ResourceNode node = null;
                listAnims.BeginUpdate();
                try
                {
                    if ((node = NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                    {
                        if (!CloseExternal())
                            return false;

                        count = listAnims.Items.Count;
                        LoadAnims(node);

                        if (count == listAnims.Items.Count)
                            MessageBox.Show(this, "No animations could be found in external file, closing.", "Error");
                        else
                        {
                            modelPanel1.AddReference(_externalNode = node);
                            node = null;
                            txtExtPath.Text = Path.GetFileName(dlgOpen.FileName);
                            return true;
                        }
                    }
                    else
                        MessageBox.Show(this, "Unable to recognize input file.");
                }
                catch (Exception x) { MessageBox.Show(this, x.ToString()); }
                finally
                {
                    if (node != null)
                        node.Dispose();
                    listAnims.EndUpdate();
                }
            }
            return false;
        }
        private bool CloseExternal()
        {
            if (_externalNode != null)
            {
                if (_externalNode.IsDirty)
                {
                    DialogResult res = MessageBox.Show(this, "You have made changes to an external file. Would you like to save those changes?", "Closing external file.", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!SaveExternal())) || (res == DialogResult.Cancel))
                        return false;
                }
                modelPanel1.RemoveReference(_externalNode);
                _externalNode.Dispose();
                _externalNode = null;
                txtExtPath.Text = "";
                UpdateReferences();
            }
            return true;
        }
        private bool SaveExternal()
        {
            if ((_externalNode == null) || (!_externalNode.IsDirty))
                return true;

            try
            {
                _externalNode.Merge();
                _externalNode.Export(_externalNode._origPath);
                return true;
            }
            catch (Exception x) { MessageBox.Show(this, x.ToString()); }
            return false;
        }

        #endregion

        private void label2_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                modelPanel1.BackColor = label2.BackColor = dlgColor.Color;
            }
        }

        private void btnOptionToggle_Click(object sender, EventArgs e)
        {
            if (spltAssets.Visible = pnlAssets.Visible = !pnlAssets.Visible)
                btnOptionToggle.Text = "<";
            else
                btnOptionToggle.Text = ">";
        }

        private void btnAnimToggle_Click(object sender, EventArgs e)
        {
            if (pnlAnim.Visible = !pnlAnim.Visible)
                btnAnimToggle.Text = ">";
            else
                btnAnimToggle.Text = "<";
        }
        private void ModelEditControl_Load(object sender, EventArgs e)
        {
            label2.BackColor = modelPanel1.BackColor;
        }

        private void listAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAnims.SelectedItems.Count > 0)
                _selectedAnim = listAnims.SelectedItems[0].Tag as CHR0Node;
            else
                _selectedAnim = null;
            AnimChanged();
        }

        private void btnOpen_Click(object sender, EventArgs e) { LoadExternal(); }
        private void textBox1_Click(object sender, EventArgs e) { LoadExternal(); }
        private void btnClose_Click(object sender, EventArgs e) { CloseExternal(); }
        private void btnSave_Click(object sender, EventArgs e) { SaveExternal(); }

        private void numFrameIndex_ValueChanged(object sender, EventArgs e) { SetFrame((int)numFrameIndex.Value); }

        private void btnPrevFrame_Click(object sender, EventArgs e) { numFrameIndex.Value--; }
        private void btnNextFrame_Click(object sender, EventArgs e) { numFrameIndex.Value++; }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (animTimer.Enabled)
                StopAnim();
            else
                PlayAnim();
        }
        private void animTimer_Tick(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            if (_animFrame >= _maxFrame)
                if (!_loop)
                    StopAnim();
                else
                    SetFrame(1);
            else
                SetFrame(_animFrame + 1);
        }
        private void numFPS_ValueChanged(object sender, EventArgs e) { animTimer.Interval = 1000 / (int)numFPS.Value; }
        private void chkLoop_CheckedChanged(object sender, EventArgs e) { _loop = chkLoop.Checked; }

        private void modelAssetPanel1_RenderStateChanged(object sender, EventArgs e) { modelPanel1.Invalidate(); }
        private void modelAssetPanel1_TargetChanged(object sender, EventArgs e)
        {
            _transformObject = modelAssetPanel1.TargetObject;
            UpdatePropDisplay();
        }

        private void btnFrames_Click(object sender, EventArgs e)
        {
            if (pnlPlayback.Visible = !pnlPlayback.Visible)
            {
            }
        }



    }
}
