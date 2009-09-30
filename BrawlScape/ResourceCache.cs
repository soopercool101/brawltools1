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
        //private static Dictionary<string, ResourceNode> _resCache = new Dictionary<string, ResourceNode>();
        private static List<ResourceTree> _treeCache = new List<ResourceTree>();
        private static List<ResourceTree> _changedTrees = new List<ResourceTree>();
        private static List<ResourceTree> _restoredTrees = new List<ResourceTree>();

        internal static ResourceTree GetTree(string relativePath) { return GetTree(relativePath, true); }
        internal static ResourceTree GetTree(string relativePath, bool searchWorking)
        {
            foreach (ResourceTree tree in _treeCache)
                if (tree.RelativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase))
                    return tree;

            ResourceNode node = NodeFactory.FromFile(null, Program.GetFilePath(relativePath, searchWorking));
            ResourceTree rt = new ResourceTree(relativePath, node);
            _treeCache.Add(rt);
            return rt;
        }

        internal static ResourceNode FindNode(string relativePath, string resourcePath) { return FindNode(relativePath, resourcePath, true); }
        internal static ResourceNode FindNode(string relativePath, string resourcePath, bool searchWorking)
        {
            return ResourceNode.FindNode(GetTree(relativePath, searchWorking).Node, resourcePath, true);
        }

        internal static void OnTreeChanged(ResourceTree t)
        {
            bool listed = _changedTrees.Contains(t);
            bool restored = _restoredTrees.Contains(t);
            bool dirty = t.Node.IsDirty;

            if ((dirty) && (restored))
                _restoredTrees.Remove(t);

            if (dirty && !listed)
                _changedTrees.Add(t);
            else if (!dirty && listed)
                _changedTrees.Remove(t);
        }

        internal static void Clear()
        {
            foreach (ResourceTree t in _treeCache)
                t.Node.Dispose();

            _treeCache.Clear();
            _changedTrees.Clear();
            _restoredTrees.Clear();
        }

        public static void SaveChanges()
        {
            while(_changedTrees.Count > 0)
            {
                ResourceTree t = _changedTrees[0];
                _changedTrees.Remove(t);

                ResourceNode n = t.Node;
                n.Rebuild();
                if (!n.HasMerged)
                    n.Merge();

                if (t.IsPair)
                    ((ARCNode)n).ExportPair(t.WorkingPath);
                else
                    n.Export(t.WorkingPath);

                //string path = t.FilePath;
                //string dir =Path.GetDirectoryName(path);
                //string name = Path.GetFileNameWithoutExtension(path);
                //string ext = Path.GetExtension(path);
                //if (name.EndsWith("_en", StringComparison.OrdinalIgnoreCase))
                //    path = Path.Combine(dir, name.Substring(0, name.LastIndexOf("_en")) + ext);

                //if (!Directory.Exists(dir))
                //    Directory.CreateDirectory(dir);

                //if ((n is ARCNode) && (((ARCNode)n).IsPair))
                //    ((ARCNode)n).ExportPair(path);
                //else
                //    n.Export(path);
            }

            foreach(ResourceTree t in _restoredTrees)
            {
                string path = t.WorkingPath;
                if (t.IsPair)
                {
                    if (File.Exists(path + ".pac"))
                        File.Delete(path + ".pac");
                    if (File.Exists(path + ".pcs"))
                        File.Delete(path + ".pcs");
                }
                else if (File.Exists(path))
                    File.Delete(path);
            }
            _restoredTrees.Clear();
        }

        internal static ResourceNode[] FindNodeByType(string relativePath, string nodePath, ResourceType type)
        {
            ResourceNode node = FindNode(relativePath, nodePath);
            return node.FindChildrenByType(null, type);
        }

        internal static void Unload(string relativePath)
        {
            foreach (ResourceTree t in _treeCache)
            {
                if (!t.RelativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (_changedTrees.Contains(t))
                    _changedTrees.Remove(t);
                if (_restoredTrees.Contains(t))
                    _restoredTrees.Remove(t);

                t.Node.Dispose();
                _treeCache.Remove(t);
                break;
            }
        }

        internal static bool Restore(string relativePath)
        {
            ResourceTree newTree;
            ResourceNode n;

            foreach (ResourceTree t in _treeCache)
            {
                if (!t.RelativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase))
                    continue;

                n = t.Node;

                if (t.IsDataCopy)
                {
                    if ((n.IsDirty) || (n.IsBranch))
                    {
                        n.Restore();
                        return true;
                    }
                    else
                    {
                        string path = Program.GetFilePath(relativePath, true);
                        if (t.IsPair)
                            path = Path.ChangeExtension(path, Path.GetExtension(t.FilePath));
                        if (path.Equals(t.FilePath, StringComparison.OrdinalIgnoreCase))
                            return false;
                    }
                }

                Unload(relativePath);
                if (t.IsWorkingCopy)
                {
                    newTree = GetTree(relativePath, false);
                    _restoredTrees.Add(newTree);
                }
                else
                    newTree = GetTree(relativePath, true);

                return true;
            }
            return false;
        }

        internal static ResourceTree LoadExternal(string relativePath, string inFile)
        {
            Unload(relativePath);
            ResourceNode node = NodeFactory.FromFile(null, inFile);
            ResourceTree t = new ResourceTree(relativePath, node);
            _treeCache.Add(t);
            _changedTrees.Add(t);
            return t;
        }
    }
}
