using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;

namespace BrawlLib.Modeling
{
    public class Primitive : IDisposable
    {
        internal GLPrimitiveType _type;

        internal int _elementCount;

        internal ushort[] _weightIndices;
        internal ushort[] _vertexIndices;
        internal ushort[] _normalIndices;
        internal ushort[][] _colorIndices = new ushort[2][];
        internal ushort[][] _uvIndices = new ushort[8][];

        internal UnsafeBuffer _precVertices;
        internal UnsafeBuffer _precNormals;
        internal UnsafeBuffer _precColors;
        internal UnsafeBuffer[] _precUVs = new UnsafeBuffer[8];

        ~Primitive() { Dispose(); }

        internal unsafe void PreparePointers(GLContext ctx)
        {
            if (_precVertices == null)
                return;

            ctx.glVertexPointer(3, GLDataType.Float, 0, _precVertices.Address);

            if (_precNormals != null)
                ctx.glNormalPointer(GLDataType.Float, 0, _precNormals.Address);

            if (_precColors != null)
                ctx.glColorPointer(4, GLDataType.Byte, 0, _precColors.Address);
        }

        internal unsafe void Render(GLContext ctx, int uvIndex)
        {
            if (uvIndex >= 0)
            {
                if (_precUVs[uvIndex] == null)
                    return;

                ctx.glTexCoordPointer(2, GLDataType.Float, 0, _precUVs[uvIndex].Address);
            }
            else
            {
            }

            ctx.glDrawArrays(_type, 0, _elementCount);
        }

        internal unsafe void Precalc(MDL0PolygonNode parent, List<IMatrixProvider> nodes)
        {
            //If already calculated, and no weights, skip?
            if ((_precVertices != null) && (_weightIndices == null))
                return;

            //Vertices
            Vector3[] verts = parent._vertexNode.Vertices;
            if (_precVertices == null)
                _precVertices = new UnsafeBuffer(_elementCount * 12);

            Vector3* vPtr = (Vector3*)_precVertices.Address;
            if (_weightIndices != null)
                for (int i = 0; i < _elementCount; i++)
                    *vPtr++ = nodes[_weightIndices[i]].FrameMatrix.Multiply(verts[_vertexIndices[i]]);
            else
                for (int i = 0; i < _elementCount; i++)
                    *vPtr++ = verts[_vertexIndices[i]];

            //Normals
            if (_normalIndices != null)
            {
                Vector3[] norms = parent._normalNode.Normals;
                if (_precNormals == null)
                    _precNormals = new UnsafeBuffer(_elementCount * 12);
                Vector3* nPtr = (Vector3*)_precNormals.Address;
                if (_weightIndices != null)
                    for (int i = 0; i < _elementCount; i++)
                        *nPtr++ = nodes[_weightIndices[i]].FrameMatrix.Multiply(norms[_normalIndices[i]]);
                else
                    for (int i = 0; i < _elementCount; i++)
                        *nPtr++ = norms[_normalIndices[i]];
            }
            else if (_precNormals != null)
            { 
                _precNormals.Dispose(); 
                _precNormals = null; 
            }

            //Colors
            if (_colorIndices[0] != null)
            {
                ARGBPixel[] colors = parent._colorSet[0].Colors;
                if (_precColors == null)
                    _precColors = new UnsafeBuffer(_elementCount * 4);
                ABGRPixel* cPtr = (ABGRPixel*)_precColors.Address;
                for (int i = 0; i < _elementCount; i++)
                    *cPtr++ = (ABGRPixel)colors[_colorIndices[0][i]];
            }
            else if (_precColors != null)
            { 
                _precColors.Dispose(); 
                _precColors = null; 
            }

            //UV points
            for (int i = 0; i < 8; i++)
            {
                if (_uvIndices[i] != null)
                {
                    Vector2[] uvs = parent._uvSet[i].Points;
                    if (_precUVs[i] == null)
                        _precUVs[i] = new UnsafeBuffer(_elementCount * 8);
                    Vector2* uPtr = (Vector2*)_precUVs[i].Address;
                    for (int x = 0; x < _elementCount; x++)
                        *uPtr++ = uvs[_uvIndices[i][x]];
                }
                else if (_precUVs[i] != null)
                { 
                    _precUVs[i].Dispose(); 
                    _precUVs[i] = null; 
                }
            }

        }

        public void Dispose()
        {
            if (_precVertices != null) { _precVertices.Dispose(); _precVertices = null; }
            if (_precNormals != null) { _precNormals.Dispose(); _precNormals = null; }
            if (_precColors != null) { _precColors.Dispose(); _precColors = null; }
            for (int i = 0; i < 8; i++)
                if (_precUVs[i] != null) { _precUVs[i].Dispose(); _precUVs[i] = null; }
        }
    }
}
