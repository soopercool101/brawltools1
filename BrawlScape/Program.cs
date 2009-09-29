using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace BrawlScape
{
    static class Program
    {
        public static string AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
        public static string AssemblyDescription = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
        public static string AssemblyCopyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;

        private static OpenFileDialog _openDlg = new OpenFileDialog();
        private static SaveFileDialog _saveDlg = new SaveFileDialog();
        private static FolderBrowserDialog _folderDlg = new FolderBrowserDialog();

        private static string _basePath;
        private static string _workingPath;
        public static string DataPath { get { return _basePath; } set { _basePath = value; Config.LastDataPath = value; } }
        public static string WorkingPath { get { return _workingPath; } set { _workingPath = value; Config.LastWorkingPath = value; } }

        [STAThread]
        static void Main()
        {
            try
            {
                _basePath = Config.LastDataPath;
                _workingPath = Config.LastWorkingPath;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception x) { MessageBox.Show(x.ToString()); }
            finally
            {
                ResourceCache.Clear();
            }
        }

        public static bool IsWorkingPath(string path)
        {
            return path.StartsWith(_workingPath);
        }

        public static string GetDataPath(string workingPath)
        {
            if (workingPath.StartsWith(_basePath))
                return workingPath;

            if (workingPath.StartsWith(_workingPath))
            {
                string path = workingPath.Replace(_workingPath, _basePath);
                if (File.Exists(path))
                    return path;

                int index = path.LastIndexOf('.');
                path = path.Substring(0, index) + "_en" + path.Substring(index);
                if (File.Exists(path))
                    return path;
            }
            return null;
        }

        public static string GetFilePath(string partialPath)
        {
            string path;
            string ext = Path.GetExtension(partialPath);
            string name = partialPath.Substring(0, partialPath.LastIndexOf(ext));

            if (ext.Equals(".pac", StringComparison.OrdinalIgnoreCase) || ext.Equals(".pcs", StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(path = Path.Combine(_workingPath, name + ".pcs")))
                    return path;
                if (File.Exists(path = Path.Combine(_workingPath, name + ".pac")))
                    return path;
                if (File.Exists(path = Path.Combine(_workingPath, name + "_en.pcs")))
                    return path;
                if (File.Exists(path = Path.Combine(_workingPath, name + "_en.pac")))
                    return path;
                if (!_workingPath.Equals(_basePath, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(path = Path.Combine(_basePath, name + ".pcs")))
                        return path;
                    if (File.Exists(path = Path.Combine(_basePath, name + ".pac")))
                        return path;
                    if (File.Exists(path = Path.Combine(_basePath, name + "_en.pcs")))
                        return path;
                    if (File.Exists(path = Path.Combine(_basePath, name + "_en.pac")))
                        return path;
                }
            }
            else
            {
                if (File.Exists(path = Path.Combine(_workingPath, name + ext)))
                    return path;
                if (File.Exists(path = Path.Combine(_workingPath, name + "_en" + ext)))
                    return path;
                if (!_workingPath.Equals(_basePath, StringComparison.OrdinalIgnoreCase))
                {
                    if (File.Exists(path = Path.Combine(_basePath, name + ext)))
                        return path;
                    if (File.Exists(path = Path.Combine(_basePath, name + "_en" + ext)))
                        return path;
                }
            }

            //return null;
            throw new FileNotFoundException(String.Format("Could not find file '{0}'. Please update your data folder to include this file and try again.", partialPath));
        }

        public static int OpenFile(string filter, out string fileName) { return OpenFile(filter, out fileName, true); }
        public static int OpenFile(string filter, out string fileName, bool categorize)
        {
            _openDlg.Filter = filter;
            if (_openDlg.ShowDialog() == DialogResult.OK)
            {
                fileName = _openDlg.FileName;
                if ((categorize) && (_openDlg.FilterIndex == 1))
                    return CategorizeFilter(_openDlg.FileName, filter);
                else
                    return _openDlg.FilterIndex;
            }
            fileName = null;
            return 0;
        }
        public static int SaveFile(string filter, string name, out string fileName) { return SaveFile(filter, name, out fileName, true); }
        public static int SaveFile(string filter, string name, out string fileName, bool categorize)
        {
            _saveDlg.Filter = filter;
            _saveDlg.FileName = name;
            if (_saveDlg.ShowDialog() == DialogResult.OK)
            {
                fileName = _saveDlg.FileName;
                if ((categorize) && (_saveDlg.FilterIndex == 1))
                    return CategorizeFilter(_saveDlg.FileName, filter);
                else
                    return _saveDlg.FilterIndex;
            }
            fileName = null;
            return 0;
        }
        public static string OpenFolder()
        {
            if (_folderDlg.ShowDialog() == DialogResult.OK)
                return _folderDlg.SelectedPath;
            return null;
        }

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
    }
}
