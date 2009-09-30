using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Drawing;
using System.Collections.Generic;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class TEX0Node : BRESEntryNode, IImageSource
    {
        public override ResourceType ResourceType { get { return ResourceType.TEX0; } }
        internal TEX0* Header { get { return (TEX0*)WorkingSource.Address; } }

        public override int DataAlign { get { return 0x20; } }

        int _width, _height;
        WiiPixelFormat _format;
        int _lod;
        bool _hasPalette;

        [Category("Texture")]
        public int Width { get { return _width; } set { _width = value; } }
        [Category("Texture")]
        public int Height { get { return _height; } set { _height = value; } }
        [Category("Texture")]
        public WiiPixelFormat Format { get { return _format; } set { _format = value; } }
        [Category("Texture")]
        public int LevelOfDetail { get { return _lod; } set { _lod = value; } }
        [Category("Texture")]
        public bool HasPalette { get { return _hasPalette; } set { _hasPalette = value; } }

        public PLT0Node GetPaletteNode() { return _parent == null ? null : _parent._parent.FindChild("Palettes(NW4R)/" + this.Name, false) as PLT0Node; }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _width = Header->_width;
            _height = Header->_height;
            _format = Header->PixelFormat;
            _lod = Header->_levelOfDetail;
            _hasPalette = Header->HasPalette;

            return false;
        }

        [Browsable(false)]
        public int ImageCount { get { return Header->_levelOfDetail; } }
        public Bitmap GetImage(int index)
        {
            Bitmap bmp = TextureFormat.Decode(Header, index + 1);
            if (HasPalette)
            {
                PLT0Node n = GetPaletteNode();
                if (n != null)
                    bmp.Palette = TextureFormat.DecodePalette(n.Header);
                else
                    bmp.GreyscalePalette();
            }
            return bmp;
        }

        protected internal override void OnAfterRebuild(IDictionary<string, VoidPtr> strings)
        {
            base.OnAfterRebuild(strings);
            Header->ResourceStringAddress = strings[Name];
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((TEX0*)address)->_header._tag == TEX0.Tag ? new TEX0Node() : null; }

        public void Replace(Bitmap bmp)
        {
            FileMap tMap, pMap;
            if (HasPalette)
            {
                PLT0Node pn = this.GetPaletteNode();
                tMap = TextureFormat.Get(Format).EncodeTextureIndexed(bmp, LevelOfDetail, pn.Colors, pn.Format, out pMap);
                pn.ReplaceRaw(pMap);
            }
            else
                tMap = TextureFormat.Get(Format).EncodeTexture(bmp, LevelOfDetail);
            ReplaceRaw(tMap);
        }
    }
}
