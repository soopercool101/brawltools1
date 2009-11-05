using System;
using BrawlLib.Wii.Textures;
using System.Runtime.InteropServices;

namespace BrawlLib.Imaging
{
    public static unsafe class Squish
    {
        public static CMPRBlock compressDXT1a(ARGBPixel* image, int imgX, int imgY, int imgW, int imgH)
        {
            ColorBlock rgba;
            ColourSet set;
            CMPRBlock block;

            //squish::WeightedClusterFit fit;
            //fit.SetMetric(compressionOptions.colorWeight.x(), compressionOptions.colorWeight.y(), compressionOptions.colorWeight.z());

            //for (int y = 0; y < imgH; y += 4)
            //{
            //    for (int x = 0; x < imgW; x += 4)
            //    {

            rgba = new ColorBlock(image, imgX, imgY, imgW, imgH);


            if (!rgba._anyAlpha && rgba._singleColor || rgba._allAlpha)
            {
                return optimalCompressDXT1a(*(ARGBPixel*)&rgba);
                //Optimal compress
            }
            else
            {
                if (rgba._anyAlpha)
                {
                    set = new ColourSet((ARGBPixel*)(&rgba), true);
                    WCF wcf = new WCF(&set);
                    wcf.Compress(&block);
                }
                else
                {
                    set = new ColourSet((ARGBPixel*)(&rgba), false);
                    FCF fcf = new FCF(&set);
                    fcf.Compress(&block);
                }
            }

            return block;
        }

        #region OptTables
        static readonly byte[,] OMatch5 = new byte[256, 2]
{
	{0x00, 0x00},
	{0x00, 0x00},
	{0x00, 0x01},
	{0x00, 0x01},
	{0x01, 0x00},
	{0x01, 0x00},
	{0x01, 0x00},
	{0x01, 0x01},
	{0x01, 0x01},
	{0x01, 0x01},
	{0x01, 0x02},
	{0x00, 0x04},
	{0x02, 0x01},
	{0x02, 0x01},
	{0x02, 0x01},
	{0x02, 0x02},
	{0x02, 0x02},
	{0x02, 0x02},
	{0x02, 0x03},
	{0x01, 0x05},
	{0x03, 0x02},
	{0x03, 0x02},
	{0x04, 0x00},
	{0x03, 0x03},
	{0x03, 0x03},
	{0x03, 0x03},
	{0x03, 0x04},
	{0x03, 0x04},
	{0x03, 0x04},
	{0x03, 0x05},
	{0x04, 0x03},
	{0x04, 0x03},
	{0x05, 0x02},
	{0x04, 0x04},
	{0x04, 0x04},
	{0x04, 0x05},
	{0x04, 0x05},
	{0x05, 0x04},
	{0x05, 0x04},
	{0x05, 0x04},
	{0x06, 0x03},
	{0x05, 0x05},
	{0x05, 0x05},
	{0x05, 0x06},
	{0x04, 0x08},
	{0x06, 0x05},
	{0x06, 0x05},
	{0x06, 0x05},
	{0x06, 0x06},
	{0x06, 0x06},
	{0x06, 0x06},
	{0x06, 0x07},
	{0x05, 0x09},
	{0x07, 0x06},
	{0x07, 0x06},
	{0x08, 0x04},
	{0x07, 0x07},
	{0x07, 0x07},
	{0x07, 0x07},
	{0x07, 0x08},
	{0x07, 0x08},
	{0x07, 0x08},
	{0x07, 0x09},
	{0x08, 0x07},
	{0x08, 0x07},
	{0x09, 0x06},
	{0x08, 0x08},
	{0x08, 0x08},
	{0x08, 0x09},
	{0x08, 0x09},
	{0x09, 0x08},
	{0x09, 0x08},
	{0x09, 0x08},
	{0x0A, 0x07},
	{0x09, 0x09},
	{0x09, 0x09},
	{0x09, 0x0A},
	{0x08, 0x0C},
	{0x0A, 0x09},
	{0x0A, 0x09},
	{0x0A, 0x09},
	{0x0A, 0x0A},
	{0x0A, 0x0A},
	{0x0A, 0x0A},
	{0x0A, 0x0B},
	{0x09, 0x0D},
	{0x0B, 0x0A},
	{0x0B, 0x0A},
	{0x0C, 0x08},
	{0x0B, 0x0B},
	{0x0B, 0x0B},
	{0x0B, 0x0B},
	{0x0B, 0x0C},
	{0x0B, 0x0C},
	{0x0B, 0x0C},
	{0x0B, 0x0D},
	{0x0C, 0x0B},
	{0x0C, 0x0B},
	{0x0D, 0x0A},
	{0x0C, 0x0C},
	{0x0C, 0x0C},
	{0x0C, 0x0D},
	{0x0C, 0x0D},
	{0x0D, 0x0C},
	{0x0D, 0x0C},
	{0x0D, 0x0C},
	{0x0E, 0x0B},
	{0x0D, 0x0D},
	{0x0D, 0x0D},
	{0x0D, 0x0E},
	{0x0C, 0x10},
	{0x0E, 0x0D},
	{0x0E, 0x0D},
	{0x0E, 0x0D},
	{0x0E, 0x0E},
	{0x0E, 0x0E},
	{0x0E, 0x0E},
	{0x0E, 0x0F},
	{0x0D, 0x11},
	{0x0F, 0x0E},
	{0x0F, 0x0E},
	{0x10, 0x0C},
	{0x0F, 0x0F},
	{0x0F, 0x0F},
	{0x0F, 0x0F},
	{0x0F, 0x10},
	{0x0F, 0x10},
	{0x0F, 0x10},
	{0x0F, 0x11},
	{0x10, 0x0F},
	{0x10, 0x0F},
	{0x11, 0x0E},
	{0x10, 0x10},
	{0x10, 0x10},
	{0x10, 0x11},
	{0x10, 0x11},
	{0x11, 0x10},
	{0x11, 0x10},
	{0x11, 0x10},
	{0x12, 0x0F},
	{0x11, 0x11},
	{0x11, 0x11},
	{0x11, 0x12},
	{0x10, 0x14},
	{0x12, 0x11},
	{0x12, 0x11},
	{0x12, 0x11},
	{0x12, 0x12},
	{0x12, 0x12},
	{0x12, 0x12},
	{0x12, 0x13},
	{0x11, 0x15},
	{0x13, 0x12},
	{0x13, 0x12},
	{0x14, 0x10},
	{0x13, 0x13},
	{0x13, 0x13},
	{0x13, 0x13},
	{0x13, 0x14},
	{0x13, 0x14},
	{0x13, 0x14},
	{0x13, 0x15},
	{0x14, 0x13},
	{0x14, 0x13},
	{0x15, 0x12},
	{0x14, 0x14},
	{0x14, 0x14},
	{0x14, 0x15},
	{0x14, 0x15},
	{0x15, 0x14},
	{0x15, 0x14},
	{0x15, 0x14},
	{0x16, 0x13},
	{0x15, 0x15},
	{0x15, 0x15},
	{0x15, 0x16},
	{0x14, 0x18},
	{0x16, 0x15},
	{0x16, 0x15},
	{0x16, 0x15},
	{0x16, 0x16},
	{0x16, 0x16},
	{0x16, 0x16},
	{0x16, 0x17},
	{0x15, 0x19},
	{0x17, 0x16},
	{0x17, 0x16},
	{0x18, 0x14},
	{0x17, 0x17},
	{0x17, 0x17},
	{0x17, 0x17},
	{0x17, 0x18},
	{0x17, 0x18},
	{0x17, 0x18},
	{0x17, 0x19},
	{0x18, 0x17},
	{0x18, 0x17},
	{0x19, 0x16},
	{0x18, 0x18},
	{0x18, 0x18},
	{0x18, 0x19},
	{0x18, 0x19},
	{0x19, 0x18},
	{0x19, 0x18},
	{0x19, 0x18},
	{0x1A, 0x17},
	{0x19, 0x19},
	{0x19, 0x19},
	{0x19, 0x1A},
	{0x18, 0x1C},
	{0x1A, 0x19},
	{0x1A, 0x19},
	{0x1A, 0x19},
	{0x1A, 0x1A},
	{0x1A, 0x1A},
	{0x1A, 0x1A},
	{0x1A, 0x1B},
	{0x19, 0x1D},
	{0x1B, 0x1A},
	{0x1B, 0x1A},
	{0x1C, 0x18},
	{0x1B, 0x1B},
	{0x1B, 0x1B},
	{0x1B, 0x1B},
	{0x1B, 0x1C},
	{0x1B, 0x1C},
	{0x1B, 0x1C},
	{0x1B, 0x1D},
	{0x1C, 0x1B},
	{0x1C, 0x1B},
	{0x1D, 0x1A},
	{0x1C, 0x1C},
	{0x1C, 0x1C},
	{0x1C, 0x1D},
	{0x1C, 0x1D},
	{0x1D, 0x1C},
	{0x1D, 0x1C},
	{0x1D, 0x1C},
	{0x1E, 0x1B},
	{0x1D, 0x1D},
	{0x1D, 0x1D},
	{0x1D, 0x1E},
	{0x1D, 0x1E},
	{0x1E, 0x1D},
	{0x1E, 0x1D},
	{0x1E, 0x1D},
	{0x1E, 0x1E},
	{0x1E, 0x1E},
	{0x1E, 0x1E},
	{0x1E, 0x1F},
	{0x1E, 0x1F},
	{0x1F, 0x1E},
	{0x1F, 0x1E},
	{0x1F, 0x1E},
	{0x1F, 0x1F},
	{0x1F, 0x1F},
};


