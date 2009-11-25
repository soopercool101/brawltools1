using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.Imaging;
using System.Drawing.Imaging;

namespace BrawlLib.Modeling
{
    public class TextureRef
    {
        public string Name;
        public GLTexture Texture;
        public bool Reset;
        public bool Selected;
        public bool Enabled = true;
        public object Source;

        private GLContext _context;

        public TextureRef() { }
        public TextureRef(string name) { Name = name; }

        public override string ToString() { return Name; }

        internal unsafe void Prepare(GLContext ctx)
        {
            if (_context == null)
                _context = ctx;

            if (Texture != null)
                Texture.Bind();
            else
                Load();

            float* p = stackalloc float[4];
            p[0] = p[1] = p[2] = p[3] = 1.0f;
            if (Selected)
                p[0] = -1.0f;

            ctx.glLight(GLLightTarget.Light0, GLLightParameter.SPECULAR, p);
            ctx.glLight(GLLightTarget.Light0, GLLightParameter.DIFFUSE, p);
        }

        public void Reload()
        {
            if (_context == null)
                return;

            _context.Capture();
            Load();
        }

        private unsafe void Load()
        {
            if (_context == null)
                return;

            if (Texture != null)
                Texture.Delete();
            Texture = new GLTexture(_context, 0, 0);
            Texture.Bind();

            //ctx._states[String.Format("{0}_TexRef", Name)] = Texture;

            Bitmap bmp = null;
            TEX0Node tNode = null;

            if (_context._states.ContainsKey("_Node_Refs"))
            {
                List<ResourceNode> nodes = _context._states["_Node_Refs"] as List<ResourceNode>;
                foreach (ResourceNode node in nodes)
                {
                    //Search node itself first
                    if ((tNode = node.RootNode.FindChild("Textures(NW4R)/" + Name, true) as TEX0Node) != null)
                    {
                        Source = tNode;
                        bmp = tNode.GetImage(0);
                    }
                    else
                    {
                        //Then search node directory
                        string path = node.RootNode._origPath;
                        DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
                        foreach (FileInfo file in dir.GetFiles(Name + ".*"))
                        {
                            if (file.Name.EndsWith(".tga"))
                            {
                                Source = file.FullName;
                                bmp = TGA.FromFile(file.FullName);
                                break;
                            }
                            else if (file.Name.EndsWith(".png") || file.Name.EndsWith(".tiff") || file.Name.EndsWith(".tif"))
                            {
                                Source = file.FullName;
                                bmp = (Bitmap)Bitmap.FromFile(file.FullName);
                                break;
                            }
                        }
                    }
                    if (bmp != null)
                        break;
                }

                if (bmp != null)
                {
                    int w = bmp.Width, h = bmp.Height, size = w * h;

                    Texture._width = w;
                    Texture._height = h;
                    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureMagFilter.LINEAR);
                    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureMinFilter.NEAREST_MIPMAP_LINEAR);
                    //_context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.BaseLevel, 0);

                    //if (tNode != null)
                    //    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, tNode.LevelOfDetail);
                    //else
                    //    _context.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, 0);

                    BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    try
                    {
                        using (UnsafeBuffer buffer = new UnsafeBuffer(size << 2))
                        {
                            ARGBPixel* sPtr = (ARGBPixel*)data.Scan0;
                            ABGRPixel* dPtr = (ABGRPixel*)buffer.Address;

                            for (int i = 0; i < size; i++)
                                *dPtr++ = (ABGRPixel)(*sPtr++);

                            _context.gluBuild2DMipmaps(GLTextureTarget.Texture2D, GLInternalPixelFormat._4, w, h, GLPixelDataFormat.RGBA, GLPixelDataType.UNSIGNED_BYTE, buffer.Address);
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(data);
                        bmp.Dispose();
                    }
                }
            }
        }

        internal void Bind(GLContext ctx)
        {
            Unbind();

            _context = ctx;

            Selected = false;
            Enabled = true;
        }
        internal void Unbind()
        {
            if (Texture != null) { Texture.Delete(); Texture = null; }
            _context = null;
        }
    }
}
