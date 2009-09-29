using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;

namespace BrawlLib.Wii.Textures
{
    unsafe class CI4 : TextureFormat
    {
        public override int BitsPerPixel { get { return 4; } }
        public override int BlockWidth { get { return 8; } }
        public override int BlockHeight { get { return 8; } }
        public override PixelFormat DecodedFormat { get { return PixelFormat.Format4bppIndexed; } }
        public override WiiPixelFormat RawFormat { get { return WiiPixelFormat.CI4; } }

        protected override void DecodeBlock(VoidPtr blockAddr, VoidPtr destAddr, int width)
        {
            byte* sPtr = (byte*)blockAddr, dPtr = (byte*)destAddr;
            for (int y = 0; y < BlockHeight; y++, dPtr += (width / 2))
                for (int x = 0; x < (BlockWidth / 2); )
                    dPtr[x++] = *sPtr++;
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            byte* dPtr = (byte*)blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
                for (int x = 0; x < BlockWidth; )
                    *dPtr++ = (byte)((_workingPalette.FindMatch(sPtr[x++]) << 4) | (_workingPalette.FindMatch(sPtr[x++]) & 0x0F));
        }
    }
}
