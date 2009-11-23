using System;
using System.Audio;
using BrawlLib.SSBBTypes;
using System.IO;

namespace BrawlLib.Wii.Audio
{
    unsafe class ADPCMStream : IAudioStream
    {
        private int _sampleRate;
        private int _numSamples;
        private int _numChannels;
        private int _blockLen;
        private int _samplesPerBlock;
        private int _lastBlockSamples, _lastBlockSize;
        private int _numBlocks;
        private int _loopStartSample, _loopEndSample;
        private int _bitsPerSample = 4;
        private bool _isLooped, _useLoop;

        private int _samplePos = 0;

        private ADPCMState[,] _blockStates;
        private ADPCMState[] _loopStates;
        private ADPCMState[] _currentStates;

        public ADPCMStream(RSTMHeader* pRSTM)
        {
            HEADHeader* pHeader = pRSTM->HEADData;
            HEADPart1* part1 = pHeader->Part1;
            byte* sPtr;
            bshort* entry;
            short[] coefs;
            ADPCMInfo* info;
            VoidPtr dataAddr;

            _numChannels = part1->_format._channels;
            _isLooped = part1->_format._looped != 0;
            _sampleRate = part1->_sampleRate;
            _numSamples = part1->_numSamples;
            _blockLen = (int)part1->_blockSize;
            _loopStartSample = part1->_loopStartSample;
            _loopEndSample = _numSamples;

            Init();

            dataAddr = pRSTM->DATAData->Data;

            _blockStates = new ADPCMState[_numChannels, _numBlocks];
            _currentStates = new ADPCMState[_numChannels];
            _loopStates = new ADPCMState[_numChannels];

            for (int i = 0; i < _numChannels; i++)
            {
                info = pHeader->GetChannelInfo(i);
                coefs = info->Coefs;

                sPtr = (byte*)dataAddr + (i * _blockLen);
                for (int x = 1; x < _numBlocks; x++ )
                {
                    entry = (bshort*)(pRSTM->ADPCData->Data + (i * 4) + ((x - 1) * 4 * _numChannels));
                    _blockStates[i, x] = new ADPCMState(
                        sPtr + (x * _blockLen * _numChannels) - ((x == _numBlocks - 1) ? (i * (_blockLen - _lastBlockSize)) : 0), entry[0], entry[1], coefs);
                }

                _loopStates[i] = new ADPCMState(sPtr + (_loopStartSample * _blockLen * _numChannels), info->_lps, info->_lyn1, info->_lyn2, coefs);
                _currentStates[i] = _blockStates[i, 0] = new ADPCMState(sPtr, info->_ps, info->_yn1, info->_yn2, coefs);
            }
        }

        public ADPCMStream(WAVEEntry* pWAVE, VoidPtr dataAddr)
        {
            ADPCMInfo* info;
            short[] coefs;

            _numChannels = pWAVE->_format._channels;
            _sampleRate = pWAVE->_frequency;
            _numSamples = pWAVE->NumSamples;

            _blockLen = (_numSamples.Align(14) / 14 * 8).Align(0x20);
            _loopStartSample = (int)pWAVE->_flags;
            _loopEndSample = _numSamples;
            //_preserveLoopState = true;

            Init();

            info = &pWAVE->_adpcInfo;
            coefs = info->Coefs;

            _blockStates = new ADPCMState[1, 1];
            _currentStates = new ADPCMState[1];
            _loopStates = new ADPCMState[1];

            _loopStates[0] = new ADPCMState((byte*)dataAddr + (_loopStartSample / 14 * 8), info->_lps, info->_lyn1, info->_lyn2, coefs);
            _currentStates[0] = _blockStates[0, 0] = new ADPCMState((byte*)dataAddr, info->_ps, info->_yn1, info->_yn2, coefs);
        }

        public void Init()
        {
            _samplesPerBlock = _blockLen / 8 * 14;
            _numBlocks = _numSamples.Align(_samplesPerBlock) / _samplesPerBlock;

            if ((_numSamples % _samplesPerBlock) != 0)
                _lastBlockSamples = _numSamples % _samplesPerBlock;
            else
                _lastBlockSamples = _samplesPerBlock;

            _lastBlockSize = _lastBlockSamples.Align(14) / 14 * 8;
        }

        private void RefreshStates()
        {
            int blockId = _samplePos / _samplesPerBlock;
            int samplePos = blockId * _samplesPerBlock;
            for (int i = 0; i < _numChannels; i++)
            {
                if (_useLoop)
                    _currentStates[i] = _loopStates[i];
                else
                    _currentStates[i] = _blockStates[i, blockId];

                for (int x = samplePos; x < _samplePos; x++)
                    _currentStates[i].ReadSample();
            }
            _useLoop = false;
        }

        public RIFFHeader GetPCMHeader()
        {
            return new RIFFHeader(1, _numChannels, 16, _sampleRate, _numSamples);
        }

        public void WriteStream(Stream outStream)
        {
            int oldPos = _samplePos;
            short sample;

            //SamplePosition = 0;
            for (_samplePos = 0; _samplePos < _numSamples; _samplePos++)
            {
                if (_samplePos % _samplesPerBlock == 0)
                    RefreshStates();

                foreach (ADPCMState state in _currentStates)
                {
                    sample = state.ReadSample();
                    outStream.WriteByte((byte)(sample & 0xFF));
                    outStream.WriteByte((byte)(sample >> 8 & 0xFF));
                }
            }
            SamplePosition = oldPos;
        }


        #region IAudioStream Members

        public WaveFormatTag Format { get { return WaveFormatTag.WAVE_FORMAT_PCM; } }
        public int BitsPerSample { get { return 16; } }
        public int Samples { get { return _numSamples; } }
        public int Channels { get { return _numChannels; } }
        public int Frequency { get { return _sampleRate; } }
        public bool IsLooping { get { return _isLooped; } }
        public int LoopStartSample { get { return _loopStartSample; } }
        public int LoopEndSample { get { return _loopEndSample; } }

        public int SamplePosition
        {
            get { return _samplePos; }
            set
            {
                value = Math.Min(Math.Max(value, 0), _numSamples);
                if (_samplePos == value)
                    return;

                _samplePos = value;

                //Refresh states up to sample pos. If first in block, will be updated on next read.
                if (_samplePos % _samplesPerBlock != 0)
                    RefreshStates();
            }
        }

        public int ReadSamples(VoidPtr destAddr, int numSamples)
        {
            short* dPtr = (short*)destAddr;
            int samples = Math.Min(numSamples, _numSamples - _samplePos);

            for (int i = 0; i < samples; i++, _samplePos++)
            {
                if (_samplePos % _samplesPerBlock == 0)
                    RefreshStates();

                for (int x = 0; x < _numChannels; x++)
                    *dPtr++ = _currentStates[x].ReadSample();
            }

            return samples;
        }

        public void Wrap()
        {
            _useLoop = true;
            SamplePosition = _loopStartSample;
        }

        public void Dispose() { }

        #endregion
    }
}
