using System;
using BrawlLib.Wii.Animations;

namespace BrawlLib.Modeling
{
    public class AnimBone
    {
        private string _name;
        public string Name { get { return _name; } }

        internal KeyframeCollection _frames;

        public AnimationFrame GetFrame(int index)
        {
            return _frames.AnimFrames[index];
        }
    }
}
