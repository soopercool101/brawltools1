﻿using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnimationFrame
    {
        public static readonly AnimationFrame Neutral = new AnimationFrame(new Vector3(1.0f), new Vector3(), new Vector3());

        public Vector3 Scale;
        public Vector3 Rotation;
        public Vector3 Translation;

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Scale._x;
                    case 1: return Scale._y;
                    case 2: return Scale._z;
                    case 3: return Rotation._x;
                    case 4: return Rotation._y;
                    case 5: return Rotation._z;
                    case 6: return Translation._x;
                    case 7: return Translation._y;
                    case 8: return Translation._z;
                    default: return float.NaN;
                }
            }
            set
            {
                switch (index)
                {
                    case 0: Scale._x = value; break;
                    case 1: Scale._y = value; break;
                    case 2: Scale._z = value; break;
                    case 3: Rotation._x = value; break;
                    case 4: Rotation._y = value; break;
                    case 5: Rotation._z = value; break;
                    case 6: Translation._x = value; break;
                    case 7: Translation._y = value; break;
                    case 8: Translation._z = value; break;
                }
            }
        }

        public AnimationFrame(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            Scale = scale; Rotation = rotation; Translation = translation;
        }

        public override string ToString()
        {
            return String.Format("{0}\r\n{1}\r\n{2}", Scale, Rotation, Translation);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AnimationKeyframe
    {
        public static readonly AnimationKeyframe Empty = new AnimationKeyframe(0);
        public static readonly AnimationKeyframe Neutral = new AnimationKeyframe() { Scale = new Vector3(1.0f) };

        public int Index;

        public Vector3 Scale;
        public Vector3 Rotation;
        public Vector3 Translation;

        public AnimationKeyframe(int index)
        {
            Index = index;
            Scale = new Vector3(float.NaN);
            Rotation = new Vector3(float.NaN);
            Translation = new Vector3(float.NaN);
        }

        const int len = 6;
        static string empty = new String('_', len);
        public override string ToString()
        {
            return String.Format("[{0}]({1},{2},{3})({4},{5},{6})({7},{8},{9})", Index + 1,
                float.IsNaN(Scale._x) ? empty : Scale._x.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Scale._y) ? empty : Scale._y.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Scale._z) ? empty : Scale._z.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Rotation._x) ? empty : Rotation._x.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Rotation._y) ? empty : Rotation._y.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Rotation._z) ? empty : Rotation._z.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Translation._x) ? empty : Translation._x.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Translation._y) ? empty : Translation._y.ToString().TruncateAndFill(len, ' '),
                float.IsNaN(Translation._z) ? empty : Translation._z.ToString().TruncateAndFill(len, ' '));
        }
    }
}
