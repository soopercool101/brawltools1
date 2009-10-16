using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace BrawlScape
{
    public class TexturePanel : UserControl
    {
        private bool _primaryActive = false;

        private IListSource<TextureDefinition> _primarySource, _secondarySource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IListSource<TextureDefinition> PrimarySource
        {
            get { return _primarySource; }
            set { if (_primarySource != value) OnPrimaryChanged(_primarySource = value); }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IListSource<TextureDefinition> SecondarySource
        {
            get { return _secondarySource; }
            set { if (_secondarySource != value) OnSecondaryChanged(_secondarySource = value); }
        }

        private void OnPrimaryChanged(IListSource<TextureDefinition> source)
        {
            if (_primaryActive)
                textureList1.CurrentSource = source;
        }
        private void OnSecondaryChanged(IListSource<TextureDefinition> source)
        {
            if (!_primaryActive)
                textureList1.CurrentSource = source;
        }

        private void btnModel_Click(object sender, EventArgs e)
        {
            btnAll.Enabled = true;
            btnModel.Enabled = false;
            _primaryActive = false;
            textureList1.CurrentSource = _secondarySource;
        }
        private void btnAll_Click(object sender, EventArgs e)
        {
            btnAll.Enabled = false;
            btnModel.Enabled = true;
            _primaryActive = true;
            textureList1.CurrentSource = _primarySource;
        }

        public Size ImageSize { get { return textureList1.ImageSize; } set { textureList1.ImageSize = value; } }

        private TextureList textureList1;
        private Panel panel1;
        private Button btnAll;
        private Button btnModel;

        public TexturePanel() { InitializeComponent(); }

        private void TexturePanel_Resize(object sender, EventArgs e)
        {
            btnModel.Width = btnAll.Width = this.Width / 2;
        }
    
        private void InitializeComponent()
        {
            this.textureList1 = new BrawlScape.TextureList();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnModel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureList1
            // 
            this.textureList1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textureList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textureList1.HideSelection = false;
            this.textureList1.ImageSize = new System.Drawing.Size(128, 128);
            this.textureList1.Location = new System.Drawing.Point(0, 20);
            this.textureList1.Margin = new System.Windows.Forms.Padding(0);
            this.textureList1.MultiSelect = false;
            this.textureList1.Name = "textureList1";
            this.textureList1.Size = new System.Drawing.Size(200, 235);
            this.textureList1.TabIndex = 0;
            this.textureList1.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.btnAll);
            this.panel1.Controls.Add(this.btnModel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 20);
            this.panel1.TabIndex = 1;
            // 
            // btnAll
            // 
            this.btnAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAll.Location = new System.Drawing.Point(100, 0);
            this.btnAll.Margin = new System.Windows.Forms.Padding(0);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(100, 20);
            this.btnAll.TabIndex = 1;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnModel
            // 
            this.btnModel.Enabled = false;
            this.btnModel.Location = new System.Drawing.Point(0, 0);
            this.btnModel.Margin = new System.Windows.Forms.Padding(0);
            this.btnModel.Name = "btnModel";
            this.btnModel.Size = new System.Drawing.Size(100, 20);
            this.btnModel.TabIndex = 0;
            this.btnModel.Text = "Model";
            this.btnModel.UseVisualStyleBackColor = true;
            this.btnModel.Click += new System.EventHandler(this.btnModel_Click);
            // 
            // TexturePanel
            // 
            this.Controls.Add(this.textureList1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TexturePanel";
            this.Size = new System.Drawing.Size(200, 255);
            this.Resize += new System.EventHandler(this.TexturePanel_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    }
}
