using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Models;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.OpenGL
{
    public class GLPolygon
    {
        public List<GLPrimitive> _primitives = new List<GLPrimitive>();

        internal VertexBuffer _vertices;
        internal VertexBuffer _normals;
        internal ColorBuffer _colors1, _colors2;
        internal VertexBuffer[] _uvData = new VertexBuffer[8];

        public int _index;
        public bool _enabled = true;
        public GLModel _model;

        public List<GLMaterial> _materials = new List<GLMaterial>();

        internal unsafe void Render(GLContext context)
        {
            if (!_enabled)
                return;

            //if (GLModel._index++ != 10)
            //    return;

            //Get texture and bind to it
            foreach (GLMaterial mat in _materials)
                mat.Bind(context);

            foreach (GLPrimitive prim in _primitives)
            {
                if (prim is GLBoneDef)
                    continue;

                prim.Render(context, this);
                //break;
            }
        }
    }
}
