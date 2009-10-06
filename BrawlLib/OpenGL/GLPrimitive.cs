using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Imaging;

namespace BrawlLib.OpenGL
{
    public class GLPrimitive
    {
        internal ushort[] _vertexIndices;
        internal ushort[] _normalIndices;
        internal ushort[][] _colorIndices = new ushort[2][];
        internal ushort[][] _uvIndices = new ushort[8][];

        internal GLPrimitiveType _type;
        internal int _elements;

        public bool _enabled = true;

        internal unsafe void Render(GLContext context, GLPolygon parent)
        {
            if (!_enabled)
                return;

            context.glBegin(_type);

            Vector3* vPtr = (Vector3*)parent._vertices.Address;
            Vector3* nPtr = (Vector3*)parent._normals.Address;
            ARGBPixel* c1Ptr = parent._colors1 != null ? (ARGBPixel*)parent._colors1.Address : null;
            ARGBPixel* c2Ptr = parent._colors2 != null ? (ARGBPixel*)parent._colors2.Address : null;
            Vector2* uvPtr = parent._uvData[0] != null ? (Vector2*)parent._uvData[0].Address : null;
            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);
            for (int i = 0; i < _elements; i++ )
            {
                if(c1Ptr != null)
                    context.glColor4((byte*)&c1Ptr[_colorIndices[0][i]]);
                if(c2Ptr != null)
                    context.glColor4((byte*)&c2Ptr[_colorIndices[0][i]]);

                if (uvPtr != null)
                    context.glTexCoord2((float*)&uvPtr[_uvIndices[0][i]]);

                context.glNormal((float*)&nPtr[_normalIndices[i]]);

                //int index = _vertexIndices[i];
                //Vector3 vec = vPtr[index];
                context.glVertex3v((float*)&vPtr[_vertexIndices[i]]);

                //min.Min(vec);
                //max.Max(vec);
            }

            context.glEnd();
        }
    }
}
