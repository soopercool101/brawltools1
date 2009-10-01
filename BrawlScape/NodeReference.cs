﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace BrawlScape
{
    public class NodeReference
    {
        public static List<NodeReference> _cache = new List<NodeReference>();

        public event EventHandler DataChanged;

        protected string _relativePath, _nodePath, _name;
        //private ResourceTree _tree;
        private ResourceNode _node;

        public ResourceTree Tree 
        { 
            get 
            {
                return ResourceCache.GetTree(_relativePath); 
            } 
        }
        public ResourceNode Node
        { 
            get 
            {
                if (_node == null)
                {
                    if ((_node = ResourceNode.FindNode(Tree.Node, _nodePath, false)) != null)
                    {
                        _node.Disposed += OnDisposed;
                        _node.Changed += OnChanged;
                        _node.ChildChanged += OnChildChanged;
                    }
                }
                return _node;
            } 
        }
        public string Name { get { return _name; } }

        protected NodeReference() { }
        private void Initialize(string relativePath, string nodePath)
        {
            _relativePath = relativePath;
            _nodePath = nodePath;

            int index = _nodePath.LastIndexOf('/');
            if (index > 0)
                _name = _nodePath.Substring(index + 1);
            else
                _name = _nodePath;

            ResourceCache.TreeLoaded += OnTreeLoaded;
            _cache.Add(this);
        }

        private void OnTreeLoaded(ResourceTree tree)
        {
            if (tree.RelativePath.Equals(_relativePath, StringComparison.OrdinalIgnoreCase))
            {
                if((Node != null) && (DataChanged != null))
                    DataChanged(this, null);
            }
        }

        public static T Get<T>(string relativePath, string nodePath) where T : NodeReference
        {
            foreach (NodeReference nref in _cache)
                if (nref._relativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase) && nref._nodePath.Equals(nodePath, StringComparison.OrdinalIgnoreCase))
                    return (T)nref;

            T node = Activator.CreateInstance<T>();
            node.Initialize(relativePath, nodePath);
            return node;
        }

        protected virtual void OnDisposed(ResourceNode node)
        {
            _node = null;
        }
        protected virtual void OnChanged(ResourceNode node)
        {
            if (DataChanged != null)
                DataChanged(this, null);
        }
        private void OnChildChanged(ResourceNode node, ResourceNode child) { OnChanged(node); }
    }
}
