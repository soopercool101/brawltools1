using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.Drawing.Imaging;

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
        public uint _texId;
        public Bitmap _bmp;
        public bool _remake = true;

        public unsafe GLTexture(GLModel gLModel, MDL0Data10Node tex)
        {
            _name = tex.Name;
        }

        public uint Initialize(GLContext context)
        {
            if ((_remake) && (_bmp != null))
            {
                _remake = false;
                uint id = 0;

                if ((id = _texId) != 0)
                    context.glDeleteTextures(1, &id);
                context.glGenTextures(1, &id);
                _texId = id;

                context.glBindTexture(GLTextureTarget.Texture2D, id);

                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureMagFilter.LINEAR);
                context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureMinFilter.LINEAR_MIPMAP_LINEAR);

                if (_bmp != null)
                {
                    //Lock bitmap and transfer to texture object
                    BitmapData data = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                    context.gluBuild2DMipmaps(GLTextureTarget.Texture2D, (GLInternalPixelFormat)4, _bmp.Width, _bmp.Height, GLPixelDataFormat.BGRA, GLPixelDataType.UNSIGNED_BYTE, (void*)data.Scan0);
                    //context.glTexImage2D(GLTexImageTarget.Texture2D, 0, (GLInternalPixelFormat)4, _bmp.Width, _bmp.Height, 0, GLPixelDataFormat.BGRA, GLPixelDataType.UNSIGNED_BYTE, (void*)data.Scan0);

                    _bmp.UnlockBits(data);
                }
            }
            return _texId;
        }

        public void Unbind(GLContext context)
        {
            if (_texId != 0)
            {
                uint id = _texId;
                context.glDeleteTextures(1, &id);
                _texId = 0;
            }
            _bmp = null;
        }

        internal void Attach(Bitmap bmp, string name)
        {
            if (name.Equals(_name))
            {
                _bmp = bmp;
                _remake = true;
            }
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
