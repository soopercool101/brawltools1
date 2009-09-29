using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;

namespace BrawlScape
{
    public partial class ItemFrame : UserControl
    {
        private ItemDefinition _selectedItem;
        private ItemDefinition SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; InitSelection(); }
        }

        private TextureDefinition _selectedTexture;
        public TextureDefinition SelectedTexture
        {
            get { return _selectedTexture; }
            set { _selectedTexture = value; }
        }

        public ItemFrame()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            _itemList.BeginUpdate();

            _itemList.Clear();
            _iconList.Images.Clear();

            int index = 0;
            Image img;
            try
            {
                foreach (ItemDefinition i in ItemDefinition.List)
                {
                    if ((img = i.GetIcon()) != null)
                    {
                        _iconList.Images.Add(img);
                        img.Dispose();
                        i.ImageIndex = index++;
                    }
                    else
                        i.ImageIndex = -1;

                    _itemList.Items.Add(i);
                }
            }
            catch (Exception x) { MessageBox.Show(x.Message); }

            _itemList.EndUpdate();
        }
        public void InitSelection()
        {
            _textureList.BeginUpdate();

            _textureList.Items.Clear();
            _textureImageList.Images.Clear();

            int index = 0;
            Image img;
            if (_selectedItem != null)
                foreach (TextureDefinition t in _selectedItem.Textures)
                {
                    if ((img = t.Texture) != null)
                    {
                        _textureImageList.Images.Add(img);
                        //img.Dispose();
                        t.ImageIndex = index++;
                    }
                    else
                        t.ImageIndex = -1;

                    _textureList.Items.Add(t);
                }

            _textureList.EndUpdate();
        }

        private void _itemList_SelectedIndexChanged(object sender, EventArgs e) { SelectedItem = _itemList.SelectedItems.Count == 0 ? null : _itemList.SelectedItems[0] as ItemDefinition; }


        private void mnuExport_Click(object sender, EventArgs e) { _selectedTexture.Export(); }
        private void mnuReplace_Click(object sender, EventArgs e) { _selectedTexture.Replace(); }
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e) { _selectedTexture.Restore(); }
        private void _textureList_SelectedIndexChanged(object sender, EventArgs e) { SelectedTexture = _textureList.SelectedItems.Count == 0 ? null : _textureList.SelectedItems[0] as TextureDefinition; }
        private void ctxTexture_Opening(object sender, CancelEventArgs e)
        {
            if (_selectedTexture == null)
                e.Cancel = true;
            else
            {
                TEX0Node node = (TEX0Node)_selectedTexture.TextureNode;

                mnuFileSize.Text = String.Format("File Size: {0}", node.WorkingSource.Length);
                mnuFormat.Text = String.Format("Format: {0}", node.Format);
                mnuLOD.Text = String.Format("LOD: {0}", node.LevelOfDetail);
                mnuSize.Text = String.Format("Size: {0} x {1}", node.Width, node.Height);

                PLT0Node pNode = node.GetPaletteNode();
                if (pNode == null)
                    mnuPalette.Text = "Palette: None";
                else
                    mnuPalette.Text = String.Format("Palette: {0}, {1} colors", pNode.Format, pNode.Colors);
            }
        }


        private void ItemFrame_Enter(object sender, EventArgs e)
        {
        }

        private void ItemFrame_Load(object sender, EventArgs e)
        {
            Initialize();
        }
    }
}
