using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct REFFHeader
    {
        //Header + string is aligned to 4 bytes

        public const uint Tag = 0x46464552;

        public SSBBCommonHeader _header;
        public uint _tag; //Same as header
        public bint _dataLength; //Size of second REFF block. (file size - 0x18)
        public bint _dataOffset; //Offset from itself. Begins first entry
        public bint _unk1; //0
        public bint _unk2; //0
        public bshort _stringLen;
        public bshort _unk3; //0

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public string IdString
        {
            get { return new String((sbyte*)Address + 0x28); }
            set
            {
                int len = value.Length + 1;
                _stringLen = (short)len;

                byte* dPtr = (byte*)Address + 0x28;
                fixed (char* sPtr = value)
                {
                    for (int i = 0; i < len; i++)
                        *dPtr++ = (byte)sPtr[i];
                }

                //Align to 4 bytes
                while ((len++ & 3) != 0)
                    *dPtr++ = 0;

                //Set data offset
                _dataOffset = 0x18 + len - 1;
            }
        }

        public REFFObjectTable* Table { get { return (REFFObjectTable*)(Address + 0x18 + _dataOffset); } }
    }

    public unsafe struct REFFObjectTable
    {
        //Table size is aligned to 4 bytes

        public bint _length;
        public bshort _entries;
        public bshort _unk1;

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public REFFObjectEntry* First { get { return (REFFObjectEntry*)(Address + 8); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct REFFObjectEntry
    {
        public bshort _strLen;
        public bint _offset1;
        public bint _offset2;

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public REFFObjectEntry* Next { get { return (REFFObjectEntry*)(Address + 10 + _strLen); } }
    }
}
