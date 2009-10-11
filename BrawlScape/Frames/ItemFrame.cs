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
    public partial class ItemFrame : UserControl, IListSource<ItemDefinition>
    {
        public ItemFrame() { InitializeComponent(); }

        private void ItemFrame_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            itemList.CurrentSource = this;
        }

        private ItemDefinition[] _items;
        public ItemDefinition[] ListItems { get { return _items == null ? _items = ItemDefinition.List.ToArray() : _items; } }

        private void itemList_ResourceChanged(ItemDefinition resource)
        {
            modelList.CurrentSource = resource;
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
    }
}
