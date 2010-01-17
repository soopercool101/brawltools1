using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.OpenGL;

namespace BrawlScape
{
    public partial class StageFrame : UserControl, IListSource<StageDefinition>
    {
        public StageFrame() { InitializeComponent(); }

        private StageDefinition[] _stages;
        public StageDefinition[] ListItems { get { return _stages == null ? _stages = StageDefinition.List.ToArray() : _stages; } }

        private void stageList_ResourceChanged(StageDefinition resource) { textureList.PrimarySource = resource; modelList.CurrentSource = resource; }
        private void modelList_ResourceChanged(ModelDefinition resource)
        {
            textureList.SecondarySource = resource;
            modelPanel.ClearTargets();
            if (resource != null)
                modelPanel.AddTarget(resource.Model);
        }

        private void StageFrame_Load(object sender, EventArgs e) { if (!DesignMode) stageList.CurrentSource = this; }
    }
}
