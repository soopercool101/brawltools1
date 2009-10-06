using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Wii.Models
{
    public class VertexBuffer : UnsafeBuffer
    {
        private int _numVertices;
        public int Vertices { get { return _numVertices; } }

        private bool _isXYZ;
        public bool IsXYZ { get { return _isXYZ; } }

        public VertexBuffer(int numVertices, bool isXYZ)
            : base(isXYZ ? numVertices * 12 : numVertices * 8)
        {
            _numVertices = numVertices;
            _isXYZ = isXYZ;
        }
    }
    public class ColorBuffer : UnsafeBuffer
    {
        private int _numColors;
        public int Colors { get { return _numColors; } }

        public ColorBuffer(int numColors)
            : base(numColors * 4)
        {
            _numColors = numColors;
        }
    }
}
