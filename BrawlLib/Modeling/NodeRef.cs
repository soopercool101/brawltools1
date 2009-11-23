using System;
using System.Collections.Generic;

namespace BrawlLib.Modeling
{
    public class NodeRef : IMatrixProvider
    {
        internal List<NodeWeight> _entries = new List<NodeWeight>();

        internal Matrix _frame;
        public Matrix FrameMatrix { get { return _frame; } }
        
        internal Matrix _inverse;
        public Matrix InverseBindMatrix { get { return _inverse; } }

        public NodeRef() { }
        public NodeRef(IMatrixProvider node) { _entries.Add(new NodeWeight(node)); }

        public void CalcBase()
        {
            if (_entries.Count == 1)
            {
                _frame = _entries[0].Node.FrameMatrix;
                _inverse = _entries[0].Node.InverseBindMatrix;
            }
            else
                _frame = _inverse = Matrix.Identity;
        }
        public void CalcWeighted()
        {
            if(_entries.Count > 1)
            {
                //Multiply the current matrix by the inverse bind matrix and scale
                _frame = new Matrix();
                foreach (NodeWeight w in _entries)
                    _frame += (w.Node.FrameMatrix * w.Node.InverseBindMatrix) * w.Weight;
            }
        }
    }

    public struct NodeWeight
    {
        public IMatrixProvider Node;
        public float Weight;

        public NodeWeight(IMatrixProvider node) { Node = node; Weight = 1.0f; }
        public NodeWeight(IMatrixProvider node, float weight) { Node = node; Weight = weight; }
    }
}
