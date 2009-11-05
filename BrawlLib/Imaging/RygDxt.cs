using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Textures;

namespace BrawlLib.Imaging
{
    public static unsafe class RygDxt
    {

        private static byte[] Expand5 = new byte[32];
        private static byte[] Expand6 = new byte[64];
        private static byte[,] OMatch5 = new byte[256, 2];
        private static byte[,] OMatch6 = new byte[256, 2];
        private static byte[] QuantRBTab = new byte[256 + 16];
        private static byte[] QuantGTab = new byte[256 + 16];

        static RygDxt()
        {
            for (int i = 0; i < 32; i++)
                Expand5[i] = (byte)((i << 3) | (i >> 2));

            for (int i = 0; i < 64; i++)
                Expand6[i] = (byte)((i << 2) | (i >> 4));

            for (int i = 0; i < 256 + 16; i++)
            {
                int v = Math.Min(Math.Max(i - 8, 0), 255);
                QuantRBTab[i] = Expand5[Mul8Bit(v, 31)];
                QuantGTab[i] = Expand6[Mul8Bit(v, 63)];
            }

            fixed (byte* o5 = OMatch5)
            fixed (byte* o6 = OMatch6)
            fixed (byte* e5 = Expand5)
            fixed (byte* e6 = Expand6)
            {
                PrepareOptTable(o5, e5, 32);
                PrepareOptTable(o6, e6, 64);
            }

        }
        static int Mul8Bit(int a, int b)
        {
            int t = a * b + 128;
            return (t + (t >> 8)) >> 8;
        }

        static void PrepareOptTable(byte* Table, byte* expand, int size)
        {
            for (int i = 0; i < 256; i++)
            {
                int bestErr = 256;

                for (int min = 0; min < size; min++)
                {
                    for (int max = 0; max < size; max++)
                    {
                        int mine = expand[min];
                        int maxe = expand[max];
                        int err = Math.Abs(maxe + Mul8Bit(mine - maxe, 0x55) - i);

                        if (err < bestErr)
                        {
                            Table[i * 2 + 0] = (byte)max;
                            Table[i * 2 + 1] = (byte)min;
                            bestErr = err;
                        }
                    }
                }
            }
        }

        public static void CompressDXTBlock(CMPRBlock* dest, ARGBPixel* img, int imgX, int imgY, int imgW, int imgH)
        {
            uint* colors = stackalloc uint[16];
            int bw = Math.Min(imgW - imgX, 4), bh = Math.Min(imgH - imgY, 4);

            int index = 0;
            bool alpha = false;
            uint* sPtr = (uint*)img + (imgX + (imgY * imgW));
            for (int y = 0, by = 0; y++ < 4; by = y % bh)
                for (int x = 0, bx = 0; x++ < 4; bx = x % bw)
                {
                    if (((colors[index++] = sPtr[bx + (by * imgW)]) & 0xFF000000) < 128)
                        alpha = true;
                }

            CompressDXTBlock(dest, (ARGBPixel*)colors, alpha, true);
        }
        public static void CompressDXTBlock(CMPRBlock* dest, ARGBPixel* src, bool alpha, bool quality)
        {
            // if alpha specified, compress alpha aswell
            if (alpha)
            {
                CompressAlphaBlock(dest, src, quality);
                //dest += 8;
            }

            // compress the color part
            CompressColorBlock(dest, src, quality);
        }

