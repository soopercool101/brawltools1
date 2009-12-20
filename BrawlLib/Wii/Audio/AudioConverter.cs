using System;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;
using System.Audio;

namespace BrawlLib.Wii.Audio
{
    public unsafe class AudioConverter
    {
        //public static void EncodeADPCM(IAudioStream source)
        //{
        //    //Handle left and right channels separately

        //    int samples = source.Samples;
        //    int blocks = (samples + 0x37FF) / 0x3800;

        //    source.SamplePosition = 0;
        //    short* left = (short*)Marshal.AllocHGlobal(samples * 2);
        //    short* right = (short*)Marshal.AllocHGlobal(samples * 2);
        //    uint sample;
        //    for (int i = 0; i < samples; i++)
        //    {
        //        source.ReadSamples(&sample, 1);
        //        left[i] = (short)(sample >> 16);
        //        right[i] = (short)(sample & 0xFFFF);
        //    }

        //    short[,] coefsl = CalcCoefs(left, samples);
        //    short[,] coefsr = CalcCoefs(right, samples);

        //    Marshal.FreeHGlobal((IntPtr)left);
        //    Marshal.FreeHGlobal((IntPtr)right);
        //}

        //private static short[,] CalculateCoefs(short* source, int samples)
        //{
        //    short[,] coefs = new short[8,2];

        //    int channels = 2;
        //    int numBlocks = (samples + 13) / 14;
        //    int unusedCounter = 0;

        //    int x;

        //    short* sampleBuffer = stackalloc short[0x3800];

        //    short* blockBuffer = stackalloc short[14 * channels];
        //    double* doubleBuffer = stackalloc double[channels];
        //    double* channelMix = stackalloc double[(channels + 1) * (channels + 1)];
        //    int* channelPtrs = stackalloc int[channels + 1];
        //    double** pChannels = (double**)&channelPtrs;
        //    for (int i = 0; i < channels + 1; i++)
        //        pChannels[i] = channelMix + ((channels + 1) * i);

        //    int* anotherBuffer= stackalloc int[channels + 1];
        //    double* omgBuffer = stackalloc double[channels + 1];

        //    short* pBlock1 = blockBuffer, pBlock2 = blockBuffer + 14;

        //    int multiIndex = 0;
        //    //int* multiBufferData = stackalloc int[numBlocks];
        //    double** multiBuffer = (double**)Marshal.AllocHGlobal(numBlocks * 4);

        //    int* bufferArrayData = stackalloc int[8];
        //    double** bufferArray = (double**)bufferArrayData;
        //    for (int i = 0; i < 8; i++)
        //        bufferArray[i] = (double*)Marshal.AllocHGlobal((channels + 1) * 8);

        //    double* buffer2 = stackalloc double[channels + 1];

        //    for (int i = 0; i < 14; i++)
        //        pBlock2[i] = 0;

        //    for (int i = 0; i < samples; i += 0x3800)
        //    {
        //        int blockSamples = Math.Min(samples - i, 0x3800);

        //        //Fill buffer
        //        for (x = 0; x < blockSamples; x++)
        //            sampleBuffer[x] = *source++;
        //        //Zero remaining
        //        while (x < 0x3800)
        //            sampleBuffer[x++] = 0;

        //        for (x = 0; x < 0x3800; x += 14)
        //        {
        //            //copy buffer contents
        //            for (int y = 0; y < 14; y++)
        //            {
        //                pBlock1[y] = pBlock2[y];
        //                pBlock2[y] = sampleBuffer[x + y];
        //            }

        //            //Calculate something
        //            Something1(pBlock2, channels, 14, doubleBuffer);

        //            if (Math.Abs(doubleBuffer[0]) > 10.0)
        //            {
        //                int temp;
        //                Something2(pBlock2, channels, 14, pChannels);
        //                if (!Something3(pChannels, channels, anotherBuffer, &temp))
        //                {
        //                    Something4(pChannels, channels, anotherBuffer, doubleBuffer);
        //                    doubleBuffer[0] = 1.0;
        //                    if (Something5(doubleBuffer, omgBuffer, channels) == 0)
        //                    {
        //                        multiBuffer[multiIndex] = (double*)Marshal.AllocHGlobal((channels + 1) * 8);
        //                        multiBuffer[multiIndex][0] = 1.0;

