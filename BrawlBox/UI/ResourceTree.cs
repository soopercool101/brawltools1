using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox
{
    public class ResourceTree : TreeView
    {
        public event EventHandler AfterDeselect;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode n = GetNodeAt(e.Location);
            if (n == null)
            {
                SelectedNode = null;
                if (AfterDeselect != null)
                    AfterDeselect(this, null);
            }
            else if (n.Bounds.Contains(e.Location))
                SelectedNode = n;
            base.OnMouseDown(e);
        }

        public void Clear()
        {
            BeginUpdate();
            foreach (BaseWrapper n in Nodes) n.Unlink();
            Nodes.Clear();
            EndUpdate();
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);
            if (e.Node is BaseWrapper)
                ((BaseWrapper)e.Node).OnExpand();
        }

        protected override void Dispose(bool disposing) { Clear(); base.Dispose(disposing); }
    }
}
