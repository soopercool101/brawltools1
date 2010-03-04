using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BrawlLib.Imaging;
using BrawlLib.OpenGL;

namespace BrawlLib.Modeling
{
    public class Vertex3
    {
        public Vector3 Position;
        public Vector3 WeightedPosition;
        public List<NodeWeight> Influences = new List<NodeWeight>(5);
        public Matrix Matrix;

        public void Weight()
        {
            if (Influences.Count > 0)
            {
                Matrix = new Matrix();
                foreach (NodeWeight w in Influences)
                    Matrix += (w.Node.FrameMatrix * w.Node.InverseBindMatrix) * w.Weight;
                WeightedPosition = Matrix.Multiply(Position);
            }
            else
                WeightedPosition = Position;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public unsafe struct FacePoint
    {
        public ushort VertexId;
        public Vector3 Normal; //Must be multiplied by vertex matrix
        public fixed uint Color[2];
        public fixed float UVs[16]; //Multiplied with pre-set texture matrix (set by material)
    }

    public unsafe class Primitive2 : IDisposable
    {
        internal GLPrimitiveType _type;
        internal int _elements;
        internal UnsafeBuffer _indices;
        internal UnsafeBuffer _data;

        public void Dispose()
        {
            if (_indices != null)
            {
                _indices.Dispose();
                _indices = null;
            }
            if (_data != null)
            {
                _data.Dispose();
                _data = null;
            }
        }

        public void Precalc(Polygon poly)
        {
            //Manage data buffer size and adjust accordingly
            int dataSize = _elements * poly._script.Stride;

            if (_data != null)
                if (dataSize != _data.Length)
                    _data.Dispose();
                else
                    goto Next;

            _data = new UnsafeBuffer(dataSize);

        Next:
            //Fill buffer with raw data
            byte* pOut = (byte*)_data.Address;
            ushort* pIndex = (ushort*)_indices.Address;
            FacePoint* pFacePoint = (FacePoint*)poly._facePoints.Address;

            for (int i = 0; i < _elements; i++)
                poly._script.Run(poly, &pFacePoint[*pIndex++], ref pOut);
        }

        byte* pRenderAddr;
        int iRenderStride;
        public void PreparePointers(ElementDefinition def, GLContext ctx)
        {
            iRenderStride = def.Stride;
            pRenderAddr = (byte*)_data.Address;

            ctx.glVertexPointer(3, GLDataType.Float, iRenderStride, pRenderAddr);
            pRenderAddr += 12;

            if (def.Normals)
            {
                ctx.glNormalPointer(GLDataType.Float, iRenderStride, pRenderAddr);
                pRenderAddr += 12;
            }
            if (def.Colors[0])
            {
                ctx.glColorPointer(4, GLDataType.Byte, iRenderStride, pRenderAddr);
                pRenderAddr += 4;
            }

        }

        public void Render(GLContext ctx, int index)
        {
            if (index >= 0)
                ctx.glTexCoordPointer(2, GLDataType.Float, iRenderStride, pRenderAddr + (index * 8));

            ctx.glDrawArrays(_type, 0, _elements);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PrimitiveScript
    {
        public int Stride;
        private byte _count;
        private fixed byte _commands[31];

        public PrimitiveScript(ElementDefinition def)
        {
            Stride = def.Stride;
            _count = 0;
            fixed (byte* c = _commands)
            {
                if (def.Weighted)
                    c[_count++] = (byte)ScriptCommand.WeightedPosition;
                else
                    c[_count++] = (byte)ScriptCommand.Position;

                if (def.Normals)
                    if (def.Weighted)
                        c[_count++] = (byte)ScriptCommand.WeightedNormal;
                    else
                        c[_count++] = (byte)ScriptCommand.Normal;

                for (int i = 0; i < 2; i++)
                    if (def.Colors[i])
                        c[_count++] = (byte)((int)ScriptCommand.Color0 + i);

                for (int i = 0; i < 8; i++)
                    if (def.UVs[i])
                        c[_count++] = (byte)((int)ScriptCommand.UV0 + i);

                c[_count++] = 0;
            }
        }

        public void Run(Polygon poly, FacePoint* point, ref byte* pOut)
        {
            Vertex3 v = poly._vertices[point->VertexId];
            ScriptCommand o;
            int index;
            fixed (byte* c = _commands)
            {
                ScriptCommand* cmd = (ScriptCommand*)c;
            Top:
                switch(o = *cmd++)
                {
                    case ScriptCommand.None: break;

                    case ScriptCommand.Position:
                        *(Vector3*)pOut = v.Position;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.WeightedPosition:
                        *(Vector3*)pOut = v.WeightedPosition;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.Normal:
                        *(Vector3*)pOut = point->Normal;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.WeightedNormal:
                        *(Vector3*)pOut = v.Matrix * point->Normal;
                        pOut += 12;
                        goto Top;

                    case ScriptCommand.Color0:
                    case ScriptCommand.Color1:
                        index = (int)(o - ScriptCommand.Color0);
                        *(ABGRPixel*)pOut = ((ABGRPixel*)point->Color)[index];
                        pOut += 4;
                        goto Top;

                        //The rest are UVs
                    default:
                        index = (int)(o - ScriptCommand.UV0);
                        *(Vector2*)pOut = ((Vector2*)point->UVs)[index];
                        pOut += 8;
                        goto Top;
                }
            }
        }

        enum ScriptCommand : byte
        {
            None,
            Position,
            WeightedPosition,
            Normal,
            WeightedNormal,
            WeightPos,
            Color0,
            Color1,
            UV0,
            UV1,
            UV2,
            UV3,
            UV4,
            UV5,
            UV6,
            UV7
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class ElementDefinition
    {
        public int Stride;
        public bool Weighted;
        public bool Normals;
        public bool[] Colors = new bool[2];
        public bool[] UVs = new bool[8];
        public bool[] TexMatrices = new bool[8];

        public void CalcStride()
        {
            Stride = 12;
            if (Normals)
                Stride += 12;

            for (int i = 0; i < 2; i++)
                if (Colors[i])
                    Stride += 4;

            for (int i = 0; i < 8; i++)
                if (UVs[i])
                    Stride += 8;
        }
    }

    public unsafe class Polygon
    {
        //internal int _elemSize;
        internal bool _visible;
        internal Bone _attachedBone;
        internal Material _material;

        internal PrimitiveScript _script;

        internal ElementDefinition _elemDef;


        internal List<Vertex3> _vertices = new List<Vertex3>();

        internal int _facePointCount;
        internal UnsafeBuffer _facePoints;

        internal List<Primitive2> _primitives = new List<Primitive2>();

        public List<Vertex3> Vertices { get { return _vertices; } }


        public void Precalc()
        {
            if(_elemDef.Weighted)
                foreach (Vertex3 v in _vertices)
                    v.Weight();

            foreach (Primitive2 p in _primitives)
                p.Precalc(this);
        }


        internal void Render(GLContext ctx)
        {
            if (!_visible)
                return;

            //Set single bind matrix
            if (_attachedBone != null)
            {
                ctx.glPushMatrix();

                Matrix m = _attachedBone.FrameMatrix;
                ctx.glMultMatrix((float*)&m);
            }

            //Enable arrays
            ctx.glEnableClientState(GLArrayType.VERTEX_ARRAY);

            if (_elemDef.Normals)
                ctx.glEnableClientState(GLArrayType.NORMAL_ARRAY);

            if (_elemDef.Colors[0])
                ctx.glEnableClientState(GLArrayType.COLOR_ARRAY);

            //Material cannot be null!
            if (_material.Children.Count == 0)
            {
                ctx.glDisable((uint)GLEnableCap.Texture2D);
                foreach (Primitive2 prim in _primitives)
                {
                    prim.PreparePointers(_elemDef, ctx);
                    prim.Render(ctx, -1);
                }
            }
            else
            {
                ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                ctx.glEnable(GLEnableCap.Texture2D);
                foreach (MDL0MaterialRefNode mr in _material.Children)
                {
                    //if ((mr._layerId1 == 0) || (!mr._textureReference.Enabled))
                    //    continue;
                    if (!mr._texture.Enabled)
                        continue;

                    mr.Bind(ctx);
                    foreach (Primitive2 prim in _primitives)
                    {
                        prim.PreparePointers(_elemDef, ctx);
                        prim.Render(ctx, 0);
                    }
                }
                ctx.glDisable((uint)GLEnableCap.Texture2D);
                ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
            }

            //Disable arrays
            if (_elemDef.Normals)
                ctx.glDisableClientState(GLArrayType.NORMAL_ARRAY);

            if (_elemDef.Colors[0])
                ctx.glDisableClientState(GLArrayType.COLOR_ARRAY);

            ctx.glDisableClientState(GLArrayType.VERTEX_ARRAY);

            //Pop matrix
            if (_attachedBone != null)
                ctx.glPopMatrix();
        }
    }
}
