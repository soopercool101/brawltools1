using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace BrawlLib.OpenGL
{
    public class GLModel
    {
        private Vector3[] _vertices, _normals;

        public Vector3[] Vertices
        {
            get { return _vertices; }
        }
    }
}
