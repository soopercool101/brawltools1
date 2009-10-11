using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace BrawlScape
{
    public delegate void ResourceChangeEvent<T>(T resource) where T : ListViewItem;

    public class ResourceList<T> : ListView where T : ListViewItem
    {
        private T _selectedResource;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public T SelectedResource
        {
            get { return _selectedResource; }
            set { if (_selectedResource != value) OnResourceChanged(_selectedResource = value); }
        }
        public event ResourceChangeEvent<T> ResourceChanged;

        private IListSource<T> _currentSource;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IListSource<T> CurrentSource
        {
            get { return _currentSource; }
            set { if(_currentSource != value) OnSourceChanged(_currentSource = value);}
        }

        private Size _imageSize = new Size(128, 128);
        public Size ImageSize
        {
            get { return _imageSize; }
            set { _imageSize = value; }
        }

        private IContainer _component;
        private ImageList _imageList;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private ImageList ImageList
        {
            get
            {
                if (_imageList == null)
                {
                    _component = new Container();
                    _imageList = new ImageList(_component);
                    _imageList.ColorDepth = ColorDepth.Depth32Bit;
                    _imageList.ImageSize = _imageSize;

                    LargeImageList = SmallImageList = _imageList;

                    if (ContextMenuStrip == null)
                        ContextMenuStrip = new TextureContextMenuStrip();
                }
                return _imageList;
            }
        }


        public ResourceList()
            : base()
        {
            MultiSelect = false;
            HideSelection = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (_component != null)) { _component.Dispose(); _component = null; }
            base.Dispose(disposing);
        }

        internal protected virtual void OnResourceChanged(T resource)
        {
            if (ContextMenuStrip is TextureContextMenuStrip)
                ((TextureContextMenuStrip)ContextMenuStrip).TextureReference = (resource is TextureDefinition) ? (resource as TextureDefinition).Reference : null;

            if (ResourceChanged != null)
                ResourceChanged(resource);
        }

        protected virtual void OnSourceChanged(IListSource<T> source)
        {
            BeginUpdate();

            Items.Clear();

            if(_imageList != null)
            _imageList.Images.Clear();

            if (_currentSource != null)
            {
                Bitmap bmp;
                int index = 0;

                T[] items = _currentSource.ListItems;
                if (items != null)
                    foreach (T item in items)
                    {
                        item.ImageIndex = -1;
                        if (item is TextureDefinition)
                        {
                            if ((bmp = (item as TextureDefinition).Texture) != null)
                            {
                                ImageList.Images.Add(bmp);
                                item.ImageIndex = index++;
                            }
                        }
                        Items.Add(item);
                    }
            }
            else
                SelectedResource = null;

            EndUpdate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            SelectedResource = SelectedItems.Count == 0 ? null : SelectedItems[0] as T;
            base.OnSelectedIndexChanged(e);
        }
    }
}
