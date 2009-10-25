using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;

namespace BrawlBox
{
    public class ResourceTree : TreeView
    {
        public event EventHandler SelectionChanged;

        private TreeNode _selected;
        new public TreeNode SelectedNode 
        { 
            get { return base.SelectedNode; } 
            set 
            {
                if (_selected == value)
                    return;

                _selected = base.SelectedNode = value;
                if (SelectionChanged != null)
                    SelectionChanged(this, null);
            } 
        }

        public ResourceTree()
        {
            this.SetStyle(ControlStyles.UserMouse, true);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x204)
            {
                int x = (int)m.LParam & 0xFFFF, y = (int)m.LParam >> 16;

                TreeNode n = GetNodeAt(x, y);
                if (n != null)
                {
                    Rectangle r = n.Bounds;
                    r.X -= 25; r.Width += 25;
                    if (r.Contains(x, y))
                        SelectedNode = n;
                }

                m.Result = IntPtr.Zero;
                return;
            }
            else if (m.Msg == 0x205)
            {
                int x = (int)m.LParam & 0xFFFF, y = (int)m.LParam >> 16;

                if ((_selected != null) && (_selected.ContextMenuStrip != null))
                {
                    Rectangle r = _selected.Bounds;
                    r.X -= 25; r.Width += 25;
                    if (r.Contains(x, y))
                        _selected.ContextMenuStrip.Show(this, x, y);
                }
            }

            base.WndProc(ref m);
        }

        public void Clear()
        {
            BeginUpdate();
            foreach (BaseWrapper n in Nodes) n.Unlink();
            Nodes.Clear();
            EndUpdate();
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            SelectedNode = e.Node;
            base.OnAfterSelect(e);
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            if (e.Node is BaseWrapper)
                ((BaseWrapper)e.Node).OnExpand();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (SelectedNode is BaseWrapper))
                ((BaseWrapper)SelectedNode).OnDoubleClick();
            else
                base.OnMouseDoubleClick(e);
        }

        protected override void Dispose(bool disposing) { Clear(); base.Dispose(disposing); }
    }
}
