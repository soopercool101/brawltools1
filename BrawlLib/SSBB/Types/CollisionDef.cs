﻿using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct CollisionHeader
    {
        public const int Size = 0x28;

        public bshort _numPoints;
        public bshort _numPlanes;
        public bshort _numObjects;
        public bshort _unk1;
        public bint _pointOffset;
        public bint _planeOffset;
        public bint _objectOffset;
        internal fixed int _pad[5];

        public CollisionHeader(int numPoints, int numPlanes, int numObjects, int unk1)
        {
            _numPoints = (short)numPoints;
            _numPlanes = (short)numPlanes;
            _numObjects = (short)numObjects;
            _unk1 = (short)unk1;
            _pointOffset = 0x28;
            _planeOffset = 0x28 + (numPoints * 8);
            _objectOffset = 0x28 + (numPoints * 8) + (numPlanes * ColPlane.Size);

            fixed (int* p = _pad)
                for (int i = 0; i < 5; i++)
                    p[i] = 0;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public BVec2* Points { get { return (BVec2*)(Address + _pointOffset); } }
        public ColPlane* Planes { get { return (ColPlane*)(Address + _planeOffset); } }
        public ColObject* Objects { get { return (ColObject*)(Address + _objectOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColPlane
    {
        public const int Size = 0x10;

        public bshort _point1;
        public bshort _point2;
        public bshort _link1;
        public bshort _link2;
        public bint _magic; //-1
        public bshort _type;
        public CollisionPlaneFlags _flags;
        public CollisionPlaneMaterial _material;

        public ColPlane(int pInd1, int pInd2, int pLink1, int pLink2, CollisionPlaneType type, CollisionPlaneFlags flags, CollisionPlaneMaterial material)
        {
            _point1 = (short)pInd1;
            _point2 = (short)pInd2;
            _link1 = (short)pLink1;
            _link2 = (short)pLink2;
            _magic = -1;
            _type = (short)type;
            _flags = flags;
            _material = material;
        }

        public CollisionPlaneType Type { get { return (CollisionPlaneType)(short)_type; } set { _type = (short)value; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ColObject
    {
        public const int Size = 0x6C;

        public bshort _planeIndex;
        public bshort _planeCount;
        public bint _unk1; //0
        public bint _unk2; //0
        public bint _unk3; //0
        public bint _unk4;
        public BVec2 _boxMin;
        public BVec2 _boxMax;
        public bshort _pointOffset;
        public bshort _pointCount;
        public bshort _unk5; //0
        public bshort _unk6; //-1
        public fixed byte _modelName[32];
        public fixed byte _boneName[32];

        public ColObject(int planeIndex, int planeCount, int pointOffset, int pointCount, Vector2 boxMin, Vector2 boxMax, string modelName, string boneName)
        {
            _planeIndex = (short)planeIndex;
            _planeCount = (short)planeCount;
            _unk1 = 0;
            _unk2 = 0;
            _unk3 = 0;
            _unk4 = 0;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _pointOffset = (short)pointOffset;
            _pointCount = (short)pointCount;
            _unk5 = 0;
            _unk6 = 0;

            fixed (byte* p = _modelName)
                SetStr(p, modelName);

            fixed (byte* p = _boneName)
                SetStr(p, boneName);
        }

        public void Set(int planeIndex, int planeCount, Vector2 boxMin, Vector2 boxMax, string modelName, string boneName)
        {
            _planeIndex = (short)planeIndex;
            _planeCount = (short)planeCount;
            _unk1 = 0;
            _unk2 = 0;
            _unk3 = 0;
            _unk4 = 0;
            _boxMin = boxMin;
            _boxMax = boxMax;
            _unk5 = 0;
            _unk6 = 0;

            ModelName = modelName;
            BoneName = boneName;
        }

        private VoidPtr Address { get { fixed (void* p = &this)return p; } }

        public string ModelName
        {
            get { return new String((sbyte*)Address + 0x2C); }
            set { SetStr((byte*)Address + 0x2C, value); }
        }
        public string BoneName
        {
            get { return new String((sbyte*)Address + 0x4C); }
            set { SetStr((byte*)Address + 0x4C, value); }
        }

        private static void SetStr(byte* dPtr, string str)
        {
            int index = 0;
            if (str != null)
            {
                //Fill string
                int len = Math.Min(str.Length, 31);
                fixed (char* p = str)
                    while (index < len)
                        *dPtr++ = (byte)p[index++];
            }

            //Fill remaining
            while (index++ < 32)
                *dPtr++ = 0;
        }
    }

    public enum CollisionPlaneMaterial : byte
    {
        Footstep1 = 0,
        Footstep2 = 1,
        Footstep3 = 2,
        Footstep4 = 3,
        Footstep5 = 4,
        Footstep6 = 5,
        Footstep7 = 6,
        Footstep8 = 7,
        Footstep9 = 8,
        Footstep10 = 9,
        Water = 0x0A,
        Bubbles = 0x0B,
        Ice = 0x0C,
        Snow = 0x0D,
        SnowIce = 0x0E,
        Footstep11 = 0x0F,
        Ice2 = 0x10,
        Footstep12 = 0x11,
        Crash1 = 0x12,
        Crash2 = 0x13,
        Crash3 = 0x14,
        LargeBubbles = 0x15,
        Footstep13 = 0x16,
        Footstep14 = 0x17,
    }

    [Flags]
    public enum CollisionPlaneType : short
    {
        None = 0x0000,
        Floor = 0x0001,
        Ceiling = 0x0002,
        RightWall = 0x0004,
        LeftWall = 0x0008,
        Unk1 = 0x0010,
        Unk2 = 0x0020
    }

    [Flags]
    public enum CollisionPlaneFlags : byte
    {
        None = 0x00,
        DropThrough = 0x01,
        Unk1 = 0x40,
        Unk2 = 0x80
    }
}