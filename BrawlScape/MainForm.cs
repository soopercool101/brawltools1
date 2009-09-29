using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlScape
{
    public partial class MainForm : Form
    {
        private Control _currentFrame;

        private StartupFrame _startupFrame;
        public StartupFrame StartupFrame
        {
            get
            {
                if (_startupFrame == null)
                    InitFrame(_startupFrame = new StartupFrame());
                return _startupFrame;
            }
        }

        private CharacterFrame _charFrame;
        public CharacterFrame CharacterFrame
        {
            get
            {
                if (_charFrame == null)
                    InitFrame(_charFrame = new CharacterFrame());
                return _charFrame;
            }
        }

        private ItemFrame _itemFrame;
        public ItemFrame ItemFrame
        {
            get
            {
                if (_itemFrame == null)
                    InitFrame(_itemFrame = new ItemFrame());
                return _itemFrame;
            }
        }

        public MainForm() { InitializeComponent(); Text = Program.AssemblyDescription; }

        private void InitFrame(Control frame)
        {
            frame.Visible = false;
            _framePanel.Controls.Add(frame);
            frame.Dock = DockStyle.Fill;
        }
        private void SwapFrame(Control frame)
        {
            if (_currentFrame == frame)
                return;

            frame.Visible = true;
            if (_currentFrame != null)
                _currentFrame.Visible = false;
            _currentFrame = frame;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SwapFrame(StartupFrame);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SwapFrame(CharacterFrame);
            btnProject.Checked = false;
            btnCharacters.Checked = true;
            btnItems.Checked = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SwapFrame(ItemFrame);
            btnProject.Checked = false;
            btnCharacters.Checked = false;
            btnItems.Checked = true;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SwapFrame(StartupFrame);
            btnProject.Checked = true;
            btnCharacters.Checked = false;
            btnItems.Checked = false;
        }

    }
}
