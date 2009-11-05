using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.Drawing.Imaging;
using BrawlLib.Imaging;

namespace BrawlLib.OpenGL
{
    public unsafe class GLMaterial
    {
        public List<GLTextureRef> _textureRefs = new List<GLTextureRef>();
        public GLModel _model;

        public GLMaterial(GLModel model, MDL0MaterialNode mat)
        {
            _model = model;

            foreach (MDL0MaterialRefNode r in mat.Children)
                _textureRefs.Add(new GLTextureRef(this, r));
        }

        public void Bind(GLContext context, uint[] texIds)
        {
            for (int i = 0; i < _textureRefs.Count; i++)
            {
                texIds[i] = _textureRefs[i].Initialize(context);
            }
        }
    }

    public unsafe class GLTexture
    {
        public string _name;
        public uint _texId = 0;

        public int _lod = 1;
        private bool _remake = false;
        private Bitmap _texture;

        public unsafe GLTexture(GLModel gLModel, MDL0Data10Node tex)
        {
            _name = tex.Name;
        }

        public uint Initialize(GLContext context)
        {
            if (_remake)
            {
                _remake = false;
                if (_texture == null)
                    return 0;

                ClearTexture(context);

                uint id = 0;
                context.glGenTextures(1, &id);
                _texId = id;

                context.glBindTexture(GLTextureTarget.Texture2D, id);

                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureMagFilter.LINEAR);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureMinFilter.NEAREST_MIPMAP_LINEAR);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.BaseLevel, 0);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, _lod - 1);

                int w = _texture.Width, h = _texture.Height, size = w * h;
                BitmapData data = _texture.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                try
                {
                    using (UnsafeBuffer buffer = new UnsafeBuffer(size << 2))
                    {
                        ARGBPixel* sPtr = (ARGBPixel*)data.Scan0;
                        ABGRPixel* dPtr = (ABGRPixel*)buffer.Address;

                        for (int i = 0; i < size; i++)
                            *dPtr++ = (ABGRPixel)(*sPtr++);

                        context.gluBuild2DMipmaps(GLTextureTarget.Texture2D, GLInternalPixelFormat._4, w, h, GLPixelDataFormat.RGBA, GLPixelDataType.UNSIGNED_BYTE, buffer.Address);
                    }
                }
                finally { _texture.UnlockBits(data); }

                ClearImages();
            }
            return _texId;
        }

        private void ClearImages()
        {
            if (_texture != null)
            {
                _texture.Dispose();
                _texture = null;
            }
        }
        private void ClearTexture(GLContext context)
        {
            if (_texId != 0)
            {
                uint id = _texId;
                context.glDeleteTextures(1, &id);
                _texId = 0;
            }
        }

        public void Unbind(GLContext context)
        {
            ClearImages();
            ClearTexture(context);
        }

        //internal void Attach(Bitmap bmp, string name)
        //{
        //    if (name.Equals(_name))
        //    {
        //        _bmp = bmp;
        //        _remake = true;
        //    }
        //}

        internal unsafe void Attach(TEX0Node tex)
        {
            ClearImages();

            _lod = tex.LevelOfDetail;
            _texture = tex.GetImage(0);

            _remake = true;
        }
    }

    public unsafe class GLTextureRef
    {
        public GLTexture _tex;
        public string _name;

        public GLTextureRef(GLMaterial mat, MDL0MaterialRefNode texRef)
        {
            _name = texRef.Name;
            foreach (GLTexture tex in mat._model._textures)
            {
                if (tex._name.Equals(_name))
                {
                    _tex = tex;
                    break;
                }
            }
        }

        public uint Initialize(GLContext context)
        {
            return _tex != null ? _tex.Initialize(context) : 0;
        }
    }
}
