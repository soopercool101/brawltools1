using System;
using System.Collections.Generic;

namespace BrawlLib.Modeling
{
    public class Animation
    {
        internal int _frameCount;
        public int Frames
        {
            get { return _frameCount; }
            set
            {
                if (_frameCount == value)
                    return;


            }
        }

        internal List<AnimBone> _boneList = new List<AnimBone>();
        public List<AnimBone> Bones { get { return _boneList; } }
    }
}
