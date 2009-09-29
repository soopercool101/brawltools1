using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSTMHeader
    {
        public const uint Tag = 0x4D545352;

        public SSBBCommonHeader _header;

        //public uint _tag;
        //public ushort _magic;
        //public ushort _version;
        //public buint _fileSize;
        //public bushort _firstOffset; //always 0x40
        //public bushort _numEntries; //always 0x02

        //public buint _headOffset;
        //public buint _headLength;
        //public buint _adpcOffset;
        //public buint _adpcLength;
        //public buint _dataOffset;
        //public buint _dataLength;
        //fixed uint _padding[6];

        //private VoidPtr Address{get{fixed(void* ptr = &this)return ptr;}}

        public HEADHeader* HEADData { get { return (HEADHeader*)_header.Entries[0].Address; } }
        public ADPCHeader* ADPCData { get { return (ADPCHeader*)_header.Entries[1].Address; } }
        public RSTMDATAHeader* DATAData { get { return (RSTMDATAHeader*)_header.Entries[2].Address; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct HEADHeader
    {
        public const uint Tag = 0x44414548;

        public uint _tag;
        public buint _size;
        public RuintCollection _entries;

        //public ruint _offset1;
        //public ruint _offset2;
        //public ruint _offset3;

        //public uint _unk1; //0x01
        //public buint _unk2; //0x18
        //public uint _unk3; //0x01
        //public buint _unk4; //0x4C
        //public uint _unk5; //0x01
        //public buint _unk6; //0x5C

        //public byte _encoding;
        //public byte _loop;
        //public byte _channels;
        //public byte _unk7; //0x00
        //public bushort _sampleRate; //0x7D00
        //public ushort _unk8;
        //public buint _loopStartSample;
        //public buint _numSamples;

        public HEADPart1* Part1 { get { return (HEADPart1*)_entries[0]; } } //Audio info
        public RuintList* Part2 { get { return (RuintList*)_entries[1]; } } //ADPC block flags?
        public RuintList* Part3 { get { return (RuintList*)_entries[2]; } } //ADPCMInfo array, one for each channel?

        public ADPCMInfo[] ChannelInfo
        {
            get
            {
                RuintList* list = Part3;
                ADPCMInfo[] arr = new ADPCMInfo[list->_numEntries._data];
                for (int i = 0; i < list->_numEntries._data; i++)
                    arr[i] = *(ADPCMInfo*)((ruint*)list->GetEntry(_entries.Address, i))->Offset(_entries.Address);
                return arr;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct HEADPart1
    {
        public AudioFormatInfo _format;
        //public byte _encoding;
        //public byte _loop;
        //public byte _channels;
        //public byte _unk1; //0x00
        public bushort _sampleRate; //0x7D00
        public ushort _unk1;
        public bint _loopStartSample;
        public bint _numSamples;
        public bint _dataOffset;
        public buint _numBlocks;
        public buint _blockSize;
        public buint _samplesPerBlock; //0x3800
        public buint _lastBlockSize; //Without padding
        public buint _lastBlockSamples;
        public buint _lastBlockTotal; //Includes padding
        public buint _unk8; //0x3800
        public buint _bitsPerSample;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct ADPCHeader
    {
        public const uint Tag = 0x43504441;

        public uint _tag;
        public buint _length;
        //fixed uint _padding[2];

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + 0x10; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSTMDATAHeader
    {
        public const uint Tag = 0x41544144;

        public uint _tag;
        public buint _length;
        public buint _dataOffset; //always 0x18 data offset?

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public VoidPtr Data { get { return Address + 8 + _dataOffset; } }
    }
}
