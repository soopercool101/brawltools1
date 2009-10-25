﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0)]
    class MDL0Wrapper : GenericWrapper
    {
        #region Menu

        private static ContextMenuStrip _menu;
        static MDL0Wrapper()
        {
            _menu = new ContextMenuStrip();
            _menu.Items.Add(new ToolStripMenuItem("&Preview", null, PreviewAction, Keys.Control | Keys.P));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("&Export", null, ExportAction, Keys.Control | Keys.E));
            _menu.Items.Add(new ToolStripMenuItem("&Replace", null, ReplaceAction, Keys.Control | Keys.R));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Res&tore", null, RestoreAction, Keys.Control | Keys.T));
            _menu.Items.Add(new ToolStripMenuItem("&Delete", null, DeleteAction, Keys.Delete));
            _menu.Items.Add(new ToolStripSeparator());
            _menu.Items.Add(new ToolStripMenuItem("Re&name", null, RenameAction, Keys.Control | Keys.N));
            _menu.Opening += MenuOpening;
            _menu.Closing += MenuClosing;
        }
        protected static void PreviewAction(object sender, EventArgs e) { GetInstance<MDL0Wrapper>().Preview(); }
        private static void MenuClosing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            _menu.Items[3].Enabled = _menu.Items[5].Enabled = _menu.Items[6].Enabled = true;
        }
        private static void MenuOpening(object sender, CancelEventArgs e)
        {
            MDL0Wrapper w = GetInstance<MDL0Wrapper>();
            if (w.Parent == null)
                _menu.Items[3].Enabled = _menu.Items[6].Enabled = false;
            else
                _menu.Items[3].Enabled = _menu.Items[6].Enabled = true;

            if ((w._resource.IsDirty) || (w._resource.IsBranch))
                _menu.Items[5].Enabled = true;
            else
                _menu.Items[5].Enabled = false;
        }
        #endregion

        public override string ExportFilter { get { return "Raw Model File (*.mdl0)|*.mdl0"; } }

        public MDL0Wrapper() { ContextMenuStrip = _menu; }

        public void Preview()
        {
            using (ModelForm form = new ModelForm())
            {
                form.ShowDialog(((MDL0Node)_resource).GetModel());
            }
        }

    }
}
