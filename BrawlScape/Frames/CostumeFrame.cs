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
    public partial class CostumeFrame : UserControl
    {
        private CostumeDefinition _selectedCostume;
        public CostumeDefinition SelectedCostume
        {
            get { return _selectedCostume; }
            set { _selectedCostume = value; InitCostume(); }
        }

        private TextureDefinition _selectedTexture;
        public TextureDefinition SelectedTexture
        {
            get { return _selectedTexture; }
            set { _selectedTexture = value; }
        }

        private ModelDefinition _selectedModel;
        public ModelDefinition SelectedModel
        {
            get { return _selectedModel; }
            set { _selectedModel = value; InitModel(); }
        }

        public CostumeFrame() { InitializeComponent(); }

        private void CharacterChanged(CharacterDefinition character)
        {
            _costumeList.BeginUpdate();
            _costumeList.Clear();

            cspImages.Images.Clear();
            int index = 0;
            Image im;
            if (character != null)
                foreach (CostumeDefinition def in character.Costumes)
                {
                    if ((im = def.Texture) != null)
                    {
                        cspImages.Images.Add(im);
                        def.ImageIndex = index++;
                    }
                    else
                        def.ImageIndex = -1;

                    _costumeList.Items.Add(def);
                }

            _costumeList.EndUpdate();

            if (_costumeList.SelectedItems.Count == 0)
                SelectedCostume = null;
        }

        public void InitCostume()
        {
            _textureList.BeginUpdate();

            _textureList.Clear();
            textureImages.Images.Clear();
            modelList.Items.Clear();

            picStock.Reference = null;
            picGame.Reference = null;

            int index = 0;
            Image im;
            if (_selectedCostume != null)
            {
                foreach (TextureDefinition def in _selectedCostume.Textures)
                {
                    if ((im = def.Texture) != null)
                    {
                        textureImages.Images.Add(im);
                        def.ImageIndex = index++;
                    }
                    else
                        def.ImageIndex = -1;

                    _textureList.Items.Add(def);
                }

                foreach (ModelDefinition def in _selectedCostume.Models)
                {
                    modelList.Items.Add(def);
                }

                picStock.Reference = _selectedCostume.StockPortrait;
                picGame.Reference = _selectedCostume.GamePortrait;
            }

            _textureList.EndUpdate();

            if (_textureList.SelectedItems.Count == 0)
                SelectedTexture = null;
        }

        private void InitModel()
        {
        }


        private void _costumeList_SelectedIndexChanged(object sender, EventArgs e) { SelectedCostume = _costumeList.SelectedItems.Count == 0 ? null : _costumeList.SelectedItems[0] as CostumeDefinition; }
        private void mnuCSPReplace_Click(object sender, EventArgs e) { _selectedCostume.Reference.Replace(); }
        private void mnuCSPResore_Click(object sender, EventArgs e) { _selectedCostume.Reference.Restore(); }
        private void mnuCSPExport_Click(object sender, EventArgs e) { _selectedCostume.Reference.Export(); }
        private void costumeContext_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedCostume == null)
                e.Cancel = true;
            else
            {
            }
        }

        private void _textureList_SelectedIndexChanged(object sender, EventArgs e) { MainForm._currentTextureNode = _textureList.SelectedItems.Count == 0 ? null : ((TextureDefinition)_textureList.SelectedItems[0]).Reference; }


        private void mnuCostumeExport_Click(object sender, EventArgs e) { _selectedCostume.ExportCostume(); }
        private void mnuCostumeImport_Click(object sender, EventArgs e)
        {
            CostumeDefinition cos = _selectedCostume;
            cos.Selected = false;
            if (cos.ImportCostume())
                cos.Reset();
            cos.Selected = true;
        }
        private void mnuCostumeRestore_Click(object sender, EventArgs e)
        {
            CostumeDefinition cos = _selectedCostume;
            cos.Selected = false;
            if (cos.RestoreCostume())
                cos.Reset();
            cos.Selected = true;
        }

        private void CostumeFrame_Load(object sender, EventArgs e)
        {
            CharacterFrame.CharacterChanged += CharacterChanged;
            _textureList.ContextMenuStrip = MainForm._textureContext;

            mnuCostumeCSP.DropDown = MainForm._textureContext;
        }

        private void mnuCostumeCSP_DropDownOpening(object sender, EventArgs e) { MainForm._currentTextureNode = _selectedCostume.Reference; }

        private void modelList_SelectedIndexChanged(object sender, EventArgs e) { SelectedModel = modelList.SelectedItem is ModelDefinition ? modelList.SelectedItem as ModelDefinition : null; }
    }
}
