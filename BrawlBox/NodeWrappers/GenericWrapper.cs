using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrawlLib.IO;

namespace BrawlBox
{
    //Contains generic members inherited by all sub-classed nodes
    class GenericWrapper : BaseWrapper
    {
        public virtual string ExportFilter { get { return "All Files (*.*)|*.*"; } }
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
            Program.SaveDialog.Filter = ExportFilter;
            Program.SaveDialog.FileName = this.Text;
            if (Program.SaveDialog.ShowDialog() == DialogResult.OK)
            {
                //Parse generic filter
                if (Program.SaveDialog.FilterIndex == 1)
                    Program.SaveDialog.FilterIndex = CategorizeFilter(Program.SaveDialog.FileName, ExportFilter);

                //Export data
                using (FileStream stream = new FileStream(Program.SaveDialog.FileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 0x1000, FileOptions.RandomAccess))
                    OnExport(stream, Program.SaveDialog.FilterIndex);
            }
        }
        public virtual void OnExport(FileStream outStream, int filterIndex)
        {
            ResourceNode.Rebuild();
            ResourceNode.Export(outStream);
        }

        [NodeAction("&Replace", ShortcutKeys = Keys.Control | Keys.R)]
        public void Test1()
        {
            MessageBox.Show("Test1");
        }
    }
}
