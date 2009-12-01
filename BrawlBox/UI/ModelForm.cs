using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.OpenGL;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox
{
    class ModelForm : Form
    {
        #region Designer

        private ModelEditControl modelEditControl1;
        private void InitializeComponent()
        {
            this.modelEditControl1 = new System.Windows.Forms.ModelEditControl();
            this.SuspendLayout();
            // 
            // modelEditControl1
            // 
            this.modelEditControl1.BackColor = System.Drawing.Color.Lavender;
            this.modelEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelEditControl1.Location = new System.Drawing.Point(0, 0);
            this.modelEditControl1.Name = "modelEditControl1";
            this.modelEditControl1.Size = new System.Drawing.Size(639, 528);
            this.modelEditControl1.TabIndex = 0;
            // 
            // ModelForm
            // 
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(639, 528);
            this.Controls.Add(this.modelEditControl1);
            this.MinimizeBox = false;
            this.Name = "ModelForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ModelForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion
        public ModelForm() { InitializeComponent(); }

        public DialogResult ShowDialog(MDL0Node model) { return ShowDialog(null, model); }
        public DialogResult ShowDialog(IWin32Window owner, MDL0Node model)
        {
            this.Text = String.Format("Advanced Model Editor - {0}", model.Name);
            modelEditControl1.TargetModel = model;
            try { return ShowDialog(owner); }
            finally { modelEditControl1.TargetModel = null; }
        }

        private void ModelForm_FormClosing(object sender, FormClosingEventArgs e) { e.Cancel = !modelEditControl1.CloseFiles(); }

    }
}
