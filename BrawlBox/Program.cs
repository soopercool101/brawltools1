using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using BrawlLib.IO;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Diagnostics;

namespace SmashBox
{
    static class Program
    {
        public static string AssemblyTitle = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
        public static string AssemblyDescription = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
        public static string AssemblyCopyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;

        private static OpenFileDialog _openDlg;
        public static OpenFileDialog OpenDialog { get { return _openDlg == null ? _openDlg = new OpenFileDialog() : _openDlg; } }
        private static SaveFileDialog _saveDlg;
        public static SaveFileDialog SaveDialog { get { return _saveDlg == null ? _saveDlg = new SaveFileDialog() : _saveDlg; } }

        private static ResourceNode _rootNode;
        public static ResourceNode RootNode { get { return _rootNode; } }
        private static string _rootPath;
        public static string RootPath { get { return _rootPath; } }

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length >= 1)
                    Open(args[0]);

                if (args.Length >= 2)
                {
                    ResourceNode target = ResourceNode.FindNode(RootNode, args[1], true);
                    if (target != null)
                        MainForm.Instance.TargetResource(target);
                    else
                        OutputMessage(String.Format("Error: Unable to find node or path '{0}'!", args[1]));
                }

                Application.EnableVisualStyles();
                Application.Run(MainForm.Instance);
            }
            catch (Exception x) { Program.OutputMessage(x.ToString()); }
            finally { Close(true); }
        }

        public static void OutputMessage(string msg)
        {
                MessageBox.Show(msg);
        }

        public static bool Close() { return Close(false); }
        public static bool Close(bool force)
        {
            if (_rootNode != null)
            {
                if ((_rootNode.IsDirty) && (!force))
                {
                    DialogResult res = MessageBox.Show("Closing", "Save changes?", MessageBoxButtons.YesNoCancel);
                    if (((res == DialogResult.Yes) && (!Save())) || (res == DialogResult.Cancel))
                        return false;
                }

                _rootNode.Dispose();
                _rootNode = null;

                MainForm.Instance.Reset();
            }
            _rootPath = null;
            return true;
        }

        public static bool Open(string path)
        {
            if (!Close())
                return false;

            try
            {
                if ((_rootNode = NodeFactory.FromFile(null, _rootPath = path)) != null)
                {
                    MainForm.Instance.Reset();
                    return true;
                }
                else
                    OutputMessage("Unable to recognize input file.");
            }
            catch (Exception x) { OutputMessage(x.ToString()); }

            Close();
            return false;
        }

        public static bool Save()
        {
            if (_rootNode != null)
            {
                try
                {
                    _rootNode.Merge(Control.ModifierKeys == (Keys.Control | Keys.Shift));
                    _rootNode.Export(_rootPath);
                    return true;
                }
                catch (Exception x){ MessageBox.Show(x.Message);}
            }
            return false;
        }
    }
}
