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

        public CostumeFrame()        
        {            
            InitializeComponent();
        }

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
                        im.Dispose();
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

            int index = 0;
            Image im;
            if (_selectedCostume != null)
                foreach (TextureDefinition def in _selectedCostume.Textures)
                {
                    if ((im = def.Texture) != null)
                    {
                        textureImages.Images.Add(im);
                        im.Dispose();
                        def.ImageIndex = index++;
                    }
                    else
                        def.ImageIndex = -1;

                    _textureList.Items.Add(def);
                }

            _textureList.EndUpdate();

            if (_textureList.SelectedItems.Count == 0)
                SelectedTexture = null;
        }


        private void _costumeList_SelectedIndexChanged(object sender, EventArgs e) { SelectedCostume = _costumeList.SelectedItems.Count == 0 ? null : _costumeList.SelectedItems[0] as CostumeDefinition; }
        private void mnuCSPReplace_Click(object sender, EventArgs e) { _selectedCostume.Replace(); }
        private void mnuCSPResore_Click(object sender, EventArgs e) { _selectedCostume.Restore(); }
        private void mnuCSPExport_Click(object sender, EventArgs e) { _selectedCostume.Export(); }
        private void costumeContext_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedCostume == null)
                e.Cancel = true;
            else
            {
            }
        }

        private void _textureList_SelectedIndexChanged(object sender, EventArgs e) { SelectedTexture = _textureList.SelectedItems.Count == 0 ? null : _textureList.SelectedItems[0] as TextureDefinition; }
        private void mnuTextureReplace_Click(object sender, EventArgs e) { _selectedTexture.Replace(); }
        private void mnuTextureExport_Click(object sender, EventArgs e) { _selectedTexture.Export(); }
        private void mnuTextureRestore_Click(object sender, EventArgs e) { _selectedTexture.Restore(); }
        private void textureContext_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedTexture == null)
                e.Cancel = true;
            else
            {
            }
        }

        private void mnuCostumeExport_Click(object sender, EventArgs e) { _selectedCostume.ExportCostume(); }
        private void mnuCostumeImport_Click(object sender, EventArgs e)
        {
            CostumeDefinition cos = _selectedCostume;
            if (_selectedCostume.ImportCostume())
            {
                cos.Selected = false;
                cos.Reset();
                cos.Selected = true;
            }
        }
        private void mnuCostumeRestore_Click(object sender, EventArgs e)
        {
            CostumeDefinition cos = _selectedCostume;
            if (_selectedCostume.RestoreCostume())
            {
                cos.Selected = false;
                cos.Reset();
                cos.Selected = true;
            }
        }

        private void CostumeFrame_Load(object sender, EventArgs e)
        {
            CharacterFrame.CharacterChanged += CharacterChanged;
        }
    }
}