        //                        for (int z = 1; z <= channels; z++)
        //                        {
        //                            if(omgBuffer[z] >= 1.0)
        //                                omgBuffer[z] = 0.9999999999;
        //                            if(omgBuffer[z] <= -1.0)
        //                                omgBuffer[z] = -0.9999999999;
        //                        }

        //                        Something6(omgBuffer, multiBuffer[multiIndex], channels);
        //                        multiIndex++;
        //                    }
        //                }
        //            }
        //            unusedCounter++;
        //        }
        //    }

        //    doubleBuffer[0] = 1.0;
        //    for (int i = 1; i <= channels; i++)
        //        doubleBuffer[i] = 0.0;

        //    for (int i = 0; i < multiIndex; i++)
        //    {
        //        Something7(multiBuffer[i], channels, bufferArray[0]);
        //        for (x = 1; x <= channels; x++)
        //            doubleBuffer[x] += bufferArray[0][x];
        //    }

        //    for (int i = 1; i <= channels; i++)
        //        doubleBuffer[i] /= multiIndex;

        //    double tempVar;
        //    Something8(doubleBuffer, channels, omgBuffer, bufferArray[0], &tempVar);
        //    for (int i = 1; i <= channels; i++)
        //    {
        //        if (omgBuffer[i] >= 1.0)
        //            omgBuffer[i] = 0.9999999999;
        //        if (omgBuffer[i] <= -1.0)
        //            omgBuffer[i] = -0.9999999999;
        //    }

        //    Something6(omgBuffer, bufferArray[0], channels);
        //    for (int i = 0; i < 3; )
        //    {
        //        for (x = 0; x <= channels; x++)
        //            buffer2[x] = 0.0;
        //        buffer2[channels - 1] = -1.0;

        //        Something9(bufferArray, buffer2, channels, 1 << i++, 0.01);
        //        Something10(bufferArray, channels, 1 << i, multiBuffer, multiIndex, 2, 0.0);
        //    }

        //    for (int i = 0; i < 8; i++)
        //    {
        //        for (int y = 0; y < channels; y++)
        //        {
        //            double d = -bufferArray[i][y + 1] * 2048;
        //            if (d > 0.0)
        //            {
        //                if (d > 32767)
        //                    coefs[i, y] = 32767;
        //                else
        //                    coefs[i, y] = (short)(d + 0.5);
        //            }
        //            else
        //            {
        //                if (d < -32768)
        //                    coefs[i, y] = -32768;
        //                else
        //                    coefs[i, y] = (short)(d - 0.5);
        //            }
        //        }
        //    }

        //    //free memory

        //    for (int i = 0; i < 8; i++)
        //        Marshal.FreeHGlobal((IntPtr)bufferArray[i]);

        //    for (int i = 0; i < multiIndex; i++)
        //        Marshal.FreeHGlobal((IntPtr)multiBuffer[i]);

        //    Marshal.FreeHGlobal((IntPtr)multiBuffer);

        //    return coefs;
        //}