        static readonly byte[,] OMatch6 = new byte[256, 2]
{
	{0x00, 0x00},
	{0x00, 0x01},
	{0x01, 0x00},
	{0x01, 0x01},
	{0x01, 0x01},
	{0x01, 0x02},
	{0x02, 0x01},
	{0x02, 0x02},
	{0x02, 0x02},
	{0x02, 0x03},
	{0x03, 0x02},
	{0x03, 0x03},
	{0x03, 0x03},
	{0x03, 0x04},
	{0x04, 0x03},
	{0x04, 0x04},
	{0x04, 0x04},
	{0x04, 0x05},
	{0x05, 0x04},
	{0x05, 0x05},
	{0x05, 0x05},
	{0x05, 0x06},
	{0x06, 0x05},
	{0x00, 0x11},
	{0x06, 0x06},
	{0x06, 0x07},
	{0x07, 0x06},
	{0x02, 0x10},
	{0x07, 0x07},
	{0x07, 0x08},
	{0x08, 0x07},
	{0x03, 0x11},
	{0x08, 0x08},
	{0x08, 0x09},
	{0x09, 0x08},
	{0x05, 0x10},
	{0x09, 0x09},
	{0x09, 0x0A},
	{0x0A, 0x09},
	{0x06, 0x11},
	{0x0A, 0x0A},
	{0x0A, 0x0B},
	{0x0B, 0x0A},
	{0x08, 0x10},
	{0x0B, 0x0B},
	{0x0B, 0x0C},
	{0x0C, 0x0B},
	{0x09, 0x11},
	{0x0C, 0x0C},
	{0x0C, 0x0D},
	{0x0D, 0x0C},
	{0x0B, 0x10},
	{0x0D, 0x0D},
	{0x0D, 0x0E},
	{0x0E, 0x0D},
	{0x0C, 0x11},
	{0x0E, 0x0E},
	{0x0E, 0x0F},
	{0x0F, 0x0E},
	{0x0E, 0x10},
	{0x0F, 0x0F},
	{0x0F, 0x10},
	{0x10, 0x0E},
	{0x10, 0x0F},
	{0x11, 0x0E},
	{0x10, 0x10},
	{0x10, 0x11},
	{0x11, 0x10},
	{0x12, 0x0F},
	{0x11, 0x11},
	{0x11, 0x12},
	{0x12, 0x11},
	{0x14, 0x0E},
	{0x12, 0x12},
	{0x12, 0x13},
	{0x13, 0x12},
	{0x15, 0x0F},
	{0x13, 0x13},
	{0x13, 0x14},
	{0x14, 0x13},
	{0x17, 0x0E},
	{0x14, 0x14},
	{0x14, 0x15},
	{0x15, 0x14},
	{0x18, 0x0F},
	{0x15, 0x15},
	{0x15, 0x16},
	{0x16, 0x15},
	{0x1A, 0x0E},
	{0x16, 0x16},
	{0x16, 0x17},
	{0x17, 0x16},
	{0x1B, 0x0F},
	{0x17, 0x17},
	{0x17, 0x18},
	{0x18, 0x17},
	{0x13, 0x21},
	{0x18, 0x18},
	{0x18, 0x19},
	{0x19, 0x18},
	{0x15, 0x20},
	{0x19, 0x19},
	{0x19, 0x1A},
	{0x1A, 0x19},
	{0x16, 0x21},
	{0x1A, 0x1A},
	{0x1A, 0x1B},
	{0x1B, 0x1A},
	{0x18, 0x20},
	{0x1B, 0x1B},
	{0x1B, 0x1C},
	{0x1C, 0x1B},
	{0x19, 0x21},
	{0x1C, 0x1C},
	{0x1C, 0x1D},
	{0x1D, 0x1C},
	{0x1B, 0x20},
	{0x1D, 0x1D},
	{0x1D, 0x1E},
	{0x1E, 0x1D},
	{0x1C, 0x21},
	{0x1E, 0x1E},
	{0x1E, 0x1F},
	{0x1F, 0x1E},
	{0x1E, 0x20},
	{0x1F, 0x1F},
	{0x1F, 0x20},
	{0x20, 0x1E},
	{0x20, 0x1F},
	{0x21, 0x1E},
	{0x20, 0x20},
	{0x20, 0x21},
	{0x21, 0x20},
	{0x22, 0x1F},
	{0x21, 0x21},
	{0x21, 0x22},
	{0x22, 0x21},
	{0x24, 0x1E},
	{0x22, 0x22},
	{0x22, 0x23},
	{0x23, 0x22},
	{0x25, 0x1F},
	{0x23, 0x23},
	{0x23, 0x24},
	{0x24, 0x23},
	{0x27, 0x1E},
	{0x24, 0x24},
	{0x24, 0x25},
	{0x25, 0x24},
	{0x28, 0x1F},
	{0x25, 0x25},
	{0x25, 0x26},
	{0x26, 0x25},
	{0x2A, 0x1E},
	{0x26, 0x26},
	{0x26, 0x27},
	{0x27, 0x26},
	{0x2B, 0x1F},
	{0x27, 0x27},
	{0x27, 0x28},
	{0x28, 0x27},
	{0x23, 0x31},
	{0x28, 0x28},
	{0x28, 0x29},
	{0x29, 0x28},
	{0x25, 0x30},
	{0x29, 0x29},
	{0x29, 0x2A},
	{0x2A, 0x29},
	{0x26, 0x31},
	{0x2A, 0x2A},
	{0x2A, 0x2B},
	{0x2B, 0x2A},
	{0x28, 0x30},
	{0x2B, 0x2B},
	{0x2B, 0x2C},
	{0x2C, 0x2B},
	{0x29, 0x31},
	{0x2C, 0x2C},
	{0x2C, 0x2D},
	{0x2D, 0x2C},
	{0x2B, 0x30},
	{0x2D, 0x2D},
	{0x2D, 0x2E},
	{0x2E, 0x2D},
	{0x2C, 0x31},
	{0x2E, 0x2E},
	{0x2E, 0x2F},
	{0x2F, 0x2E},
	{0x2E, 0x30},
	{0x2F, 0x2F},
	{0x2F, 0x30},
	{0x30, 0x2E},
	{0x30, 0x2F},
	{0x31, 0x2E},
	{0x30, 0x30},
	{0x30, 0x31},
	{0x31, 0x30},
	{0x32, 0x2F},
	{0x31, 0x31},
	{0x31, 0x32},
	{0x32, 0x31},
	{0x34, 0x2E},
	{0x32, 0x32},
	{0x32, 0x33},
	{0x33, 0x32},
	{0x35, 0x2F},
	{0x33, 0x33},
	{0x33, 0x34},
	{0x34, 0x33},
	{0x37, 0x2E},
	{0x34, 0x34},
	{0x34, 0x35},
	{0x35, 0x34},
	{0x38, 0x2F},
	{0x35, 0x35},
	{0x35, 0x36},
	{0x36, 0x35},
	{0x3A, 0x2E},
	{0x36, 0x36},
	{0x36, 0x37},
	{0x37, 0x36},
	{0x3B, 0x2F},
	{0x37, 0x37},
	{0x37, 0x38},
	{0x38, 0x37},
	{0x3D, 0x2E},
	{0x38, 0x38},
	{0x38, 0x39},
	{0x39, 0x38},
	{0x3E, 0x2F},
	{0x39, 0x39},
	{0x39, 0x3A},
	{0x3A, 0x39},
	{0x3A, 0x3A},
	{0x3A, 0x3A},
	{0x3A, 0x3B},
	{0x3B, 0x3A},
	{0x3B, 0x3B},
	{0x3B, 0x3B},
	{0x3B, 0x3C},
	{0x3C, 0x3B},
	{0x3C, 0x3C},
	{0x3C, 0x3C},
	{0x3C, 0x3D},
	{0x3D, 0x3C},
	{0x3D, 0x3D},
	{0x3D, 0x3D},
	{0x3D, 0x3E},
	{0x3E, 0x3D},
	{0x3E, 0x3E},
	{0x3E, 0x3E},
	{0x3E, 0x3F},
	{0x3F, 0x3E},
	{0x3F, 0x3F},
	{0x3F, 0x3F},
};

