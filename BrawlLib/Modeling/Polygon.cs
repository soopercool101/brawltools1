using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Modeling
{
    public class Polygon
    {
        internal string _name;
        public string Name { get { return _name; } set { _name = value; } }

        public VertexList _vertices;
    }
}