        public static void CalcCoefs(short* source, int samples, short* dest)
        {
            //short[,] coefs = new short[8,2];

            //double d10 = 10.0;
            //int channels = 2;
            //int num2 = 2;
            //int chunkSamples = 14;
            //int var30 = 0;
            short* sPtr = source;
            int numBlocks = (samples + 13) / 14;

            int nBits = 3;
            //while ((1 << ++nBits) < 8) ;

            int* bufferArrayData = stackalloc int[8];
            double** bufferArray = (double**)bufferArrayData;
            for (int z = 0; z < 8; z++)
                bufferArray[z] = (double*)Marshal.AllocHGlobal(3 * 8);

            double* buffer2 = stackalloc double[3];
            short* sampleBuffer = (short*)Marshal.AllocHGlobal(0x7000);
            short* chunkBuffer = stackalloc short[28];
            //double** chunkBuffer = (double**)chunkBufferData;
            for (int z = 0; z < 28; z++)
                chunkBuffer[z] = 0;

            double* sChunkBuffer = stackalloc double[3];
            double* omgBuffer = stackalloc double[3];

            int* pChannelData = stackalloc int[3];
            double** pChannels = (double**)pChannelData;
            for (int z = 0; z <= 2; z++)
                pChannels[z] = (double*)Marshal.AllocHGlobal(3 * 8);

            int* anotherBuffer = stackalloc int[3];

            double** multiBuffer = (double**)Marshal.AllocHGlobal(numBlocks * 4 * 2);
            int unused = 0;
            int multiIndex = 0;

            int blockSamples;
            int temp;
            for (int x = samples; x > 0; )
            {
                if (x > 0x3800)
                {
                    blockSamples = 0x3800;
                    x -= 0x3800;
                }
                else
                {
                    blockSamples = x;
                    for (int z = 0; (z < 14) && ((z + blockSamples) < 0x3800); z++)
                        sampleBuffer[blockSamples + z] = 0;
                    x = 0;
                }

                short* tPtr = sampleBuffer;
                for (int z = 0; z < blockSamples; z++)
                    *tPtr++ = *sPtr++;

                for (int i = 0; i < blockSamples; )
                {
                    for (int z = 0; z < 14; z++)
                        chunkBuffer[z] = chunkBuffer[z + 14];
                    for (int z = 0; z < 14; z++)
                        chunkBuffer[z + 14] = sampleBuffer[i++];

                    Something1(&chunkBuffer[14], sChunkBuffer);
                    if (Math.Abs(sChunkBuffer[0]) > 10.0)
                    {
                        Something2(&chunkBuffer[14], pChannels);
                        if (!Something3(pChannels, anotherBuffer, &temp))
                        {
                            Something4(pChannels, 2, anotherBuffer, sChunkBuffer);
                            sChunkBuffer[0] = 1.0;
                            if (Something5(sChunkBuffer, omgBuffer, 2) == 0)
                            {
                                multiBuffer[multiIndex] = (double*)Marshal.AllocHGlobal(3 * 8);
                                multiBuffer[multiIndex][0] = 1.0;
                                for (int z = 1; z <= 2; z++)
                                {
                                    if(omgBuffer[z] >= 1.0)
                                        omgBuffer[z] = 0.9999999999;
                                    if(omgBuffer[z] <= -1.0)
                                        omgBuffer[z] = -0.9999999999;
                                }
                                Something6(omgBuffer, multiBuffer[multiIndex], 2);
                                multiIndex++;
                            }
                        }
                    }
                    unused++;
                }
            }

            sChunkBuffer[0] = 1.0;
            for (int y = 1; y <= 2; y++)
                sChunkBuffer[y] = 0.0;

            for (int z = 0; z < multiIndex; z++)
            {
                Something7(multiBuffer[z], bufferArray[0]);
                for (int y = 1; y <= 2; y++)
                    sChunkBuffer[y] = sChunkBuffer[y] + bufferArray[0][y];
            }

            for (int y = 1; y <= 2; y++)
                sChunkBuffer[y] /= multiIndex;

            double tmp;
            Something8(sChunkBuffer, 2, omgBuffer, bufferArray[0], &tmp);
            for (int y = 1; y <= 2; y++)
            {
                if (omgBuffer[y] >= 1.0)
                    omgBuffer[y] = 0.9999999999;
                if (omgBuffer[y] <= -1.0)
                    omgBuffer[y] = -0.9999999999;
            }
            Something6(omgBuffer, bufferArray[0], 2);

            for (int w = 0; w < nBits; )
            {
                //int mask = 1 << w;
                for (int z = 0; z <= 2; z++)
                    buffer2[z] = 0.0;
                buffer2[1] = -1.0;
                Something9(bufferArray, buffer2, 2, 1 << w++, 0.01);
                //w++;
                Something10(bufferArray, 2, 1 << w, multiBuffer, multiIndex, 2, 0.0);
            }

            //Write output
            for (int z = 0; z < 8; z++)
                for (int y = 0; y < 2; y++)
                {
                    double d = -bufferArray[z][y + 1] * 2048.0;
                    if (d > 0.0)
                        dest[(z << 1) + y] = (d > 32767.0) ? (short)32767 : (short)(d + 0.5);
                    else
                        dest[(z << 1) + y] = (d < -32768.0) ? (short)-32768 : (short)(d - 0.5);
                }

            //Free memory
            for (int i = 0; i < multiIndex; i++)
                Marshal.FreeHGlobal((IntPtr)multiBuffer[i]);
            Marshal.FreeHGlobal((IntPtr)multiBuffer);

            for (int i = 0; i <= 2; i++)
                Marshal.FreeHGlobal((IntPtr)pChannels[i]);

            for (int i = 0; i < 8; i++)
                Marshal.FreeHGlobal((IntPtr)bufferArray[i]);

            Marshal.FreeHGlobal((IntPtr)sampleBuffer);

            //return coefs;
        }

