using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace BrawlScape
{
    class ReferencedPictureBox : PictureBox
    {
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
            }
        }

        private void ClearImage(object s, EventArgs e) { Image = null; }
        private void ResetImage(object s, EventArgs e) { Image = _texRef == null ? null : _texRef.Texture; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            MainForm._currentTextureNode = _texRef;
            base.OnMouseDown(e);
        }

        protected override void OnCreateControl() { ContextMenuStrip = MainForm._textureContext; }
    }
}
