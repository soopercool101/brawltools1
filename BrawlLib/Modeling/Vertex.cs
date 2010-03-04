using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

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
            Influence = influence.Clone();
        }

        public void Weight()
        {
            if (Influence != null)
            {
                Influence.CalcWeighted();
                WeightedPosition = Influence._frame.Multiply(Position);
            }
            else
                WeightedPosition = Position;
        }
    }
}