        public static unsafe void EncodeBlock(short* source, int samples, byte* dest, short* coefs)
        {
            int x, y;
            short* blockBuffer = stackalloc short[16];

            blockBuffer[0] = *source++;
            blockBuffer[1] = *source++;
            for (int i = 0; i < samples; i += 14, dest += 8)
            {
                int chunkSamples = samples - i;
                if (chunkSamples > 14) chunkSamples = 14;

                for (x = 0, y = 2; x < chunkSamples; x++)
                    blockBuffer[y++] = *source++;

                while (y < 16)
                    blockBuffer[y++] = 0;

                EncodeChunk(blockBuffer, dest, coefs, blockBuffer);
            }
        }

        //Make sure source includes the yn values (16 samples total)
        private static unsafe void EncodeChunk(short* source, byte* dest, short* coefs, short* yn)
        {
            //int* sampleBuffer = stackalloc int[14];
            int* buffer1 = stackalloc int[128];
            int* buffer2 = stackalloc int[112];

            long bestDistance = long.MaxValue, distAccum;
            int bestIndex = 0;
            int bestScale = 0;

            int distance, index, scale;

            int* p1, p2, t1, t2;
            short* sPtr;
            int v1, v2, v3;

            //Iterate through each coef set, finding the set with the smallest error
            p1 = buffer1; 
            p2 = buffer2;
            for (int i = 0; i < 8; i++, p1 += 16, p2 += 14, coefs += 2)
            {
                //Set yn values
                t1 = p1;
                *t1++ = source[0];
                *t1++ = source[1];

                //Round and clamp samples for this coef set
                distance = 0;
                sPtr = source;
                for (int y = 0; y < 14; y++)
                {
                    //Multiply previous samples by coefs
                    *t1++ = v1 = ((*sPtr++ * coefs[1]) + (*sPtr++ * coefs[0])) >> 11;
                    //Subtract from current sample
                    v2 = *sPtr-- - v1;
                    //Clamp
                    v3 = (v2 >= 32767) ? 32767 : (v2 <= -32768) ? -32768 : v2;
                    //Compare distance
                    if (Math.Abs(v3) > Math.Abs(distance))
                        distance = v3;
                }

                //Set initial scale
                for (scale = 0; (scale <= 12) && ((distance > 7) || (distance < -8)); scale++, distance >>= 1) ;
                scale = (scale <= 1) ? -1 : scale - 2;

                do
                {
                    scale++;
                    distAccum = 0;
                    index = 0;

                    t1 = p1;
                    t2 = p2;
                    sPtr = source + 2;
                    for (int y = 0; y < 14; y++)
                    {
                        //Multiply previous 
                        v1 = ((*t1++ * coefs[1]) + (*t1++ * coefs[0]));
                        //Evaluate from real sample
                        v2 = ((*sPtr << 11) - v1) / 2048;
                        //Round to nearest sample
                        v3 = (v2 > 0) ? (int)((double)v2 / (1 << scale) + 0.4999999f) : (int)((double)v2 / (1 << scale) - 0.4999999f);

                        //Clamp sample and set index
                        if(v3 < -8)
                        {
                            if (index < (v3 = -8 - v3))
                                index = v3;
                            v3 = -8;
                        }
                        else if (v3 > 7)
                        {
                            if (index < (v3 -= 7))
                                index = v3;
                            v3 = 7;
                        }

                        //Store result
                        *t2++ = v3;

                        //Round and expand
                        v1 = (v1 + ((v3 * (1 << scale)) << 11) + 1024) >> 11;
                        //Clamp and store
                        *t1-- = v2 = (v1 >= 32767) ? 32767 : (v1 <= -32768) ? -32768 : v1;
                        //Accumulate distance
                        v3 = *sPtr++ - v2;
                        distAccum += v3 * v3;

                        //Break if we're higher than a previous search
                        if (distAccum > bestDistance)
                            break;
                    }

                    for (int x = index + 8; x > 256; x >>= 1)
                        if (++scale >= 12)
                            scale = 11;

                } while ((scale < 12) && (index > 1));

                if (distAccum <= bestDistance)
                {
                    bestDistance = distAccum;
                    bestIndex = i;
                    bestScale = scale;
                }
            }

            p1 = buffer1 + (bestIndex << 4) + 14;
            p2 = buffer2 + (bestIndex * 14);

            //Set resulting yn values
            *yn++ = (short)*p1++;
            *yn++ = (short)*p1++;

            //Write ps
            *dest++ = (byte)((bestIndex << 4) | (bestScale & 0xF));

            //Write output samples
            for (int y = 0; y++ < 7; )
                *dest++ = (byte)((*p2++ << 4) | (*p2++ & 0xF));
        }

