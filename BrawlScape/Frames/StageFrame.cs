using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlScape
{
    public partial class StageFrame : UserControl
    {
        public delegate void StageChangeEvent(StageDefinition def);
        public static event StageChangeEvent StageChanged;

        private StageDefinition _currentStage;
        public StageDefinition SelectedStage
        {
            get { return _currentStage; }
            set
            {
                _currentStage = value;
                MainForm._currentTextureNode = value == null ? null : value.Reference;
                if (StageChanged != null)
                    StageChanged(_currentStage);
            }
        }

        public StageFrame() { InitializeComponent(); }

        private void StageFrame_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                Initialize();

        }

        private void Initialize()
        {
            stageList.BeginUpdate();

            stageList.Items.Clear();
            stageIcons.Images.Clear();

            Image im;
            int index = 0;
            foreach (StageDefinition def in StageDefinition.List)
            {
                if ((im = def.Texture) != null)
                {
                    stageIcons.Images.Add(im);
                    def.ImageIndex = index++;
                }
                else
                    def.ImageIndex = -1;

                stageList.Items.Add(def);
            }

            stageList.EndUpdate();
        }

        private void InitStage()
        {
        }

        private void stageList_SelectedIndexChanged(object sender, EventArgs e) { SelectedStage = stageList.SelectedItems.Count == 0 ? null : stageList.SelectedItems[0] as StageDefinition; }
    }
}