        #endregion

        private static CMPRBlock optimalCompressDXT1(ARGBPixel c)
        {
            CMPRBlock block = new CMPRBlock();

            uint indices = 0xAAAAAAAA;

            block._root0._data = (ushort)((OMatch5[c.R, 0] << 11) | (OMatch6[c.G, 0] << 5) | OMatch5[c.B, 0]);
            block._root1._data = (ushort)((OMatch5[c.R, 1] << 11) | (OMatch6[c.G, 1] << 5) | OMatch5[c.B, 1]);

            if (block._root0 < block._root1)
            {
                VoidPtr.Swap((short*)&block._root0, (short*)&block._root1);
                indices ^= 0x55555555;
            }
            block._lookup = indices;
            return block;
        }
        public static CMPRBlock optimalCompressDXT1a(ARGBPixel rgba)
        {
            if (rgba.A < 128)
            {
                CMPRBlock block = new CMPRBlock();
                block._root0._data = 0;
                block._root1._data = 0;
                block._lookup = 0xFFFFFFFF;
                return block;
            }
            else
            {
                return optimalCompressDXT1(rgba);
            }
        }

        static readonly Vector3 _grid = new Vector3(31.0f, 63.0f, 31.0f);
        static readonly Vector3 _gridrcp = new Vector3(0.03227752766457f, 0.01583151765563f, 0.03227752766457f);
        static readonly Vector3 _half = new Vector3(0.5f);

        static void WriteColourBlock(wRGB565Pixel a, wRGB565Pixel b, byte* indices, CMPRBlock* block)
        {
            block->_root0 = a;
            block->_root1 = b;

            int lookup = 0;
            byte* bytes = (byte*)&lookup;

            for (int i = 0, x = 30; i < 16; x -= 2)
                lookup |= indices[i++] << x;

            block->_lookup = (uint)lookup;
        }

