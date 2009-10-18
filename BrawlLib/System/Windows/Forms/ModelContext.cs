using System;
using BrawlLib.OpenGL;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class ModelContext : ContextMenuStrip
    {
        private ToolStripMenuItem _mnuColor;
        private ToolStripMenuItem _mnuAllOn, _mnuAllOff;
        private ToolStripSeparator _sep1;

        private GLModel _model;
        public GLModel Model
        {
            get { return _model; }
            set { if (_model != value) OnChanged(_model = value); }
        }

        public ModelContext(IContainer container) : base(container) { InitializeComponent(); }
        public ModelContext() : base() { InitializeComponent(); }

        private void InitializeComponent()
        {
            _mnuColor = new ToolStripMenuItem("Background Color", null, OnColorClicked);
            _mnuAllOn = new ToolStripMenuItem("Show All", null, OnAllEnabled);
            _mnuAllOff = new ToolStripMenuItem("Hide All", null, OnAllDisabled);
            _sep1 = new ToolStripSeparator();
        }

        private void OnChanged(GLModel mr)
        {
            ToolStripItemCollection col = Items;
            for (int i = 3; i < col.Count; i++)
                col[i].Dispose();
            Items.Clear();

            Items.Add(_mnuColor);
            Items.Add(_mnuAllOn);
            Items.Add(_mnuAllOff);
            Items.Add(_sep1);

            if (mr != null)
                foreach (GLPolygon poly in mr._polygons)
                {
                    ToolStripMenuItem i = new ToolStripMenuItem(String.Format("polygon{0}", poly._index), null, OnClick);
                    i.Tag = poly;
                    i.Checked = poly._enabled;
                    Items.Add(i);
                }
        }

        private void OnClick(object s, EventArgs e)
        {
            ToolStripMenuItem i = s as ToolStripMenuItem;
            ((GLPolygon)i.Tag)._enabled = i.Checked = !i.Checked;
            this.SourceControl.Invalidate();
        }

        private void OnAllEnabled(object s, EventArgs e)
        {
            ToolStripItemCollection col = Items;
            for (int i = col.IndexOf(_sep1) + 1; i < col.Count; i++)
            {
                ToolStripMenuItem item = col[i] as ToolStripMenuItem;
                ((GLPolygon)item.Tag)._enabled = item.Checked = true;
            }
            this.SourceControl.Invalidate();
        }

        private void OnAllDisabled(object s, EventArgs e)
        {
            ToolStripItemCollection col = Items;
            for (int i = col.IndexOf(_sep1) + 1; i < col.Count; i++)
            {
                ToolStripMenuItem item = col[i] as ToolStripMenuItem;
                ((GLPolygon)item.Tag)._enabled = item.Checked = false;
            }
            this.SourceControl.Invalidate();
        }

        private void OnColorClicked(object s, EventArgs e)
        {
            ModelPanel.ChooseColor();
        }
    }
}
