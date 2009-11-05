using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.OpenGL;

namespace BrawlBox
{
    class ModelForm : Form
    {
        private System.ComponentModel.IContainer components;
        private Panel pnlOptions;
        private Label label2;
        private Label label1;
        private CheckedListBox listPolygons;
        private ColorDialog dlgColor;
        private CheckBox checkBox1;
        private ModelPanel modelPanel1;

        private GLModel _model;
        private Panel panel1;
        private Button btnOptionToggle;
        private Panel pnlAnim;
        private Button btnAnimToggle;
        private bool _updating = false;

        public ModelForm()
        {
            InitializeComponent();
            modelPanel1.ContextMenuStrip = null;
        }

        public DialogResult ShowDialog(GLModel model) { return ShowDialog(null, model); }
        public DialogResult ShowDialog(IWin32Window owner, GLModel model)
        {
            this.Text = model._name;
            modelPanel1.TargetModel = _model = model;
            try { return ShowDialog(owner); }
            finally 
            {
                modelPanel1.TargetModel = _model = null;
                listPolygons.Items.Clear();
            }
        }

        private void ModelForm_Shown(object sender, EventArgs e)
        {
            checkBox1.CheckState = CheckState.Checked;
            label2.BackColor = modelPanel1.BackColor;

            listPolygons.BeginUpdate();
            foreach (GLPolygon poly in _model._polygons)
                listPolygons.Items.Add(poly, CheckState.Checked);
            listPolygons.EndUpdate();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                modelPanel1.BackColor = label2.BackColor = dlgColor.Color;
            }
        }

        private void btnOptionToggle_Click(object sender, EventArgs e)
        {
            if (pnlOptions.Visible = !pnlOptions.Visible)
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

        private void listPolygons_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_updating)
            {
                if (e.CurrentValue == CheckState.Checked)
                    e.NewValue = CheckState.Indeterminate;
            }

            GLPolygon poly = listPolygons.Items[e.Index] as GLPolygon;

            poly._enabled = e.NewValue == CheckState.Checked || e.NewValue == CheckState.Indeterminate;
            poly._wireframe = e.NewValue == CheckState.Indeterminate;

            if (!_updating)
                modelPanel1.Invalidate();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (listPolygons.Items.Count == 0)
                return;

            _updating = true;

            listPolygons.BeginUpdate();
            for (int i = 0; i < listPolygons.Items.Count; i++)
                listPolygons.SetItemCheckState(i, checkBox1.CheckState);
            listPolygons.EndUpdate();

            _updating = false;
            modelPanel1.Invalidate();
        }

        #region Designer

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.listPolygons = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.btnOptionToggle = new System.Windows.Forms.Button();
            this.pnlAnim = new System.Windows.Forms.Panel();
            this.btnAnimToggle = new System.Windows.Forms.Button();
            this.pnlOptions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.White;
            this.pnlOptions.Controls.Add(this.listPolygons);
            this.pnlOptions.Controls.Add(this.panel1);
            this.pnlOptions.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlOptions.Location = new System.Drawing.Point(0, 0);
            this.pnlOptions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.pnlOptions.Size = new System.Drawing.Size(98, 480);
            this.pnlOptions.TabIndex = 1;
            this.pnlOptions.Visible = false;
            // 
            // listPolygons
            // 
            this.listPolygons.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listPolygons.CausesValidation = false;
            this.listPolygons.CheckOnClick = true;
            this.listPolygons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPolygons.IntegralHeight = false;
            this.listPolygons.Location = new System.Drawing.Point(2, 56);
            this.listPolygons.Margin = new System.Windows.Forms.Padding(0);
            this.listPolygons.Name = "listPolygons";
            this.listPolygons.Size = new System.Drawing.Size(96, 424);
            this.listPolygons.TabIndex = 2;
            this.listPolygons.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listPolygons_ItemCheck);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 56);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(67, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 2;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(2, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Back Color:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.Location = new System.Drawing.Point(0, 36);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.checkBox1.Size = new System.Drawing.Size(96, 20);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "All";
            this.checkBox1.ThreeState = true;
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckStateChanged += new System.EventHandler(this.checkBox1_CheckStateChanged);
            // 
            // dlgColor
            // 
            this.dlgColor.AnyColor = true;
            this.dlgColor.FullOpen = true;
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.InitialYFactor = -275;
            this.modelPanel1.InitialZoomFactor = -15;
            this.modelPanel1.Location = new System.Drawing.Point(113, 0);
            this.modelPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(430, 480);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            // 
            // btnOptionToggle
            // 
            this.btnOptionToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOptionToggle.Location = new System.Drawing.Point(98, 0);
            this.btnOptionToggle.Name = "btnOptionToggle";
            this.btnOptionToggle.Size = new System.Drawing.Size(15, 480);
            this.btnOptionToggle.TabIndex = 2;
            this.btnOptionToggle.Text = ">";
            this.btnOptionToggle.UseVisualStyleBackColor = false;
            this.btnOptionToggle.Click += new System.EventHandler(this.btnOptionToggle_Click);
            // 
            // pnlAnim
            // 
            this.pnlAnim.BackColor = System.Drawing.Color.White;
            this.pnlAnim.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlAnim.Location = new System.Drawing.Point(558, 0);
            this.pnlAnim.Name = "pnlAnim";
            this.pnlAnim.Size = new System.Drawing.Size(82, 480);
            this.pnlAnim.TabIndex = 3;
            this.pnlAnim.Visible = false;
            // 
            // btnAnimToggle
            // 
            this.btnAnimToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAnimToggle.Location = new System.Drawing.Point(543, 0);
            this.btnAnimToggle.Name = "btnAnimToggle";
            this.btnAnimToggle.Size = new System.Drawing.Size(15, 480);
            this.btnAnimToggle.TabIndex = 4;
            this.btnAnimToggle.Text = "<";
            this.btnAnimToggle.UseVisualStyleBackColor = false;
            this.btnAnimToggle.Click += new System.EventHandler(this.btnAnimToggle_Click);
            // 
            // ModelForm
            // 
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.modelPanel1);
            this.Controls.Add(this.btnAnimToggle);
            this.Controls.Add(this.pnlAnim);
            this.Controls.Add(this.btnOptionToggle);
            this.Controls.Add(this.pnlOptions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "ModelForm";
            this.ShowInTaskbar = false;
            this.Text = "Advanced Model Editor";
            this.Shown += new System.EventHandler(this.ModelForm_Shown);
            this.pnlOptions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion






    }
}