        static void WriteColourBlock3(Vector3 start, Vector3 end, byte* indices, CMPRBlock* block)
        {
            wRGB565Pixel a = (wRGB565Pixel)start;
            wRGB565Pixel b = (wRGB565Pixel)end;

            byte* remapped = stackalloc byte[16];

            if (a <= b)
            {
                // use the indices directly
                for (int i = 0; i < 16; ++i)
                    remapped[i] = indices[i];
            }
            else
            {
                // swap a and b
                VoidPtr.Swap((short*)&a, (short*)&b);
                for (int i = 0; i < 16; ++i)
                {
                    if (indices[i] == 0)
                        remapped[i] = 1;
                    else if (indices[i] == 1)
                        remapped[i] = 0;
                    else
                        remapped[i] = indices[i];
                }
            }

            // write the block
            WriteColourBlock(a, b, remapped, block);
        }
        static void WriteColourBlock4(Vector3 start, Vector3 end, byte* indices, CMPRBlock* block)
        {
            wRGB565Pixel a = (wRGB565Pixel)start;
            wRGB565Pixel b = (wRGB565Pixel)end;

            byte* remapped = stackalloc byte[16];

            if (a < b)
            {
                // swap a and b
                VoidPtr.Swap((short*)&a, (short*)&b);
                for (int i = 0; i < 16; ++i)
                    remapped[i] = (byte)((indices[i] ^ 0x1) & 0x3);
            }
            else if (a == b)
            {
                // use index 0
                for (int i = 0; i < 16; ++i)
                    remapped[i] = 0;
            }
            else
            {
                // use the indices directly
                for (int i = 0; i < 16; ++i)
                    remapped[i] = indices[i];
            }

            // write the block
            WriteColourBlock(a, b, remapped, block);
        }

        static void ComputeWeightedCovariance(int n, Vector3* points, float* weights, Vector3 metric, float* pOut)
        {
            // compute the centroid
            float total = 0.0f;
            Vector3 centroid = new Vector3();
            for (int i = 0; i < n; ++i)
            {
                total += weights[i];
                centroid += weights[i] * points[i];
            }
            centroid /= total;

            // accumulate the covariance matrix
            Memory.Fill(pOut, 24, 0);
            for (int i = 0; i < n; ++i)
            {
                Vector3 a = (points[i] - centroid) * metric;
                Vector3 b = weights[i] * a;

                pOut[0] += a._x * b._x;
                pOut[1] += a._x * b._y;
                pOut[2] += a._x * b._z;
                pOut[3] += a._y * b._y;
                pOut[4] += a._y * b._z;
                pOut[5] += a._z * b._z;
            }
        }

