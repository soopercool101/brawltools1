using System;
using BrawlLib.IO;
using System.Audio;
using BrawlLib.SSBBTypes;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Audio
{
    public static class RSTMConverter
    {
        public static unsafe FileMap Encode(IAudioStream stream)
        {
            int tmp;
            bool looped = stream.IsLooping;
            int channels = stream.Channels;
            int samples = looped ? stream.LoopEndSample : stream.Samples; //Set sample size to end sample. That way the audio gets cut off when encoding.
            int blocks = (samples + 0x37FF) / 0x3800;
            int sampleRate = stream.Frequency;
            int lbSamples, lbSize, lbTotal;

            //Initialize stream info
            if ((tmp = samples % 0x3800) != 0)
            {
                lbSamples = tmp;
                lbSize = tmp / 14 * 8;
                if ((tmp = lbSamples % 14) != 0)
                    lbSize += (tmp + 1) / 2 + 1;
                lbTotal = lbSize.Align(0x20);
            }
            else
            {
                lbSamples = 0x3800;
                lbTotal = lbSize = 0x2000;
            }

            //Get section sizes
            int rstmSize = 0x40;
            int headSize = (0x68 + (channels * 0x40)).Align(0x20);
            int adpcSize = ((blocks - 1) * 4 * channels + 0x10).Align(0x20);
            int dataSize = ((blocks - 1) * 0x2000 + lbTotal) * channels + 0x20;

            //Create file map
            FileMap map = FileMap.FromTempFile(rstmSize + headSize + adpcSize + dataSize);

            //Get section pointers
            RSTMHeader* rstm = (RSTMHeader*)map.Address;
            HEADHeader* head = (HEADHeader*)((int)rstm + rstmSize);
            ADPCHeader* adpc = (ADPCHeader*)((int)head + headSize);
            RSTMDATAHeader* data = (RSTMDATAHeader*)((int)adpc + adpcSize);

            //Initialize sections
            rstm->Set(headSize, adpcSize, dataSize);
            head->Set(headSize, channels);
            adpc->Set(adpcSize);
            data->Set(dataSize);

            //Set HEAD data
            HEADPart1* part1 = head->Part1;
            part1->_format = new AudioFormatInfo(2, (byte)(looped ? 1 : 0), (byte)channels, 0);
            part1->_sampleRate = (ushort)sampleRate;
            part1->_unk1 = 0;
            part1->_loopStartSample = looped ? stream.LoopStartSample : 0;
            part1->_numSamples = samples;
            part1->_dataOffset = rstmSize + headSize + adpcSize + 0x20;
            part1->_numBlocks = blocks;
            part1->_blockSize = 0x2000;
            part1->_samplesPerBlock = 0x3800;
            part1->_lastBlockSize = lbSize;
            part1->_lastBlockSamples = lbSamples;
            part1->_lastBlockTotal = lbTotal;
            part1->_unk8 = 0x3800;
            part1->_bitsPerSample = 4;
            
            //Create one ADPCMInfo for each channel
            int* adpcData = stackalloc int[channels];
            ADPCMInfo** pAdpcm = (ADPCMInfo**)adpcData;
            for (int i = 0; i < channels; i++)
                *(pAdpcm[i] = head->GetChannelInfo(i)) = new ADPCMInfo();

            //Create buffer for each channel
            int* bufferData = stackalloc int[channels];
            short** channelBuffers = (short**)bufferData;
            for (int i = 0; i < channels; i++)
            {
                channelBuffers[i] = (short*)Marshal.AllocHGlobal(samples * 2 + 4) + 2; //Add two samples for initial yn values
                channelBuffers[i][-2] = channelBuffers[i][-1] = 0; //Clear initial yn
            }

            //Fill buffers
            stream.SamplePosition = 0;
            short* sampleBuffer = stackalloc short[channels];
            for (int i = 0; i < samples; i++)
            {
                stream.ReadSamples(sampleBuffer, 1);
                for (int x = 0; x < channels; x++)
                    channelBuffers[x][i] = sampleBuffer[x];
            }

            //Calculate coefs
            for (int i = 0; i < channels; i++)
                AudioConverter.CalcCoefs(channelBuffers[i], samples, (short*)pAdpcm[i]);

            //Encode blocks
            byte* dPtr = (byte*)data->Data;
            bshort* pyn = (bshort*)adpc->Data;
            for (int sIndex = 0, bIndex = 1; sIndex < samples; sIndex += 0x3800, bIndex++)
            {
                int blockSamples = Math.Min(samples - sIndex, 0x3800);
                for(int x = 0 ; x < channels ; x++)
                {
                    short* sPtr = channelBuffers[x] + sIndex;

                    //Set block yn values
                    if (bIndex > 1)
                    {
                        *pyn++ = sPtr[1];
                        *pyn++ = sPtr[0];
                    }

                    //Encode block (need to deal with yn)
                    AudioConverter.EncodeBlock(sPtr, blockSamples, dPtr, (short*)pAdpcm[x]);

                    //Advance output pointer
                    dPtr += (bIndex == blocks) ? lbTotal : 0x2000;
                }
            }

            //Reverse coefs
            for (int i = 0; i < channels; i++)
            {
                short* p = pAdpcm[i]->_coefs;
                for (int x = 0; x < 16; x++, p++)
                    *p = p->Reverse();
            }

            //Free memory
            for (int i = 0; i < channels; i++)
                Marshal.FreeHGlobal((IntPtr)(channelBuffers[i] - 2));

            //Write loop states
            if (looped)
            {
                int sample = stream.LoopStartSample;
                if (sample != 0) //Only need to set states if loop start > 0
                {
                    //Can't we just use block states?

                    //The loop state is set to the last two encoded samples. 
                    //This means we will have to decode the stream until we reach the specified sample.
                    using (ADPCMStream str = new ADPCMStream(rstm))
                    {
                        //Read yn samples into buffer
                        short* buffer = stackalloc short[4];
                        str.SamplePosition = sample - 2;
                        str.ReadSamples(buffer, 2);

                        //Get ps values and write states
                        int block = sample / 0x3800;
                        int chunk = (sample - (block * 0x3800)) / 14;

                        byte* bPtr = (byte*)data->Data + (block * 0x2000 * channels) + (chunk * 8);
                        int bOffset = (block == blocks - 1) ? lbSize : 0x2000;
                        for (int i = 0; i < channels; i++, bPtr += bOffset)
                        {
                            pAdpcm[i]->_ps = *bPtr;
                            pAdpcm[i]->_yn2 = buffer[i];
                            pAdpcm[i]->_yn1 = buffer[i + channels];
                        }
                    }
                }
            }

            return map;
        }
    }
}
