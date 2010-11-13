using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.Wii.Models;

namespace BrawlLib.Modeling
{
    public class Vertex3
    {
        public Vector3 Position;
        public Vector3 WeightedPosition;
        internal IMatrixNode _influence;

        public IMatrixNode Influence
        {
            get { return _influence; }
            set
            {
                if (_influence == value)
                    return;

                if (_influence != null)
                    _influence.ReferenceCount--;

                if ((_influence = value) != null)
                    _influence.ReferenceCount++;
            }
        }

        public Vertex3(Vector3 position)
        {
            Position = position;
        }
        public Vertex3(Vector3 position, IMatrixNode influence)
        {
            Position = position;
            Influence = influence;
        }

        //Pre-multiply vertex using influence.
        //Influences must have already been calculated.
        public void Weight()
        {
            WeightedPosition = (_influence != null) ? _influence.Matrix.Multiply(Position) : Position;
            //if (_influence != null)
            //    WeightedPosition = _influence._matrix.Multiply(Position);
            //else
            //    WeightedPosition = Position;
        }

        public bool Equals(Vertex3 v)
        {
            if (object.ReferenceEquals(this, v))
                return true;

            return (Position == v.Position) && (_influence == v._influence);
        }
    }
}
