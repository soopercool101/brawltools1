using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlScape
{
    public partial class MainForm : Form
    {
        public MainForm() 
        { 
            InitializeComponent(); 
            Text = Program.AssemblyTitle; 
        }
    }
}