        // Color block compression
        static void CompressColorBlock(CMPRBlock* block, ARGBPixel* src, bool quality)
        {
            uint* sData = (uint*)src;

            uint* dblock = stackalloc uint[16];
            uint* color = stackalloc uint[4];

            //const Pixel *block = (const Pixel *) src;
            //Pixel dblock[16],color[4];

            // check if block is constant
            uint min, max;
            min = max = sData[0];

            for (int i = 1; i < 16; i++)
            {
                min = Math.Min(min, sData[i]);
                max = Math.Max(max, sData[i]);
            }

            // perform block compression
            ushort min16, max16;
            uint mask;

            if (min != max) // no constant color
            {
                // first step: compute dithered version for PCA if desired
                if (quality)
                    DitherBlock((byte*)dblock, (byte*)src);

                // second step: pca+map along principal axis
                OptimizeColorsBlock(quality ? (ARGBPixel*)dblock : src, &max16, &min16);
                if (max16 != min16)
                {
                    EvalColors((ARGBPixel*)color, max16, min16);
                    mask = MatchColorsBlock(src, (ARGBPixel*)color, quality);
                }
                else
                    mask = 0;

                // third step: refine
                if (RefineBlock(quality ? (ARGBPixel*)dblock : src, &max16, &min16, mask))
                {
                    if (max16 != min16)
                    {
                        EvalColors((ARGBPixel*)color, max16, min16);
                        mask = MatchColorsBlock(src, (ARGBPixel*)color, quality);
                    }
                    else
                        mask = 0;
                }
            }
            else // constant color
            {
                int r = src[0].R;
                int g = src[0].G;
                int b = src[0].B;

                mask = 0xaaaaaaaa;
                max16 = (ushort)((OMatch5[r, 0] << 11) | (OMatch6[g, 0] << 5) | OMatch5[b, 0]);
                min16 = (ushort)((OMatch5[r, 1] << 11) | (OMatch6[g, 1] << 5) | OMatch5[b, 1]);
            }

            // write the color block
            if (max16 < min16)
            {
                VoidPtr.Swap(&max16, &min16);
                //sSwap(max16, min16);
                mask ^= 0x55555555;
            }

            block->_root0._data = max16;
            block->_root1._data = min16;
            block->_lookup = mask;

            //((sU16 *) dest)[0] = max16;
            //((sU16 *) dest)[1] = min16;
            //((sU32 *) dest)[1] = mask;
        }

        // Alpha block compression (this is easy for a change)
        private static void CompressAlphaBlock(CMPRBlock* dest, ARGBPixel* src, bool quality)
        {
            // const Pixel *block = (const Pixel *) src;

            // find min/max color
            int min, max;
            min = max = src[0].A;

            for (int i = 1; i < 16; i++)
            {
                min = Math.Min(min, src[i].A);
                max = Math.Max(max, src[i].A);
            }

            // encode them
            dest->_root0 = (wRGB565Pixel)(*(ARGBPixel*)&max);
            dest->_root1 = (wRGB565Pixel)(*(ARGBPixel*)&min);
            //*dest++ = max;
            //*dest++ = min;

            // determine bias and emit color indices
            int dist = max - min;
            int bias = min * 7 - (dist >> 1);
            int dist4 = dist * 4;
            int dist2 = dist * 2;
            int mask = 0;

            for (int i = 0, bits = 30; i < 16; i++, bits -= 2)
            {
                int a = src[i].A * 7 - bias;
                int ind, t;

                // select index (hooray for bit magic)
                t = (dist4 - a) >> 31; ind = t & 4; a -= dist4 & t;
                t = (dist2 - a) >> 31; ind += t & 2; a -= dist2 & t;
                t = (dist - a) >> 31; ind += t & 1;

                ind = -ind & 7;
                ind ^= (2 > ind) ? 1 : 0;

                // write index
                mask |= ind << bits;
                //if ((bits += 3) >= 8)
                //{
                //    *dest++ = mask;
                //    mask >>= 8;
                //    bits -= 8;
                //}
            }

            dest->_lookup = (uint)mask;
        }