        private static unsafe void Something10(double** bufferArray, int channels, int mask, double** multiBuffer, int multiIndex, int num2, double val)
        {
            int* bufferListData = stackalloc int[mask];
            double** bufferList = (double**)bufferListData;

            int* buffer1 = stackalloc int[mask];
            double* buffer2 = stackalloc double[channels + 1];

            int index;
            double value, tempVal = 0;

            for (int i = 0; i < mask; i++)
                bufferList[i] = (double*)Marshal.AllocHGlobal(8 * channels + 8);

            for (int x = 0; x < num2; x++)
            {
                for (int y = 0; y < mask; y++)
                {
                    buffer1[y] = 0;
                    for (int i = 0; i <= channels; i++)
                        bufferList[y][i] = 0.0;
                }
                for (int z = 0; z < multiIndex; z++)
                {
                    index = 0;
                    value= 1.0e30;
                    for (int i = 0; i < mask; i++)
                    {
                        tempVal = Something11(bufferArray[i], multiBuffer[z], channels);
                        if (tempVal < value)
                        {
                            value = tempVal;
                            index = i;
                        }
                    }
                    buffer1[index]++;
                    Something7(multiBuffer[z], buffer2);
                    for (int i = 0; i <= channels; i++)
                        bufferList[index][i] += buffer2[i];
                }

                for (int i = 0; i < mask; i++)
                    if(buffer1[i] > 0)
                        for (int y = 0; y <= channels; y++)
                            bufferList[i][y] /= buffer1[i];

                for (int i = 0; i < mask; i++)
                {
                    Something8(bufferList[i], channels, buffer2, bufferArray[i], &tempVal);
                    for (int y = 1; y <= channels; y++)
                    {
                        if(buffer2[y] >= 1.0)
                            buffer2[y] = 0.9999999999;
                        if (buffer2[y] <= -1.0)
                            buffer2[y] = -0.9999999999;
                    }
                    Something6(buffer2, bufferArray[i], channels);
                }
            }

            for (int i = 0; i < mask; i++)
                Marshal.FreeHGlobal((IntPtr)bufferList[i]);
        }

        private static unsafe double Something11(double* source1, double* source2, int channels)
        {
            double* b = stackalloc double[3];
            Something12(source2, channels, b);
            double val1 = (source1[0] * source1[0]) + (source1[1] * source1[1]) + (source1[2] * source1[2]);
            double val2 = (source1[0] * source1[1]) + (source1[1] * source1[2]);
            double val3 = source1[0] * source1[2];
            return (b[0] * val1) + (2.0 * b[1] * val2) + (2.0 * b[2] * val3);
        }

