using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace BrawlScape
{
    public class TextureReference : NodeReference
    {
        private Bitmap _texture;
        public Bitmap Texture
        {
            get
            {
                if (_texture == null)
                    try { _texture = ((TEX0Node)Node).GetImage(0); }
                    catch { }
                return _texture;
            }
        }

        protected override void OnDisposed(ResourceNode node)
        {
            base.OnDisposed(node);
            if (_texture != null) { _texture.Dispose(); _texture = null; }
        }

        protected override void OnChanged(ResourceNode node)
        {
            if (_texture != null) { _texture.Dispose(); _texture = null; }
            base.OnChanged(node);
        }


        public void Replace()
        {
            TEX0Node tNode = Node as TEX0Node;

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
            switch (Program.SaveFile(Filters.TextureReplaceFilter, Name, out path))
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
                    ((TEX0Node)Node).Export(path); break;
                default: return;
            }
            bmp.Dispose();
        }

        public void Restore()
        {
            TEX0Node tNode = Node as TEX0Node;
            PLT0Node pNode = tNode.GetPaletteNode();
            if (tNode.IsDirty)
            {
                if (pNode != null)
                    pNode.Restore();

                tNode.Restore();
            }
            else
            {
                ResourceTree t = Tree;
                if (t.IsWorkingCopy)
                {
                    string path = Program.GetFilePath(_relativePath, false);
                    using (ResourceNode oldNode = NodeFactory.FromFile(null, path))
                    {
                        ResourceNode origNode = oldNode.FindChild(_nodePath, false);
                        if (pNode != null)
                        {
                            ResourceNode origp = ((TEX0Node)origNode).GetPaletteNode();
                            pNode.ReplaceRaw(origp.WorkingSource.Address, origp.WorkingSource.Length);
                        }
                        tNode.ReplaceRaw(origNode.WorkingSource.Address, origNode.WorkingSource.Length);
                    }
                }
            }
        }
    }
}
