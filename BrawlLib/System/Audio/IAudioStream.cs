using System;
namespace System.Audio
{
    public interface IAudioStream: IDisposable
    {
        WaveFormatTag Format { get; }
        int BitsPerSample { get; }
        int Samples { get; }
        int Channels { get; }
        int Frequency { get; }
        bool IsLooping { get; }
        int LoopStartSample { get; }
        int LoopEndSample { get; }
        int SamplePosition { get; set; }

        int ReadSamples(VoidPtr destAddr, int numSamples);
        void Wrap();
    }
}
