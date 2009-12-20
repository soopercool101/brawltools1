using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Audio
{
    public abstract class AudioBuffer : IDisposable
    {
        //Buffer will be valid for two seconds. The application MUST update/fill before then.
        //This is plenty of time, as timer updates should occur every 10 - 100 ms.
        internal const int DefaultBufferSpan = 2;

        internal AudioProvider _owner;
        public AudioProvider Owner { get { return _owner; } }

        internal IAudioStream _source;
        public IAudioStream Source { get { return _source; } }

        internal WaveFormatTag _format;
        public WaveFormatTag Format { get { return _format; } }

        internal int _frequency;
        public int Frequency { get { return _frequency; } }

        internal int _channels;
        public int Channels { get { return _channels; } }

        internal int _bitsPerSample;
        public int BitsPerSample { get { return _bitsPerSample; } }

        //Number of samples that can be stored inside the buffer.
        internal int _sampleLength;
        public int SampleLength { get { return _sampleLength; } }

        //Total byte length of the buffer.
        internal int _dataLength;
        public int DataLength { get { return _dataLength; } }

        //Number of bytes in each sample. (_bitsPerSample * _channels / 8)
        internal int _blockAlign;
        public int BlockAlign{get{return _blockAlign;}}

        //Byte offset within buffer in which to continue writing.
        //Read-only. It is the responsibility of the application to update the audio data in a timely manner.
        //As data is written, this is updated automatically.
        internal int _writeOffset;
        public int WriteOffset { get { return _writeOffset; } }

        //Byte offset within buffer in which reading is currently commencing.
        //The application must call Update (or Fill) to update this value.
        internal int _readOffset;
        public int ReadOffset { get { return _readOffset; } }

        //Cumulative sample position in which to continue writing.
        //This value is updated automatically when fill is called.
        internal int _writeSample;
        public int WriteSample { get { return _writeSample; } }

        //Cumulative sample position in which the buffer is currently reading.
        //This value is updated as Update is called.
        internal int _readSample;
        public int ReadSample { get { return _readSample; } }

        //Sets whether the buffer manages looping.
        //Use this with Source.
        internal bool _loop = false;
        public bool Loop { get { return _loop; } set { _loop = value; } }

        //internal bool _playing = false;
        //public bool IsPlaying { get { return _playing; } }

        //The number of samples to play.
        //Can be used with Loop for automatic looping.
        //If not looped, the buffer will automatically stop upon reaching this value.
        //internal int _totalSamples;
        //public int TotalSamples { get { return _totalSamples; } set { _totalSamples = value; } }

        //Byte offset within buffer in which playback is commencing.
        internal abstract int PlayCursor { get; set; }

        public abstract int Volume { get; set; }
        public abstract int Pan { get; set; }

        ~AudioBuffer() { Dispose(); }
        public virtual void Dispose()
        {
            if (_owner != null)
            {
                _owner._buffers.Remove(this);
                _owner = null;
            }
            GC.SuppressFinalize(this);
        }

        public abstract void Play();
        public abstract void Stop();
        public abstract BufferData Lock(int offset, int length);
        public abstract void Unlock(BufferData data);

        //Should only be used while playback is stopped
        public void Seek(int samplePos)
        {
            _readOffset = _writeOffset = PlayCursor;
            _readSample = _writeSample = samplePos;

            if (_source != null)
                _source.SamplePosition = samplePos;
        }
        public void Reset()
        {
            _readOffset = _writeOffset = PlayCursor;
        }

        public virtual void Update()
        {
            //Get current sample offset.
            int sampleOffset = PlayCursor / _blockAlign;
            //Get current byte offset
            int byteOffset = sampleOffset * _blockAlign;
            //Get sample difference since last update, taking into account circular wrapping.
            int sampleDifference = (((byteOffset < _readOffset) ? (byteOffset + _dataLength) : byteOffset) - _readOffset) / _blockAlign;
            //Get byte difference
            //int byteDifference = sampleDifference * _blockAlign;

            //If no change, why continue?
            if (sampleDifference == 0)
                return;

            //Advance sample read position.
            _readSample += sampleDifference;
            //Set new read offset.
            _readOffset = byteOffset;

            //Update looping
            if (_source != null)
            {
                if (_loop)
                {
                    int start, end;
                    if (_source.IsLooping)
                    {
                        start = _source.LoopStartSample;
                        end = _source.LoopEndSample;
                    }
                    else
                    {
                        start = 0;
                        end = _source.Samples;
                    }

                    _readSample = start + ((_readSample - start) % (end - start));
                }
                else if (_readSample >= _source.Samples)
                {
                    _readSample = _source.Samples;
                    Stop();
                }
            }
        }


        //Fills the buffer starting from writeOffset, taking into account circular wrapping.
        //Divides available samples by 8, so as not to flood the buffer all at once. Also reduces initialization and fill time.
        //Calls Update to move readOffset ahead.
        //public virtual void Fill()
        //{
        //    if (_source == null)
        //        return;

        //    Update();

        //    //Get number of samples available for writing. 
        //    int sampleCount = (((_readOffset <= _writeOffset) ? (_readOffset + _dataLength) : _readOffset) - _writeOffset) / _blockAlign / 8;
        //    int byteCount = sampleCount * _blockAlign;

        //    //Lock buffer and fill
        //    BufferData data = Lock(_writeOffset, byteCount);
        //    data.Fill(_source, _loop);
        //    Unlock(data);

        //    //Advance offsets
        //    _writeOffset = (_writeOffset + byteCount) % _dataLength;
        //    _writeSample = _source.SamplePosition;


            //int samplesRead, bytesRead;
            //if (!_loop)
            //{
            //    //Fill first part of buffer
            //    samplesRead = _source.ReadSamples(data._part1Address, data._part1Samples);

            //    //If not all samples could be read
            //    if (samplesRead < data._part1Samples)
            //    {
            //        //Zero remaining
            //        bytesRead = samplesRead * _blockAlign;
            //        Memory.Fill((VoidPtr)data._part1Address + bytesRead, (uint)(data._part1Length - bytesRead), 0);
            //        //Zero second part if exists.
            //        if (data.IsSplit)
            //            Memory.Fill(data._part2Address, (uint)data._part2Length, 0);
            //    }
            //    else if (data.IsSplit)
            //    {
            //        //Fill second part of buffer.
            //        samplesRead = _source.ReadSamples(data._part2Address, data._part2Samples);

            //        //If not all samples could be read, zero fill
            //        if (samplesRead < data._part2Samples)
            //        {
            //            bytesRead = samplesRead * _blockAlign;
            //            Memory.Fill((VoidPtr)data._part2Address + bytesRead, (uint)(data._part2Length - bytesRead), 0);
            //        }
            //    }
            //}
            //else
            //{
            //    //When looping you don't have to worry about filling the remainder
            //    int end = _source.IsLooping ? _source.LoopEndSample : _source.Samples;

            //    //Does end fall within sample range?
            //    if ((_writeSample <= end) && ((_writeSample + sampleCount) > end))
            //    {
            //        //We must loop!
            //        int loopSamples = end - _writeSample;

            //        if (loopSamples > data._part1Samples)
            //        {
            //            samplesRead = _source.ReadSamples(data._part1Address, data._part1Length);
            //        }

            //        if (samplesRead < loopSamples)
            //        {
            //            //Continue loop samples to next part
            //            samplesRead = _source.ReadSamples(data._part2Address, loopSamples - data._part1Samples);
            //            bytesRead = samplesRead * _blockAlign;
            //            //Wrap source
            //            if (_source.IsLooping)
            //                _source.Wrap();
            //            else
            //                _source.SamplePosition = 0;
            //            //Finish reading
            //            _source.ReadSamples((VoidPtr)data._part2Address + bytesRead, data._part2Length - bytesRead);
            //        }
            //        if (loopSamples < data._part1Samples)
            //        {
            //        }
            //    }
            //    else
            //    {
            //        _source.ReadSamples(data._part1Address, data._part1Samples);
            //        if (data.IsSplit)
            //            _source.ReadSamples(data._part2Address, data._part2Samples);
            //    }
            //}

            //if(_loop)
            //{
            //    bool loop = (_writeSample <= end) && ((_writeSample + sampleCount) > end);
            //    if (loop)
            //    {

            //    }
            //}
            //bool loop = _loop && (_writeSampleOffset <= _loopEndSample) && ((_writeSampleOffset + numSamples) > _loopEndSample);
            //if (loop)
            //    numSamples = _loopEndSample - _writeSampleOffset;


            //stream.ReadSamples(data.Part1Address, (int)data.Part1Length / _blockAlign);
            //if (data.IsSplit)
            //    stream.ReadSamples(data.Part2Address, (int)data.Part2Length / _blockAlign);

            //Unlock(data);

            //Advance positions.
            //_writeOffset = (_writeOffset + byteCount) % _dataLength;
            //_writeOffset += byteCount;
            //_writeSample = _source.SamplePosition;
            //_writeSample += sampleCount;
        //}

        public virtual void Fill()
        {
            //This only works if a source has been assigned!
            if (_source == null)
                return;

            //Update read position
            Update();

            //Get number of samples available for writing. 
            int sampleCount = (((_readOffset <= _writeOffset) ? (_readOffset + _dataLength) : _readOffset) - _writeOffset) / _blockAlign / 8;

            //Fill samples
            Fill(_source, sampleCount, _loop);
        }
        public virtual void Fill(IAudioStream source, int samples, bool loop)
        {
            int byteCount = samples * _blockAlign;

            //Lock buffer and fill
            BufferData data = Lock(_writeOffset, byteCount);
            data.Fill(source, loop);
            Unlock(data);

            //Advance offsets
            _writeOffset = (_writeOffset + byteCount) % _dataLength;
            _writeSample = source.SamplePosition;
        }

        //internal void Fill(IAudioStream stream, int samples, bool loop, bool manage)
        //{
        //    if (stream == null)
        //        return;

        //        Update();

        //    //Get number of samples available for writing. 
        //    int sampleCount = (((_readOffset <= _writeOffset) ? (_readOffset + _dataLength) : _readOffset) - _writeOffset) / _blockAlign / 8;
        //    int byteCount = sampleCount * _blockAlign;
        //}

    }
}
