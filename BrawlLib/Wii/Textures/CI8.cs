using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using BrawlLib.Imaging;

namespace BrawlLib.Wii.Textures
{
    unsafe class CI8 : TextureFormat
    {
        public override int BitsPerPixel { get { return 8; } }
        public override int BlockWidth { get { return 8; } }
        public override int BlockHeight { get { return 4; } }
        public override PixelFormat DecodedFormat { get { return PixelFormat.Format8bppIndexed; } }
        public override WiiPixelFormat RawFormat { get { return WiiPixelFormat.CI8; } }

        protected override void DecodeBlock(VoidPtr blockAddr, VoidPtr destAddr, int width)
        {
            byte* sPtr = (byte*)blockAddr, dPtr = (byte*)destAddr;
            for (int y = 0; y < BlockHeight; y++, dPtr += width)
                for (int x = 0; x < BlockWidth;x++ )
                    dPtr[x] = *sPtr++;
        }

        protected override void EncodeBlock(ARGBPixel* sPtr, VoidPtr blockAddr, int width)
        {
            byte* dPtr = (byte*)blockAddr;
            for (int y = 0; y < BlockHeight; y++, sPtr += width)
                for (int x = 0; x < BlockWidth; )
                    *dPtr++ = (byte)_workingPalette.FindMatch(sPtr[x++]);
        }
    }
}
