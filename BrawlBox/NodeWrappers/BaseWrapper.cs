using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Reflection;

namespace BrawlBox
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class NodeWrapperAttribute : Attribute
    {

        ResourceType _type;
        public NodeWrapperAttribute(ResourceType type) { _type = type; }
        public ResourceType WrappedType { get { return _type; } }

        private static Dictionary<ResourceType, Type> _wrappers;
        public static Dictionary<ResourceType, Type> Wrappers
        {
            get
            {
                if (_wrappers == null)
                {
                    _wrappers = new Dictionary<ResourceType, Type>();
                    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                        foreach(NodeWrapperAttribute attr in t.GetCustomAttributes(typeof(NodeWrapperAttribute), true))
                            _wrappers[attr._type] = t;
                }
                return _wrappers;
            }
        }
    }

    public abstract class BaseWrapper : TreeNode
    {
        protected static readonly ContextMenuStrip _emptyMenu = new ContextMenuStrip();

        protected bool _discovered = false;

        protected ResourceNode _resource;
        public ResourceNode ResourceNode
        {
            get { return _resource; }
            //set { Link(value); }
        }

        protected BaseWrapper() { }
        //protected BaseWrapper(ResourceNode resourceNode) { Link(resourceNode); }

        protected static T GetInstance<T>() where T : BaseWrapper { return MainForm.Instance.resourceTree.SelectedNode as T; }

        public void Link(ResourceNode res)
        {
            Unlink();
            if (res != null)
            {
                this.Text = res.Name;
                TreeNodeCollection nodes = Nodes;

                //Should we continue down the tree?
                if ((IsExpanded) && (res.HasChildren))
                {
                    //Add/link each resource node
                    foreach (ResourceNode n in res.Children)
                    {
                        bool found = false;
                        foreach (BaseWrapper tn in nodes)
                            if (tn.Text == n.Name)
                            {
                                tn.Link(n);
                                found = true;
                                break;
                            }

                        if (!found)
                            nodes.Add(Wrap(n));
                    }

                    //Remove empty nodes
                    for (int i = 0; i < nodes.Count; )
                    {
                        BaseWrapper n = nodes[i] as BaseWrapper;
                        if (n._resource == null)
                            n.Remove();
                        else
                            i++;
                    }

                    _discovered = true;
                }
                else
                {
                    //Node will be reset and undiscovered
                    nodes.Clear();
                    //Collapse();
                    if (res.HasChildren)
                    {
                        nodes.Add(new GenericWrapper());
                        _discovered = false;
                    }
                    else
                        _discovered = true;
                }

                SelectedImageIndex = ImageIndex = (int)res.ResourceType & 0xFF;

                res.ChildAdded += OnChildAdded;
                res.ChildRemoved += OnChildRemoved;
                res.Replaced += OnReplaced;
                res.Restored += OnRestored;
                res.Renamed += OnRenamed;
            }
            _resource = res;
        }
        public void Unlink()
        {
            if (_resource != null)
            {
                _resource.ChildAdded -= OnChildAdded;
                _resource.ChildRemoved -= OnChildRemoved;
                _resource.Replaced -= OnReplaced;
                _resource.Restored -= OnRestored;
                _resource.Renamed -= OnRenamed;
                _resource = null;
            }

            foreach (BaseWrapper n in Nodes)
                n.Unlink();
        }

        internal protected virtual void OnChildAdded(ResourceNode parent, ResourceNode child)
        {
            Nodes.Add(Wrap(child));
        }
        internal protected virtual void OnChildRemoved(ResourceNode parent, ResourceNode child)
        {
            foreach(BaseWrapper w in Nodes)
                if (w._resource == child)
                {
                    w.Unlink();
                    w.Remove();
                }
        }
        internal protected virtual void OnRestored(ResourceNode node)
        {
            Link(node);

            if ((TreeView != null) && (TreeView.SelectedNode == this))
            {
                ((ResourceTree)TreeView).SelectedNode = null;
                TreeView.SelectedNode = this;
            }
        }
        internal protected virtual void OnReplaced(ResourceNode node)
        {
            if ((TreeView != null) && (TreeView.SelectedNode == this))
            {
                ((ResourceTree)TreeView).SelectedNode = null;
                TreeView.SelectedNode = this;
            }
        }
        internal protected virtual void OnRenamed(ResourceNode node) { Text = node.Name; }

        internal protected virtual void OnExpand()
        {
            if (!_discovered)
            {
                Nodes.Clear();
                foreach (ResourceNode n in _resource.Children)
                    Nodes.Add(Wrap(n));
                _discovered = true;
            }
        }
        internal protected virtual void OnDoubleClick() { }

        internal BaseWrapper FindResource(ResourceNode n, bool searchChildren)
        {
            BaseWrapper node;
            if (_resource == n)
                return this;
            else
            {
                OnExpand();
                foreach (BaseWrapper c in Nodes)
                    if (c._resource == n)
                        return c;
                    else if ((searchChildren) && ((node = c.FindResource(n, true)) != null))
                        return node;
            }
            return null;
        }

        public static BaseWrapper Wrap(ResourceNode node)
        {
            BaseWrapper w;
            if (!NodeWrapperAttribute.Wrappers.ContainsKey(node.ResourceType))
                w = new GenericWrapper();
            else
                w = Activator.CreateInstance(NodeWrapperAttribute.Wrappers[node.ResourceType]) as BaseWrapper;
            w.Link(node);
            return w;
        }

    }
}
