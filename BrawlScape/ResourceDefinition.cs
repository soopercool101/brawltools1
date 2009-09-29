using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;

namespace BrawlScape
{
    public class ResourceDefinition : ListViewItem
    {
        protected string _filePath, _nodePath;

        protected ResourceNode _node;
        public ResourceNode Node
        {
            get
            {
                if (_node == null)
                {
                    if ((_node = ResourceCache.FindNode(_filePath, _nodePath)) != null)
                    {
                        //_node.Dirty += OnDirty;
                        //_node.Clean += OnClean;
                        _node.Changed += OnChanged;
                        _node.ChildChanged += OnChildChanged;
                    }
                }
                return _node;
            }
        }

        //protected virtual void OnDirty(ResourceNode n) { }
        //protected virtual void OnClean(ResourceNode n) { }
        protected virtual void OnChanged(ResourceNode n) { }
        protected virtual void OnChildChanged(ResourceNode n, ResourceNode child) { }

        protected virtual Image GetPreviewImage(Size size)
        {
            return null;
        }
    }
}
