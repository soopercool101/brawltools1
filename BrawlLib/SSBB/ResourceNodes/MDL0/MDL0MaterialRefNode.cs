using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;
using System.Drawing;
using System.IO;
using BrawlLib.Imaging;
using System.Drawing.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialRefNode : ResourceNode
    {
        internal MDL0Data7Part3* Header { get { return (MDL0Data7Part3*)_origSource.Address; } }

        internal int _unk1;
        internal int _unk2;
        internal int _unk3;
        internal int _unk4;
        internal int _unk5;
        internal int _layerId1;
        internal int _layerId2;
        internal int _unk8;
        internal int _unk9;
        internal int _unk10;
        internal int _unk11;
        internal float _float;

        [Category("Texture Reference")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown5 { get { return _unk5; } set { _unk5 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int LayerId1 { get { return _layerId1; } set { _layerId1 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int LayerId2 { get { return _layerId2; } set { _layerId2 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown8 { get { return _unk8; } set { _unk8 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown9 { get { return _unk9; } set { _unk9 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public float Float { get { return _float; } set { _float = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown10 { get { return _unk10; } set { _unk10 = value; SignalPropertyChange(); } }
        [Category("Texture Reference")]
        public int Unknown11 { get { return _unk11; } set { _unk11 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;
            _unk5 = Header->_unk5;
            _layerId1 = Header->_layerId1;
            _layerId2 = Header->_layerId2;
            _unk8 = Header->_unk8;
            _unk9 = Header->_unk9;
            _float = Header->_float;
            _unk10 = Header->_unk10;
            _unk11 = Header->_unk11;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;
            return false;
        }

        internal unsafe void GetStrings(StringTable table)
        {
            table.Add(Name);
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Data7Part3* header = (MDL0Data7Part3*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            header->_unk1 = _unk1;
            header->_unk2 = _unk2;
            header->_unk3 = _unk3;
            header->_unk4 = _unk4;
            header->_unk5 = _unk5;
            header->_layerId1 = _layerId1;
            header->_layerId2 = _layerId2;
            header->_unk8 = _unk8;
            header->_unk9 = _unk9;
            header->_unk10 = _unk10;
            header->_unk11 = _unk11;
            header->_float = _float;
        }

        internal void Prepare(GLContext ctx)
        {
            GLTexture tex;
            string name = String.Format("{0}_TexObj", Name);

            if (ctx._states.ContainsKey(name))
            {
                tex = ctx._states[name] as GLTexture;
                ctx.glBindTexture(GLTextureTarget.Texture2D, tex._id);
            }
            else
            {
                uint texId;
                ctx.glGenTextures(1, &texId);
                tex = new GLTexture() { _id = texId };
                ctx._states[name] = tex;

                ctx.glBindTexture(GLTextureTarget.Texture2D, texId);

                Bitmap bmp = null;

                //Find texture
                TEX0Node tNode = RootNode.FindChild("Textures(NW4R)/" + Name, true) as TEX0Node;
                if (tNode != null)
                    bmp = tNode.GetImage(0);
                else
                {
                    //Search for texture in node path
                    string path = RootNode._origPath;
                    DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
                    foreach (FileInfo file in dir.GetFiles(Name + ".*"))
                    {
                        if (file.Name.EndsWith(".tga"))
                        {
                            bmp = TGA.FromFile(file.FullName);
                            break;
                        }
                        else if (file.Name.EndsWith(".png") || file.Name.EndsWith(".tiff") || file.Name.EndsWith(".tif"))
                        {
                            bmp = (Bitmap)Bitmap.FromFile(file.FullName);
                            break;
                        }
                    }
                }

                if (bmp != null)
                {
                    ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MagFilter, (int)GLTextureMagFilter.LINEAR);
                    ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MinFilter, (int)GLTextureMinFilter.NEAREST_MIPMAP_LINEAR);
                    ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.BaseLevel, 0);

                    if (tNode != null)
                        ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, tNode.LevelOfDetail);
                    else
                        ctx.glTexParameter(GLTextureTarget.Texture2D, GLTextureParameter.MaxLevel, 0);


                    int w = bmp.Width, h = bmp.Height, size = w * h;
                    BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    try
                    {
                        using (UnsafeBuffer buffer = new UnsafeBuffer(size << 2))
                        {
                            ARGBPixel* sPtr = (ARGBPixel*)data.Scan0;
                            ABGRPixel* dPtr = (ABGRPixel*)buffer.Address;

                            for (int i = 0; i < size; i++)
                                *dPtr++ = (ABGRPixel)(*sPtr++);

                            ctx.gluBuild2DMipmaps(GLTextureTarget.Texture2D, GLInternalPixelFormat._4, w, h, GLPixelDataFormat.RGBA, GLPixelDataType.UNSIGNED_BYTE, buffer.Address);
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(data);
                        bmp.Dispose();
                    }
                }
                else
                {
                }
            }
        }
    }
}
