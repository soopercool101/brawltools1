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
            set 
            { 
                _selectedItem = value;
                MainForm._currentTextureNode = value == null ? null : value.Reference;
                InitSelection(); 
            }
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
                    if ((img = i.Texture) != null)
                    {
                        _iconList.Images.Add(img);
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
                        t.ImageIndex = index++;
                    }
                    else
                        t.ImageIndex = -1;

                    _textureList.Items.Add(t);
                }

            _textureList.EndUpdate();
        }

        private void _itemList_SelectedIndexChanged(object sender, EventArgs e) { SelectedItem = _itemList.SelectedItems.Count == 0 ? null : _itemList.SelectedItems[0] as ItemDefinition; }

        private void _textureList_SelectedIndexChanged(object sender, EventArgs e) { MainForm._currentTextureNode = _textureList.SelectedItems.Count == 0 ? null : ((TextureDefinition)_textureList.SelectedItems[0]).Reference; }

        private void ItemFrame_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
                Initialize();

            _itemList.ContextMenuStrip = MainForm._textureContext;
            _textureList.ContextMenuStrip = MainForm._textureContext;
        }
    }
}
