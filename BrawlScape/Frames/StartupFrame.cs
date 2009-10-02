using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlScape
{
    public partial class StartupFrame : UserControl
    {
        public StartupFrame()
        {
            InitializeComponent();
        }


        private void rdoRawMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRawMode.Checked)
            {
                rdoProjMode.Checked = false;
                txtProjectPath.Text = txtDataPath.Text;
                btnProjectBrowse.Enabled = false;
            }
        }

        private void rdoProjMode_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoProjMode.Checked)
            {
                rdoRawMode.Checked = false;
                btnProjectBrowse.Enabled = true;
            }
        }

        private void txtDataPath_TextChanged(object sender, EventArgs e)
        {
            //Program.DataPath = txtDataPath.Text;
        }
        private void txtProjectPath_TextChanged(object sender, EventArgs e)
        {
            //Program.WorkingPath = txtProjectPath.Text;
        }

        private void btnDataBrowse_Click(object sender, EventArgs e)
        {
            string path = Program.OpenFolder();
            if (path != null)
                Program.DataPath = txtDataPath.Text = path;
        }
        private void btnProjectBrowse_Click(object sender, EventArgs e)
        {
            string path = Program.OpenFolder();
            if (path != null)
                Program.WorkingPath = txtProjectPath.Text = path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResourceCache.SaveChanges();
            MessageBox.Show("Done!");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResourceCache.Clear();
        }

        private void StartupFrame_Load(object sender, EventArgs e)
        {
            txtDataPath.Text = Program.DataPath;
            txtProjectPath.Text = Program.WorkingPath;
        }
    }
}
