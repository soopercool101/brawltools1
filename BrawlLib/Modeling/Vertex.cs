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
        public Influence Influence;
        //public Matrix Matrix;

        public Vertex3(Vector3 position)
        {
            Position = position;
        }
        public Vertex3(Vector3 position, Influence influence)
        {
            Position = position;
            Influence = influence;
        }

        public void Weight()
        {
            if (Influence != null)
            {
                //Influence.CalcMatrix();
                WeightedPosition = Influence._matrix.Multiply(Position);
            }
            else
                WeightedPosition = Position;
        }

        public bool Equals(Vertex3 v)
        {
            if (object.ReferenceEquals(this, v))
                return true;

            return (Position == v.Position) && (Influence == v.Influence);
        }
    }
}
