using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;

namespace BrawlScape
{
    public class ResourceDefinition<T> : ListViewItem where T:NodeReference
    {
        protected T _nodeRef;
        public T Reference { get { return _nodeRef; } }

        public ResourceDefinition(string treePath, string nodePath)
        {
            _nodeRef = NodeReference.Get<T>(treePath, nodePath);
            Text = _nodeRef.Name;
            _nodeRef.DataChanged += OnChanged;
        }

        protected virtual void OnChanged(object sender, EventArgs e)        {        }

        public void Reset()
        {
            if (_nodeRef != null)
            {
                _nodeRef.DataChanged -= OnChanged;
                _nodeRef = null;
            }
        }

        //protected string _filePath, _nodePath;

        //protected ResourceNode _node;
        //public ResourceNode Node
        //{
        //    get
        //    {
        //        if (_node == null)
        //        {
        //            if ((_node = ResourceCache.FindNode(_filePath, _nodePath)) != null)
        //            {
        //                //_node.Dirty += OnDirty;
        //                //_node.Clean += OnClean;
        //                _node.Changed += OnChanged;
        //                _node.ChildChanged += OnChildChanged;
        //            }
        //        }
        //        return _node;
        //    }
        //}

        //protected virtual void OnDirty(ResourceNode n) { }
        //protected virtual void OnClean(ResourceNode n) { }
        //protected virtual void OnChanged(ResourceNode n) { }
        //protected virtual void OnChildChanged(ResourceNode n, ResourceNode child) { }

        //protected virtual Image GetPreviewImage(Size size)
        //{
        //    return null;
        //}
    }
}