        // Block dithering function. Simply dithers a block to 565 RGB.
        // (Floyd-Steinberg)
        static void DitherBlock(byte* dest, byte* src)
        {
            int* err = stackalloc int[8];
            int* ep1 = err, ep2 = err + 4;
            //sInt err[8],*ep1 = err,*ep2 = err+4;

            fixed (byte* gtab = QuantGTab)
            fixed (byte* rbtab = QuantRBTab)
            {

                // process channels seperately
                for (int ch = 0; ch < 3; ch++)
                {
                    byte* bp = src;
                    byte* dp = dest;
                    // sU8 *bp = (sU8 *) block;
                    // sU8 *dp = (sU8 *) dest;
                    byte* quant = (ch == 1) ? gtab + 8 : rbtab + 8;
                    // sU8 *quant = (ch == 1) ? QuantGTab+8 : QuantRBTab+8;

                    bp += ch;
                    dp += ch;
                    Memory.Fill(err, 32, 0);
                    //sSetMem(err,0,sizeof(err));

                    for (int y = 0; y < 4; y++)
                    {
                        // pixel 0
                        dp[0] = quant[bp[0] + ((3 * ep2[1] + 5 * ep2[0]) >> 4)];
                        ep1[0] = bp[0] - dp[0];

                        // pixel 1
                        dp[4] = quant[bp[4] + ((7 * ep1[0] + 3 * ep2[2] + 5 * ep2[1] + ep2[0]) >> 4)];
                        ep1[1] = bp[4] - dp[4];

                        // pixel 2
                        dp[8] = quant[bp[8] + ((7 * ep1[1] + 3 * ep2[3] + 5 * ep2[2] + ep2[1]) >> 4)];
                        ep1[2] = bp[8] - dp[8];

                        // pixel 3
                        dp[12] = quant[bp[12] + ((7 * ep1[2] + 5 * ep2[3] + ep2[2]) >> 4)];
                        ep1[3] = bp[12] - dp[12];

                        // advance to next line
                        int* t = ep1; ep1 = ep2; ep2 = t;
                        //sSwap(ep1,ep2);
                        bp += 16;
                        dp += 16;
                    }
                }
            }
        }

        // The color optimization function. (Clever code, part 1)
        static void OptimizeColorsBlock(ARGBPixel* block, ushort* max16, ushort* min16)
        {
            int nIterPower = 4;

            int* mu = stackalloc int[3];
            int* min = stackalloc int[3];
            int* max = stackalloc int[3];

            // determine color distribution
            // int mu[3],min[3],max[3];

            for (int ch = 0; ch < 3; ch++)
            {
                byte* bp = (byte*)block + ch;
                //const sU8 *bp = ((const sU8 *) block) + ch;
                int muv, minv, maxv;

                muv = minv = maxv = bp[0];
                for (int i = 4; i < 64; i += 4)
                {
                    muv += bp[i];
                    minv = Math.Min(minv, bp[i]);
                    maxv = Math.Max(maxv, bp[i]);
                }

                mu[ch] = (muv + 8) >> 4;
                min[ch] = minv;
                max[ch] = maxv;
            }

            // determine covariance matrix
            int* cov = stackalloc int[6];
            //sInt cov[6];
            for (int i = 0; i < 6; i++)
                cov[i] = 0;

            for (int i = 0; i < 16; i++)
            {
                int r = block[i].R - mu[2];
                int g = block[i].G - mu[1];
                int b = block[i].B - mu[0];

                cov[0] += r * r;
                cov[1] += r * g;
                cov[2] += r * b;
                cov[3] += g * g;
                cov[4] += g * b;
                cov[5] += b * b;
            }

            // convert covariance matrix to float, find principal axis via power iter
            float* covf = stackalloc float[6];
            float vfr, vfg, vfb;
            for (int i = 0; i < 6; i++)
                covf[i] = cov[i] / 255.0f;

            vfr = max[2] - min[2];
            vfg = max[1] - min[1];
            vfb = max[0] - min[0];

            for (int iter = 0; iter < nIterPower; iter++)
            {
                float r = vfr * covf[0] + vfg * covf[1] + vfb * covf[2];
                float g = vfr * covf[1] + vfg * covf[3] + vfb * covf[4];
                float b = vfr * covf[2] + vfg * covf[4] + vfb * covf[5];

                vfr = r;
                vfg = g;
                vfb = b;
            }

            float magn = Math.Max(Math.Max(Math.Abs(vfr), Math.Abs(vfg)), Math.Abs(vfb));
            int v_r, v_g, v_b;

            if (magn < 4.0f) // too small, default to luminance
            {
                v_r = 148;
                v_g = 300;
                v_b = 58;
            }
            else
            {
                magn = 512.0f / magn;
                v_r = (int)(vfr * magn);
                v_g = (int)(vfg * magn);
                v_b = (int)(vfb * magn);
            }

            // Pick colors at extreme points
            int mind = 0x7fffffff, maxd = -0x7fffffff;
            ARGBPixel minp = new ARGBPixel(), maxp = new ARGBPixel();

            for (int i = 0; i < 16; i++)
            {
                int dot = block[i].R * v_r + block[i].G * v_g + block[i].B * v_b;

                if (dot < mind)
                {
                    mind = dot;
                    minp = block[i];
                }

                if (dot > maxd)
                {
                    maxd = dot;
                    maxp = block[i];
                }
            }

            // Reduce to 16 bit colors
            *max16 = As16Bit(maxp);
            *min16 = As16Bit(minp);
        }