        static Vector3 ComputePrincipleComponent(float* matrix)
        {
            Vector3 v = new Vector3(1.0f);
            for (int i = 0; i < 8; i++)
            {
                float x = v._x * matrix[0] + v._y * matrix[1] + v._z * matrix[2];
                float y = v._x * matrix[1] + v._y * matrix[3] + v._z * matrix[4];
                float z = v._x * matrix[2] + v._y * matrix[4] + v._z * matrix[5];

                float norm = Math.Max(Math.Max(x, y), z);
                float iv = 1.0f / norm;
                if (norm == 0.0f)
                {		// @@ I think this is not necessary in this case!!
                    return new Vector3();
                }

                v = new Vector3(x * iv, y * iv, z * iv);
            }

            return v;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ColorBlock
        {
            fixed uint colors[16];
            public bool _anyAlpha, _allAlpha, _singleColor;

            public ColorBlock(ARGBPixel* image, int imgX, int imgY, int imgW, int imgH)
            {
                int bw = Math.Min(imgW - imgX, 4);
                int bh = Math.Min(imgH - imgY, 4);
                _anyAlpha = false;
                _allAlpha = true;
                _singleColor = true;

                fixed (uint* p = colors)
                {
                    int index = 0;
                    ARGBPixel pixel;
                    ARGBPixel* dPtr = (ARGBPixel*)p;
                    ARGBPixel* sPtr = image + (imgX + (imgY * imgW));
                    for (int y = 0, by = 0; y++ < 4; by = y % bh)
                        for (int x = 0, bx = 0; x++ < 4; bx = x % bw)
                        {
                            pixel = dPtr[index++] = sPtr[bx + (by * imgW)];
                            if (pixel.A < 128) _anyAlpha = true;
                            else _allAlpha = false;
                            if (pixel != dPtr[0]) _singleColor = false;
                        }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ColourSet
        {
            public fixed float m_points[48];
            public fixed int m_remap[16];
            public fixed float m_weights[16];

            int m_count;
            bool m_transparent;

            public int Count { get { return m_count; } }
            public bool IsTransparent { get { return m_transparent; } }
            public Vector3* Points { get { fixed (float* p = m_points) return (Vector3*)p; } }
            public float* Weights { get { fixed (float* p = m_weights) return p; } }
            public int* Remap { get { fixed (int* p = m_remap) return p; } }

            public ColourSet(ARGBPixel* rgba, bool weightByAlpha)
            {
                m_count = 0;
                m_transparent = false;

                int* remap = Remap;
                Vector3* points = Points;
                float* weights = Weights;

                // create the minimal set
                for (int i = 0; i < 16; ++i)
                {
                    // check for transparent pixels when using dxt1
                    if (rgba[i].A == 0)
                    {
                        remap[i] = -1;
                        m_transparent = true;
                    }
                    else
                        remap[i] = m_count;

                    // normalise coordinates to [0,1]
                    float x = (float)rgba[i].R / 255.0f;
                    float y = (float)rgba[i].G / 255.0f;
                    float z = (float)rgba[i].B / 255.0f;

                    // ensure there is always non-zero weight even for zero alpha
                    float w = (float)(rgba[i].A + 1) / 256.0f;

                    // add the point
                    points[m_count] = new Vector3(x, y, z);
                    weights[m_count] = (weightByAlpha ? w : 1.0f);

                    // advance
                    ++m_count;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct WCF
        {
            float m_besterror;
            Vector3 m_metric;
            ColourSet* m_colours;
            fixed int m_order[16];
            fixed float m_weights[16];
            fixed float m_weighted[48];

            Vector3 m_xxsum, m_xsum;
            float m_wsum;

            public int* Order { get { fixed (int* p = m_order)return p; } }
            public float* Weights { get { fixed (float* w = m_weights)return w; } }
            public Vector3* Points { get { fixed (float* w = m_weighted)return (Vector3*)w; } }

            public WCF(ColourSet* colours)
            {
                m_colours = colours;

                m_metric = new Vector3(1.0f);

                m_besterror = float.MaxValue;
                Vector3 metric = m_metric;
                m_xxsum = m_xsum = new Vector3();
                m_wsum = 0.0f;

                // cache some values
                int count = m_colours->Count;
                Vector3* values = (Vector3*)m_colours->m_points;
                float* weights = m_colours->m_weights;
                int* order = Order;

                // get the covariance matrix
                float* covariance = stackalloc float[6];

                ComputeWeightedCovariance(count, values, weights, metric, covariance);

                // compute the principle component
                Vector3 principle = ComputePrincipleComponent(covariance);

                // build the list of values

                float* dps = stackalloc float[16];
                for (int i = 0; i < count; ++i)
                {
                    dps[i] = Vector3.Dot(values[i], principle);
                    order[i] = i;
                }

                // stable sort
                for (int i = 0; i < count; ++i)
                {
                    for (int j = i; j > 0 && dps[j] < dps[j - 1]; --j)
                    {
                        VoidPtr.Swap(dps + j, dps + (j - 1));
                        VoidPtr.Swap(order + j, order + (j - 1));
                    }
                }

                Vector3* weighted = Points;
                float* newWeights = Weights;

                for (int i = 0; i < count; ++i)
                {
                    int p = order[i];
                    weighted[i] = weights[p] * values[p];
                    m_xxsum += weighted[i] * weighted[i];
                    m_xsum += weighted[i];

                    m_wsum += newWeights[i] = weights[p];
                }
            }

            public void Compress(CMPRBlock* block)
            {
                Compress3(block);
                if (!m_colours->IsTransparent)
                    Compress4(block);
            }

            public void Compress3(CMPRBlock* block)
            {
                // declare variables
                Vector3 beststart = new Vector3();
                Vector3 bestend = new Vector3();
                float besterror = float.MaxValue;

                Vector3* weighted = Points;
                float* weights = Weights;
                int* order = Order;

                Vector3 x0 = new Vector3();
                float w0 = 0.0f;

                int b0 = 0, b1 = 0;

                // check all possible clusters for this total order
                for (int c0 = 0; c0 <= 16; c0++)
                {
                    Vector3 x1 = new Vector3();
                    float w1 = 0.0f;

                    for (int c1 = 0; c1 <= 16 - c0; c1++)
                    {
                        float w2 = m_wsum - w0 - w1;

                        // These factors could be entirely precomputed.
                        float alpha2_sum = w0 + w1 * 0.25f;
                        float beta2_sum = w2 + w1 * 0.25f;
                        float alphabeta_sum = w1 * 0.25f;
                        float factor = 1.0f / (alpha2_sum * beta2_sum - alphabeta_sum * alphabeta_sum);

                        Vector3 alphax_sum = x0 + x1 * 0.5f;
                        Vector3 betax_sum = m_xsum - alphax_sum;

                        Vector3 a = (alphax_sum * beta2_sum - betax_sum * alphabeta_sum) * factor;
                        Vector3 b = (betax_sum * alpha2_sum - alphax_sum * alphabeta_sum) * factor;

                        // clamp the output to [0, 1]
                        a.Clamp(0.0f, 1.0f);
                        b.Clamp(0.0f, 1.0f);

                        // clamp to the grid
                        a = Vector3.Floor(_grid * a + _half) * _gridrcp;
                        b = Vector3.Floor(_grid * b + _half) * _gridrcp;

                        // compute the error
                        Vector3 e1 = a * a * alpha2_sum + b * b * beta2_sum + 2.0f * (a * b * alphabeta_sum - a * alphax_sum - b * betax_sum);

                        // apply the metric to the error term
                        float error = e1._x + e1._y + e1._z;

                        // keep the solution if it wins
                        if (error < besterror)
                        {
                            besterror = error;
                            beststart = a;
                            bestend = b;
                            b0 = c0;
                            b1 = c1;
                        }

                        x1 += weighted[c0 + c1];
                        w1 += weights[c0 + c1];
                    }

                    x0 += weighted[c0];
                    w0 += weights[c0];
                }

                // save the block if necessary
                if (besterror < m_besterror)
                {
                    // compute indices from cluster sizes.
                    byte* bestindices = stackalloc byte[16];
                    {
                        int i = 0;
                        for (; i < b0; i++)
                        {
                            if (weights[i] < 0.5f)
                                bestindices[i] = 3;
                            else
                                bestindices[i] = 0;
                        }
                        for (; i < b0 + b1; i++)
                        {
                            if (weights[i] < 0.5f)
                                bestindices[i] = 3;
                            else
                                bestindices[i] = 2;
                        }
                        for (; i < 16; i++)
                        {
                            if (weights[i] < 0.5f)
                                bestindices[i] = 3;
                            else
                                bestindices[i] = 1;
                        }
                    }

                    // remap the indices
                    byte* ordered = stackalloc byte[16];
                    for (int i = 0; i < 16; ++i)
                        ordered[order[i]] = bestindices[i];

                    // save the block
                    WriteColourBlock3(beststart, bestend, ordered, block);

                    // save the error
                    m_besterror = besterror;
                }
            }

            public void Compress4(CMPRBlock* block)
            {
                Vector3 beststart = new Vector3();
                Vector3 bestend = new Vector3();
                float besterror = float.MaxValue;

                Vector3* weighted = Points;
                float* weights = Weights;
                int* order = Order;

                Vector3 x0 = new Vector3();
                float w0 = 0.0f;
                int b0 = 0, b1 = 0, b2 = 0;

                // check all possible clusters for this total order
                for (int c0 = 0; c0 <= 16; c0++)
                {
                    Vector3 x1 = new Vector3();
                    float w1 = 0.0f;

                    for (int c1 = 0; c1 <= 16 - c0; c1++)
                    {
                        Vector3 x2 = new Vector3();
                        float w2 = 0.0f;

                        for (int c2 = 0; c2 <= 16 - c0 - c1; c2++)
                        {
                            float w3 = m_wsum - w0 - w1 - w2;

                            float alpha2_sum = w0 + w1 * (4.0f / 9.0f) + w2 * (1.0f / 9.0f);
                            float beta2_sum = w3 + w2 * (4.0f / 9.0f) + w1 * (1.0f / 9.0f);
                            float alphabeta_sum = (w1 + w2) * (2.0f / 9.0f);
                            float factor = 1.0f / (alpha2_sum * beta2_sum - alphabeta_sum * alphabeta_sum);

                            Vector3 alphax_sum = x0 + x1 * (2.0f / 3.0f) + x2 * (1.0f / 3.0f);
                            Vector3 betax_sum = m_xsum - alphax_sum;

                            Vector3 a = (alphax_sum * beta2_sum - betax_sum * alphabeta_sum) * factor;
                            Vector3 b = (betax_sum * alpha2_sum - alphax_sum * alphabeta_sum) * factor;


                            a.Clamp(0.0f, 1.0f);
                            b.Clamp(0.0f, 1.0f);


                            a = Vector3.Floor(_grid * a + _half) * _gridrcp;
                            b = Vector3.Floor(_grid * b + _half) * _gridrcp;

                            // compute the error
                            Vector3 e1 = a * a * alpha2_sum + b * b * beta2_sum + 2.0f * (a * b * alphabeta_sum - a * alphax_sum - b * betax_sum);

                            // apply the metric to the error term
                            float error = e1._x + e1._y + e1._z;

                            // keep the solution if it wins
                            if (error < besterror)
                            {
                                besterror = error;
                                beststart = a;
                                bestend = b;
                                b0 = c0;
                                b1 = c1;
                                b2 = c2;
                            }

                            x2 += weighted[c0 + c1 + c2];
                            w2 += weights[c0 + c1 + c2];
                        }

                        x1 += weighted[c0 + c1];
                        w1 += weights[c0 + c1];
                    }

                    x0 += weighted[c0];
                    w0 += weights[c0];
                }

                // save the block if necessary
                if (besterror < m_besterror)
                {
                    // compute indices from cluster sizes.
                    byte* bestindices = stackalloc byte[16];
                    {
                        int i = 0;
                        for (; i < b0; i++)
                        {
                            bestindices[i] = 0;
                        }
                        for (; i < b0 + b1; i++)
                        {
                            bestindices[i] = 2;
                        }
                        for (; i < b0 + b1 + b2; i++)
                        {
                            bestindices[i] = 3;
                        }
                        for (; i < 16; i++)
                        {
                            bestindices[i] = 1;
                        }
                    }

                    // remap the indices
                    byte* ordered = stackalloc byte[16];
                    for (int i = 0; i < 16; ++i)
                        ordered[order[i]] = bestindices[i];

                    // save the block
                    WriteColourBlock4(beststart, bestend, ordered, block);

                    // save the error
                    m_besterror = besterror;
                }

            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct FCF
        {
            float m_besterror;
            Vector3 m_metric;
            ColourSet* m_colours;
            fixed int m_order[16];
            fixed float m_weights[16];
            fixed float m_weighted[48];

            Vector3 m_xxsum, m_xsum;

            public int* Order { get { fixed (int* p = m_order)return p; } }
            public float* Weights { get { fixed (float* w = m_weights)return w; } }
            public Vector3* Points { get { fixed (float* w = m_weighted)return (Vector3*)w; } }

            public FCF(ColourSet* colours)
            {
                m_colours = colours;
                //m_flags = flags;

                m_metric = new Vector3(1.0f);

                // initialise the best error
                m_besterror = float.MaxValue;
                Vector3 metric = m_metric;
                m_xxsum = new Vector3();
                m_xsum = new Vector3();

                // cache some values
                int count = m_colours->Count;
                Vector3* values = (Vector3*)m_colours->m_points;
                int* order = Order;

                // get the covariance matrix
                float* covariance = stackalloc float[6];
                ComputeWeightedCovariance(count, values, m_colours->m_weights, metric, covariance);

                // compute the principle component
                Vector3 principle = ComputePrincipleComponent(covariance);

                // build the list of values
                float* dps = stackalloc float[16];
                //float dps[16];
                for (int i = 0; i < count; ++i)
                {
                    dps[i] = Vector3.Dot(values[i], principle);
                    order[i] = i;
                }

                // stable sort
                for (int i = 0; i < count; ++i)
                {
                    for (int j = i; j > 0 && dps[j] < dps[j - 1]; --j)
                    {
                        VoidPtr.Swap(dps + j, dps + (j - 1));
                        VoidPtr.Swap(order + j, order + (j - 1));
                    }
                }

                // weight all the points

                Vector3* unweighted = Points;

                for (int i = 0; i < count; ++i)
                {
                    unweighted[i] = values[order[i]];
                    m_xxsum += unweighted[i] * unweighted[i];
                    m_xsum += unweighted[i];
                }
            }

            public void Compress(CMPRBlock* block)
            {
                Compress3(block);
                if (!m_colours->IsTransparent)
                    Compress4(block);
            }

            private void Compress3(CMPRBlock* block)
            {
                // declare variables
                Vector3 beststart = new Vector3();
                Vector3 bestend = new Vector3();
                float besterror = float.MaxValue;

                Vector3* unweighted = Points;
                float* weights = Weights;
                int* order = Order;

                Vector3 x0 = new Vector3();
                Vector3 x1;
                int b0 = 0, b1 = 0;
                int index = 0;
                
                float* fx0 = (float*)&x0, fx1 = (float*)&x1;

                Vector3 e1, e2, e3, a, b;
                float* ef1 = (float*)&e1, ef2 = (float*)&e2, ef3 = (float*)&e3, fa = (float*)&a, fb = (float*)&b;

                Vector3 grid = _grid, gridrcp = _gridrcp, half = _half;
                float* fg = (float*)&grid, fgr = (float*)&gridrcp, fh = (float*)&half;

                Vector3 alphax_sum, betax_sum;
                float* fas = (float*)&alphax_sum, fbs = (float*)&betax_sum;

                // check all possible clusters for this total order
                for (int c0 = 0; c0 <= 16; c0++)
                {
                    x1 = new Vector3();

                    for (int c1 = 0; c1 <= 16 - c0; c1++)
                    {
                        float alpha2_sum = ClusterFitPrecomp.s_threeElement[index, 0];
                        float beta2_sum = ClusterFitPrecomp.s_threeElement[index, 1];
                        float alphabeta_sum = ClusterFitPrecomp.s_threeElement[index, 2];
                        float factor = ClusterFitPrecomp.s_threeElement[index, 3];
                        index++;

                        alphax_sum = x1;
                        Maths.FMult3(fas, 0.5f);
                        Maths.FAdd3(fas, fx0);
                        //Vector3 alphax_sum = x0 + x1 * 0.5f;

                        betax_sum = m_xsum;
                        Maths.FSub3(fbs, fas);
                        //Vector3 betax_sum = m_xsum - alphax_sum;

                        a = alphax_sum;
                        Maths.FMult3(fa, beta2_sum);
                        e1 = betax_sum;
                        Maths.FMult3(ef1, alphabeta_sum);
                        Maths.FSub3(fa, ef1);
                        Maths.FMult3(fa, factor);
                        //a = (alphax_sum * beta2_sum - betax_sum * alphabeta_sum) * factor;

                        b = betax_sum;
                        Maths.FMult3(fb, alpha2_sum);
                        e1 = alphax_sum;
                        Maths.FMult3(ef1, alphabeta_sum);
                        Maths.FSub3(fb, ef1);
                        Maths.FMult3(fb, factor);
                        //b = (betax_sum * alpha2_sum - alphax_sum * alphabeta_sum) * factor;

                        // clamp the output to [0, 1]
                        a.Clamp(0.0f, 1.0f);
                        b.Clamp(0.0f, 1.0f);

                        // clamp to the grid
                        Maths.FMult3(fa, fg);
                        Maths.FAdd3(fa, fh);
                        Maths.FFloor3(fa);
                        Maths.FMult3(fa, fgr);

                        Maths.FMult3(fb, fg);
                        Maths.FAdd3(fb, fh);
                        Maths.FFloor3(fb);
                        Maths.FMult3(fb, fgr);

                        //a = Vector3.Floor(_grid * a + _half) * _gridrcp;
                        //b = Vector3.Floor(_grid * b + _half) * _gridrcp;

                        // compute the error

                        //e1 = a;
                        //e2 = b;
                        //e3 = a;
                        //e1.Multiply(&a);
                        //e1.Multiply(alpha2_sum);
                        //e2.Multiply(&b);
                        //e2.Multiply(beta2_sum);
                        //e1.Add(&e2);
                        //e2 = a;
                        //e2.Multiply(&b);
                        //e2.Multiply(alphabeta_sum);
                        //e3.Multiply(&alphax_sum);
                        //e2.Sub(&e3);
                        //e3 = b;
                        //e3.Multiply(&betax_sum);
                        //e2.Sub(&e3);
                        //e2.Multiply(2.0f);
                        //e1.Add(&e2);

                        e1 = a;
                        Maths.FMult3(ef1, ef1);
                        Maths.FMult3(ef1, alpha2_sum);

                        e2 = b;
                        Maths.FMult3(ef2, ef2);
                        Maths.FMult3(ef2, beta2_sum);
                        Maths.FAdd3(ef1, ef2);

                        e2 = a;
                        Maths.FMult3(ef2, fb);
                        Maths.FMult3(ef2, alphabeta_sum);

                        e3 = a;
                        Maths.FMult3(ef3, (float*)&alphax_sum);
                        Maths.FSub3(ef2, ef3);

                        e3 = b;
                        Maths.FMult3(ef3, (float*)&betax_sum);
                        Maths.FSub3(ef2, ef3);
                        Maths.FMult3(ef2, 2.0f);

                        Maths.FAdd3(ef1, ef2);

                        //e1 = a;
                        //Vector3.Mult(Vector3.Mult(ef1, ef1), alpha2_sum);
                        //e2 = b;
                        //Vector3.Add(ef1, Vector3.Mult(Vector3.Mult(ef2, ef2), beta2_sum));
                        //e2 = a;
                        //Vector3.Mult(Vector3.Mult(ef2, fb), alphabeta_sum);
                        //e3 = a;
                        //Vector3.Sub(ef2, Vector3.Mult(ef3, (float*)&alphax_sum));
                        //e3 = b;
                        //Vector3.Sub(ef2, Vector3.Mult(ef3, (float*)&betax_sum));
                        //Vector3.Mult(ef2, 2.0f);
                        //Vector3.Add(ef1, ef2);

                        //Vector3 e1 = a * a * alpha2_sum + b * b * beta2_sum + 2.0f * (a * b * alphabeta_sum - a * alphax_sum - b * betax_sum);

                        // apply the metric to the error term
                        float error = e1._x + e1._y + e1._z;

                        // keep the solution if it wins
                        if (error < besterror)
                        {
                            besterror = error;
                            beststart = a;
                            bestend = b;
                            b0 = c0;
                            b1 = c1;
                        }

                        x1 += unweighted[c0 + c1];
                    }

                    x0 += unweighted[c0];
                }

                // save the block if necessary
                if (besterror < m_besterror)
                {
                    // compute indices from cluster sizes.
                    /*uint bestindices = 0;
                    {
                        int i = b0;
                        for(; i < b0+b1; i++) {
                            bestindices |= 2 << (2 * m_order[i]);
                        }
                        for(; i < 16; i++) {
                            bestindices |= 1 << (2 * m_order[i]);
                        }
                    }*/

                    byte* bestindices = stackalloc byte[16];
                    {
                        int i = 0;
                        for (; i < b0; i++)
                        {
                            bestindices[i] = 0;
                        }
                        for (; i < b0 + b1; i++)
                        {
                            bestindices[i] = 2;
                        }
                        for (; i < 16; i++)
                        {
                            bestindices[i] = 1;
                        }
                    }

                    // remap the indices
                    byte* ordered = stackalloc byte[16];
                    for (int i = 0; i < 16; ++i)
                        ordered[order[i]] = bestindices[i];

                    // save the block
                    WriteColourBlock3(beststart, bestend, ordered, block);

                    // save the error
                    m_besterror = besterror;
                }
            }

            private void Compress4(CMPRBlock* block)
            {
                // declare variables
                Vector3 beststart = new Vector3();
                Vector3 bestend = new Vector3();
                float besterror = float.MaxValue;

                Vector3* unweighted = Points;
                float* weights = Weights;
                int* order = Order;

                Vector3 x0 = new Vector3();
                Vector3 x1, x2;
                
                float* fx0 = (float*)&x0, fx1 = (float*)&x1, fx2 = (float*)&x2;

                int b0 = 0, b1 = 0, b2 = 0;
                int index = 0;

                Vector3 e1, e2, e3, a, b;
                float* ef1 = (float*)&e1, ef2 = (float*)&e2, ef3 = (float*)&e3, fa = (float*)&a, fb = (float*)&b;

                Vector3 grid = _grid, gridrcp = _gridrcp, half = _half;
                float* fg = (float*)&grid, fgr = (float*)&gridrcp, fh = (float*)&half;

                Vector3 alphax_sum, betax_sum;
                float* fas = (float*)&alphax_sum, fbs = (float*)&betax_sum;

                float factor1 = 2.0f / 3.0f, factor2 = 1.0f / 3.0f;

                // check all possible clusters for this total order
                for (int c0 = 0; c0 <= 16; c0++)
                {
                    x1 = new Vector3();

                    for (int c1 = 0; c1 <= 16 - c0; c1++)
                    {
                        x2 = new Vector3();

                        for (int c2 = 0; c2 <= 16 - c0 - c1; c2++)
                        {
                            float alpha2_sum = ClusterFitPrecomp.s_fourElement[index, 0];
                            float beta2_sum = ClusterFitPrecomp.s_fourElement[index, 1];
                            float alphabeta_sum = ClusterFitPrecomp.s_fourElement[index, 2];
                            float factor = ClusterFitPrecomp.s_fourElement[index, 3];
                            index++;

                            //alphax_sum = x1;
                            //Maths.FAdd3(fas, fx1);
                            //Maths.FMult3(fas, factor1);
                            //Maths.FMult3

                            alphax_sum = x1;
                            Maths.FMult3(fas, factor1);
                            e1 = x2;
                            Maths.FMult3(ef1, factor2);
                            Maths.FAdd3(fas, ef1);
                            Maths.FAdd3(fas, fx0);
                            //alphax_sum = x0 + x1 * (2.0f / 3.0f) + x2 * (1.0f / 3.0f);

                            betax_sum = m_xsum;
                            Maths.FSub3(fbs, fas);
                            //betax_sum = m_xsum - alphax_sum;

                            a = alphax_sum;
                            Maths.FMult3(fa, beta2_sum);
                            e1 = betax_sum;
                            Maths.FMult3(ef1, alphabeta_sum);
                            Maths.FSub3(fa, ef1);
                            Maths.FMult3(fa, factor);
                            //a = (alphax_sum * beta2_sum - betax_sum * alphabeta_sum) * factor;

                            b = betax_sum;
                            Maths.FMult3(fb, alpha2_sum);
                            e1 = alphax_sum;
                            Maths.FMult3(ef1, alphabeta_sum);
                            Maths.FSub3(fb, ef1);
                            Maths.FMult3(fb, factor);
                            //b = (betax_sum * alpha2_sum - alphax_sum * alphabeta_sum) * factor;

                            // clamp the output to [0, 1]
                            a.Clamp(0.0f, 1.0f);
                            b.Clamp(0.0f, 1.0f);

                            // clamp to the grid

                            Maths.FMult3(fa, fg);
                            Maths.FAdd3(fa, fh);
                            Maths.FFloor3(fa);
                            Maths.FMult3(fa, fgr);

                            Maths.FMult3(fb, fg);
                            Maths.FAdd3(fb, fh);
                            Maths.FFloor3(fb);
                            Maths.FMult3(fb, fgr);

                            //a = Vector3.Floor(_grid * a + _half) * _gridrcp;
                            //b = Vector3.Floor(_grid * b + _half) * _gridrcp;

                            // compute the error

                            //e1 = a;
                            //e2 = b;
                            //e3 = a;
                            //e1.Multiply(&a);
                            //e1.Multiply(alpha2_sum);
                            //e2.Multiply(&b);
                            //e2.Multiply(beta2_sum);
                            //e1.Add(&e2);
                            //e2 = a;
                            //e2.Multiply(&b);
                            //e2.Multiply(alphabeta_sum);
                            //e3.Multiply(&alphax_sum);
                            //e2.Sub(&e3);
                            //e3 = b;
                            //e3.Multiply(&betax_sum);
                            //e2.Sub(&e3);
                            //e2.Multiply(2.0f);
                            //e1.Add(&e2);

                            e1 = a;
                            Maths.FMult3(ef1, ef1);
                            Maths.FMult3(ef1, alpha2_sum);

                            e2 = b;
                            Maths.FMult3(ef2, ef2);
                            Maths.FMult3(ef2, beta2_sum);
                            Maths.FAdd3(ef1, ef2);

                            e2 = a;
                            Maths.FMult3(ef2, fb);
                            Maths.FMult3(ef2, alphabeta_sum);

                            e3 = a;
                            Maths.FMult3(ef3, fas);
                            Maths.FSub3(ef2, ef3);

                            e3 = b;
                            Maths.FMult3(ef3, fbs);
                            Maths.FSub3(ef2, ef3);
                            Maths.FMult3(ef2, 2.0f);

                            Maths.FAdd3(ef1, ef2);

                            //Vector3 e1 = a * a * alpha2_sum + b * b * beta2_sum + 2.0f * (a * b * alphabeta_sum - a * alphax_sum - b * betax_sum);

                            // apply the metric to the error term
                            float error = e1._x + e1._y + e1._z;

                            // keep the solution if it wins
                            if (error < besterror)
                            {
                                besterror = error;
                                beststart = a;
                                bestend = b;
                                b0 = c0;
                                b1 = c1;
                                b2 = c2;
                            }

                            x2 += unweighted[c0 + c1 + c2];
                        }

                        x1 += unweighted[c0 + c1];
                    }

                    x0 += unweighted[c0];
                }

                // save the block if necessary
                if (besterror < m_besterror)
                {
                    // compute indices from cluster sizes.
                    /*uint bestindices = 0;
                    {
                        int i = b0;
                        for(; i < b0+b1; i++) {
                            bestindices = 2 << (2 * m_order[i]);
                        }
                        for(; i < b0+b1+b2; i++) {
                            bestindices = 3 << (2 * m_order[i]);
                        }
                        for(; i < 16; i++) {
                            bestindices = 1 << (2 * m_order[i]);
                        }
                    }*/
                    byte* bestindices = stackalloc byte[16];
                    {
                        int i = 0;
                        for (; i < b0; i++)
                        {
                            bestindices[i] = 0;
                        }
                        for (; i < b0 + b1; i++)
                        {
                            bestindices[i] = 2;
                        }
                        for (; i < b0 + b1 + b2; i++)
                        {
                            bestindices[i] = 3;
                        }
                        for (; i < 16; i++)
                        {
                            bestindices[i] = 1;
                        }
                    }

                    // remap the indices
                    byte* ordered = stackalloc byte[16];
                    for (int i = 0; i < 16; ++i)
                        ordered[order[i]] = bestindices[i];

                    // save the block
                    WriteColourBlock4(beststart, bestend, ordered, block);

                    // save the error
                    m_besterror = besterror;
                }
            }
        }
    }
}
