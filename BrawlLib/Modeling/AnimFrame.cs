using System;

namespace BrawlLib.Modeling
{
    public class AnimFrame
    {
        internal float[] _values = new float[9];
        internal float[] _kValues = new float[9];

        internal AnimFrame _prev, _next;

        public Vector3 Scale
        {
            get { return new Vector3(this[0], this[1], this[2]); }
            set { this[0] = value._x; this[1] = value._y; this[2] = value._z; }
        }
        public Vector3 Rotate
        {
            get { return new Vector3(this[3], this[4], this[5]); }
            set { this[3] = value._x; this[4] = value._y; this[5] = value._z; }
        }
        public Vector3 Translate
        {
            get { return new Vector3(this[6], this[7], this[8]); }
            set { this[6] = value._x; this[7] = value._y; this[8] = value._z; }
        }

        public float this[int index]
        {
            get { return (float.IsNaN(_kValues[index])) ? _values[index] : _kValues[index]; }
            set 
            { 
                _kValues[index] = value;
                if (float.IsNaN(value))
                {
                }
            }
        }

        public bool IsKey
        {
            get
            {
                for (int i = 0; i < 9; i++)
                    if (!float.IsNaN(_kValues[i]))
                        return true;
                return false;
            }
        }

        //public bool IsKey(int index) { return !float.IsNaN(_kValues[index]); }
    }
}
