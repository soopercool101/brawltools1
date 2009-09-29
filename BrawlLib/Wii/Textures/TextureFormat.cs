using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;
using BrawlLib.SSBBTypes;
using BrawlLib.Imaging;
using BrawlLib.IO;

namespace BrawlLib.Wii.Textures
{
    public unsafe abstract class TextureFormat
    {
        public static readonly TextureFormat I4 = new I4();
        public static readonly TextureFormat IA4 = new IA4();
        public static readonly TextureFormat I8 = new I8();
        public static readonly TextureFormat IA8 = new IA8();
        public static readonly TextureFormat RGB565 = new RGB565();
        public static readonly TextureFormat RGB5A3 = new RGB5A3();
        public static readonly TextureFormat CI4 = new CI4();
        public static readonly TextureFormat CI8 = new CI8();
        public static readonly TextureFormat CMPR = new CMP();
        public static readonly TextureFormat RGBA8 = new RGBA8();

        public abstract WiiPixelFormat RawFormat { get; }
        public abstract PixelFormat DecodedFormat { get; }
        public abstract int BitsPerPixel { get; }
        public abstract int BlockWidth { get; }
        public abstract int BlockHeight { get; }
        public bool IsIndexed { get { return ((RawFormat == WiiPixelFormat.CI4) || (RawFormat == WiiPixelFormat.CI8)); } }

        //protected static ColorPalette _cachedPalette;
        //protected static WiiPaletteFormat _paletteFormat;

        public int GetMipOffset(int width, int height, int mipLevel) { return GetMipOffset(ref width, ref height, mipLevel); }
        public int GetMipOffset(ref int width, ref int height, int mipLevel)
        {
            int offset = 0;
            while (mipLevel-- > 1)
            {
                offset += ((width.Align(BlockWidth) * height.Align(BlockHeight)) * BitsPerPixel) / 8;
                width = Math.Max(width / 2, 1);
                height = Math.Max(height / 2, 1);
            }
            return offset;
        }

        //public virtual void GeneratePreviewIndexed(Bitmap src, Bitmap dst, int numColors, WiiPaletteFormat format)
        //{
        //    _cachedPalette = src.GeneratePalette(QuantizationAlgorithm.WeightedAverage, numColors);
        //    _paletteFormat = format;
        //    _cachedPalette.Clamp(format);

        //    src.CopyTo(dst);
        //    dst.Clamp(_cachedPalette);
        //}

        public virtual void GeneratePreview(Bitmap src, Bitmap dst)
        {
            src.CopyTo(dst);
            dst.Clamp(RawFormat);
        }

        protected ColorPalette _workingPalette;
        public virtual FileMap EncodeTextureIndexed(Bitmap src, int mipLevels, int numColors, WiiPaletteFormat format, out FileMap paletteFile)
        {
            _workingPalette = src.GeneratePalette(QuantizationAlgorithm.WeightedAverage, numColors);
            _workingPalette.Clamp(format);
            FileMap map = EncodeTexture(src, mipLevels);
            paletteFile = EncodePalette(_workingPalette, format);
            _workingPalette = null;
            return map;
        }
        public virtual FileMap EncodeTexture(Bitmap src, int mipLevels)
        {
            //paletteFile = null;
            int w = src.Width, h = src.Height;
            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);
            int bw = BlockWidth, bh = BlockHeight;

