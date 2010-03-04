using System;
using BrawlLib.Imaging;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Textures;

namespace BrawlLib.Wii.Models
{
    public unsafe delegate void ColorConverter(ref byte* pIn, ref byte* pOut);
    public unsafe class ColorCodec
    {
        #region Encoders

        private static void Color_RGBA_wRGB565(ref byte* pIn, ref byte* pOut)
        {
            int data = *pIn++ >> 3;
            data |= (*pIn++ >> 2) << 5;
            data |= (*pIn++ >> 3) << 11;

            byte* p = (byte*)&data;
            *pOut++ = p[1];
            *pOut++ = p[0];
        }
        private static void Color_RGBA_RGB(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            pIn++;
        }
        private static void Color_RGBA_RGBX(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = 0;
            pIn++;
        }
        private static void Color_RGBA_wRGBA4(ref byte* pIn, ref byte* pOut)
        {
            int data = 0;
            byte* p = (byte*)data;

            int i = 16;
            while ((i -= 4) >= 0)
                data |= (*pIn++ >> 4) << i;

            *pOut++ = p[1];
            *pOut++ = p[0];
        }
        private static void Color_RGBA_wRGBA6(ref byte* pIn, ref byte* pOut)
        {
            int data = 0;
            byte* p = (byte*)data;

            int i = 24;
            while ((i -= 6) >= 0)
                data |= (*pIn++ >> 2) << i;

            *pOut++ = p[2];
            *pOut++ = p[1];
            *pOut++ = p[0];
        }

        #endregion

        #region Decoders

        private static void Color_wRGB565_RGBA(ref byte* pIn, ref byte* pOut)
        {
            int val, data = *(bushort*)pIn;
            pIn += 2;

            val = data & 0xF800;
            *pOut++ = (byte)((val >> 8) | (val >> 13));
            val = data & 0x7E0;
            *pOut++ = (byte)((val >> 3) | (val >> 9));
            val = data & 0x1F;
            *pOut++ = (byte)((val << 3) | (val >> 2));
            *pOut++ = 255;
        }
        private static void Color_RGB_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = 255;
        }
        private static void Color_RGBX_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = 255;
            pIn++;
        }
        private static void Color_wRGBA4_RGBA(ref byte* pIn, ref byte* pOut)
        {
            int val, data = *(bushort*)pIn;
            pIn += 2;

            int i = 4;
            while (i-- > 0)
            {
                val = (data >> (i << 2)) & 0xF;
                *pOut++ = (byte)(val | (val << 4));
            }
        }
        private static void Color_wRGBA6_RGBA(ref byte* pIn, ref byte* pOut)
        {
            int val, data;
            byte* t = (byte*)&data + 3;
            *t-- = *pIn++;
            *t-- = *pIn++;
            *t-- = *pIn++;
            *t-- = 0;

            int i = 24;
            while ((i -= 6) >= 0)
            {
                val = (data >> i) & 0x3F;
                *pOut++ = (byte)((val << 2) | (val >> 4));
            }
        }
        private static void Color_RGBA_RGBA(ref byte* pIn, ref byte* pOut)
        {
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
            *pOut++ = *pIn++;
        }

        #endregion

        public WiiColorComponentType _outType;
        public bool _hasAlpha;
        public int _dataLen;
        public int _entries;
        public int _outStride;
        public int _outLength;

        public static ColorCodec Evaluate(RGBAPixel[] pixels)
        {
            ColorCodec codec = new ColorCodec();
            bool hasAlpha = false;
            int count = pixels.Length;
            byte* pData, pCeil, sPtr;

            codec._entries = count;

            //Do we have alpha?
            fixed (RGBAPixel* p = pixels)
            {
                pData = (byte*)p;
                pCeil = (byte*)p + (count << 2);

                for (sPtr = pData + 3; sPtr < pCeil; sPtr += 4)
                    if (*sPtr != 255)
                    { codec._hasAlpha = hasAlpha = true; break; }

                //Determine format
                if (hasAlpha)
                    codec._outType = WiiColorComponentType.RGBA8;
                else
                    codec._outType = WiiColorComponentType.RGB8;
            }

            switch (codec._outType)
            {
                case WiiColorComponentType.RGB565:
                case WiiColorComponentType.RGBA4:
                    codec._outStride = 2; break;
                case WiiColorComponentType.RGB8:
                case WiiColorComponentType.RGBA6:
                    codec._outStride = 3; break;
                case WiiColorComponentType.RGBA8:
                case WiiColorComponentType.RGBX8:
                    codec._outStride = 4; break;
            }

            codec._outLength = codec._outStride * count;
            return codec;
        }

        public void Write(RGBAPixel[] colors, byte* pOut)
        {
            fixed (RGBAPixel* p = colors)
                Write((byte*)p, pOut);
        }

        private void Write(byte* pIn, byte* pOut)
        {
            ColorConverter enc;
            byte* sPtr = pIn;
            int i = 0, ceil = _outLength.Align(0x20);

            //Get encoder
            switch (_outType)
            {
                case WiiColorComponentType.RGB565: enc = Color_RGBA_wRGB565; break;
                case WiiColorComponentType.RGB8: enc = Color_RGBA_RGB; break;
                case WiiColorComponentType.RGBX8: enc = Color_RGBA_RGBX; break;
                case WiiColorComponentType.RGBA4: enc = Color_RGBA_wRGBA4; break;
                case WiiColorComponentType.RGBA6: enc = Color_RGBA_wRGBA6; break;
                case WiiColorComponentType.RGBA8: enc = Color_RGBA_RGBA; break;
                default: return;
            }

            //Write data using encoder
            while (i++ < _entries)
                enc(ref sPtr, ref pOut);

            //Zero-fill
            while (i++ < ceil)
                *pOut++ = 0;
        }


        public static UnsafeBuffer Decode(MDL0ColorData* header)
        {
            int count = header->_numEntries;
            UnsafeBuffer buffer = new UnsafeBuffer(count * 4);
            byte* pIn = (byte*)header + header->_dataOffset;
            byte* pOut = (byte*)buffer.Address;

            ColorConverter dec;
            switch (header->Type)
            {
                case WiiColorComponentType.RGB565: dec = Color_wRGB565_RGBA; break;
                case WiiColorComponentType.RGB8: dec = Color_RGB_RGBA; break;
                case WiiColorComponentType.RGBA4: dec = Color_wRGBA4_RGBA; break;
                case WiiColorComponentType.RGBA6: dec = Color_wRGBA6_RGBA; break;
                case WiiColorComponentType.RGBA8: dec = Color_RGBA_RGBA; break;
                case WiiColorComponentType.RGBX8: dec = Color_RGBX_RGBA; break;
                default: return null;
            }

            while (count-- > 0)
                dec(ref pIn, ref pOut);

            return buffer;
        }
    }
}
