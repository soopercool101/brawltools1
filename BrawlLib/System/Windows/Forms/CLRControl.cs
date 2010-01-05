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

        private Label label1;
        private Label lblBase;
        private Label lblColor;
        private ContextMenuStrip ctxMenu;
        private IContainer components;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ListBox lstColors;
    
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lstColors = new System.Windows.Forms.ListBox();
            this.ctxMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBase = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.ctxMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base Color:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lstColors
            // 
            this.lstColors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstColors.ContextMenuStrip = this.ctxMenu;
            this.lstColors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstColors.FormattingEnabled = true;
            this.lstColors.IntegralHeight = false;
            this.lstColors.Location = new System.Drawing.Point(0, 23);
            this.lstColors.Name = "lstColors";
            this.lstColors.Size = new System.Drawing.Size(334, 219);
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
            this.lblBase.Location = new System.Drawing.Point(88, 1);
            this.lblBase.Name = "lblBase";
            this.lblBase.Size = new System.Drawing.Size(112, 20);
            this.lblBase.TabIndex = 2;
            this.lblBase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColor
            // 
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(202, 1);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(41, 20);
            this.lblColor.TabIndex = 3;
            this.lblColor.Click += new System.EventHandler(this.lblBase_Click);
            // 
            // CLRControl
            // 
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.lblBase);
            this.Controls.Add(this.lstColors);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "CLRControl";
            this.Size = new System.Drawing.Size(334, 242);
            this.ctxMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CLR0EntryNode _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CLR0EntryNode TargetNode
        {
            get { return _targetNode; }
            set
            {
                _targetNode = value;
                NodeChanged();
            }
        }

        private GoodColorDialog dlgColor = new GoodColorDialog();

        public CLRControl() { InitializeComponent(); }

        private void NodeChanged()
        {
            lstColors.BeginUpdate();
            lstColors.Items.Clear();

            if (_targetNode != null)
            {
                foreach (ARGBPixel p in _targetNode._colors)
                    lstColors.Items.Add(p);

                UpdateBase();
            }

            lstColors.EndUpdate();
        }

        private void UpdateBase()
        {
            RGBPixel p = (RGBPixel)_targetNode._baseColor;
            lblBase.Text = p.ToString();
            lblColor.BackColor = (Color)p;
        }

        private void lstColors_DoubleClick(object sender, EventArgs e)
        {
            int index = lstColors.SelectedIndex;

            if ((_targetNode == null) || (index < 0))
                return;

            ARGBPixel p = (ARGBPixel)lstColors.Items[index];
            dlgColor.Color = (Color)p;
            dlgColor.EditAlpha = true;
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                p = (ARGBPixel)dlgColor.Color;
                lstColors.Items[index] = p;
                _targetNode._colors[index] = p;
                _targetNode.SignalPropertyChange();
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
            if (_targetNode == null)
                return;

            dlgColor.EditAlpha = false;
            dlgColor.Color = (Color)_targetNode._baseColor;
            if (dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                _targetNode.BaseColor = (ARGBPixel)dlgColor.Color;
                UpdateBase();
            }
        }

        ARGBPixel _copyColor;
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstColors.SelectedIndex >= 0)
            {
                _copyColor = (ARGBPixel)lstColors.SelectedItem;
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = lstColors.SelectedIndex;
            if (index >= 0)
            {
                lstColors.Items[index] = _copyColor;
                _targetNode._colors[index] = _copyColor;
                _targetNode.SignalPropertyChange();
                lstColors.Invalidate();
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
