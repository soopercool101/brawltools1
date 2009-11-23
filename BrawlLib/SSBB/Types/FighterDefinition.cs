using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout( LayoutKind.Sequential, Pack=1)]
    unsafe struct FDefHeader
    {
        bint _fileSize; 
        bint _lookupOffset;
        bint _numLookupEntries;
        bint _numEntries1; //Has string entry
        bint _numEntries2; //Has string entry

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public FDefLookupHeader* LookupHeader { get { return (FDefLookupHeader*)(Address + _lookupOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct FDefLookupHeader
    {
        bint _offset1;
        bint _offset2;
        bint _offset3;
        bint _offset4;
        bint _offset5;
        bint _offset6;
        bint _offset7;
        bint _offset8;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Data { get { return (bint*)(Address + 0x20); } }
    }
}
