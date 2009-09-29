using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.IO;

namespace BrawlScape
{
    public static class ResourceCache
    {
        private static Dictionary<string, ResourceNode> _resCache = new Dictionary<string, ResourceNode>();
        private static List<ResourceNode> _changedTrees = new List<ResourceNode>();

        internal static ResourceNode FindNode(string filePath, string resourcePath)
        {
            if (!_resCache.ContainsKey(filePath))
            {
                ResourceNode node = NodeFactory.FromFile(null, Program.GetFilePath(filePath));
                _resCache[filePath] = node;
                node.Changed += OnTreeChanged;
                node.ChildChanged += OnTreeChildChanged;
            }
            return ResourceNode.FindNode(_resCache[filePath], resourcePath, true);
        }

        private static void OnTreeChanged(ResourceNode n)
        {
            bool listed = _changedTrees.Contains(n);
            bool dirty = n.IsDirty;

            if (dirty && !listed)
                _changedTrees.Add(n);
            else if (!dirty && listed)
                _changedTrees.Remove(n);
        }
        private static void OnTreeChildChanged(ResourceNode n, ResourceNode child)
        {
            OnTreeChanged(n);
        }

        internal static void Clear()
        {
            _changedTrees.Clear();
            foreach (ResourceNode n in _resCache.Values)
                n.Dispose();
            _resCache.Clear();
        }

        public static void SaveChanges()
        {
            while (_changedTrees.Count > 0)
            {
                ResourceNode n = _changedTrees[0];
                n.Rebuild();
                if (!n.HasMerged)
                    n.Merge();

                string path = n.FilePath;
                if (path.StartsWith(Program.DataPath))
                    path = Program.WorkingPath + path.Replace(Program.DataPath, "");

                string name = Path.GetFileNameWithoutExtension(path);
                if (name.EndsWith("_en", StringComparison.OrdinalIgnoreCase))
                    path = Path.Combine(Path.GetDirectoryName(path), name.Substring(0, name.LastIndexOf("_en")) + Path.GetExtension(path));

                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if ((n is ARCNode) && (((ARCNode)n).IsPair))
                    ((ARCNode)n).ExportPair(path);
                else
                    n.Export(path);
            }
        }

        internal static ResourceNode[] FindNodeByType(string filePath, string nodePath, ResourceType type)
        {
            ResourceNode node = FindNode(filePath, nodePath);
            return node.FindChildrenByType(null, type);
        }
    }
}
