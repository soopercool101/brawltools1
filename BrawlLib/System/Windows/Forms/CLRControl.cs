using System;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class CLRControl : UserControl
    {
        #region Designer

        private Label lblPrimary;
        private Label lblBase;
        private Label lblColor;
        private ContextMenuStrip ctxMenu;
        private IContainer components;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private Panel pnlPrimary;
        private ListBox lstColors;
    
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblPrimary = new System.Windows.Forms.Label();
            this.lstColors = new System.Windows.Forms.ListBox();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBase = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.pnlPrimary = new System.Windows.Forms.Panel();
            this.ctxMenu.SuspendLayout();
            this.pnlPrimary.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPrimary
            // 
            this.lblPrimary.Location = new System.Drawing.Point(0, 2);
            this.lblPrimary.Name = "lblPrimary";
            this.lblPrimary.Size = new System.Drawing.Size(75, 20);
            this.lblPrimary.TabIndex = 0;
            this.lblPrimary.Text = "Base Color:";
            this.lblPrimary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lstColors
            // 
            this.lstColors.ContextMenuStrip = this.ctxMenu;
            this.lstColors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstColors.FormattingEnabled = true;
            this.lstColors.IntegralHeight = false;
            this.lstColors.Location = new System.Drawing.Point(0, 24);
            this.lstColors.Name = "lstColors";
            this.lstColors.Size = new System.Drawing.Size(334, 218);
            this.lstColors.TabIndex = 1;
            this.lstColors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstColors_DrawItem);
            this.lstColors.DoubleClick += new System.EventHandler(this.lstColors_DoubleClick);
            this.lstColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstColors_MouseDown);
            // 
            // ctxMenu
            // 
            this.ctxMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.ctxMenu.Name = "ctxMenu";
            this.ctxMenu.Size = new System.Drawing.Size(145, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // lblBase
            // 
            this.lblBase.BackColor = System.Drawing.Color.Transparent;
            this.lblBase.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBase.Location = new System.Drawing.Point(88, 2);
            this.lblBase.Name = "lblBase";
            this.lblBase.Size = new System.Drawing.Size(112, 20);
            this.lblBase.TabIndex = 2;
            this.lblBase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColor
            // 
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(202, 2);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(41, 20);
            this.lblColor.TabIndex = 3;
            this.lblColor.Click += new System.EventHandler(this.lblBase_Click);
            // 
            // pnlPrimary
            // 
            this.pnlPrimary.Controls.Add(this.lblPrimary);
            this.pnlPrimary.Controls.Add(this.lblBase);
            this.pnlPrimary.Controls.Add(this.lblColor);
            this.pnlPrimary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPrimary.Location = new System.Drawing.Point(0, 0);
            this.pnlPrimary.Name = "pnlPrimary";
            this.pnlPrimary.Size = new System.Drawing.Size(334, 24);
            this.pnlPrimary.TabIndex = 4;
            // 
            // CLRControl
            // 
            this.Controls.Add(this.lstColors);
            this.Controls.Add(this.pnlPrimary);
            this.DoubleBuffered = true;
            this.Name = "CLRControl";
            this.Size = new System.Drawing.Size(334, 242);
            this.ctxMenu.ResumeLayout(false);
            this.pnlPrimary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ARGBPixel _primaryColor;
        private ARGBPixel _copyColor;

        private IColorSource _colorSource;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IColorSource ColorSource
        {
            get { return _colorSource; }
            set { _colorSource = value; SourceChanged(); }
        }


        private GoodColorDialog dlgColor = new GoodColorDialog();

        public CLRControl() { InitializeComponent(); }

        private void SourceChanged()
        {
            lstColors.BeginUpdate();
            lstColors.Items.Clear();

            if (_colorSource != null)
            {
                int count = _colorSource.ColorCount;
                for (int i = 0; i < count; i++)
                    lstColors.Items.Add(_colorSource.GetColor(i));

                if (pnlPrimary.Visible = _colorSource.HasPrimary)
                {
                    _primaryColor = _colorSource.PrimaryColor;
                    lblPrimary.Text = _colorSource.PrimaryColorName;
                    UpdateBase();
                }
            }

            lstColors.EndUpdate();
        }

        private void UpdateBase()
        {
            RGBPixel p = (RGBPixel)_primaryColor;
            lblBase.Text = p.ToString();
            lblColor.BackColor = (Color)p;
        }

        private void lstColors_DoubleClick(object sender, EventArgs e)
        {
            int index = lstColors.SelectedIndex;

            if ((_colorSource == null) || (index < 0))
                return;

            dlgColor.Color = (Color)(ARGBPixel)lstColors.Items[index];
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                ARGBPixel p = (ARGBPixel)dlgColor.Color;
                lstColors.Items[index] = p;
                _colorSource.SetColor(index, p);
            }
        }

        private static Font _renderFont = new Font(FontFamily.GenericMonospace, 9.0f);
        private void lstColors_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            int index = e.Index;

            g.FillRectangle(Brushes.White, r);

            if (index >= 0)
            {
                ARGBPixel p = (ARGBPixel)lstColors.Items[index];

                if ((e.State & DrawItemState.Selected) != 0)
                    g.FillRectangle(Brushes.LightBlue, r.X, r.Y, 200, r.Height);

                g.DrawString(String.Format("[{0:d2}]  {1}", index, p), _renderFont, Brushes.Black, 4.0f, e.Bounds.Y - 2);

                r.X += 200;
                r.Width = 40;

                using (Brush b = new SolidBrush((Color)p))
                    g.FillRectangle(b, r);

                g.DrawRectangle(Pens.Black, r);
            }
        }

        private void lblBase_Click(object sender, EventArgs e)
        {
            if (_colorSource == null)
                return;

            dlgColor.Color = (Color)_primaryColor;
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _primaryColor = (ARGBPixel)dlgColor.Color;
                _colorSource.PrimaryColor = _primaryColor;
                UpdateBase();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstColors.SelectedIndex >= 0)
                _copyColor = (ARGBPixel)lstColors.SelectedItem;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = lstColors.SelectedIndex;
            if (index >= 0)
            {
                lstColors.Items[index] = _copyColor;
                _colorSource.SetColor(index, _copyColor);
                //lstColors.Invalidate();
            }
        }

        private void lstColors_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = lstColors.IndexFromPoint(e.Location);
                lstColors.SelectedIndex = index;
            }
        }

    }
}
