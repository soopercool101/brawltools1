using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Textures;

namespace BrawlLib.Imaging
{
    public static unsafe class NVDXT
    {

        private static CMPRBlock compressDXT1(ARGBPixel* pBlock)
        {
            float* pointData = stackalloc float[48];
            Vector3* points = (Vector3*)pointData;

            extractColorBlockRGB(pBlock, points);

            // find min and max colors
            Vector3 maxColor, minColor;
            findMinMaxColorsBox(points, 16, &maxColor, &minColor);

            selectDiagonal(points, 16, &maxColor, &minColor);

            insetBBox(&maxColor, &minColor);

            ushort color0 = roundAndExpand(&maxColor);
            ushort color1 = roundAndExpand(&minColor);

            if (color0 < color1)
            {
                Vector3 t = maxColor; maxColor = minColor; minColor = t;
                VoidPtr.Swap(&color0, &color1);
            }

            CMPRBlock block = new CMPRBlock();
            block._root0._data = color0;
            block._root1._data = color1;
            block._lookup = computeIndices4(points, &maxColor, &minColor);


            optimizeEndPoints4(points, &block);

            return block;

        }

        public static CMPRBlock compressDXT1a(ARGBPixel* img, int imgX, int imgY, int imgW, int imgH)
        {
            uint* pData = stackalloc uint[16];
            ARGBPixel* pBlock = (ARGBPixel*)pData;

            bool hasAlpha = false;
            bool isSingle = true;

            ARGBPixel p;
            ARGBPixel* sPtr = img + (imgX + (imgY * imgW));
            int index = 0;
            for (int y = 0; y < 4; y++)
                for (int x = 0; x < 4; x++)
                {
                    p = sPtr[x + (y * imgW)];
                    pBlock[index++] = p;
                    if (p != pBlock[0]) isSingle = false;
                    if (p.A < 128) hasAlpha = true;
                }

            if (isSingle)
                return optimalCompressDXT1a(sPtr[0]);

            if (!hasAlpha)
                return compressDXT1(pBlock);

            // @@ Handle single RGB, with varying alpha? We need tables for single color compressor in 3 color mode.
            //else if (rgba.isSingleColorNoAlpha()) { ... }

            float* pointData = stackalloc float[48];
            Vector3* points = (Vector3*)pointData;

            // read block
            //Vector3 block[16];
            int num = extractColorBlockRGBA(pBlock, points);

            // find min and max colors
            Vector3 maxColor, minColor;
            findMinMaxColorsBox(points, num, &maxColor, &minColor);

            selectDiagonal(points, num, &maxColor, &minColor);

            insetBBox(&maxColor, &minColor);

            ushort color0 = roundAndExpand(&maxColor);
            ushort color1 = roundAndExpand(&minColor);

            if (color0 < color1)
            {
                Vector3 t = maxColor; maxColor = minColor; minColor = t;
                VoidPtr.Swap(&color0, &color1);
            }

            CMPRBlock block = new CMPRBlock();
            block._root0._data = color1;
            block._root1._data = color0;
            block._lookup = computeIndices3(pBlock, &maxColor, &minColor);

            //	optimizeEndPoints(block, dxtBlock);

            return block;
        }


