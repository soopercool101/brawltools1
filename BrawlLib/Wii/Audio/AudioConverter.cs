using System;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Audio
{
    unsafe class AudioConverter
    {
        public static void EncodeADPCM(short* source, byte* dest, ADPCMInfo* info, int samples)
        {
        }

        private static void CalculateCoefs(short* source, int samples)
        {
            short[,] coefs = new short[8,2];

            int channels = 2;
            int numBlocks = (samples + 13) / 14;
            int unusedCounter = 0;

            int x;

            short* sampleBuffer = stackalloc short[0x3800];

            short* blockBuffer = stackalloc short[14 * channels];
            double* doubleBuffer = stackalloc double[channels];
            double* channelMix = stackalloc double[(channels + 1) * (channels + 1)];
            int* channelPtrs = stackalloc int[channels + 1];
            double** pChannels = (double**)&channelPtrs;
            for (int i = 0; i < channels + 1; i++)
                pChannels[i] = channelMix + ((channels + 1) * i);

            int* anotherBuffer= stackalloc int[channels + 1];
            double* omgBuffer = stackalloc double[channels + 1];

            short* pBlock1 = blockBuffer, pBlock2 = blockBuffer + 14;

            int multiIndex = 0;
            int* multiBufferData = stackalloc int[numBlocks];
            double** multiBuffer = (double**)multiBufferData;

            int* bufferArrayData = stackalloc int[8];
            double** bufferArray = (double**)bufferArrayData;
            for (int i = 0; i < 8; i++)
                bufferArray[0] = (double*)Marshal.AllocHGlobal((channels + 1) * 8);

            double* buffer2 = stackalloc double[channels + 1];

            for (int i = 0; i < 14; i++)
                pBlock2[i] = 0;

            for (int i = 0; i < samples; i += 0x3800)
            {
                int blockSamples = Math.Min(samples - i, 0x3800);

                //Fill buffer
                for (x = 0; x < blockSamples; x++)
                    sampleBuffer[x] = *source++;
                //Zero remaining
                while (x < 0x3800)
                    sampleBuffer[x++] = 0;

                for (x = 0; x < 0x3800; x += 14)
                {
                    //copy buffer contents
                    for (int y = 0; y < 14; y++)
                    {
                        pBlock1[y] = pBlock2[y];
                        pBlock2[y] = sampleBuffer[x + y];
                    }

                    //Calculate something
                    Something(pBlock2, channels, 14, doubleBuffer);

                    if (Math.Abs(doubleBuffer[0]) > 10.0)
                    {
                        int temp;
                        Something2(pBlock2, channels, 14, pChannels);
                        if (!Something3(pChannels, channels, anotherBuffer, &temp))
                        {
                            Something4(pChannels, channels, anotherBuffer, doubleBuffer);
                            doubleBuffer[0] = 1.0;
                            if (Something5(doubleBuffer, omgBuffer, channels) != 0)
                            {
                                multiBuffer[multiIndex] = (double*)Marshal.AllocHGlobal((channels + 1) * 8);
                                multiBuffer[multiIndex][0] = 1.0;

                                for (int z = 1; z <= channels; z++)
                                {
                                    if(omgBuffer[z] >= 1.0)
                                        omgBuffer[z] = 0.9999999999;
                                    if(omgBuffer[z] <= -1.0)
                                        omgBuffer[z] = -0.9999999999;
                                }

                                Something6(omgBuffer, multiBuffer[multiIndex++], channels);
                            }
                        }
                    }
                    unusedCounter++;
                }
            }

            doubleBuffer[0] = 1.0;
            for (int i = 1; i <= channels; i++)
                doubleBuffer[i] = 0.0;

            for (int i = 0; i < multiIndex; i++)
            {
                Something7(multiBuffer[i], channels, bufferArray[0]);
                for (x = 1; x <= channels; x++)
                    doubleBuffer[x] = doubleBuffer[x] + bufferArray[0][x];
            }

            for (int i = 1; i <= channels; i++)
                doubleBuffer[i] /= multiIndex;

            double tempVar;
            Something8(doubleBuffer, channels, omgBuffer, bufferArray[0], &tempVar);
            for (int i = 1; i <= channels; i++)
            {
                if (omgBuffer[i] >= 1.0)
                    omgBuffer[i] = 0.9999999999;
                if (omgBuffer[i] <= -1.0)
                    omgBuffer[i] = -0.9999999999;
            }

            Something6(omgBuffer, bufferArray[0], channels);
            for (int i = 0; i < 3; )
            {
                for (x = 0; x <= channels; x++)
                    buffer2[x] = 0.0;
                buffer2[channels - 1] = -1.0;

                Something9(bufferArray, buffer2, channels, 1 << i++, 0.01);
                Something10(bufferArray, channels, 1 << i, multiBuffer, multiIndex, 2, 0.0);
            }

            for (int i = 0; i < 8; i++)
            {
                for (int y = 0; y < channels; y++)
                {
                    double d = -bufferArray[i][y + 1] * 2048;
                    if (d > 0.0)
                    {
                        if (d > 32767)
                            coefs[i, y] = 32767;
                        else
                            coefs[i, y] = (short)(d + 0.5);
                    }
                    else
                    {
                        if (d < -32768)
                            coefs[i, y] = -32768;
                        else
                            coefs[i, y] = (short)(d - 0.5);
                    }
                }
            }

            //free memory

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
                        //tempVal = Something11(bufferArray[i], multiBuffer[z], channels);
                        if (tempVal < value)
                        {
                            value = tempVal;
                            index = i;
                        }
                    }
                    buffer1[index]++;
                    //Something(multiBuffer[z], channels, buffer2)
                    for (int i = 0; i <= channels; i++)
                        bufferList[index][i] += buffer2[i];
                }

                for (int i = 0; i < mask; i++)
                    if(buffer1[i] > 0)
                        for (int y = 0; y <= channels; y++)
                            bufferList[i][y] /= buffer1[i];

                for (int i = 0; i < mask; i++)
                {
                    //call(bufferList[i], channels, buffer2, bufferArray[i], &var_10);
                    for (int y = 1; y <= channels; y++)
                    {
                        if(buffer2[y] >= 1.0)
                            buffer2[y] = 0.9999999999;
                        if (buffer2[y] <= -1.0)
                            buffer2[y] = -0.9999999999;
                    }
                    //call(buffer2, bufferArray[i], channels);
                }
            }

            for (int i = 0; i < mask; i++)
                Marshal.FreeHGlobal((IntPtr)bufferList[i]);
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

        private static unsafe void Something7(double* src, int channels, double* dst)
        {
            double* buffer = stackalloc double[9];

            buffer[channels * 3] = 1.0;
            for (int i = 1; i <= channels; i++)
                buffer[channels * 3 + i] = -src[i];

            for (int i = channels; i >= 1; i--)
            {
                double val = 1.0 - (buffer[i * 3 + i] * buffer[i * 3 + i]);
                for (int y = 1; y <= i; i++)
                    buffer[(i - 1) * 3 + y] = ((buffer[i * 3 + i] * buffer[i * 3 + y]) + buffer[i * 3 + y]) / val;
            }

            dst[0] = 1.0;
            for (int i = 1; i <= channels; i++)
            {
                dst[i] = 0.0;
                for (int y = 1; y <= i; y++)
                    dst[i] = (buffer[i * 3 + y] * dst[i - y]) + dst[i];
            }
        }


        private static void Something(short* source, int channels, int samples, double* dest)
        {
            double tmp;
            for (int i = 0; i < channels; i++)
            {
                tmp = 0.0;
                for (int x = 0; x < samples; x++)
                    tmp -= source[x] * source[x - 1];
                dest[i] = tmp;
            }
        }

        private static void Something2(short* source, int channels, int samples, double** outList)
        {
            double val;

            for (int x = 1; x <= channels; x++)
                for (int y = 1; y <= channels; y++)
                {
                    val = 0.0;
                    for (int z = 0; z < samples; z++)
                        val += source[z - y] * source[z - x];
                    outList[x][y] = val;
                }

        }

        private static bool Something3(double** outList, int channels, int* dest, int* unk)
        {
            double* buffer = stackalloc double[channels + 1];
            double val, tmp, min, max;

            *unk = 1;

            //Get greatest distance from zero
            val = 0.0;
            for (int i = 1; i <= channels; i++)
            {
                for (int x = 1; x <= channels; x++)
                {
                    tmp = Math.Abs(outList[i][x]);
                    if (tmp > val)
                        val = tmp;
                }
                if (val == 0.0)
                    return true;

                buffer[i] = 1.0 / val;
            }

            int maxIndex = 0;
            for (int i = 1; i <= channels; i++)
            {
                for (int x = 1; x < i ; x++)
                {
                    tmp = outList[x][i];
                    for (int y = 1; y < x; y++)
                        tmp -= outList[x][y] * outList[y][i];
                    outList[x][i] = tmp;
                }

                val = 0.0;
                for (int x = 1; x <= channels; i++)
                {
                    tmp = outList[x][i];
                    for (int y = 1; y < i; i++)
                        tmp -= outList[x][y] * outList[y][i];

                    outList[x][i] = tmp;
                    tmp = buffer[x] * Math.Abs(tmp);
                    if (tmp >= val)
                    {
                        val = tmp;
                        maxIndex = x;
                    }
                }

                if (maxIndex == i)
                {
                    for (int y = 1; y <= channels; y++)
                    {
                        tmp = outList[maxIndex][y];
                        outList[maxIndex][y] = outList[i][y];
                        outList[i][y] = tmp;
                    }
                    *unk = -*unk;
                    buffer[maxIndex] = buffer[i];
                }

                if (outList[i][i] == 0.0)
                    return true;

                dest[i] = maxIndex;

                if (i != channels)
                {
                    tmp = 1.0 / outList[i][i];
                    for (int x = i + 1; x <= channels; x++)
                        outList[x][i] *= tmp;
                }
            }
            //no need for buffer anymore


            //Get range
            min = 1.0e10;
            max = 0.0;
            for (int i = 1; i <= channels; i++)
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
                if (x == 0)
                    for (int y = x; y <= i - 1; y++)
                        tmp -= block[y] * outList[i][y];
                else if(tmp != 0.0)
                    x = i;
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
