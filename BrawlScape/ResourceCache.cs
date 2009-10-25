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
        public delegate void TreeLoadedEvent(ResourceTree tree);
        public static event TreeLoadedEvent TreeLoaded;

        //private static Dictionary<string, ResourceNode> _resCache = new Dictionary<string, ResourceNode>();
        private static List<ResourceTree> _treeCache = new List<ResourceTree>();
        //private static List<ResourceTree> _changedTrees = new List<ResourceTree>();
        //private static List<ResourceTree> _restoredTrees = new List<ResourceTree>();
        private static HashSet<string> _errorList = new HashSet<string>();

        internal static ResourceTree GetTree(string relativePath) { return GetTree(relativePath, true); }
        internal static ResourceTree GetTree(string relativePath, bool searchWorking)
        {
            foreach (ResourceTree tree in _treeCache)
                if (tree.RelativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase))
                    return tree;

            string path = Program.GetFilePath(relativePath, searchWorking);
            if (path == null)
            {
                ReportError(relativePath);
                return null;
            }

            ResourceNode node = NodeFactory.FromFile(null, path);
            ResourceTree rt = new ResourceTree(relativePath, node);
            _treeCache.Add(rt);

            if (TreeLoaded != null)
                TreeLoaded(rt);

            return rt;
        }

        internal static void ReportError(string relativePath)
        {
            if (_errorList.Contains(relativePath))
                return;

            _errorList.Add(relativePath);
            MessageBox.Show(String.Format("Could not find file '{0}' in your data or project directories. Please replace this file and try again.", relativePath));
        }

        internal static ResourceNode FindNode(string relativePath, string resourcePath) { return FindNode(relativePath, resourcePath, true, false); }
        internal static ResourceNode FindNode(string relativePath, string resourcePath, bool searchWorking, bool searchChildren)
        {
            ResourceTree tree = GetTree(relativePath, searchWorking);
            if (tree == null)
                return null;
            return ResourceNode.FindNode(tree.Node, resourcePath, searchChildren);
        }

        //internal static void OnTreeChanged(ResourceTree t)
        //{
        //    bool listed = _changedTrees.Contains(t);
        //    bool restored = _restoredTrees.Contains(t);
        //    bool dirty = t.Node.IsDirty;

        //    if ((dirty) && (restored))
        //        _restoredTrees.Remove(t);

        //    if (dirty && !listed)
        //        _changedTrees.Add(t);
        //    else if (!dirty && listed)
        //        _changedTrees.Remove(t);
        //}
        //internal static void OnNodeChanged(ResourceTree t, ResourceNode n)        {        }

        internal static void Clear()
        {
            foreach (ResourceTree t in _treeCache)
                t.Node.Dispose();

            _treeCache.Clear();
            //_changedTrees.Clear();
            //_restoredTrees.Clear();
        }

        public static void SaveChanges()
        {
            ResourceTree common5 = null;
            foreach (ResourceTree t in _treeCache)
            {
                if ((t.RelativePath == "system\\common5.pac") && (t.Node.IsDirty))
                    common5 = t;
            }

            foreach (ResourceTree tree in _treeCache)
            {
                ResourceNode n = tree.Node;

                if (!n.IsDirty)
                {
                    //Is original?
                    if (tree.IsDataCopy)
                    {
                        //Delete
                    }
                    continue;
                }

                string dir = Path.GetDirectoryName(tree.WorkingPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (tree == common5)
                    continue;

                n.Rebuild();
                if (!n.HasMerged)
                    n.Merge();

                if (tree.IsPair)
                    ((ARCNode)n).ExportPair(tree.WorkingPath);
                else
                    n.Export(tree.WorkingPath);

                if (tree.RelativePath == "menu2\\sc_selcharacter.pac")
                {
                    if (common5 == null)
                        common5 = GetTree("system\\common5.pac");

                    common5.Node.FindChild("sc_selcharacter_en", false).ReplaceRaw(n.WorkingSource.Address, n.WorkingSource.Length);
                }

                //set absolute path
            }

            //while(_changedTrees.Count > 0)
            //{
            //    ResourceTree t = _changedTrees[0];
            //    _changedTrees.Remove(t);

            //    string dir = Path.GetDirectoryName(t.WorkingPath);
            //    if (!Directory.Exists(dir))
            //        Directory.CreateDirectory(dir);

            //    if (t == common5)
            //        continue;

            //    ResourceNode n = t.Node;
            //    n.Rebuild();
            //    if (!n.HasMerged)
            //        n.Merge();

            //    if (t.IsPair)
            //        ((ARCNode)n).ExportPair(t.WorkingPath);
            //    else
            //        n.Export(t.WorkingPath);

            //    if (t.RelativePath == "menu2\\sc_selcharacter.pac")
            //    {
            //        if (common5 == null)
            //            common5 = GetTree("system\\common5.pac");

            //        common5.Node.FindChild("sc_selcharacter_en", false).ReplaceRaw(n.WorkingSource.Address, n.WorkingSource.Length);
            //    }

            //    //string path = t.FilePath;
            //    //string dir = Path.GetDirectoryName(path);
            //    //string name = Path.GetFileNameWithoutExtension(path);
            //    //string ext = Path.GetExtension(path);
            //    //if (name.EndsWith("_en", StringComparison.OrdinalIgnoreCase))
            //    //    path = Path.Combine(dir, name.Substring(0, name.LastIndexOf("_en")) + ext);

            //    //if (!Directory.Exists(dir))
            //    //    Directory.CreateDirectory(dir);

            //    //if ((n is ARCNode) && (((ARCNode)n).IsPair))
            //    //    ((ARCNode)n).ExportPair(path);
            //    //else
            //    //    n.Export(path);
            //}

            if (common5 != null)
            {
                ResourceNode n = common5.Node;
                n.Rebuild();
                if (!n.HasMerged)
                    n.Merge();

                n.Export(common5.WorkingPath);
            }

            //foreach(ResourceTree t in _restoredTrees)
            //{
            //    string path = t.WorkingPath;
            //    if (t.IsPair)
            //    {
            //        if (File.Exists(path + ".pac"))
            //            File.Delete(path + ".pac");
            //        if (File.Exists(path + ".pcs"))
            //            File.Delete(path + ".pcs");
            //    }
            //    else if (File.Exists(path))
            //        File.Delete(path);
            //}
            //_restoredTrees.Clear();
        }

        internal static ResourceNode[] FindNodeByType(string relativePath, string nodePath, ResourceType type)
        {
            ResourceNode node = FindNode(relativePath, nodePath);
            if (node == null)
                return null;
            return node.FindChildrenByType(null, type);
        }

        internal static void Unload(string relativePath)
        {
            foreach (ResourceTree t in _treeCache)
            {
                if (!t.RelativePath.Equals(relativePath, StringComparison.OrdinalIgnoreCase))
                    continue;

                //if (_changedTrees.Contains(t))
                //    _changedTrees.Remove(t);
                //if (_restoredTrees.Contains(t))
                //    _restoredTrees.Remove(t);

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
                    //_restoredTrees.Add(newTree);
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
            //_changedTrees.Add(t);

            if (TreeLoaded != null)
                TreeLoaded(t);

            return t;
        }
    }
}