        private static unsafe void Something12(double* source, int channels, double* dest)
        {
            double v1 = 1.0, v2 = -source[1], v3 = -source[2];
            double val = (v3 * v2 + v2) / (1.0 - v3 * v3);

            dest[0] = 1.0;
            dest[1] = val * dest[0];
            dest[2] = (v2 * dest[1]) + (v3 * dest[0]);
        }

        private static unsafe void Something9(double** bufferArray, double* buffer2, int channels, int mask, double value)
        {
            for (int i = 0; i < mask; i++)
                for (int y = 0; y <= channels; y++)
                    bufferArray[mask + i][y] = (value * buffer2[y]) + bufferArray[i][y];
        }

        private static unsafe int Something8(double* src, int channels, double* omgBuffer, double* dst, double* outVar)
        {
            int count = 0;
            double val = src[0];

            dst[0] = 1.0;
            for (int i = 1; i <= channels; i++)
            {
                double v2 = 0.0;
                for (int y = 1; y < i; y++)
                    v2 += dst[y] * src[i - y];

                if (val > 0.0)
                    dst[i] = -(v2 + src[i]) / val;
                else
                    dst[i] = 0.0;

                omgBuffer[i] = dst[i];

                if (Math.Abs(omgBuffer[i]) > 1.0)
                    count++;

                for (int y = 1; y < i; y++)
                    dst[y] += dst[i] * dst[i - y];

                val *= 1.0 - (dst[i] * dst[i]);
            }

            *outVar = val;
            return count;
        }

        private static unsafe void Something7(double* src, double* dst)
        {
            double v1, v2, v3;
            double* buffer = stackalloc double[9];
            //DVector3* p = (DVector3*)buffer;

            //p[2] = new DVector3(1.0, -src[1], -src[2]);
            //for (int i = 2; i > 0; i--)
            //{
            //    //v3 = 1.0 - (v2 = (v1 = p[i][i]) * v1);
            //    v1 = p[i][i];
            //    v2 = v1 * v1;
            //    v3 = 1.0 - v2;
            //    for (int y = 1; y <= i; y++)
            //        p[i - 1][y] = (v2 + v1) / v3;
            //}

            //dst[0] = 1.0;
            //for (int i = 1; i <= 2; i++)
            //{
            //    dst[i] = 0.0;
            //    for (int y = 1; y <= i; y++)
            //        dst[i] += p[i][y] * dst[i - y];
            //}
      
            buffer[2 * 3] = 1.0;
            for (int i = 1; i <= 2; i++)
                buffer[2 * 3 + i] = -src[i];

            for (int i = 2; i > 0; i--)
            {
                double val = 1.0 - (buffer[i * 3 + i] * buffer[i * 3 + i]);
                for (int y = 1; y <= i; y++)
                    buffer[(i - 1) * 3 + y] = ((buffer[i * 3 + i] * buffer[i * 3 + y]) + buffer[i * 3 + y]) / val;
            }

            dst[0] = 1.0;
            for (int i = 1; i <= 2; i++)
            {
                dst[i] = 0.0;
                for (int y = 1; y <= i; y++)
                    dst[i] += buffer[i * 3 + y] * dst[i - y];
            }
        }


        private static void Something1(short* source, double* dest)
        {
            for (int i = 0; i <= 2; i++)
            {
                dest[i] = 0.0f;
                for (int x = 0; x < 14; x++)
                    dest[i] -= source[x - i] * source[x];
            }
        }

        private static void Something2(short* source, double** outList)
        {
            for (int x = 1; x <= 2; x++)
                for (int y = 1; y <= 2; y++)
                {
                    outList[x][y] = 0.0;
                    for (int z = 0; z < 14; z++)
                        outList[x][y] += source[z - x] * source[z - y];
                }
        }