            int fileSize = GetMipOffset(w, h, mipLevels + 1) + 0x40;
            //paletteFile = null;
            FileMap fileView = FileMap.FromTempFile(fileSize);
            try
            {
                //Build TEX header
                TEX0* header = (TEX0*)fileView.Address;
                *header = new TEX0(w, h, RawFormat, mipLevels);

                int sStep = bw * Image.GetPixelFormatSize(PixelFormat.Format32bppArgb) / 8;
                int dStep = bw * bh * BitsPerPixel / 8;
                VoidPtr baseAddr = header->PixelData;
                using (DIB dib = DIB.FromBitmap(src, bw, bh, PixelFormat.Format32bppArgb))
                {
                    for (int i = 1; i <= mipLevels; i++)
                    {
                        int mw = w, mh = h;
                        VoidPtr dstAddr = baseAddr;
                        if (i != 1)
                        {
                            dstAddr += GetMipOffset(ref mw, ref mh, i);
                            using (Bitmap mip = src.GenerateMip((int)i))
                            {
                                dib.ReadBitmap(mip, mw, mh);
                            }
                        }

                        mw = mw.Align(bw);
                        mh = mh.Align(bh);

                        int bStride = mw * BitsPerPixel / 8;
                        for (int y = 0; y < mh; y += bh)
                        {
                            VoidPtr sPtr = (int)dib.Scan0 + (y * dib.Stride);
                            VoidPtr dPtr = dstAddr + (y * bStride);
                            for (int x = 0; x < mw; x += bw, dPtr += dStep, sPtr += sStep)
                                EncodeBlock((ARGBPixel*)sPtr, dPtr, aw);
                        }
                    }
                }
                return fileView;
            }
            catch (Exception x)
            {
                //MessageBox.Show(x.ToString());
                fileView.Dispose();
                return null;
            }
        }
        protected abstract void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width);

        public Bitmap DecodeTexture(TEX0* texture) { return DecodeTexture(texture, 1); }
        public virtual Bitmap DecodeTexture(TEX0* texture, int mipLevel)
        {
            int w = (int)(ushort)texture->_width, h = (int)(ushort)texture->_height;
            VoidPtr addr = texture->PixelData + GetMipOffset(ref w, ref h, mipLevel);
            int aw = w.Align(BlockWidth), ah = h.Align(BlockHeight);

            using (DIB dib = new DIB(w, h, BlockWidth, BlockHeight, DecodedFormat))
            {
                int dStep = BlockWidth * Image.GetPixelFormatSize(DecodedFormat) / 8;
                int sStep = BlockWidth * BlockHeight * BitsPerPixel / 8;
                int bStride = aw * BitsPerPixel / 8;
                for (int y = 0; y < ah; y += BlockHeight)
                {
                    VoidPtr dPtr = (int)dib.Scan0 + (y * dib.Stride);
                    VoidPtr sPtr = addr + (y * bStride);
                    for (int x = 0; x < aw; x += BlockWidth, dPtr += dStep, sPtr += sStep)
                        DecodeBlock(sPtr, dPtr, aw);
                }
                return dib.ToBitmap();
            }
        }

        protected abstract void DecodeBlock(VoidPtr blockAddr, VoidPtr destAddr, int width);

        public static TextureFormat Get(WiiPixelFormat format)
        {
            switch (format)
            {
                case WiiPixelFormat.I4: return I4;
                case WiiPixelFormat.IA4: return IA4;
                case WiiPixelFormat.I8: return I8;
                case WiiPixelFormat.IA8: return IA8;
                case WiiPixelFormat.RGB565: return RGB565;
                case WiiPixelFormat.RGB5A3: return RGB5A3;
                case WiiPixelFormat.CI4: return CI4;
                case WiiPixelFormat.CI8: return CI8;
                case WiiPixelFormat.CMP: return CMPR;
                case WiiPixelFormat.RGBA8: return RGBA8;
            }
            return null;
        }

        public static Bitmap Decode(TEX0* texture, int mipLevel) { return Get(texture->PixelFormat).DecodeTexture(texture, mipLevel); }

        public static FileMap EncodePalette(ColorPalette pal, WiiPaletteFormat format)
        {
            int count = pal.Entries.Length;
            int viewLen = (pal.Entries.Length * 2) + 0x40;
            FileMap fileView = FileMap.FromTempFile(viewLen);
            try
            {
                PLT0* header = (PLT0*)fileView.Address;
                *header = new PLT0(count, format);

                switch (format)
                {
                    case WiiPaletteFormat.IA8:
                        {
                            IA8Pixel* dPtr = (IA8Pixel*)header->PaletteData;
                            for (int i = 0; i < count; i++)
                                dPtr[i] = (IA8Pixel)pal.Entries[i];
                            break;
                        }
                    case WiiPaletteFormat.RGB565:
                        {
                            wRGB565Pixel* dPtr = (wRGB565Pixel*)header->PaletteData;
                            for (int i = 0; i < count; i++)
                                dPtr[i] = (wRGB565Pixel)pal.Entries[i];
                            break;
                        }
                    case WiiPaletteFormat.RGB5A3:
                        {
                            wRGB5A3Pixel* dPtr = (wRGB5A3Pixel*)header->PaletteData;
                            for (int i = 0; i < count; i++)
                                dPtr[i] = (wRGB5A3Pixel)pal.Entries[i];
                            break;
                        }
                }

                return fileView;
            }
            catch (Exception x)
            {
                fileView.Dispose();
                throw x;
                //MessageBox.Show(x.ToString());
                //fileView.Dispose();
                //return null;
            }
        }

        public static ColorPalette DecodePalette(PLT0* palette)
        {
            int count = palette->_numEntries;
            ColorPalette pal = ColorPaletteExtension.CreatePalette(ColorPaletteFlags.HasAlpha, count);
            switch (palette->PaletteFormat)
            {
                case WiiPaletteFormat.IA8:
                    {
                        IA8Pixel* sPtr = (IA8Pixel*)palette->PaletteData;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)sPtr[i];
                        break;
                    }
                case WiiPaletteFormat.RGB565:
                    {
                        wRGB565Pixel* sPtr = (wRGB565Pixel*)palette->PaletteData;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)sPtr[i];
                        break;
                    }
                case WiiPaletteFormat.RGB5A3:
                    {
                        wRGB5A3Pixel* sPtr = (wRGB5A3Pixel*)palette->PaletteData;
                        for (int i = 0; i < count; i++)
                            pal.Entries[i] = (Color)(ARGBPixel)sPtr[i];
                        break;
                    }
            }
            return pal;
        }
    }
}
