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

            foreach (MDL0MaterialReference r in mat.Children)
                _textureRefs.Add(new GLTextureRef(this, r));
        }

        public void Bind(GLContext context)
        {
            foreach (GLTextureRef r in _textureRefs)
            {
                r.Bind(context);
                break;
            }
        }
    }

    public unsafe class GLTexture
    {
        public string _name;
        public uint _texId;
        public Bitmap _bmp;

        public unsafe GLTexture(GLModel gLModel, MDL0Data10Node tex)
        {
            _name = tex.Name;
        }

        public void Bind(GLContext context)
        {
            if (_bmp == null)
                return;

            if (_texId == 0)
            {
                uint id = 0;
                context.glGenTextures(1, &id);
                _texId = id;

                context.glBindTexture(GLTextureBindTarget.Texture2D, id);


                if (_bmp != null)
                {
                    //Lock bitmap and transfer to texture object
                    BitmapData data = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    context.glTexImage2D(GLTextureBindTarget.Texture2D, 0, GLInternalPixelFormat.RGBA, _bmp.Width, _bmp.Height, 0, GLPixelDataFormat.BGRA, GLPixelDataType.UNSIGNED_BYTE, (void*)data.Scan0);

                    //_bmp.UnlockBits(data);
                }
            }
            else
            {
                context.glBindTexture(GLTextureBindTarget.Texture2D, _texId);

                //BitmapData data = _bmp.LockBits(new Rectangle(0, 0, _bmp.Width, _bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                //context.glDrawPixels(_bmp.Width, _bmp.Height, GLPixelDataFormat.BGRA, GLPixelDataType.UNSIGNED_BYTE, (void*)data.Scan0);
                //_bmp.UnlockBits(data);

                context.glBegin(GLPrimitiveType.Quads);

                context.glTexCoord(0.0f, 0.0f);
                context.glVertex(-10.0f, -10.0f);
                context.glTexCoord(0.0f, 1.0f);
                context.glVertex(10.0f, -10.0f);
                context.glTexCoord(1.0f, 1.0f);
                context.glVertex(10.0f, 10.0f);
                context.glTexCoord(1.0f, 0.0f);
                context.glVertex(-10.0f, 10.0f);

                context.glEnd();
            }
        }

        public void Unbind(GLContext context)
        {
            if (_texId != 0)
            {
                uint id = _texId;
                context.glDeleteTextures(1, &id);
                _texId = 0;
            }
        }

        internal void Attach(Bitmap bmp, string name)
        {
            if (name.Equals(_name))
                _bmp = bmp;
        }
    }

    public unsafe class GLTextureRef
    {
        public GLTexture _tex;
        public string _name;

        public GLTextureRef(GLMaterial mat, MDL0MaterialReference texRef)
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

        public void Bind(GLContext context)
        {
            if (_tex != null)
                _tex.Bind(context);
        }
    }
}
