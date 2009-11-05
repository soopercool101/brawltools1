using System;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Animations
{
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct AnimationFrame
    {
        public static readonly AnimationFrame Neutral = new AnimationFrame(new Vector3(1.0f), new Vector3(), new Vector3());

        public Vector3 Scale;
        public Vector3 Rotation;
        public Vector3 Translation;

        //public Vector3 scale { get { return Scale; } set { Scale = value; } }
        //public Vector3 rotation { get { return Rotation; } set { Rotation = value; } }
        //public Vector3 translation { get { return Translation; } set { Translation = value; } }

        public AnimationFrame(Vector3 scale, Vector3 rotation, Vector3 translation)
        {
            Scale = scale; Rotation = rotation; Translation = translation;
        }

        public override string ToString()
        {
            return String.Format("{0}\r\n{1}\r\n{2}", Scale, Translation, Rotation);
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
    }
}
