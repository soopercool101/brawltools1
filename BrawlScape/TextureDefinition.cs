using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Imaging;
using System.Drawing.Imaging;
using System.IO;

namespace BrawlScape
{
    public class TextureDefinition : ListViewItem//, IDisposable
    {
        private string _nodePath, _filePath;

        //private TEX0Node _node;
        public TEX0Node TextureNode
        {
            get { return ResourceCache.FindNode(_filePath, _nodePath) as TEX0Node; }
        }

        //~TextureDefinition() { Dispose(); }
        //public TextureDefinition(TEX0Node n)
        //{
        //    Text = n.Name;
        //    _node = n;
        //    _node.Changed += OnChanged;
        //}

        public TextureDefinition(string filePath, string nodePath)
        {
            _filePath = filePath;
            _nodePath = nodePath;
            TEX0Node tNode = TextureNode;
            Text = tNode.Name;
            tNode.Changed += OnChanged;
        }

        protected void OnChanged(ResourceNode n)
        {
            //Dispose();
            if (ListView != null)
            {
                ImageList list = ListView.LargeImageList;
                list.Images[this.ImageIndex] = new Bitmap(Texture, list.ImageSize.Width, list.ImageSize.Height);
                ListView.RedrawItems(this.Index, this.Index, false);
            }
        }


        //private Bitmap _texture;
        public Bitmap Texture { get { return TextureNode.GetImage(0); } }

        public void Replace()
        {
            TEX0Node tNode = TextureNode;

            string path;
            Bitmap bmp = null;
            switch (Program.OpenFile(Filters.TextureReplaceFilter, out path))
            {
                case 2:
                case 4:
                case 5:
                case 6:
                case 7:
                    bmp = Bitmap.FromFile(path) as Bitmap; break;

                case 3:
                    bmp = TGA.FromFile(path); break;

                case 8:
                    tNode.Replace(path); return;
                default: return;
            }

            try
            {
                if ((bmp.Width != tNode.Width) || (bmp.Height != tNode.Height))
                    MessageBox.Show(String.Format("Texture size does not match original! ({0} x {1})", tNode.Width, tNode.Height));
                else
                    tNode.Replace(bmp);
            }
            finally { bmp.Dispose(); }
        }

        public void Export()
        {
            string path;
            Bitmap bmp = null;
            switch (Program.SaveFile(Filters.TextureReplaceFilter, Text, out path))
            {
                case 2:
                    bmp = Texture; bmp.Save(path, ImageFormat.Png); break;
                case 3:
                    bmp = Texture; bmp.SaveTGA(path); break;
                case 4:
                    bmp = Texture; bmp.Save(path, ImageFormat.Tiff); break;
                case 5:
                    bmp = Texture; bmp.Save(path, ImageFormat.Bmp); break;
                case 6:
                    bmp = Texture; bmp.Save(path, ImageFormat.Jpeg); break;
                case 7:
                    bmp = Texture; bmp.Save(path, ImageFormat.Gif); break;
                case 8:
                    TextureNode.Export(path); break;
                default: return;
            }
            bmp.Dispose();
        }

        public void Restore()
        {
            TEX0Node tNode = TextureNode;
            PLT0Node pNode = tNode.GetPaletteNode();
            if (tNode.IsDirty)
            {
                if (pNode != null)
                    pNode.Restore();

                tNode.Restore();
            }
            else
            {
                ResourceNode n = tNode.RootNode;
                string path = n.FilePath;
                if (Program.IsWorkingPath(path))
                {
                    path = Program.GetDataPath(path);
                    if (!File.Exists(path))
                        MessageBox.Show(String.Format("File cannot be found!"));
                    else
                        using (ResourceNode oldNode = NodeFactory.FromFile(null, path))
                        {
                            ResourceNode orig = ResourceNode.FindNode(oldNode, tNode.TreePath, false);
                            if (pNode != null)
                            {
                                ResourceNode origp = ((TEX0Node)orig).GetPaletteNode();
                                pNode.ReplaceRaw(origp.WorkingSource.Address, origp.WorkingSource.Length);
                            }
                            tNode.ReplaceRaw(orig.WorkingSource.Address, orig.WorkingSource.Length);
                        }

                }
            }
        }

        //public void Dispose()
        //{
        //    if (_texture != null)
        //    {
        //        _texture.Dispose();
        //        _texture = null;
        //    }
        //}
    }
}
