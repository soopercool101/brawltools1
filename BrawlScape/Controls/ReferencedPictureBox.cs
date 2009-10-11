using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlScape
{
    class ReferencedPictureBox : PictureBox
    {
        private TextureContextMenuStrip _context;
        private TextureReference _texRef;
        [Browsable(false)]
        public TextureReference Reference
        {
            get { return _texRef; }
            set
            {
                if (_texRef != null)
                {
                    _texRef.Disposed -= ResetImage;
                    _texRef.DataChanged -= ResetImage;
                }

                _texRef = value;
                Image = null;

                if (_texRef != null)
                {
                    _texRef.Disposed += ClearImage;
                    _texRef.DataChanged += ResetImage;
                    Image = _texRef.Texture;
                }

                _context.TextureReference = _texRef;
            }
        }

        private void ClearImage(NodeReference r) { Image = null; }
        private void ResetImage(NodeReference r) { Image = _texRef == null ? null : _texRef.Texture; }

        public ReferencedPictureBox()
            : base()
        { 
            ContextMenuStrip = _context = new TextureContextMenuStrip();
        }
    }
}
