using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Wii.Models
{
    public class InfluenceManager
    {
        internal List<Influence> _influences = new List<Influence>();
        public List<Influence> Influences { get { return _influences; } }

        public Influence AddOrCreate(Influence inf)
        {
            foreach (Influence i in _influences)
                if (i.Equals(inf))
                    return i;
            _influences.Add(inf);
            return inf;
        }

        //Increases reference count
        public Influence AddOrCreateInc(Influence inf)
        {
            Influence i = AddOrCreate(inf);
            i._refCount++;
            return i;
        }

        public void Remove(Influence inf)
        {
            for (int i = 0; i < _influences.Count; i++)
                if (object.ReferenceEquals(_influences[i], inf))
                {
                    if (inf._refCount-- <= 0)
                        _influences.RemoveAt(i);
                    break;
                }
        }
    }

    public class Influence
    {
        internal int _refCount = 0;

        internal BoneWeight[] _weights;
        public BoneWeight[] Weights { get { return _weights; } }

        internal Matrix _matrix;
        public Matrix Matrix { get { return _matrix; } }

        public Influence(int capacity) { _weights = new BoneWeight[capacity]; }
        public Influence(BoneWeight[] weights) { _weights = weights; }
        public Influence(MDL0BoneNode bone) { _weights = new BoneWeight[] { new BoneWeight(bone) }; }

        public Influence Clone()
        {
            Influence i = new Influence(_weights.Length);
            _weights.CopyTo(i._weights, 0);
            return i;
        }

        public Influence Splice(BoneWeight weight)
        {
            Influence i = new Influence(_weights.Length + 1);
            _weights.CopyTo(i._weights, 0);
            i._weights[_weights.Length] = weight;
            return i;
        }

        public void CalcMatrix()
        {
            if (_weights.Length > 1)
            {
                _matrix = new Matrix();
                foreach (BoneWeight w in _weights)
                    _matrix += (w.Bone.FrameMatrix * w.Bone.InverseBindMatrix) * w.Weight;
            }
            else if (_weights.Length == 1)
                _matrix = _weights[0].Bone.FrameMatrix;
            else
                _matrix = Matrix.Identity;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is Influence)
        //        return Equals(obj as Influence);
        //    return false;
        //}

        public bool Equals(Influence inf)
        {
            bool found;

            if (object.ReferenceEquals(this, inf))
                return true;

            if (_weights.Length != inf._weights.Length)
                return false;

            foreach (BoneWeight w1 in _weights)
            {
                found = false;
                foreach (BoneWeight w2 in inf._weights) { if (w1 == w2) { found = true; break; } }
                if (!found)
                    return false;
            }
            return true;
        }
    }

    public struct BoneWeight
    {
        public MDL0BoneNode Bone;
        public float Weight;

        public BoneWeight(MDL0BoneNode bone) : this(bone, 1.0f) { }
        public BoneWeight(MDL0BoneNode bone, float weight) { Bone = bone; Weight = weight; }

        public static bool operator ==(BoneWeight b1, BoneWeight b2) { return (b1.Bone == b2.Bone) && (b1.Weight - b2.Weight < 0.0001); }
        public static bool operator !=(BoneWeight b1, BoneWeight b2) { return !(b1 == b2); }
        public override bool Equals(object obj)
        {
            if (obj is BoneWeight)
                return this == (BoneWeight)obj;
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }
}
