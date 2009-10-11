using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;

namespace BrawlScape
{
    public delegate void DefinitionChangeEvent<T>(ResourceDefinition<T> def) where T:NodeReference;

    public class ResourceDefinition<T> : ListViewItem where T:NodeReference
    {
        protected T _nodeRef;
        public T Reference { get { return _nodeRef; } }

        public ResourceDefinition(string treePath, string nodePath)
        {
            if ((treePath != null) && (nodePath != null))
            {
                _nodeRef = NodeReference.Get<T>(treePath, nodePath);
                Text = _nodeRef.Name;
                _nodeRef.DataChanged += OnChanged;
            }
        }

        public event DefinitionChangeEvent<T> Changed;
        protected virtual void OnChanged(NodeReference reference)       
        {
            if (Changed != null)
                Changed(this);
        }

        public void Reset()
        {
            if (_nodeRef != null)
            {
                _nodeRef.DataChanged -= OnChanged;
                _nodeRef = null;
            }
        }
    }
}