        private static bool Something3(double** outList, int* dest, int* unk)
        {
            double* buffer = stackalloc double[3];
            double val, tmp, min, max;

            *unk = 1;

            //Get greatest distance from zero
            for (int x = 1; x <= 2; x++)
            {
                val = 0.0;
                for (int i = 1; i <= 2; i++)
                {
                    tmp = Math.Abs(outList[x][i]);
                    if (tmp > val)
                        val = tmp;
                }
                if (val == 0.0)
                    return true;

                buffer[x] = 1.0 / val;
            }

            int maxIndex = 0;
            for (int i = 1; i <= 2; i++)
            {
                for (int x = 1; x < i ; x++)
                {
                    tmp = outList[x][i];
                    for (int y = 1; y < x; y++)
                        tmp -= outList[x][y] * outList[y][i];
                    outList[x][i] = tmp;
                }

                val = 0.0;
                for (int x = i; x <= 2; x++)
                {
                    tmp = outList[x][i];
                    for (int y = 1; y < i; y++)
                        tmp -= outList[x][y] * outList[y][i];

                    outList[x][i] = tmp;
                    tmp = Math.Abs(tmp) * buffer[x];
                    if (tmp >= val)
                    {
                        val = tmp;
                        maxIndex = x;
                    }
                }

                if (maxIndex == i)
                {
                    for (int y = 1; y <= 2; y++)
                    {
                        tmp = outList[maxIndex][y];
                        outList[maxIndex][y] = outList[i][y];
                        outList[i][y] = tmp;
                    }
                    *unk = -*unk;
                    buffer[maxIndex] = buffer[i];
                }

                dest[i] = maxIndex;

                if (outList[i][i] == 0.0)
                    return true;

                if (i != 2)
                {
                    tmp = 1.0 / outList[i][i];
                    for (int x = i + 1; x <= 2; x++)
                        outList[x][i] *= tmp;
                }
            }
            //no need for buffer anymore

            //Get range
            min = 1.0e10;
            max = 0.0;
            for (int i = 1; i <= 2; i++)
            {
                tmp = Math.Abs(outList[i][i]);
                if (tmp < min)
                    min = tmp;
                if (tmp > max)
                    max = tmp;
            }

            if (min / max < 1.0e-10)
                return true;

            return false;
        }

        private static void Something4(double** outList, int num, int* dest, double* block)
        {
            int index;
            double tmp;

            for (int i = 1, x = 0; i <= num; i++)
            {
                index = dest[i];
                tmp = block[index];
                block[index] = block[i];
                if (x != 0)
                    for (int y = x; y <= i - 1; y++)
                        tmp -= block[y] * outList[i][y];
                else if(tmp != 0.0)
                    x = i;
                block[i] = tmp;
            }

            for(int i = num; i > 0 ; i--)
            {
                tmp = block[i];
                for (int y = i + 1; y <= num; y++)
                    tmp -= block[y] * outList[i][y];
                block[i] = tmp / outList[i][i];
            }
        }

        private static int Something5(double* block, double* buffer, int num)
        {
            int count = 0;
            double* dBuffer = stackalloc double[num + 1];

            buffer[num] = block[num];

            for (int i = num - 1; i > 0; i--)
            {
                for (int x = 0; x <= i; x++)
                {
                    double dTemp = 1.0 - (buffer[i + 1] * buffer[i + 1]);
                    if (dTemp == 0.0)
                        return 1;

                    dBuffer[x] = (block[x] - (block[i + 1 - x] * buffer[i + 1])) / dTemp;
                }

                for (int x = 0; x <= i; x++)
                    block[x] = dBuffer[x];

                buffer[i] = dBuffer[i];
                if (Math.Abs(buffer[i]) > 1.0)
                    count++;
            }

            return count;
        }

        private static unsafe void Something6(double* omgBuffer, double* buffer, int channels)
        {
            buffer[0] = 1.0;

            for (int i = 1; i <= channels; i++)
            {
                buffer[i] = omgBuffer[i];
                for (int x = 1; x < i; x++)
                    buffer[x] = (buffer[i] * buffer[i - x]) + buffer[x];
            }
        }
    }
}