        // The color matching function
        public static uint MatchColorsBlock(ARGBPixel* block, ARGBPixel* color, bool dither)
        {
            int mask = 0;
            int dirr = color[0].R - color[1].R;
            int dirg = color[0].G - color[1].G;
            int dirb = color[0].B - color[1].B;

            int* dots = stackalloc int[16];
            //int dots[16];
            for (int i = 0; i < 16; i++)
                dots[i] = block[i].R * dirr + block[i].G * dirg + block[i].B * dirb;

            int* stops = stackalloc int[4];
            // int stops[4];
            for (int i = 0; i < 4; i++)
                stops[i] = color[i].R * dirr + color[i].G * dirg + color[i].B * dirb;

            int c0Point = (stops[1] + stops[3]) >> 1;
            int halfPoint = (stops[3] + stops[2]) >> 1;
            int c3Point = (stops[2] + stops[0]) >> 1;

            if (!dither)
            {
                // the version without dithering is straightforward
                for (int i = 0; i < 16; i++)
                {
                    mask <<= 2;
                    int dot = dots[i];

                    if (dot < halfPoint)
                        mask |= (dot < c0Point) ? 1 : 3;
                    else
                        mask |= (dot < c3Point) ? 2 : 0;
                }
            }
            else
            {
                // with floyd-steinberg dithering (see above)

                int* err = stackalloc int[8];
                int* ep1 = err, ep2 = err + 4;

                //int err[8],*ep1 = err,*ep2 = err+4;
                int* dp = dots;

                c0Point <<= 4;
                halfPoint <<= 4;
                c3Point <<= 4;
                for (int i = 0; i < 8; i++)
                    err[i] = 0;

                for (int y = 4; y-- > 0; )
                {
                    int dot, lmask, step;

                    // pixel 0
                    dot = (dp[0] << 4) + (3 * ep2[1] + 5 * ep2[0]);
                    if (dot < halfPoint)
                        step = (dot < c0Point) ? 1 : 3;
                    else
                        step = (dot < c3Point) ? 2 : 0;

                    ep1[0] = dp[0] - stops[step];
                    lmask = step << 6;

                    // pixel 1
                    dot = (dp[1] << 4) + (7 * ep1[0] + 3 * ep2[2] + 5 * ep2[1] + ep2[0]);
                    if (dot < halfPoint)
                        step = (dot < c0Point) ? 1 : 3;
                    else
                        step = (dot < c3Point) ? 2 : 0;

                    ep1[1] = dp[1] - stops[step];
                    lmask |= step << 4;

                    // pixel 2
                    dot = (dp[2] << 4) + (7 * ep1[1] + 3 * ep2[3] + 5 * ep2[2] + ep2[1]);
                    if (dot < halfPoint)
                        step = (dot < c0Point) ? 1 : 3;
                    else
                        step = (dot < c3Point) ? 2 : 0;

                    ep1[2] = dp[2] - stops[step];
                    lmask |= step << 2;

                    // pixel 3
                    dot = (dp[3] << 4) + (7 * ep1[2] + 5 * ep2[3] + ep2[2]);
                    if (dot < halfPoint)
                        step = (dot < c0Point) ? 1 : 3;
                    else
                        step = (dot < c3Point) ? 2 : 0;

                    ep1[3] = dp[3] - stops[step];
                    lmask |= step;

                    // advance to next line
                    int* t = ep1; ep1 = ep2; ep2 = t;
                    //sSwap(ep1,ep2);
                    dp += 4;
                    mask |= lmask << (y << 3);
                }
            }

            return (uint)mask;
        }

