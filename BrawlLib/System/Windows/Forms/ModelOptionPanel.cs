using System;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms
{
    class ModelOptionPanel : UserControl
    {
        #region Designer

        private Label lblBackColor;
        private CheckBox chkPolygons;
        private CheckBox chkBones;
        private ColorDialog dlgColor;
        private Button btnCamReset;
        private Label label1;
    
        private void InitializeComponent()
        {
            this.lblBackColor = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPolygons = new System.Windows.Forms.CheckBox();
            this.chkBones = new System.Windows.Forms.CheckBox();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.btnCamReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBackColor
            // 
            this.lblBackColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBackColor.Location = new System.Drawing.Point(68, 1);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(18, 18);
            this.lblBackColor.TabIndex = 4;
            this.lblBackColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBackColor.Click += new System.EventHandler(this.lblBackColor_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Back Color:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkPolygons
            // 
            this.chkPolygons.Location = new System.Drawing.Point(102, 1);
            this.chkPolygons.Name = "chkPolygons";
            this.chkPolygons.Size = new System.Drawing.Size(74, 20);
            this.chkPolygons.TabIndex = 5;
            this.chkPolygons.Text = "Polygons";
            this.chkPolygons.ThreeState = true;
            this.chkPolygons.UseVisualStyleBackColor = true;
            this.chkPolygons.CheckStateChanged += new System.EventHandler(this.chkPolygons_CheckStateChanged);
            // 
            // chkBones
            // 
            this.chkBones.Location = new System.Drawing.Point(174, 1);
            this.chkBones.Name = "chkBones";
            this.chkBones.Size = new System.Drawing.Size(61, 20);
            this.chkBones.TabIndex = 6;
            this.chkBones.Text = "Bones";
            this.chkBones.UseVisualStyleBackColor = true;
            this.chkBones.CheckedChanged += new System.EventHandler(this.chkBones_CheckedChanged);
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // btnCamReset
            // 
            this.btnCamReset.Location = new System.Drawing.Point(231, 0);
            this.btnCamReset.Name = "btnCamReset";
            this.btnCamReset.Size = new System.Drawing.Size(85, 20);
            this.btnCamReset.TabIndex = 7;
            this.btnCamReset.Text = "Reset Camera";
            this.btnCamReset.UseVisualStyleBackColor = true;
            this.btnCamReset.Click += new System.EventHandler(this.btnCamReset_Click);
            // 
            // ModelOptionPanel
            // 
            this.Controls.Add(this.btnCamReset);
            this.Controls.Add(this.chkBones);
            this.Controls.Add(this.chkPolygons);
            this.Controls.Add(this.lblBackColor);
            this.Controls.Add(this.label1);
            this.Name = "ModelOptionPanel";
            this.Size = new System.Drawing.Size(322, 19);
            this.ResumeLayout(false);

        }

        #endregion

        private bool _updating = false;

        public event EventHandler RenderStateChanged;
        public event EventHandler ClearColorChanged;
        public event EventHandler CamResetClicked;

        private MDL0Node _targetModel;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MDL0Node TargetModel
        {
            get { return _targetModel; }
            set
            {
                if ((_targetModel = value) != null)
                {
                    Enabled = true;

                    _updating = true;
                    chkPolygons.CheckState = _targetModel._renderPolygons ? (_targetModel._renderPolygonsWireframe ? CheckState.Indeterminate : CheckState.Checked) : CheckState.Unchecked;
                    chkBones.Checked = _targetModel._renderBones;
                    _updating = false;
                }
                else
                    Enabled = false;
            }
        }

        private Color _clearColor;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ClearColor
        {
            get { return _clearColor; }
            set { lblBackColor.BackColor = _clearColor = value; }
        }

        public ModelOptionPanel() { InitializeComponent(); }

        private void chkPolygons_CheckStateChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            _targetModel._renderPolygonsWireframe = (chkPolygons.CheckState == CheckState.Indeterminate);
            _targetModel._renderPolygons = (_targetModel._renderPolygonsWireframe) || (chkPolygons.CheckState == CheckState.Checked);

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void chkBones_CheckedChanged(object sender, EventArgs e)
        {
            if ((_updating) || (_targetModel == null))
                return;

            _targetModel._renderBones = chkBones.Checked;

            if (RenderStateChanged != null)
                RenderStateChanged(this, null);
        }

        private void lblBackColor_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                lblBackColor.BackColor = _clearColor = dlgColor.Color;
                if (ClearColorChanged != null)
                    ClearColorChanged(this, null);
            }
        }

        private void btnCamReset_Click(object sender, EventArgs e)
        {
            if (CamResetClicked != null)
                CamResetClicked(this, null);
        }
    }
}