        private static CMPRBlock optimalCompressDXT1(ARGBPixel c)
        {
            CMPRBlock block = new CMPRBlock();

            uint indices = 0xAAAAAAAA;

            block._root0._data = (ushort)((RygPrecomp.OMatch5[c.R, 0] << 11) | (RygPrecomp.OMatch6[c.G, 0] << 5) | RygPrecomp.OMatch5[c.B, 0]);
            block._root1._data = (ushort)((RygPrecomp.OMatch5[c.R, 1] << 11) | (RygPrecomp.OMatch6[c.G, 1] << 5) | RygPrecomp.OMatch5[c.B, 1]);

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

        private static int extractColorBlockRGBA(ARGBPixel* colors, Vector3* points)
        {
            int num = 0;

            for (int i = 0; i < 16; i++)
            {
                ARGBPixel p = colors[i];
                if (p.A > 127)
                    points[num++] = new Vector3(p.R, p.G, p.B);
            }

            return num;
        }

        private static void extractColorBlockRGB(ARGBPixel* colors, Vector3* points)
        {
            for (int i = 0; i < 16; i++)
            {
                ARGBPixel p = colors[i];
                points[i] = new Vector3(p.R, p.G, p.B);
            }
        }

        private static void findMinMaxColorsBox(Vector3* points, int num, Vector3* maxColor, Vector3* minColor)
        {
            *maxColor = new Vector3();
            *minColor = new Vector3(255.0f);

            for (uint i = 0; i < num; i++)
            {
                maxColor->Max(&points[i]);
                minColor->Min(&points[i]);
            }
        }

        private static void selectDiagonal(Vector3* points, int num, Vector3* maxColor, Vector3* minColor)
        {
            Vector3 center = (*maxColor + *minColor) * 0.5f;
            Vector3 t;
            Vector2* tp = (Vector2*)&t;

            Vector2 covariance = new Vector2();
            for (uint i = 0; i < num; i++)
            {
                t = points[i] - center;
                covariance += *tp * t._z;
            }

            float x0 = maxColor->_x;
            float y0 = maxColor->_y;
            float x1 = minColor->_x;
            float y1 = minColor->_y;

            if (covariance._x < 0)
                VoidPtr.Swap(&x0, &x1);

            if (covariance._y < 0)
                VoidPtr.Swap(&y0, &y1);

            maxColor->_x = x0;
            maxColor->_y = y0;

            minColor->_x = x1;
            minColor->_y = y1;
        }

        private static void insetBBox(Vector3* maxColor, Vector3* minColor)
        {
            Vector3 inset = (*maxColor - *minColor) / 16.0f - (8.0f / 255.0f) / 16.0f;

            maxColor->Sub(&inset);
            maxColor->Clamp(0.0f, 255.0f);

            minColor->Add(&inset);
            minColor->Clamp(0.0f, 255.0f);
        }

        private static ushort roundAndExpand(Vector3* v)
        {
            uint r = (uint)(Math.Min(Math.Max(v->_x * (31.0f / 255.0f), 0.0f), 31.0f) + 0.5f);
            uint g = (uint)(Math.Min(Math.Max(v->_y * (63.0f / 255.0f), 0.0f), 63.0f) + 0.5f);
            uint b = (uint)(Math.Min(Math.Max(v->_z * (31.0f / 255.0f), 0.0f), 31.0f) + 0.5f);


            ushort w = (ushort)((r << 11) | (g << 5) | b);

            r = (r << 3) | (r >> 2);
            g = (g << 2) | (g >> 4);
            b = (b << 3) | (b >> 2);
            *v = new Vector3(r, g, b);

            return w;
        }

        private static uint computeIndices3(ARGBPixel* colors, Vector3* maxColor, Vector3* minColor)
        {
            float* pData = stackalloc float[9];
            Vector3* palette = (Vector3*)pData;

            palette[0] = *minColor;
            palette[1] = *maxColor;
            palette[2] = (palette[0] + palette[1]) * 0.5f;

            uint indices = 0;
            for (int i = 0; i < 16; i++)
            {
                ARGBPixel p = colors[i];
                Vector3 color = new Vector3(p.R, p.G, p.B);

                float d0 = colorDistance(&palette[0], &color);
                float d1 = colorDistance(&palette[1], &color);
                float d2 = colorDistance(&palette[2], &color);

                uint index;
                if (p.A < 128) index = 3;
                else if (d0 < d1 && d0 < d2) index = 0;
                else if (d1 < d2) index = 1;
                else index = 2;

                indices <<= 2;
                indices |= index;
            }

            return indices;
        }
        private static uint computeIndices4(Vector3* points, Vector3* maxColor, Vector3* minColor)
        {
            float* pData = stackalloc float[12];
            Vector3* palette = (Vector3*)pData;

            palette[0] = *maxColor;
            palette[1] = *minColor;
            palette[2] = Vector3.Lerp(palette[0], palette[1], 1.0f / 3.0f);
            palette[3] = Vector3.Lerp(palette[0], palette[1], 2.0f / 3.0f);

            uint indices = 0;
            for (int i = 0; i < 16; i++)
            {
                Vector3 color = points[i];

                float d0 = colorDistance(&palette[0], &color);
                float d1 = colorDistance(&palette[1], &color);
                float d2 = colorDistance(&palette[2], &color);
                float d3 = colorDistance(&palette[3], &color);

                indices <<= 2;

                if (d3 < d2 && d3 < d1 && d3 < d0)
                    indices |= 3;
                else if (d2 < d1 && d2 < d0)
                    indices |= 2;
                else if (d1 < d0)
                    indices |= 1;

                //uint b0 = (d0 > d3) ? (uint)1 : 0;
                //uint b1 = (d1 > d2) ? (uint)1 : 0;
                //uint b2 = (d0 > d2) ? (uint)1 : 0;
                //uint b3 = (d1 > d3) ? (uint)1 : 0;
                //uint b4 = (d2 > d3) ? (uint)1 : 0;

                //uint x0 = b1 & b2;
                //uint x1 = b0 & b3;
                //uint x2 = b0 & b4;

                //indices <<= 2;
                //indices |= x2 | ((x0 | x1) << 1);
            }

            return indices;
        }

        private static void optimizeEndPoints4(Vector3* points, CMPRBlock* block)
        {
            float alpha2_sum = 0.0f;
            float beta2_sum = 0.0f;
            float alphabeta_sum = 0.0f;
            Vector3 alphax_sum = new Vector3();
            Vector3 betax_sum = new Vector3();
            uint indices = block->_lookup;

            for (int i = 0, bi = 30; i < 16; ++i, bi -= 2)
            {
                uint bits = indices >> bi;

                float beta = bits & 1;
                if ((bits & 2) != 0) beta = (1 + beta) / 3.0f;
                float alpha = 1.0f - beta;

                alpha2_sum += alpha * alpha;
                beta2_sum += beta * beta;
                alphabeta_sum += alpha * beta;
                alphax_sum += alpha * points[i];
                betax_sum += beta * points[i];
            }

            float denom = alpha2_sum * beta2_sum - alphabeta_sum * alphabeta_sum;
            if (Math.Abs(denom) <= 0.0001f) 
                return;

            float factor = 1.0f / denom;

            Vector3 a = (alphax_sum * beta2_sum - betax_sum * alphabeta_sum) * factor;
            Vector3 b = (betax_sum * alpha2_sum - alphax_sum * alphabeta_sum) * factor;

            a.Clamp(0.0f, 255.0f);
            b.Clamp(0.0f, 255.0f);

            ushort color0 = roundAndExpand(&a);
            ushort color1 = roundAndExpand(&b);

            if (color0 < color1)
            {
                Vector3 t = a; a = b; b = t;
                VoidPtr.Swap(&color0, &color1);
            }

            //CMPRBlock block = new CMPRBlock();
            block->_root0._data = color0;
            block->_root1._data = color1;
            block->_lookup = computeIndices4(points, &a, &b);
        }

        private static float colorDistance(Vector3* c0, Vector3* c1)
        {
            Vector3 v = *c0 - *c1;
            return Vector3.Dot(v,v);
        }

    }
}
