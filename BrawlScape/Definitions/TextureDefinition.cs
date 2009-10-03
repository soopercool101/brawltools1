using System;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using BrawlLib.Imaging;
using System.Drawing.Imaging;
using System.IO;

namespace BrawlScape
{
    public class TextureDefinition : ResourceDefinition<TextureReference> //ListViewItem//, IDisposable
    {
        public TextureDefinition(string treePath, string nodePath) : base(treePath, nodePath) { }

        protected override void OnChanged(object sender, EventArgs e)
        {
            //Dispose();
            if (ListView != null)
            {
                ImageList list = ListView.LargeImageList;
                list.Images[this.ImageIndex] = new Bitmap(Texture, list.ImageSize.Width, list.ImageSize.Height);
                ListView.RedrawItems(this.Index, this.Index, false);
            }
        }

        //No disposing of this texture! The cache will do it.
        public virtual Bitmap Texture { get { return _nodeRef == null ? null : Reference.Texture; } }
    }
}
