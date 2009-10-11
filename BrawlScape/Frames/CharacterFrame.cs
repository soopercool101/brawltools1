using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BrawlLib.OpenGL;

namespace BrawlScape
{
    public partial class CharacterFrame : UserControl, IListSource<CharacterDefinition>
    {

        public CharacterFrame() { InitializeComponent(); }

        private void CharacterFrame_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            charList.CurrentSource = this;
            mnuCharIcon.DropDown = new TextureContextMenuStrip();
            mnuNameStrip.DropDown = new TextureContextMenuStrip();
            mnuCostumeCSP.DropDown = new TextureContextMenuStrip();
        }

        private void mnuCostumeExport_Click(object sender, EventArgs e) { costumeList.SelectedResource.ExportCostume(); }
        private void mnuCostumeImport_Click(object sender, EventArgs e) { costumeList.SelectedResource.ImportCostume(); }
        private void mnuCostumeRestore_Click(object sender, EventArgs e) { costumeList.SelectedResource.RestoreCostume(); }

        public CharacterDefinition[] _characters;
        public CharacterDefinition[] ListItems { get { return _characters == null ? _characters = CharacterDefinition.List.ToArray() : _characters; } }

        private void charList_ResourceChanged(CharacterDefinition resource)
        {
            costumeList.CurrentSource = resource;
            if (resource != null)
            {
                ((TextureContextMenuStrip)mnuCharIcon.DropDown).TextureReference = resource.Reference;
                ((TextureContextMenuStrip)mnuNameStrip.DropDown).TextureReference = resource.NameReference;
            }
        }
        private void costumeList_ResourceChanged(CostumeDefinition resource)
        {
            modelList.CurrentSource = resource;
            if (resource != null)
            {
                ((TextureContextMenuStrip)mnuCostumeCSP.DropDown).TextureReference = resource.Reference;
                gamePortrait.Reference = resource.GamePortrait;
                stockPortrait.Reference = resource.StockPortrait;
            }
            else
            {
                stockPortrait.Reference = gamePortrait.Reference = null;
            }

            if ((modelList.SelectedResource == null) && (modelList.Items.Count != 0))
                modelList.SelectedIndices.Add(0);
        }
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

        private void characterContext_Opening(object sender, CancelEventArgs e) { if (charList.SelectedResource == null) e.Cancel = true; }
        private void costumeContext_Opening(object sender, CancelEventArgs e) { if (costumeList.SelectedResource == null) e.Cancel = true; }
    }
}
