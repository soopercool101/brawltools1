using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Modeling
{
    public class Influence : IMatrixProvider
    {
        //internal int _index;
        internal List<NodeWeight> _entries;

        internal Matrix _frame;
        public Matrix FrameMatrix { get { return _frame; } }

        internal Matrix _inverse;
        public Matrix InverseBindMatrix { get { return _inverse; } }

        private Influence() { }
        public Influence(int capacity) { _entries = new List<NodeWeight>(capacity); }
        public Influence(MDL0BoneNode bone) : this(1) { _entries.Add(new NodeWeight(bone)); }

        public Influence Clone()
        {
            Influence nr = new Influence();
            nr._entries = new List<NodeWeight>(_entries);
            return nr;
        }

        public void Merge(Influence inf, float weight)
        {
            _entries.Add(new NodeWeight(inf._entries[0].Bone, weight));
        }

        public void CalcBase()
        {
            if (_entries.Count == 1)
            {
                _frame = _entries[0].Bone.FrameMatrix;
                _inverse = _entries[0].Bone.InverseBindMatrix;
            }
            else
                _frame = _inverse = Matrix.Identity;
        }
        public void CalcWeighted()
        {
            if (_entries.Count > 1)
            {
                //Multiply the current matrix by the inverse bind matrix and scale
                _frame = new Matrix();
                foreach (NodeWeight w in _entries)
                    _frame += (w.Bone.FrameMatrix * w.Bone.InverseBindMatrix) * w.Weight;
            }
            else
                _frame = _entries[0].Bone.FrameMatrix;
        }
    }

    public struct NodeWeight
    {
        public MDL0BoneNode Bone;
        public float Weight;

        public NodeWeight(MDL0BoneNode bone) : this(bone, 1.0f) { }
        public NodeWeight(MDL0BoneNode bone, float weight) { Bone = bone; Weight = weight; }
    }
}
