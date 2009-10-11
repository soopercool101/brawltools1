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

        private void stageList_ResourceChanged(StageDefinition resource) { modelList.CurrentSource = resource; }
        private void modelList_ResourceChanged(ModelDefinition resource) 
        {
            textureList.CurrentSource = resource;
            if (resource != null)
            {
                resource.AttachTextures();
                modelPanel.TargetModel = resource.Model;
            }
            else
                modelPanel.TargetModel = null;
        }

        private void StageFrame_Load(object sender, EventArgs e) { stageList.CurrentSource = this; }
    }
}
