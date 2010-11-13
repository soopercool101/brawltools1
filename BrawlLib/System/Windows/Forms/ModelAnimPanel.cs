using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.Drawing;
using BrawlLib.Modeling;
using System.IO;
using System.ComponentModel;
using BrawlLib;
using System.Collections.Generic;

namespace System.Windows.Forms
{
    class ModelAnimPanel : UserControl
    {
        public delegate void ReferenceEventHandler(ResourceNode node);
        #region Designer

        private GroupBox grpTransform;
        private Button btnPaste;
        private Button btnCopy;
        private Button btnCut;
        private Label label5;
        private NumericInputBox numScaleZ;
        private NumericInputBox numTransX;
        private NumericInputBox numScaleY;
        private Label label6;
        private NumericInputBox numScaleX;
        private Label label7;
        internal NumericInputBox numRotZ;
        private Label label8;
        internal NumericInputBox numRotY;
        private Label label9;
        internal NumericInputBox numRotX;
        private Label label10;
        private NumericInputBox numTransZ;
        private Label label11;
        private NumericInputBox numTransY;
        private Label label12;
        private GroupBox grpExt;
        private TextBox txtExtPath;
        private Button btnClose;
        private Button btnOpen;
        private Button btnSave;
        private ListView listAnims;
        private ColumnHeader nameColumn;
        private OpenFileDialog dlgOpen;
        private ContextMenuStrip ctxAnim;
        private ToolStripMenuItem sourceToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private SaveFileDialog dlgSave;
        private Button btnClear;
        private Button btnDelete;
        private Button btnInsert;
        private GroupBox grpTransAll;
        private IContainer components;
        private Button btnClean;
        private Button btnPasteAll;
        private Button btnCopyAll;
        private Label label13;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Animations", System.Windows.Forms.HorizontalAlignment.Left);
            this.grpTransform = new System.Windows.Forms.GroupBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnCut = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numScaleZ = new System.Windows.Forms.NumericInputBox();
            this.numTransX = new System.Windows.Forms.NumericInputBox();
            this.numScaleY = new System.Windows.Forms.NumericInputBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numScaleX = new System.Windows.Forms.NumericInputBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numRotZ = new System.Windows.Forms.NumericInputBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numRotY = new System.Windows.Forms.NumericInputBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numRotX = new System.Windows.Forms.NumericInputBox();
            this.label10 = new System.Windows.Forms.Label();
            this.numTransZ = new System.Windows.Forms.NumericInputBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numTransY = new System.Windows.Forms.NumericInputBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.grpExt = new System.Windows.Forms.GroupBox();
            this.txtExtPath = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.listAnims = new System.Windows.Forms.ListView();
            this.nameColumn = new System.Windows.Forms.ColumnHeader();
            this.ctxAnim = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.grpTransAll = new System.Windows.Forms.GroupBox();
            this.btnClean = new System.Windows.Forms.Button();
            this.btnPasteAll = new System.Windows.Forms.Button();
            this.btnCopyAll = new System.Windows.Forms.Button();
            this.grpTransform.SuspendLayout();
            this.grpExt.SuspendLayout();
            this.ctxAnim.SuspendLayout();
            this.grpTransAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpTransform
            // 
            this.grpTransform.Controls.Add(this.btnPaste);
            this.grpTransform.Controls.Add(this.btnCopy);
            this.grpTransform.Controls.Add(this.btnCut);
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
            this.grpTransform.Enabled = false;
            this.grpTransform.Location = new System.Drawing.Point(0, 355);
            this.grpTransform.Name = "grpTransform";
            this.grpTransform.Size = new System.Drawing.Size(173, 239);
            this.grpTransform.TabIndex = 22;
            this.grpTransform.TabStop = false;
            this.grpTransform.Text = "Transform Frame";
            // 
            // btnPaste
            // 
            this.btnPaste.Location = new System.Drawing.Point(116, 215);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(50, 20);
            this.btnPaste.TabIndex = 23;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(62, 215);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(50, 20);
            this.btnCopy.TabIndex = 22;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnCut
            // 
            this.btnCut.Location = new System.Drawing.Point(8, 215);
            this.btnCut.Name = "btnCut";
            this.btnCut.Size = new System.Drawing.Size(50, 20);
            this.btnCut.TabIndex = 21;
            this.btnCut.Text = "Cut";
            this.btnCut.UseVisualStyleBackColor = true;
            this.btnCut.Click += new System.EventHandler(this.btnCut_Click);
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
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Translation Y:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Translation Z:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Rotation X:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Rotation Y:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Rotation Z:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 20);
            this.label11.TabIndex = 10;
            this.label11.Text = "Scale X:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(116, 14);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(50, 20);
            this.btnClear.TabIndex = 26;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(62, 14);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 20);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(8, 14);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(50, 20);
            this.btnInsert.TabIndex = 24;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
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
            this.grpExt.TabIndex = 23;
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
            this.txtExtPath.Click += new System.EventHandler(this.txtExtPath_Click);
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
            // listAnims
            // 
            this.listAnims.AutoArrange = false;
            this.listAnims.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn});
            this.listAnims.ContextMenuStrip = this.ctxAnim;
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
            this.listAnims.Size = new System.Drawing.Size(173, 221);
            this.listAnims.TabIndex = 24;
            this.listAnims.UseCompatibleStateImageBehavior = false;
            this.listAnims.View = System.Windows.Forms.View.Details;
            this.listAnims.SelectedIndexChanged += new System.EventHandler(this.listAnims_SelectedIndexChanged);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 160;
            // 
            // ctxAnim
            // 
            this.ctxAnim.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exportToolStripMenuItem,
            this.replaceToolStripMenuItem});
            this.ctxAnim.Name = "ctxAnim";
            this.ctxAnim.Size = new System.Drawing.Size(125, 76);
            this.ctxAnim.Opening += new System.ComponentModel.CancelEventHandler(this.ctxAnim_Opening);
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Enabled = false;
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.sourceToolStripMenuItem.Text = "Source";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.replaceToolStripMenuItem.Text = "Replace...";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // grpTransAll
            // 
            this.grpTransAll.Controls.Add(this.btnClean);
            this.grpTransAll.Controls.Add(this.btnPasteAll);
            this.grpTransAll.Controls.Add(this.btnCopyAll);
            this.grpTransAll.Controls.Add(this.btnClear);
            this.grpTransAll.Controls.Add(this.btnInsert);
            this.grpTransAll.Controls.Add(this.btnDelete);
            this.grpTransAll.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpTransAll.Enabled = false;
            this.grpTransAll.Location = new System.Drawing.Point(0, 290);
            this.grpTransAll.Name = "grpTransAll";
            this.grpTransAll.Size = new System.Drawing.Size(173, 65);
            this.grpTransAll.TabIndex = 25;
            this.grpTransAll.TabStop = false;
            this.grpTransAll.Text = "Transform All";
            // 
            // btnClean
            // 
            this.btnClean.Location = new System.Drawing.Point(116, 40);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(50, 20);
            this.btnClean.TabIndex = 29;
            this.btnClean.Text = "Clean";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // btnPasteAll
            // 
            this.btnPasteAll.Location = new System.Drawing.Point(62, 40);
            this.btnPasteAll.Name = "btnPasteAll";
            this.btnPasteAll.Size = new System.Drawing.Size(50, 20);
            this.btnPasteAll.TabIndex = 28;
            this.btnPasteAll.Text = "Paste";
            this.btnPasteAll.UseVisualStyleBackColor = true;
            this.btnPasteAll.Click += new System.EventHandler(this.btnPasteAll_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Location = new System.Drawing.Point(8, 40);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(50, 20);
            this.btnCopyAll.TabIndex = 27;
            this.btnCopyAll.Text = "Copy";
            this.btnCopyAll.UseVisualStyleBackColor = true;
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // ModelAnimPanel
            // 
            this.Controls.Add(this.listAnims);
            this.Controls.Add(this.grpTransAll);
            this.Controls.Add(this.grpExt);
            this.Controls.Add(this.grpTransform);
            this.Name = "ModelAnimPanel";
            this.Size = new System.Drawing.Size(173, 594);
            this.grpTransform.ResumeLayout(false);
            this.grpTransform.PerformLayout();
            this.grpExt.ResumeLayout(false);
            this.grpExt.PerformLayout();
            this.ctxAnim.ResumeLayout(false);
            this.grpTransAll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ResourceNode _externalNode;
        private ListViewGroup _CHRGroup = new ListViewGroup("Character Animations");
        internal NumericInputBox[] _transBoxes = new NumericInputBox[9];
        private AnimationFrame _tempFrame = AnimationFrame.Neutral;

        public event EventHandler RenderStateChanged;
        public event EventHandler AnimStateChanged;
        public event EventHandler SelectedAnimationChanged;
        public event ReferenceEventHandler ReferenceLoaded;
        public event ReferenceEventHandler ReferenceClosed;

        private object _transformObject = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object TransformObject
        {
            get { return _transformObject; }
            set { _transformObject = value; UpdatePropDisplay(); }
        }

        private int _animFrame = 0;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentFrame
        {
            get { return _animFrame; }
            set { _animFrame = value; UpdateModel(); }
        }

        private MDL0Node _targetModel = null;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set { _targetModel = value; UpdateReferences(); UpdateModel(); }
        }

        private CHR0Node _selectedAnim;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CHR0Node SelectedAnimation
        {
            get { return _selectedAnim; }
        }

        private bool _enableTransform = true;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableTransformEdit
        {
            get { return _enableTransform; }
            set { grpTransform.Enabled = grpTransAll.Enabled = (_enableTransform = value) && (_transformObject != null); }
        }

        public ModelAnimPanel() 
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

        public bool CloseReferences()
        {
            return CloseExternal();
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
                if (SelectedAnimationChanged != null)
                    SelectedAnimationChanged(this, null);
            }

            listAnims.EndUpdate();

            return count != listAnims.Items.Count;
        }

        private void UpdateModel()
        {
            if (_targetModel == null)
                return;

            if (_selectedAnim != null)
                _targetModel.ApplyCHR(_selectedAnim, _animFrame);
            else
                _targetModel.ApplyCHR(null, 0);

            UpdatePropDisplay();
            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void LoadAnims(ResourceNode node)
        {
            switch (node.ResourceType)
            {
                case ResourceType.ARC:
                case ResourceType.BRES:
                case ResourceType.BRESGroup:
                    foreach (ResourceNode n in node.Children)
                        LoadAnims(n);
                    break;

                case ResourceType.CHR0:
                    listAnims.Items.Add(new ListViewItem(node.Name, (int)node.ResourceType, _CHRGroup) { Tag = node });
                    break;
            }
        }

        private void UpdatePropDisplay()
        {
            grpTransAll.Enabled = _enableTransform && (_selectedAnim != null);
            btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = _animFrame != 0;
            grpTransform.Enabled = _enableTransform && (_transformObject != null);
            for (int i = 0; i < 9; i++)
                ResetBox(i);
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
                    KeyframeEntry e = entry.Keyframes.GetKeyframe((KeyFrameMode)index + 0x10, _animFrame - 1);
                    if (e == null)
                    {
                        box.Value = entry.Keyframes[KeyFrameMode.ScaleX + index, _animFrame - 1];
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        box.Value = e._value;
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
        internal unsafe void BoxChanged(object sender, EventArgs e)
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
                                    entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);
                            for (int i = 3; i < 9; i++)
                                if (p[i] != 0.0f)
                                    entry.SetKeyframe(KeyFrameMode.ScaleX + i, 0, p[i]);

                            entry.SetKeyframe(KeyFrameMode.ScaleX + index, _animFrame - 1, box.Value);
                        }
                    }
                    else //Set existing 
                    {
                        if (float.IsNaN(box.Value))
                            entry.RemoveKeyframe(KeyFrameMode.ScaleX + index, _animFrame - 1);
                        else
                            entry.SetKeyframe(KeyFrameMode.ScaleX + index, _animFrame - 1, box.Value);
                    }
                }
                else
                {
                    //Change base transform
                    FrameState state = bone._bindState;
                    float* p = (float*)&state;
                    p[index] = float.IsNaN(box.Value) ? (index > 2 ? 0.0f : 1.0f) : box.Value;
                    state.CalcTransforms();
                    bone._bindState = state;
                    bone.RecalcBindState();
                    bone.SignalPropertyChange();
                }

                _targetModel.ApplyCHR(_selectedAnim, _animFrame);
                ResetBox(index);
                if (RenderStateChanged != null)
                    RenderStateChanged(this, null);
            }
        }

        private bool LoadExternal()
        {
            int count;
            dlgOpen.Filter = "All Files (*.*)|*.*";
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
                            _externalNode = node;
                            node = null;
                            txtExtPath.Text = Path.GetFileName(dlgOpen.FileName);

                            if (ReferenceLoaded != null)
                                ReferenceLoaded(_externalNode);

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
                if (ReferenceClosed != null)
                    ReferenceClosed(_externalNode);

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

        private void btnOpen_Click(object sender, EventArgs e) { LoadExternal(); }
        private void btnSave_Click(object sender, EventArgs e) { SaveExternal(); }
        private void btnClose_Click(object sender, EventArgs e) { CloseExternal(); }
        private void txtExtPath_Click(object sender, EventArgs e) { LoadExternal(); }

        private void listAnims_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listAnims.SelectedItems.Count > 0)
                _selectedAnim = listAnims.SelectedItems[0].Tag as CHR0Node;
            else
                _selectedAnim = null;

            if (_selectedAnim == null)
                btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = false;
            else
                btnInsert.Enabled = btnDelete.Enabled = btnClear.Enabled = true;

            _copyAllIndex = -1;

            if (SelectedAnimationChanged != null)
                SelectedAnimationChanged(this, null);
        }

        private unsafe void btnCut_Click(object sender, EventArgs e)
        {
            AnimationFrame frame;
            float* p = (float*)&frame;

            for (int i = 0; i < 9; i++)
            {
                p[i] = _transBoxes[i].Value;
                _transBoxes[i].Value = float.NaN;
                BoxChanged(_transBoxes[i], null);
            }

            _tempFrame = frame;
        }

        private unsafe void btnCopy_Click(object sender, EventArgs e)
        {
            AnimationFrame frame;
            float* p = (float*)&frame;

            for (int i = 0; i < 9; i++)
                p[i] = _transBoxes[i].Value;

            _tempFrame = frame;
        }

        private unsafe void btnPaste_Click(object sender, EventArgs e)
        {
            AnimationFrame frame = _tempFrame;
            float* p = (float*)&frame;

            for (int i = 0; i < 9; i++)
            {
                _transBoxes[i].Value = p[i];
                BoxChanged(_transBoxes[i], null);
            }
        }

        private void ctxAnim_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedAnim == null)
                e.Cancel = true;
            else
            {
                sourceToolStripMenuItem.Text = String.Format("Source: {0}", Path.GetFileName(_selectedAnim.RootNode._origPath));
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            dlgSave.FileName = _selectedAnim.Name;
            dlgSave.Filter = ExportFilters.CHR0;
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                _selectedAnim.Export(dlgSave.FileName);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedAnim == null)
                return;

            dlgOpen.Filter = ExportFilters.CHR0;
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                _selectedAnim.Replace(dlgOpen.FileName);

                if (SelectedAnimationChanged != null)
                    SelectedAnimationChanged(this, null);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if ((_selectedAnim == null) || (_animFrame == 0))
                return;

            _selectedAnim.InsertKeyframe(_animFrame - 1);
            if (AnimStateChanged != null)
                AnimStateChanged(this, null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((_selectedAnim == null) || (_animFrame == 0))
                return;

            _selectedAnim.DeleteKeyframe(_animFrame - 1);
            if (AnimStateChanged != null)
                AnimStateChanged(this, null);
        }

        private int _copyAllIndex = -1;

        private static Dictionary<string, AnimationFrame> _copyAllState = new Dictionary<string, AnimationFrame>();

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            _copyAllState.Clear();

            if (_animFrame == 0)
                foreach (MDL0BoneNode bone in _targetModel.FindChildrenByType("Bones", ResourceType.MDL0Bone))
                    _copyAllState[bone._name] = (AnimationFrame)bone._bindState;
            else
                foreach (CHR0EntryNode entry in _selectedAnim.Children)
                    _copyAllState[entry._name] = entry.GetAnimFrame(_animFrame - 1);
        }

        private void btnPasteAll_Click(object sender, EventArgs e)
        {
            if (_copyAllState.Count == 0)
                return;

            if (_animFrame == 0)
            {
                foreach (MDL0BoneNode bone in _targetModel.FindChildrenByType("Bones", ResourceType.MDL0Bone))
                    if (_copyAllState.ContainsKey(bone._name))
                    {
                        bone._bindState = new FrameState(_copyAllState[bone._name]);
                        bone.RecalcBindState();
                        bone.SignalPropertyChange();
                    }
            }
            else
                foreach (CHR0EntryNode entry in _selectedAnim.Children)
                    if (_copyAllState.ContainsKey(entry._name))
                        entry.SetKeyframe(_animFrame - 1, _copyAllState[entry._name]);

            UpdateModel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (_animFrame == 0)
                return;

            foreach (CHR0EntryNode entry in _selectedAnim.Children)
                entry.RemoveKeyframe(_animFrame - 1);

            UpdateModel();
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            ResourceNode group = _targetModel.FindChild("Bones", false);
            if (group == null)
                return;

            List<CHR0EntryNode> badNodes = new List<CHR0EntryNode>();
            foreach (CHR0EntryNode entry in _selectedAnim.Children)
            {
                if (group.FindChild(entry._name, true) == null)
                    badNodes.Add(entry);
                //else
                //    entry.Keyframes.Clean();
            }

            foreach (CHR0EntryNode n in badNodes)
            {
                n.Remove();
                n.Dispose();
            }

            UpdatePropDisplay();
        }
    }
}
