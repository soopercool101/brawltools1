using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    public enum ScalingType : byte
    {
        Anisotropic = 0,
        None = 1,
        Isotropic = 2
    }

    public enum AnimDataFormat : byte
    {
        F4B = 1,
        F6B = 2,
        F3F = 3,
        F1B = 4,
        F1F = 6
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnimationCode
    {
        public uint _data;

//0000 0000 0000 0000 0000 0000 0001 0000		Scaling mode

//0000 0000 0000 0000 1110 0000 0000 0000		Scale fixed
//0000 0000 0000 0111 0000 0000 0000 0000		Rotation fixed
//0000 0000 0011 1000 0000 0000 0000 0000		Translation fixed

//0000 0000 0100 0000 0000 0000 0000 0000		Scale exists
//0000 0000 1000 0000 0000 0000 0000 0000		Rotation exists
//0000 0001 0000 0000 0000 0000 0000 0000		Translation exists

//0000 0110 0000 0000 0000 0000 0000 0000		Scale format
//0011 1000 0000 0000 0000 0000 0000 0000		Rotation format
//1100 0000 0000 0000 0000 0000 0000 0000		Translation format

        public bool HasScale { get { return (_data & 0x400000) != 0; } set { _data = (_data & 0xFFBFFFFF) | (value ? (uint)0x400000 : 0); } }
        public bool IsScaleIsotropic { get { return (_data & 0x10) != 0; } set { _data = (_data & 0xFFFFFFEF) | (value ? (uint)0x10 : 0); } }
        public bool IsIsotropicFixed { get { return (_data & 0xE000) != 0; } }
        public bool IsScaleXFixed { get { return (_data & 0x2000) != 0; } set { _data = (_data & 0xFFFFDFFF) | ((value) ? (uint)0x2000 : 0); } }
        public bool IsScaleYFixed { get { return (_data & 0x4000) != 0; } set { _data = (_data & 0xFFFFBFFF) | ((value) ? (uint)0x4000 : 0); } }
        public bool IsScaleZFixed { get { return (_data & 0x8000) != 0; } set { _data = (_data & 0xFFFF7FFF) | ((value) ? (uint)0x8000 : 0); } }
        public AnimDataFormat ScaleDataFormat { get { return (AnimDataFormat)((_data >> 25) & 3); } set { _data = (_data & 0xF9FFFFFF) | ((uint)value << 25); } }

        public bool HasRotation { get { return (_data & 0x800000) != 0; } set { _data = (_data & 0xFF7FFFFF) | (value ? (uint)0x800000 : 0); } }
        public bool IsRotationXFixed { get { return (_data & 0x10000) != 0; } set { _data = (_data & 0xFFFEFFFF) | ((value) ? (uint)0x10000 : 0); } }
        public bool IsRotationYFixed { get { return (_data & 0x20000) != 0; } set { _data = (_data & 0xFFFDFFFF) | ((value) ? (uint)0x20000 : 0); } }
        public bool IsRotationZFixed { get { return (_data & 0x40000) != 0; } set { _data = (_data & 0xFFFBFFFF) | ((value) ? (uint)0x40000 : 0); } }
        public AnimDataFormat RotationDataFormat { get { return (AnimDataFormat)((_data >> 27) & 7); } set { _data = (_data & 0xC7FFFFFF) | ((uint)value << 27); } }

        public bool HasTranslation { get { return (_data & 0x1000000) != 0; } set { _data = (_data & 0xFEFFFFFF) | (value ? (uint)0x1000000 : 0); } }
        public bool IsTranslationXFixed { get { return (_data & 0x080000) != 0; } set { _data = (_data & 0xFFF7FFFF) | ((value) ? (uint)0x080000 : 0); } }
        public bool IsTranslationYFixed { get { return (_data & 0x100000) != 0; } set { _data = (_data & 0xFFEFFFFF) | ((value) ? (uint)0x100000 : 0); } }
        public bool IsTranslationZFixed { get { return (_data & 0x200000) != 0; } set { _data = (_data & 0xFFDFFFFF) | ((value) ? (uint)0x200000 : 0); } }
        public AnimDataFormat TranslationDataFormat { get { return (AnimDataFormat)(_data >> 30); } set { _data = (_data & 0x3FFFFFFF) | ((uint)value << 30); } }

        public static implicit operator AnimationCode(uint data) { return new AnimationCode() { _data = data }; }
        public static implicit operator uint(AnimationCode code) { return code._data; }

    }
}
