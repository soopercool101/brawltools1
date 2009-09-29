using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlScape
{
    public partial class CharacterFrame : UserControl
    {
        private CharacterDefinition _selectedCharacter;
        public CharacterDefinition SelectedCharacter
        {
            get { return _selectedCharacter; }
            set { _selectedCharacter = value; InitCharacter(); }
        }

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

        public CharacterFrame()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            _charList.BeginUpdate();

            _charList.Clear();

            foreach (Image img in csfList.Images)
                img.Dispose();
            csfList.Images.Clear();

            int index = 0;
            Image im;
            try
            {

                foreach (CharacterDefinition def in CharacterDefinition.List)
                {
                    if ((im = def.GetCSF()) != null)
                    {
                        csfList.Images.Add(im);
                        def.ImageIndex = index++;
                    }
                    else
                        def.ImageIndex = -1;

                    _charList.Items.Add(def);
                }
            }
            catch (Exception x) { MessageBox.Show(x.Message); }

            _charList.EndUpdate();
        }
        public void InitCharacter()
        {
            _costumeList.BeginUpdate();
            _costumeList.Clear();

            cspImages.Images.Clear();
            int index = 0;
            Image im;
            if (_selectedCharacter != null)
                foreach (CostumeDefinition def in _selectedCharacter.Costumes)
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

            int index = 0;
            Image im;
            if (_selectedCostume != null)
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

            _textureList.EndUpdate();

            if (_textureList.SelectedItems.Count == 0)
                SelectedTexture = null;
        }

        private void _charList_SelectedIndexChanged(object sender, EventArgs e) { SelectedCharacter = _charList.SelectedItems.Count == 0 ? null : _charList.SelectedItems[0] as CharacterDefinition; }


        private void _costumeList_SelectedIndexChanged(object sender, EventArgs e) { SelectedCostume = _costumeList.SelectedItems.Count == 0 ? null : _costumeList.SelectedItems[0] as CostumeDefinition; }
        private void mnuCSPReplace_Click(object sender, EventArgs e) { _selectedCostume.Replace(); }
        private void mnuCSPResore_Click(object sender, EventArgs e) { _selectedCostume.Restore(); }
        private void mnuCSPExport_Click(object sender, EventArgs e) { _selectedCostume.Export(); }
        private void costumeContext_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedCharacter == null)
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

        private void CharacterFrame_Enter(object sender, EventArgs e)
        {
        }

        private void CharacterFrame_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        private void mnuCostumeExport_Click(object sender, EventArgs e) { _selectedCostume.ExportCostume(); }
        private void mnuCostumeImport_Click(object sender, EventArgs e) { _selectedCostume.ImportCostume(); }
        private void mnuCostumeRestore_Click(object sender, EventArgs e) { _selectedCostume.RestoreCostume(); }
    }
}
