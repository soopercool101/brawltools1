using System;
using BrawlLib.SSBB.ResourceNodes;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    public unsafe class CollisionEditor : UserControl
    {
        #region Designer

        private ModelPanel _modelPanel;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private CheckedListBox _lstModels;
        private CheckBox chkAllModels;
        private SplitContainer splitContainer3;
        private Panel pnlPlaneProps;
        private Label label5;
        private ComboBox cboMaterial;
        private Panel pnlObjProps;
        private ToolStrip toolStrip1;
        private ToolStripButton btnSplit;
        private ToolStripButton btnMerge;
        private ToolStripButton btnDelete;
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem snapToolStripMenuItem;
        private Panel panel1;
        private TrackBar trackBar1;
        private Button btnResetRot;
        private ToolStripButton btnResetCam;
        private GroupBox groupBox1;
        private CheckBox chkFallThrough;
        private GroupBox groupBox2;
        private CheckBox chkFlagUnk2;
        private CheckBox chkFlagUnk1;
        private CheckBox chkFloor;
        private CheckBox chkLeftWall;
        private CheckBox chkCeiling;
        private CheckBox chkRightWall;
        private CheckBox chkTypeUnk1;
        private CheckBox chkTypeUnk2;
        private Panel pnlPointProps;
        private NumericInputBox numX;
        private Label label2;
        private NumericInputBox numY;
        private Label label1;
        private TreeView _treeObjects;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollisionEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._lstModels = new System.Windows.Forms.CheckedListBox();
            this.chkAllModels = new System.Windows.Forms.CheckBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._treeObjects = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.snapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlObjProps = new System.Windows.Forms.Panel();
            this.pnlPlaneProps = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkTypeUnk1 = new System.Windows.Forms.CheckBox();
            this.chkRightWall = new System.Windows.Forms.CheckBox();
            this.chkCeiling = new System.Windows.Forms.CheckBox();
            this.chkFloor = new System.Windows.Forms.CheckBox();
            this.chkLeftWall = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTypeUnk2 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnk2 = new System.Windows.Forms.CheckBox();
            this.chkFlagUnk1 = new System.Windows.Forms.CheckBox();
            this.chkFallThrough = new System.Windows.Forms.CheckBox();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlPointProps = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.NumericInputBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numX = new System.Windows.Forms.NumericInputBox();
            this._modelPanel = new System.Windows.Forms.ModelPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSplit = new System.Windows.Forms.ToolStripButton();
            this.btnMerge = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnResetCam = new System.Windows.Forms.ToolStripButton();
            this.btnResetRot = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlPlaneProps.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlPointProps.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._modelPanel);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(694, 467);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._lstModels);
            this.splitContainer2.Panel1.Controls.Add(this.chkAllModels);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(209, 467);
            this.splitContainer2.SplitterDistance = 180;
            this.splitContainer2.TabIndex = 2;
            // 
            // _lstModels
            // 
            this._lstModels.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._lstModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lstModels.FormattingEnabled = true;
            this._lstModels.IntegralHeight = false;
            this._lstModels.Location = new System.Drawing.Point(0, 17);
            this._lstModels.Name = "_lstModels";
            this._lstModels.Size = new System.Drawing.Size(209, 163);
            this._lstModels.TabIndex = 1;
            this._lstModels.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._lstModels_ItemCheck);
            // 
            // chkAllModels
            // 
            this.chkAllModels.AutoSize = true;
            this.chkAllModels.Checked = true;
            this.chkAllModels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllModels.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkAllModels.Location = new System.Drawing.Point(0, 0);
            this.chkAllModels.Name = "chkAllModels";
            this.chkAllModels.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.chkAllModels.Size = new System.Drawing.Size(209, 17);
            this.chkAllModels.TabIndex = 2;
            this.chkAllModels.Text = "All";
            this.chkAllModels.UseVisualStyleBackColor = true;
            this.chkAllModels.CheckedChanged += new System.EventHandler(this.chkAllModels_CheckedChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this._treeObjects);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.pnlObjProps);
            this.splitContainer3.Panel2.Controls.Add(this.pnlPlaneProps);
            this.splitContainer3.Panel2.Controls.Add(this.pnlPointProps);
            this.splitContainer3.Panel2MinSize = 130;
            this.splitContainer3.Size = new System.Drawing.Size(209, 283);
            this.splitContainer3.SplitterDistance = 148;
            this.splitContainer3.TabIndex = 1;
            // 
            // _treeObjects
            // 
            this._treeObjects.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._treeObjects.CheckBoxes = true;
            this._treeObjects.ContextMenuStrip = this.contextMenuStrip1;
            this._treeObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeObjects.HideSelection = false;
            this._treeObjects.Location = new System.Drawing.Point(0, 0);
            this._treeObjects.Name = "_treeObjects";
            this._treeObjects.Size = new System.Drawing.Size(209, 148);
            this._treeObjects.TabIndex = 0;
            this._treeObjects.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this._treeObjects_AfterCheck);
            this._treeObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeObjects_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snapToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            // 
            // snapToolStripMenuItem
            // 
            this.snapToolStripMenuItem.Name = "snapToolStripMenuItem";
            this.snapToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.snapToolStripMenuItem.Text = "Snap";
            this.snapToolStripMenuItem.Click += new System.EventHandler(this.snapToolStripMenuItem_Click);
            // 
            // pnlObjProps
            // 
            this.pnlObjProps.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlObjProps.Location = new System.Drawing.Point(0, 260);
            this.pnlObjProps.Name = "pnlObjProps";
            this.pnlObjProps.Size = new System.Drawing.Size(209, 130);
            this.pnlObjProps.TabIndex = 1;
            this.pnlObjProps.Visible = false;
            // 
            // pnlPlaneProps
            // 
            this.pnlPlaneProps.Controls.Add(this.groupBox2);
            this.pnlPlaneProps.Controls.Add(this.groupBox1);
            this.pnlPlaneProps.Controls.Add(this.cboMaterial);
            this.pnlPlaneProps.Controls.Add(this.label5);
            this.pnlPlaneProps.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPlaneProps.Location = new System.Drawing.Point(0, 130);
            this.pnlPlaneProps.Name = "pnlPlaneProps";
            this.pnlPlaneProps.Size = new System.Drawing.Size(209, 130);
            this.pnlPlaneProps.TabIndex = 0;
            this.pnlPlaneProps.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.chkTypeUnk1);
            this.groupBox2.Controls.Add(this.chkRightWall);
            this.groupBox2.Controls.Add(this.chkCeiling);
            this.groupBox2.Controls.Add(this.chkFloor);
            this.groupBox2.Controls.Add(this.chkLeftWall);
            this.groupBox2.Location = new System.Drawing.Point(104, 28);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(105, 102);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Types";
            // 
            // chkTypeUnk1
            // 
            this.chkTypeUnk1.Location = new System.Drawing.Point(8, 81);
            this.chkTypeUnk1.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeUnk1.Name = "chkTypeUnk1";
            this.chkTypeUnk1.Size = new System.Drawing.Size(86, 18);
            this.chkTypeUnk1.TabIndex = 4;
            this.chkTypeUnk1.Text = "Unknown 1";
            this.chkTypeUnk1.UseVisualStyleBackColor = true;
            this.chkTypeUnk1.CheckedChanged += new System.EventHandler(this.chkTypeUnk1_CheckedChanged);
            // 
            // chkRightWall
            // 
            this.chkRightWall.Location = new System.Drawing.Point(8, 49);
            this.chkRightWall.Margin = new System.Windows.Forms.Padding(0);
            this.chkRightWall.Name = "chkRightWall";
            this.chkRightWall.Size = new System.Drawing.Size(86, 18);
            this.chkRightWall.TabIndex = 6;
            this.chkRightWall.Text = "Right Wall";
            this.chkRightWall.UseVisualStyleBackColor = true;
            this.chkRightWall.CheckedChanged += new System.EventHandler(this.chkRightWall_CheckedChanged);
            // 
            // chkCeiling
            // 
            this.chkCeiling.Location = new System.Drawing.Point(8, 33);
            this.chkCeiling.Margin = new System.Windows.Forms.Padding(0);
            this.chkCeiling.Name = "chkCeiling";
            this.chkCeiling.Size = new System.Drawing.Size(86, 18);
            this.chkCeiling.TabIndex = 4;
            this.chkCeiling.Text = "Ceiling";
            this.chkCeiling.UseVisualStyleBackColor = true;
            this.chkCeiling.CheckedChanged += new System.EventHandler(this.chkCeiling_CheckedChanged);
            // 
            // chkFloor
            // 
            this.chkFloor.Location = new System.Drawing.Point(8, 17);
            this.chkFloor.Margin = new System.Windows.Forms.Padding(0);
            this.chkFloor.Name = "chkFloor";
            this.chkFloor.Size = new System.Drawing.Size(86, 18);
            this.chkFloor.TabIndex = 3;
            this.chkFloor.Text = "Floor";
            this.chkFloor.UseVisualStyleBackColor = true;
            this.chkFloor.CheckedChanged += new System.EventHandler(this.chkFloor_CheckedChanged);
            // 
            // chkLeftWall
            // 
            this.chkLeftWall.Location = new System.Drawing.Point(8, 65);
            this.chkLeftWall.Margin = new System.Windows.Forms.Padding(0);
            this.chkLeftWall.Name = "chkLeftWall";
            this.chkLeftWall.Size = new System.Drawing.Size(86, 18);
            this.chkLeftWall.TabIndex = 5;
            this.chkLeftWall.Text = "Left Wall";
            this.chkLeftWall.UseVisualStyleBackColor = true;
            this.chkLeftWall.CheckedChanged += new System.EventHandler(this.chkLeftWall_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.chkTypeUnk2);
            this.groupBox1.Controls.Add(this.chkFlagUnk2);
            this.groupBox1.Controls.Add(this.chkFlagUnk1);
            this.groupBox1.Controls.Add(this.chkFallThrough);
            this.groupBox1.Location = new System.Drawing.Point(0, 28);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(104, 102);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Flags";
            // 
            // chkTypeUnk2
            // 
            this.chkTypeUnk2.Location = new System.Drawing.Point(8, 65);
            this.chkTypeUnk2.Margin = new System.Windows.Forms.Padding(0);
            this.chkTypeUnk2.Name = "chkTypeUnk2";
            this.chkTypeUnk2.Size = new System.Drawing.Size(86, 18);
            this.chkTypeUnk2.TabIndex = 3;
            this.chkTypeUnk2.Text = "Unknown 3";
            this.chkTypeUnk2.UseVisualStyleBackColor = true;
            this.chkTypeUnk2.CheckedChanged += new System.EventHandler(this.chkTypeUnk2_CheckedChanged);
            // 
            // chkFlagUnk2
            // 
            this.chkFlagUnk2.Location = new System.Drawing.Point(8, 49);
            this.chkFlagUnk2.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnk2.Name = "chkFlagUnk2";
            this.chkFlagUnk2.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnk2.TabIndex = 2;
            this.chkFlagUnk2.Text = "Unknown 2";
            this.chkFlagUnk2.UseVisualStyleBackColor = true;
            this.chkFlagUnk2.CheckedChanged += new System.EventHandler(this.chkFlagUnk2_CheckedChanged);
            // 
            // chkFlagUnk1
            // 
            this.chkFlagUnk1.Location = new System.Drawing.Point(8, 33);
            this.chkFlagUnk1.Margin = new System.Windows.Forms.Padding(0);
            this.chkFlagUnk1.Name = "chkFlagUnk1";
            this.chkFlagUnk1.Size = new System.Drawing.Size(86, 18);
            this.chkFlagUnk1.TabIndex = 1;
            this.chkFlagUnk1.Text = "Unknown 1";
            this.chkFlagUnk1.UseVisualStyleBackColor = true;
            this.chkFlagUnk1.CheckedChanged += new System.EventHandler(this.chkFlagUnk1_CheckedChanged);
            // 
            // chkFallThrough
            // 
            this.chkFallThrough.Location = new System.Drawing.Point(8, 17);
            this.chkFallThrough.Margin = new System.Windows.Forms.Padding(0);
            this.chkFallThrough.Name = "chkFallThrough";
            this.chkFallThrough.Size = new System.Drawing.Size(86, 18);
            this.chkFallThrough.TabIndex = 0;
            this.chkFallThrough.Text = "Fall-Through";
            this.chkFallThrough.UseVisualStyleBackColor = true;
            this.chkFallThrough.CheckedChanged += new System.EventHandler(this.chkFallThrough_CheckedChanged);
            // 
            // cboMaterial
            // 
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(66, 4);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(139, 21);
            this.cboMaterial.TabIndex = 12;
            this.cboMaterial.SelectedIndexChanged += new System.EventHandler(this.cboMaterial_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Material:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlPointProps
            // 
            this.pnlPointProps.Controls.Add(this.label2);
            this.pnlPointProps.Controls.Add(this.numY);
            this.pnlPointProps.Controls.Add(this.label1);
            this.pnlPointProps.Controls.Add(this.numX);
            this.pnlPointProps.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPointProps.Location = new System.Drawing.Point(0, 0);
            this.pnlPointProps.Name = "pnlPointProps";
            this.pnlPointProps.Size = new System.Drawing.Size(209, 130);
            this.pnlPointProps.TabIndex = 15;
            this.pnlPointProps.Visible = false;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(18, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numY
            // 
            this.numY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numY.Location = new System.Drawing.Point(59, 32);
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(100, 20);
            this.numY.TabIndex = 2;
            this.numY.Text = "0";
            this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(18, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numX
            // 
            this.numX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numX.Location = new System.Drawing.Point(59, 13);
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(100, 20);
            this.numX.TabIndex = 0;
            this.numX.Text = "0";
            this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
            // 
            // _modelPanel
            // 
            this._modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._modelPanel.InitialYFactor = 100;
            this._modelPanel.InitialZoomFactor = 5;
            this._modelPanel.Location = new System.Drawing.Point(0, 25);
            this._modelPanel.Name = "_modelPanel";
            this._modelPanel.RotationScale = 0.1F;
            this._modelPanel.Size = new System.Drawing.Size(481, 442);
            this._modelPanel.TabIndex = 0;
            this._modelPanel.TranslationScale = 0.05F;
            this._modelPanel.ZoomScale = 2.5F;
            this._modelPanel.PreRender += new System.Windows.Forms.GLRenderEventHandler(this._modelPanel_PreRender);
            this._modelPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseMove);
            this._modelPanel.PostRender += new System.Windows.Forms.GLRenderEventHandler(this._modelPanel_PostRender);
            this._modelPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseDown);
            this._modelPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this._modelPanel_MouseUp);
            this._modelPanel.KeyDown += new System.Windows.Forms.KeyEventHandler(this._modelPanel_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.btnResetRot);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 25);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSplit,
            this.btnMerge,
            this.btnDelete,
            this.btnResetCam});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(317, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSplit
            // 
            this.btnSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSplit.Enabled = false;
            this.btnSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(34, 22);
            this.btnSplit.Text = "Split";
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMerge.Enabled = false;
            this.btnMerge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(45, 22);
            this.btnMerge.Text = "Merge";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.Enabled = false;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(44, 22);
            this.btnDelete.Text = "Delete";
            // 
            // btnResetCam
            // 
            this.btnResetCam.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResetCam.Image = ((System.Drawing.Image)(resources.GetObject("btnResetCam.Image")));
            this.btnResetCam.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetCam.Name = "btnResetCam";
            this.btnResetCam.Size = new System.Drawing.Size(83, 22);
            this.btnResetCam.Text = "Reset Camera";
            this.btnResetCam.Click += new System.EventHandler(this.btnResetCam_Click);
            // 
            // btnResetRot
            // 
            this.btnResetRot.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnResetRot.FlatAppearance.BorderSize = 0;
            this.btnResetRot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetRot.Location = new System.Drawing.Point(317, 0);
            this.btnResetRot.Name = "btnResetRot";
            this.btnResetRot.Size = new System.Drawing.Size(16, 25);
            this.btnResetRot.TabIndex = 4;
            this.btnResetRot.Text = "*";
            this.btnResetRot.UseVisualStyleBackColor = true;
            this.btnResetRot.Click += new System.EventHandler(this.btnResetRot_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.trackBar1.Location = new System.Drawing.Point(333, 0);
            this.trackBar1.Maximum = 180;
            this.trackBar1.Minimum = -180;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(148, 25);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // CollisionEditor
            // 
            this.BackColor = System.Drawing.Color.Lavender;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CollisionEditor";
            this.Size = new System.Drawing.Size(694, 467);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlPlaneProps.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnlPointProps.ResumeLayout(false);
            this.pnlPointProps.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private const float SelectWidth = 7.0f;
        private const float PointSelectRadius = 1.5f;

        private CollisionNode _targetNode;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CollisionNode TargetNode
        {
            get { return _targetNode; }
            set { TargetChanged(value); }
        }

        private bool _updating;
        private TreeNode _selectedNode;
        private CollisionPlane _selectedPlane;
        private CollisionObject _selectedObject;
        private Matrix _snapMatrix;

        private bool _hovering;
        private List<CollisionLink> _selectedLinks = new List<CollisionLink>();
        private List<CollisionPlane> _selectedPlanes = new List<CollisionPlane>();

        private bool _selecting, _selectInverse;
        private Vector3 _selectStart, _selectLast, _selectEnd;

        public CollisionEditor()
        {
            InitializeComponent();
            _modelPanel._defaultTranslate = new Vector3(0.0f, 10.0f, 250.0f);

            pnlObjProps.Dock = DockStyle.Fill;
            pnlPlaneProps.Dock = DockStyle.Fill;
            pnlPointProps.Dock = DockStyle.Fill;

            foreach (CollisionPlaneMaterial m in Enum.GetValues(typeof(CollisionPlaneMaterial)))
                cboMaterial.Items.Add(m);
        }


        private void TargetChanged(CollisionNode node)
        {
            ClearSelection();
            trackBar1.Value = 0;
            _snapMatrix = Matrix.Identity;

            _lstModels.BeginUpdate();
            _lstModels.Items.Clear();

            _treeObjects.BeginUpdate();
            _treeObjects.Nodes.Clear();

            _modelPanel.ClearAll();

            _selectedNode = null;
            //SelectionChanged();

            if ((_targetNode = node) != null)
            {
                AttachModels();
                AttachObjects();
            }

            _lstModels.EndUpdate();
            _treeObjects.EndUpdate();

            _modelPanel.ResetCamera();
        }

        private void SelectionModified()
        {
            _selectedPlanes.Clear();
            foreach (CollisionLink l in _selectedLinks)
                foreach (CollisionPlane p in l._members)
                    if (_selectedLinks.Contains(p._linkLeft) &&
                        _selectedLinks.Contains(p._linkRight) &&
                        !_selectedPlanes.Contains(p))
                        _selectedPlanes.Add(p);

            UpdatePropVisibility();
            UpdatePropPanels();
        }

        private void UpdatePropVisibility()
        {
            pnlPlaneProps.Visible = false;
            pnlObjProps.Visible = false;
            pnlPointProps.Visible = false;

            if (_modelPanel.Capture)
            {
                if (_selectedPlanes.Count > 0)
                    pnlPlaneProps.Visible = true;
                else if (_selectedLinks.Count == 1)
                    pnlPointProps.Visible = true;
            }
        }
        private void UpdatePropPanels()
        {
            _updating = true;

            if (pnlPlaneProps.Visible)
            {
                CollisionPlane p = _selectedPlanes[0];

                //Material
                cboMaterial.SelectedItem = p._material;
                //Flags
                chkFallThrough.Checked = (p._flags & CollisionPlaneFlags.DropThrough) != 0;
                chkFlagUnk1.Checked = (p._flags & CollisionPlaneFlags.Unk1) != 0;
                chkFlagUnk2.Checked = (p._flags & CollisionPlaneFlags.Unk2) != 0;
                //Type
                chkCeiling.Checked = (p._type & CollisionPlaneType.Ceiling) != 0;
                chkFloor.Checked = (p._type & CollisionPlaneType.Floor) != 0;
                chkLeftWall.Checked = (p._type & CollisionPlaneType.LeftWall) != 0;
                chkRightWall.Checked = (p._type & CollisionPlaneType.RightWall) != 0;
                chkTypeUnk1.Checked = (p._type & CollisionPlaneType.Unk1) != 0;
                chkTypeUnk2.Checked = (p._type & CollisionPlaneType.Unk2) != 0;
            }
            else if (pnlPointProps.Visible)
            {
                numX.Value = _selectedLinks[0]._value._x;
                numY.Value = _selectedLinks[0]._value._y;
            }

            _updating = false;
        }

        private void AttachModels()
        {
            if (_targetNode._parent != null)
            {
                foreach (MDL0Node n in _targetNode._parent.FindChildrenByType(null, ResourceType.MDL0))
                {
                    _lstModels.Items.Add(n, true);
                    _modelPanel.AddTarget(n);
                    n._renderBones = false;
                }
            }
        }
        private void AttachObjects()
        {
            int oCount = 0;
            foreach (CollisionObject obj in _targetNode._objects)
            {
                _treeObjects.Nodes.Add(new TreeNode(String.Format("Object {0}", oCount++)) { Checked = true, Tag = obj });

                //Link objects to bones and set matrices
                //obj._transform = obj._inverseTransform = Matrix.Identity;
                //foreach (MDL0Node node in _lstModels.Items)
                //    if (node._name == obj._modelName)
                //    {
                //        foreach (MDL0BoneNode bone in node.FindChildrenByType("Bones", ResourceType.MDL0Bone))
                //            if (obj._boneName == bone._name)
                //            {
                //                obj._transform = bone._bindMatrix;
                //                obj._inverseTransform = bone._inverseBindMatrix;
                //                break;
                //            }
                //        break;
                //    }

            }

        }


        private void ClearSelection()
        {
            foreach (CollisionLink l in _selectedLinks)
                l._highlight = false;
            _selectedLinks.Clear();
            _selectedPlanes.Clear();
        }

        //To do: fix selections that are hidden and/or invalid
        private void FixSelection()
        {
        }

        private void UpdateSelection(bool finish)
        {
            foreach (CollisionObject obj in _targetNode._objects)
                foreach (CollisionLink link in obj._points)
                {
                    link._highlight = false;
                    if (!obj._render)
                        continue;

                    Vector3 point = (Vector3)link._value;

                    if (_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        if (finish)
                            _selectedLinks.Remove(link);
                        continue;
                    }

                    if (_selectedLinks.Contains(link))
                        link._highlight = true;
                    else if (!_selectInverse && point.Contained(_selectStart, _selectEnd, 0.0f))
                    {
                        link._highlight = true;
                        if (finish)
                            _selectedLinks.Add(link);
                    }
                }
        }
        public void UpdateTools()
        {
            if (_selecting || _hovering || (_selectedLinks.Count == 0))
            {
                btnMerge.Enabled = btnSplit.Enabled = false;
            }
            else
            {
                btnMerge.Enabled = _selectedLinks.Count > 1;
                btnSplit.Enabled = true;
            }
        }
        private void ShowModels(bool show)
        {
            _updating = true;
            for (int i = 0; i < _lstModels.Items.Count; i++)
                _lstModels.SetItemChecked(i, show);
            _updating = false;

            _modelPanel.Invalidate();
        }

        private void _lstModels_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            MDL0Node m = _lstModels.Items[e.Index] as MDL0Node;

            m._visible = e.NewValue == CheckState.Checked;

            if (!_updating)
                _modelPanel.Invalidate();
        }

        private void _treeObjects_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is CollisionObject)
                (e.Node.Tag as CollisionObject)._render = e.Node.Checked;
            if (e.Node.Tag is CollisionPlane)
                (e.Node.Tag as CollisionPlane)._render = e.Node.Checked;

            _modelPanel.Invalidate();
        }

        private void _treeObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _selectedNode = e.Node;
            //SelectionChanged();
        }

        private void chkAllModels_CheckedChanged(object sender, EventArgs e)
        {
            ShowModels(chkAllModels.Checked);
        }


        private void BeginHover(Vector3 point)
        {
            if (_hovering)
                return;

            _selectStart = _selectLast = point;
            _hovering = true;
            UpdateTools();
        }
        private void UpdateHover(int x, int y)
        {
            if (!_hovering)
                return;

            _selectEnd = Vector3.IntersectZ(_modelPanel.UnProject(x, y, 0.0f), _modelPanel.UnProject(x, y, 1.0f), _selectLast._z);

            //Apply difference in start/end
            Vector3 diff = _selectEnd - _selectLast;
            _selectLast = _selectEnd;

            //Move points
            foreach (CollisionLink p in _selectedLinks)
                p._value += diff;

            _modelPanel.Invalidate();

            UpdatePropPanels();
        }
        private void CancelHover()
        {
            if (!_hovering)
                return;

            _hovering = false;
            Vector3 diff = _selectStart - _selectLast;
            foreach (CollisionLink l in _selectedLinks)
                l._value += diff;
            _modelPanel.Invalidate();

            UpdatePropPanels();
        }
        private void FinishHover()
        {
            _hovering = false;
        }
        private void BeginSelection(Vector3 point, bool inverse)
        {
            if (_selecting)
                return;

            _selectStart = _selectEnd = point;

            _selectEnd._z += SelectWidth;
            _selectStart._z -= SelectWidth;

            _selecting = true;
            _selectInverse = inverse;

            UpdateTools();
        }
        private void CancelSelection()
        {
            if (!_selecting)
                return;

            _selecting = false;
            _selectStart = _selectEnd = new Vector3(float.MaxValue);
            UpdateSelection(false);
            _modelPanel.Invalidate();
        }
        private void FinishSelection()
        {
            if (!_selecting)
                return;

            _selecting = false;
            UpdateSelection(true);
            _modelPanel.Invalidate();

            SelectionModified();
        }

        private void _modelPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Move selection based on absolution location
                //if ((Control.ModifierKeys & Keys.Alt) != 0)
                //{
                //    if (_selectedLinks.Count > 0)
                //        BeginHover(Vector3.IntersectZ(_modelPanel.UnProject(e.X, e.Y, 0.0f), _modelPanel.UnProject(e.X, e.Y, 1.0f), 0.0f));
                //    return;
                //}

                //Create new point/planes
                //if ((Control.ModifierKeys & Keys.Control) != 0)
                //{
                //    if (_selectedLinks.Count > 0)
                //    {
                //    }
                //    else
                //    {

                //    }
                //    return;
                //}

                bool create = Control.ModifierKeys == Keys.Alt;
                bool add = Control.ModifierKeys == Keys.Shift;
                bool subtract = Control.ModifierKeys == Keys.Control;

                float depth = _modelPanel.GetDepth(e.X, e.Y);
                Vector3 target = _modelPanel.UnProject(e.X, e.Y, depth);

                if (depth < 1.0f)
                {
                    Vector2 point = (Vector2)target;

                    //Hit-detect points first
                    foreach (CollisionObject obj in _targetNode._objects)
                        if (obj._render)
                            foreach (CollisionLink p in obj._points)
                                if (p._value.Contained(point, point, PointSelectRadius))
                                {
                                    if (create)
                                    {
                                        //Connect all selected links to point
                                        foreach (CollisionLink l in _selectedLinks)
                                            l.Connect(p);

                                        //Select point
                                        ClearSelection();
                                        p._highlight = true;
                                        _selectedLinks.Add(p);
                                        SelectionModified();

                                        _modelPanel.Invalidate();
                                        return;
                                    }

                                    if (subtract)
                                    {
                                        p._highlight = false;
                                        _selectedLinks.Remove(p);
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }
                                    else if (!_selectedLinks.Contains(p))
                                    {
                                        if (!add)
                                            ClearSelection();

                                        _selectedLinks.Add(p);
                                        p._highlight = true;
                                        _modelPanel.Invalidate();
                                        SelectionModified();
                                    }

                                    if ((!add) && (!subtract))
                                        BeginHover(target);

                                    return;
                                }

                    float dist;
                    float bestDist = float.MaxValue;
                    CollisionPlane bestMatch = null;

                    //Hit-detect planes finding best match
                    foreach (CollisionObject obj in _targetNode._objects)
                        if (obj._render)
                            foreach (CollisionPlane p in obj._planes)
                                if (point.Contained(p.PointLeft, p.PointRight, PointSelectRadius))
                                {
                                    dist = point.TrueDistance(p.PointLeft) + point.TrueDistance(p.PointRight) - p.PointLeft.TrueDistance(p.PointRight);
                                    if (dist < bestDist)
                                    { bestDist = dist; bestMatch = p; }
                                }

                    if (bestMatch != null)
                    {
                        if (create)
                        {
                            //Create new point at plane intersection. Nothing else to do right?

                        }

                        if (subtract)
                        {
                            _selectedLinks.Remove(bestMatch._linkLeft);
                            _selectedLinks.Remove(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = false;
                            _modelPanel.Invalidate();

                            SelectionModified();
                            return;
                        }

                        //Select both points
                        if (!_selectedLinks.Contains(bestMatch._linkLeft) || !_selectedLinks.Contains(bestMatch._linkRight))
                        {
                            if (!add)
                                ClearSelection();

                            _selectedLinks.Add(bestMatch._linkLeft);
                            _selectedLinks.Add(bestMatch._linkRight);
                            bestMatch._linkLeft._highlight = bestMatch._linkRight._highlight = true;
                            _modelPanel.Invalidate();

                            SelectionModified();
                        }

                        if (!add)
                            BeginHover(target);

                        return;
                    }
                }

                //Nothing found :(

                //Trace ray to Z axis
                target = Vector3.IntersectZ(target, _modelPanel.UnProject(e.X, e.Y, 0.0f), 0.0f);

                if (create)
                {
                    if (_selectedLinks.Count == 0)
                    {
                    }
                    else if (_selectedLinks.Count == 1)
                    {
                        //Create new plane extending to point
                        CollisionLink link = _selectedLinks[0];
                        _selectedLinks[0] = link.Branch((Vector2)target);
                        _selectedLinks[0]._highlight = true;
                        link._highlight = false;
                        SelectionModified();
                        _modelPanel.Invalidate();

                        //Hover new point so it can be moved
                        BeginHover(target);
                        return;
                    }
                }

                if (!add && !subtract)
                    ClearSelection();

                BeginSelection(target, subtract);
            }
        }
        private void _modelPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FinishSelection();
                FinishHover();
                UpdateTools();
            }
        }

        private void _modelPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selecting)
            {
                Vector3 ray1 = _modelPanel.UnProject(new Vector3(e.X, e.Y, 0.0f));
                Vector3 ray2 = _modelPanel.UnProject(new Vector3(e.X, e.Y, 1.0f));

                _selectEnd = Vector3.IntersectZ(ray1, ray2, 0.0f);
                _selectEnd._z += SelectWidth;

                //Update selection
                UpdateSelection(false);

                _modelPanel.Invalidate();
            }
            UpdateHover(e.X, e.Y);
        }

        private void _modelPanel_PreRender(object sender, GLContext context)
        {
            context.glPushMatrix();

            //Rotate adjuster
            context.glRotate(trackBar1.Value, 0.0f, 1.0f, 0.0f);

            //Apply snap matrix
            fixed (Matrix* m = &_snapMatrix)
                context.glMultMatrix((float*)m);
        }

        private unsafe void _modelPanel_PostRender(object sender, GLContext context)
        {
            //Pop snap matrix
            context.glPopMatrix();

            //Clear depth buffer so we can hit-detect
            context.glClear(GLClearMask.DepthBuffer);

            //Render objects
            if (_targetNode != null)
                _targetNode.Render(context);

            //Render selection box
            if (!_selecting)
                return;

            context.glEnable(GLEnableCap.DepthTest);

            //Draw lines
            context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
            context.glColor(0.0f, 0.0f, 1.0f, 0.5f);
            context.DrawBox(_selectStart, _selectEnd);

            //Draw box
            context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);
            context.glColor(1.0f, 1.0f, 0.0f, 0.2f);
            context.DrawBox(_selectStart, _selectEnd);
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            for (int i = _selectedLinks.Count; --i >= 0; )
                _selectedLinks.AddRange(_selectedLinks[i].Split());
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _selectedLinks.Count - 1; )
            {
                if (_selectedLinks[i].Merge(_selectedLinks[i + 1]))
                    _selectedLinks.RemoveAt(i + 1);
                else
                    i++;
            }
            _modelPanel.Invalidate();
        }

        private void snapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedObject is CollisionObject)
            {
                _snapMatrix = Matrix.Identity;

                //Find model/node and set snap matrix
                CollisionObject obj = _selectedObject as CollisionObject;

                foreach (MDL0Node node in _lstModels.Items)
                    if (node._name == obj._modelName)
                    {
                        foreach (MDL0BoneNode bone in node.FindChildrenByType("Bones", ResourceType.MDL0Bone))
                            if (obj._boneName == bone._name)
                            {
                                _snapMatrix = bone._inverseBindMatrix;
                                break;
                            }
                        break;
                    }

                _modelPanel.Invalidate();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e) { _modelPanel.Invalidate(); }
        private void btnResetRot_Click(object sender, EventArgs e) { trackBar1.Value = 0; _modelPanel.Invalidate(); }
        private void btnResetCam_Click(object sender, EventArgs e) { _modelPanel.ResetCamera(); }

        private void _modelPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (_hovering)
                    CancelHover();
                else if (_selecting)
                    CancelSelection();
                else
                    ClearSelection();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                foreach (CollisionLink link in _selectedLinks)
                    link.Delete();

                ClearSelection();
                SelectionModified();
                _modelPanel.Invalidate();
            }
        }

        #region Plane Properties

        private void SetType(int bits, bool set)
        {
            if (_updating)
                return;

            int mask = ~bits;
            bits = set ? bits : 0;
            foreach (CollisionPlane plane in _selectedPlanes)
                plane._type = (CollisionPlaneType)(((int)plane._type & mask) | bits);
        }
        private void SetFlag(int bits, bool set)
        {
            if (_updating)
                return;

            int mask = ~bits;
            bits = set ? bits : 0;
            foreach (CollisionPlane plane in _selectedPlanes)
                plane._flags = (CollisionPlaneFlags)(((int)plane._flags & mask) | bits);
        }

        private void cboMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            foreach (CollisionPlane plane in _selectedPlanes)
                plane._material = (CollisionPlaneMaterial)cboMaterial.SelectedItem;
        }

        private void chkFloor_CheckedChanged(object sender, EventArgs e) { SetType((int)CollisionPlaneType.Floor, chkFloor.Checked); }
        private void chkCeiling_CheckedChanged(object sender, EventArgs e) { SetType((int)CollisionPlaneType.Ceiling, chkCeiling.Checked); }
        private void chkRightWall_CheckedChanged(object sender, EventArgs e) { SetType((int)CollisionPlaneType.RightWall, chkRightWall.Checked); }
        private void chkLeftWall_CheckedChanged(object sender, EventArgs e) { SetType((int)CollisionPlaneType.LeftWall, chkLeftWall.Checked); }
        private void chkTypeUnk1_CheckedChanged(object sender, EventArgs e) { SetType((int)CollisionPlaneType.Unk1, chkTypeUnk1.Checked); }
        private void chkTypeUnk2_CheckedChanged(object sender, EventArgs e) { SetType((int)CollisionPlaneType.Unk2, chkTypeUnk2.Checked); }

        private void chkFallThrough_CheckedChanged(object sender, EventArgs e) { SetFlag((int)CollisionPlaneFlags.DropThrough, chkFallThrough.Checked); }
        private void chkFlagUnk1_CheckedChanged(object sender, EventArgs e) { SetFlag((int)CollisionPlaneFlags.Unk1, chkFlagUnk1.Checked); }
        private void chkFlagUnk2_CheckedChanged(object sender, EventArgs e) { SetFlag((int)CollisionPlaneFlags.Unk2, chkFlagUnk2.Checked); }

        #endregion

        #region Point Properties

        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            foreach (CollisionLink link in _selectedLinks)
                link._value._x = numX.Value;
            _modelPanel.Invalidate();
        }

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (_updating)
                return;
            foreach (CollisionLink link in _selectedLinks)
                link._value._y = numY.Value;
            _modelPanel.Invalidate();
        }

        #endregion
    }
}
