using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox
{
    //Contains generic members inherited by all sub-classed nodes
    class GenericWrapper : BaseWrapper
    {
        public virtual string ExportFilter { get { return "Raw Data File (*.*)|*.*"; } }
        public virtual string ReplaceFilter { get { return ExportFilter; } }
        public static int CategorizeFilter(string path, string filter)
        {
            string ext = "*" + Path.GetExtension(path);

            string[] split = filter.Split('|');
            for (int i = 3; i < split.Length; i += 2)
                foreach (string s in split[i].Split(';'))
                    if (s.Equals(ext, StringComparison.OrdinalIgnoreCase))
                        return (i + 1) / 2;
            return 1;
        }

        [NodeAction("&Export", ShortcutKeys = Keys.Control | Keys.E)]
        public virtual void Export()
        {
            string outPath;
            int index;
            if ((index = Program.SaveFile(ExportFilter, Text, out outPath)) > 0)
                OnExport(outPath, index);
            //if (index != 0)
            //    using (FileStream stream = new FileStream(outPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, FileOptions.RandomAccess))
            //        OnExport(stream, index);
        }
        public virtual void OnExport(string outPath, int filterIndex)
        {
            ResourceNode.Rebuild();
            ResourceNode.Export(outPath);
        }

        [NodeAction("&Replace", ShortcutKeys = Keys.Control | Keys.R)]
        public virtual void Replace()
        {
            string inPath;
            int index = Program.OpenFile(ReplaceFilter, out inPath);
            if (index != 0)
            {
                OnReplace(inPath, index);
                ResourceNode n = _resource;
                this.Unlink();
                this.Link(n);
            }
        }

        public virtual void OnReplace(string inStream, int filterIndex)
        {
            ResourceNode.Replace(inStream);
            TreeView.SelectedNode = null;
            TreeView.SelectedNode = this;
        }

        [NodeAction("&Delete")]
        public void Delete()
        {
            this.Remove();
            ResourceNode.Remove();
            ResourceNode.Dispose();
            this.Unlink();
        }
    }
}
