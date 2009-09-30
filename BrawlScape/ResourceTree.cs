using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;

namespace BrawlScape
{
    public class ResourceTree
    {
        string _relativePath, _absolutePath;
        ResourceNode _node;
        int _state;
        bool _isPair;

        public ResourceNode Node { get { return _node; } }
        public string RelativePath { get { return _relativePath; } }

        public string DataPath { get { return Path.Combine(Program.DataPath, _relativePath); } }
        public string WorkingPath { get { return Path.Combine(Program.WorkingPath, _relativePath); } }
        public string FilePath { get { return _absolutePath; } }

        public bool IsDataCopy { get { return _state == 0; } }
        public bool IsWorkingCopy { get { return _state == 1; } }
        public bool IsFloatingCopy { get { return _state == 2; } }
        public bool IsPair { get { return _isPair; } }

        public ResourceTree(string relativePath, ResourceNode node)
        {
            _relativePath = relativePath;
            _absolutePath = node.FilePath;
            _node = node;
            _isPair = !Path.HasExtension(relativePath);

            string path = node.FilePath;
            if (_isPair)
                path = path.Substring(0, path.LastIndexOf("."));

            if (path.Equals(Path.Combine(Program.DataPath, _relativePath), StringComparison.OrdinalIgnoreCase))
                _state = 0;
            else if (path.Equals(Path.Combine(Program.WorkingPath, _relativePath), StringComparison.OrdinalIgnoreCase))
                _state = 1;
            else
                _state = 2;

            node.Changed += OnChanged;
            node.ChildChanged += OnChildChanged;
        }

        private void OnChanged(ResourceNode n) { ResourceCache.OnTreeChanged(this); }
        private void OnChildChanged(ResourceNode n, ResourceNode c) { OnChanged(n); }
    }
}
