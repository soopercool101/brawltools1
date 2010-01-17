using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlScape.Frames
{
    public partial class AdvFrame : UserControl, IListSource<AdvStageDefinition>
    {
        public AdvFrame()
        {
            InitializeComponent();
        }

        private void sseAreaList_ResourceChanged(AdvAreaDefinition resource)
        {
            texturePanel.PrimarySource = resource;
            modelList.CurrentSource = resource;
        }

        private void sseStageList_ResourceChanged(AdvStageDefinition resource)
        {
            sseAreaList.CurrentSource = resource;
        }

        private void SSEFrame_Load(object sender, EventArgs e)
        {
            if (!DesignMode) sseStageList.CurrentSource = this;
        }

        public AdvStageDefinition[] _stages;
        public AdvStageDefinition[] ListItems { get { return _stages == null ? _stages = AdvStageDefinition.List.ToArray() : _stages; } }

        private void modelList_ResourceChanged(ModelDefinition resource)
        {
            texturePanel.SecondarySource = resource;
            modelControl.ClearTargets();
            if (resource != null)
                modelControl.AddTarget(resource.Model);
        }
    }
}
