using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0
    {
        public const uint Tag = 0x304E4353;
        public const int Size = 0x44;

        public BRESCommonHeader _header;
        public bint _dataOffset;
        public bint _lightSetOffset;
        public bint _ambLightOffset;
        public bint _lightOffset;
        public bint _fogOffset;
        public bint _cameraOffset;
        public bint _stringOffset;
        public bint _unk1;
        public bshort _unk2;
        public bshort _unk3;
        public bint _unk4;
        public bshort _unk5;
        public bshort _unk6;
        public bshort _unk7;
        public bshort _unk8;
        public bshort _unk9;
        public bshort _unk10;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public ResourceGroup* Group { get { return (ResourceGroup*)(Address + _dataOffset); } }

        public SCN0LightSet* LightSets { get { return (SCN0LightSet*)(Address + _lightSetOffset); } }
        public SCN0AmbientLight* AmbientLights { get { return (SCN0AmbientLight*)(Address + _ambLightOffset); } }
        public SCN0Light* Lights { get { return (SCN0Light*)(Address + _lightOffset); } }
        public SCN0Fog* Fogs { get { return (SCN0Fog*)(Address + _fogOffset); } }
        public SCN0Camera* Cameras { get { return (SCN0Camera*)(Address + _cameraOffset); } }

        public string ResourceString { get { return new String((sbyte*)this.ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)this.Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0CommonHeader
    {
        public const int Size = 0x14;

        public bint _length;
        public bint _scn0Offset;
        public bint _stringOffset;
        public bint _nodeIndex;
        public bint _realIndex;

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }

        public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
        public VoidPtr ResourceStringAddress
        {
            get { return (VoidPtr)Address + _stringOffset; }
            set { _stringOffset = (int)value - (int)Address; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0LightSet
    {
        public SCN0CommonHeader _header;

        public bint _ambNameOffset;
        public bshort _magic; //0xFFFF
        public byte _numLights;
        public byte _unk1;
        public fixed int _entries[8]; //string offsets

        public bint _pad1, _pad2, _pad3, _pad4; //0xFFFFFFFF

        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
        public bint* Offsets { get { fixed (void* ptr = _entries)return (bint*)ptr; } }

        public string AmbientString { get { return new String((sbyte*)AmbientStringAddress); } }
        public VoidPtr AmbientStringAddress
        {
            get { return (VoidPtr)Address + _ambNameOffset; }
            set { _ambNameOffset = (int)value - (int)Address; }
        }

        public bint* StringOffsets { get { return (bint*)(Address + 0x1C); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0AmbientLight
    {
        public SCN0CommonHeader _header;

        public byte _unk1; //0x80
        public byte _unk2; //0x00
        public byte _unk3; //0x00
        public byte _unk4; //0x03, entries?

        public byte _b1;
        public byte _b2;
        public byte _b3;
        public byte _b4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Light
    {
        public SCN0CommonHeader _header;

        public bint _unk1;
        public bint _unk2;
        public bshort _unk3; //negative
        public bshort _unk4;
        public bint _unk5;

        public BVec3 _vec1;
        public byte _b1;
        public byte _b2;
        public byte _b3;
        public byte _b4;
        public BVec3 _vec2;

        public bint _unk6;
        public bint _unk7;
        public bint _unk8;
        public bint _unk9; //2
        public bint _unk10;
        public bint _unk11; //sometimes -1 else 0
        public bfloat _unk12;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Fog
    {
        public SCN0CommonHeader _header;

        public bint _unk1; //0xE0000000
        public bint _unk2;

        public bfloat _float1;
        public bfloat _float2;

        public byte _b1;
        public byte _b2;
        public byte _b3;
        public byte _b4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SCN0Camera
    {
        public SCN0CommonHeader _header;

        public bint _unk1; //0
        public bshort _unk2; //negative, -2
        public bshort _unk3; //3
        public bint _unk4; //0

        public BVec3 _vec1;
        public BVec3 _vec2;
        public BVec3 _vec3;
        public BVec3 _vec4;
        public BVec3 _vec5;
    }
}
