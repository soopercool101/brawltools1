using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using System.Reflection;

namespace SmashBox
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
        protected bool _discovered = false;

        protected ResourceNode _resource;
        public ResourceNode ResourceNode
        {
            get { return _resource; }
            //set { Link(value); }
        }

        protected BaseWrapper() { }
        protected BaseWrapper(ResourceNode resourceNode) { Link(resourceNode); }

        public void Link(ResourceNode res)
        {
            Unlink();
            if (res != null)
            {
                this.Text = res.Name;
                //Should we continue down the tree?
                if ((IsExpanded) && (res.HasChildren))
                {
                    foreach (BaseWrapper tn in Nodes)
                    {
                        bool found = false;
                        foreach (ResourceNode n in res.Children)
                        {
                            if (tn.Text.Equals(n.Name, StringComparison.Ordinal))
                            {
                                tn.Link(n);
                                found = true;
                                break;
                            }
                        }
                        //No match found, so remove node
                        //Already unlinked
                        if (!found)
                            tn.Remove();
                    }
                    _discovered = true;
                }
                else
                {
                    //Node will be reset and undiscovered
                    Nodes.Clear();
                    //Collapse();
                    if (res.HasChildren)
                    {
                        Nodes.Add(new GenericWrapper());
                        _discovered = false;
                    }
                    else
                        _discovered = true;
                }

                SelectedImageIndex = ImageIndex = (int)res.ResourceType & 0xFF;
            }
            _resource = res;
        }
        public void Unlink()
        {
            _resource = null;
            foreach (BaseWrapper n in Nodes)
                n.Unlink();
        }

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
