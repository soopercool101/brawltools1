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
        private ModelPanel modelPanel1;

        public ModelForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(GLModel model) { return ShowDialog(null, model); }
        public DialogResult ShowDialog(IWin32Window owner, GLModel model)
        {
            this.Text = model._name;
            modelPanel1.TargetModel = model;
            try { return ShowDialog(owner); }
            finally { modelPanel1.TargetModel = null; }
        }

        private void InitializeComponent()
        {
            this.modelPanel1 = new System.Windows.Forms.ModelPanel();
            this.SuspendLayout();
            // 
            // modelPanel1
            // 
            this.modelPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel1.InitialYFactor = -275;
            this.modelPanel1.InitialZoomFactor = -15;
            this.modelPanel1.Location = new System.Drawing.Point(0, 0);
            this.modelPanel1.Name = "modelPanel1";
            this.modelPanel1.RotationScale = 0.4F;
            this.modelPanel1.Size = new System.Drawing.Size(284, 262);
            this.modelPanel1.TabIndex = 0;
            this.modelPanel1.TranslationScale = 0.05F;
            this.modelPanel1.ZoomScale = 2.5F;
            // 
            // ModelForm
            // 
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.modelPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "ModelForm";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }
    }
}