        // The refinement function. (Clever code, part 2)
        // Tries to optimize colors to suit block contents better.
        // (By solving a least squares system via normal equations+Cramer's rule)
        static readonly int[] w1Tab = new int[] { 3, 0, 2, 1 };
        static readonly int[] prods = new int[] { 0x090000, 0x000900, 0x040102, 0x010402 };
        private static bool RefineBlock(ARGBPixel* block, ushort* max16, ushort* min16, uint mask)
        {
            //static const sInt w1Tab[4] = { 3,0,2,1 };
            //static const sInt prods[4] = { 0x090000,0x000900,0x040102,0x010402 };
            // ^some magic to save a lot of multiplies in the accumulating loop...

            int akku = 0;
            int At1_r, At1_g, At1_b;
            int At2_r, At2_g, At2_b;
            uint cm = mask;

            At1_r = At1_g = At1_b = 0;
            At2_r = At2_g = At2_b = 0;
            for (int i = 0; i < 16; i++, cm >>= 2)
            {
                int step = (int)cm & 3;
                int w1 = w1Tab[step];
                int r = block[i].R;
                int g = block[i].G;
                int b = block[i].B;

                akku += prods[step];
                At1_r += w1 * r;
                At1_g += w1 * g;
                At1_b += w1 * b;
                At2_r += r;
                At2_g += g;
                At2_b += b;
            }

            At2_r = 3 * At2_r - At1_r;
            At2_g = 3 * At2_g - At1_g;
            At2_b = 3 * At2_b - At1_b;

            // extract solutions and decide solvability
            int xx = akku >> 16;
            int yy = (akku >> 8) & 0xff;
            int xy = (akku >> 0) & 0xff;

            if ((yy == 0) || (xx == 0) || (xx * yy == xy * xy))
                return false;

            float frb = 3.0f * 31.0f / 255.0f / (xx * yy - xy * xy);
            float fg = frb * 63.0f / 31.0f;

            ushort oldMin = *min16;
            ushort oldMax = *max16;

            // solve.
            *max16 = (ushort)((int)Math.Min(Math.Max((At1_r * yy - At2_r * xy) * frb + 0.5f, 0), 31) << 11);

            *max16 |= (ushort)((int)Math.Min(Math.Max((At1_g * yy - At2_g * xy) * fg + 0.5f, 0), 63) << 5);
            *max16 |= (ushort)((int)Math.Min(Math.Max((At1_b * yy - At2_b * xy) * frb + 0.5f, 0), 31) << 0);

            *min16 = (ushort)((int)Math.Min(Math.Max((At2_r * xx - At1_r * xy) * frb + 0.5f, 0), 31) << 11);
            *min16 |= (ushort)((int)Math.Min(Math.Max((At2_g * xx - At1_g * xy) * fg + 0.5f, 0), 63) << 5);
            *min16 |= (ushort)((int)Math.Min(Math.Max((At2_b * xx - At1_b * xy) * frb + 0.5f, 0), 31) << 0);

            return (oldMin != *min16) || (oldMax != *max16);
        }

        static void EvalColors(ARGBPixel* color, ushort c0, ushort c1)
        {
            color[0] = From16Bit(c0);
            color[1] = From16Bit(c1);
            color[2] = LerpRGB(color[0], color[1], 0x55);
            color[3] = LerpRGB(color[0], color[1], 0xaa);
        }

        static ushort As16Bit(ARGBPixel c)
        {
            return (ushort)((Mul8Bit(c.R, 31) << 11) + (Mul8Bit(c.G, 63) << 5) + Mul8Bit(c.B, 31));
        }

        private static ARGBPixel From16Bit(ushort v)
        {
            int rv = (v & 0xf800) >> 11;
            int gv = (v & 0x07e0) >> 5;
            int bv = (v & 0x001f) >> 0;

            return new ARGBPixel(0, Expand5[rv], Expand6[gv], Expand5[bv]);
        }

        private static ARGBPixel LerpRGB(ARGBPixel p1, ARGBPixel p2, int f)
        {
            return new ARGBPixel(0, (byte)(p1.R + Mul8Bit(p2.R - p1.R, f)), (byte)(p1.G + Mul8Bit(p2.G - p1.G, f)), (byte)(p1.B + Mul8Bit(p2.B - p1.B, f)));
        }
    }
}
