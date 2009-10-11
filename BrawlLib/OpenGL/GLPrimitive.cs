using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Imaging;

namespace BrawlLib.OpenGL
{
    public class GLPrimitive
    {
        internal ushort[] _nodeIndices;
        internal ushort[] _vertexIndices;
        internal ushort[] _normalIndices;
        internal ushort[][] _colorIndices = new ushort[2][];
        internal ushort[][] _uvIndices = new ushort[8][];

        internal Vector3[] _vertices, _normals;

        internal GLPolygon _parent;
        internal GLPrimitiveType _type;
        internal int _elements;

        public bool _enabled = true;

        internal unsafe void Rebuild()
        {
            if(_vertices == null)
                _vertices = new Vector3[_elements];

            Vector3 vec;
            Vector3* sPtr = (Vector3*)_parent._vertices.Address;
            Matrix43 m = _parent._node != null ? _parent._node._matrix : Matrix43.Identity;
            for (int i = 0; i < _elements; i++)
            {
                if (_nodeIndices != null)
                    m = _parent._model._nodes[_nodeIndices[i]]._matrix;

                _vertices[i] = m.Multiply(sPtr[_vertexIndices[i]]);
            }
        }

        internal unsafe void Render(GLContext context)
        {
            if (!_enabled)
                return;

            //context.glEnable(GLEnableCap.Texture2D);

            if (_parent._uvData[0] == null)
                return;

            context.glBegin(_type);

            Vector3* vPtr = (Vector3*)_parent._vertices.Address;
            Vector3* nPtr = (Vector3*)_parent._normals.Address;
            ARGBPixel* c1Ptr = _parent._colors1 != null ? (ARGBPixel*)_parent._colors1.Address : null;
            ARGBPixel* c2Ptr = _parent._colors2 != null ? (ARGBPixel*)_parent._colors2.Address : null;
            Vector2* uvPtr = _parent._uvData[0] != null ? (Vector2*)_parent._uvData[0].Address : null;
            Vector3 v = new Vector3(float.MaxValue);
            for (int i = 0; i < _elements; i++ )
            {
                if(c1Ptr != null)
                    context.glColor4((byte*)&c1Ptr[_colorIndices[0][i]]);
                //if(c2Ptr != null)
                //    context.glColor4((byte*)&c2Ptr[_colorIndices[1][i]]);

                if (uvPtr != null)
                    context.glTexCoord2((float*)&uvPtr[_uvIndices[0][i]]);

                //context.glNormal((float*)&nPtr[_normalIndices[i]]);


                v = _vertices[i];
                context.glVertex3v((float*)&v);

                //int index = _vertexIndices[i];
                //Vector3 vec = vPtr[index];
                //Vector3 vec = vPtr[_vertexIndices[i]];
                //Vector3 v2 = vec;

                //vec._x = (v2._x * m[0]) + (v2._y * m[4]) + (v2._z * m[8]) + m[12];
                //vec._y = (v2._x * m[1]) + (v2._y * m[5]) + (v2._z * m[9]) + m[13];
                //vec._z = (v2._x * m[2]) + (v2._y * m[6]) + (v2._z * m[10]) + m[14];

                //if (_weights != null)
                //{
                //    vec._x /= _weights[i];
                //    vec._y /= _weights[i];
                //    vec._z /= _weights[i];
                //}

                //context.glVertex(vec._x, vec._y, vec._z);
                //context.glVertex3v((float*)&vec);

                //context.glVertex3v((float*)&vPtr[_vertexIndices[i]]);

                //min.Min(vec);
                //max.Max(vec);
            }

            context.glEnd();
        }
    }
}
