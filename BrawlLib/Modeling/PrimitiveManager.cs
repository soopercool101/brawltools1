using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Models;
using BrawlLib.Imaging;
using BrawlLib.Wii.Graphics;
using BrawlLib.OpenGL;

namespace BrawlLib.Modeling
{
    public unsafe class NewPrimitive : IDisposable
    {
        internal GLPrimitiveType _type;
        internal int _elementCount;
        internal UnsafeBuffer _indices;

        public NewPrimitive(int elements, GLPrimitiveType type)
        {
            _elementCount = elements;
            _type = type;
            _indices = new UnsafeBuffer(_elementCount * 2); //ushort or uint?
        }
        ~NewPrimitive() { Dispose(); }
        public void Dispose()
        {
            if (_indices != null)
            {
                _indices.Dispose();
                _indices = null;
            }
        }

        internal unsafe void Render(GLContext ctx)
        {
            ctx.glDrawElements(_type, _elementCount, GLElementType.UNSIGNED_SHORT, _indices.Address);
        }
    }

    unsafe class PrimitiveManager
    {
        public List<Vertex3> _vertices;

        internal int _pointCount, _stride;
        internal UnsafeBuffer[] _faceData = new UnsafeBuffer[12];
        internal bool[] _dirty = new bool[12];

        UnsafeBuffer _graphicsBuffer;

        internal NewPrimitive _faces, _lines, _points;

        public PrimitiveManager() { }
        public PrimitiveManager(MDL0Polygon* polygon, AssetStorage assets, Influence[] nodes)
        {
            _pointCount = polygon->_numVertices;

            //Grab asset lists
            byte*[] pAssetList = new byte*[12];
            byte*[] pOutList = new byte*[12];
            int id;

            if ((id = polygon->_vertexId) >= 0)
            {
                pAssetList[0] = (byte*)assets.Assets[0][id].Address;
                pOutList[0] = (byte*)(_faceData[0] = new UnsafeBuffer(2 * _pointCount)).Address;
            }
            //Save vertices for last

            if ((id = polygon->_normalId) >= 0)
            {
                pAssetList[1] = (byte*)assets.Assets[1][id].Address;
                pOutList[1] = (byte*)(_faceData[1] = new UnsafeBuffer(12 * _pointCount)).Address;
            }

            for (int i = 0, x = 2; i < 2; i++, x++)
                if ((id = ((bshort*)polygon->_colorIds)[i]) >= 0)
                {
                    pAssetList[x] = (byte*)assets.Assets[2][id].Address;
                    pOutList[x] = (byte*)(_faceData[x] = new UnsafeBuffer(4 * _pointCount)).Address;
                }

            for (int i = 0, x = 4; i < 8; i++, x++)
                if ((id = ((bshort*)polygon->_uids)[i]) >= 0)
                {
                    pAssetList[x] = (byte*)assets.Assets[3][id].Address;
                    pOutList[x] = (byte*)(_faceData[x] = new UnsafeBuffer(8 * _pointCount)).Address;
                }


            //Compile decode script
            ElementDescriptor desc = new ElementDescriptor(polygon);

            //Extract primitives
            fixed (byte** pOut = pOutList)
            fixed (byte** pAssets = pAssetList)
                ExtractPrimitives((byte*)polygon->PrimitiveData, ref desc, pOut, pAssets);

            //Compile vertex list using nodes
            _vertices = desc.Finish((Vector3*)pAssetList[0], nodes);
        }

        internal void ExtractPrimitives(byte* pData, ref ElementDescriptor desc, byte** pOut, byte** pAssets)
        {
            int count;
            ushort index = 0, temp;
            byte* pTemp = pData;

            //Get element count for each primitive type
            int d3 = 0, d2 = 0, d1 = 0;
            ushort* p3, p2, p1;

            //Get counts for each primitive type, and decode faces
        CountTop:
            switch ((GXListCommand)(*pTemp++))
            {
                case GXListCommand.LoadIndexA:
                    desc.SetNode(ref pTemp);
                    goto CountTop;

                case GXListCommand.LoadIndexB:
                case GXListCommand.LoadIndexC:
                case GXListCommand.LoadIndexD:
                    pTemp += 4;
                    goto CountTop;

                case GXListCommand.DrawQuads:
                    count = *(bushort*)pTemp;
                    d3 += count / 2 * 3;
                    break;

                case GXListCommand.DrawTriangles:
                    count = *(bushort*)pTemp;
                    d3 += count;
                    break;

                case GXListCommand.DrawTriangleFan:
                case GXListCommand.DrawTriangleStrip:
                    count = *(bushort*)pTemp;
                    d3 += (count - 2) * 3;
                    break;

                case GXListCommand.DrawLines:
                    count = *(bushort*)pTemp;
                    d2 += count;
                    break;

                case GXListCommand.DrawLineStrip:
                    count = *(bushort*)pTemp;
                    d2 += (count - 1) * 2;
                    break;

                case GXListCommand.DrawPoints:
                    count = *(bushort*)pTemp;
                    d1 += count;
                    break;

                default: goto Next;
            }

            //Extract face points here!
            pTemp += 2;
            desc.Run(ref pTemp, pAssets, pOut, count);

            goto CountTop;

        Next:
            if (d3 > 0)
            { _faces = new NewPrimitive(d3, GLPrimitiveType.Triangles); p3 = (ushort*)_faces._indices.Address; }
            else
            { _faces = null; p3 = null; }

            if (d2 > 0)
            { _lines = new NewPrimitive(d2, GLPrimitiveType.Lines); p2 = (ushort*)_lines._indices.Address; }
            else
            { _lines = null; p2 = null; }

            if (d1 > 0)
            { _points = new NewPrimitive(d1, GLPrimitiveType.Points); p1 = (ushort*)_points._indices.Address; }
            else
            { _points = null; p1 = null; }


            //Extract indices in reverse order, this way we get CCW winding.
        Top:
            switch ((GXListCommand)(*pData++))
            {
                case GXListCommand.LoadIndexA:
                case GXListCommand.LoadIndexB:
                case GXListCommand.LoadIndexC:
                case GXListCommand.LoadIndexD:
                    pData += 4;
                    goto Top;

                case GXListCommand.DrawQuads:
                    count = *(bushort*)pData;

                    for (int i = 0; i < count; i += 4)
                    {
                        *p3++ = index;
                        *p3++ = (ushort)(index + 2);
                        *p3++ = (ushort)(index + 1);
                        *p3++ = index;
                        *p3++ = (ushort)(index + 3);
                        *p3++ = (ushort)(index + 2);

                        //if ((i & 3) == 2)
                        //{
                        //    *p3++ = index;
                        //    *p3++ = index;
                        //    *p3++ = (ushort)(index++ - 1);
                        //}
                        //*p3++ = index++;
                        index += 4;
                    }
                    break;

                case GXListCommand.DrawTriangles:
                    count = *(bushort*)pData;

                    for (int i = 0; i < count; i += 3)
                    {
                        *p3++ = (ushort)(index + 2);
                        *p3++ = (ushort)(index + 1);
                        *p3++ = index;
                        index += 3;
                    }
                    break;

                case GXListCommand.DrawTriangleFan:
                    count = *(bushort*)pData;

                    temp = index++;
                    for (int i = 2; i < count; i++)
                    {
                        *p3++ = temp;
                        *p3++ = (ushort)(index + 1);
                        *p3++ = index++;
                    }
                    index++;

                    break;

                case GXListCommand.DrawTriangleStrip:
                    count = *(bushort*)pData;

                    //temp = index;
                    index += 2;
                    for (int i = 2; i < count; i++)
                    {
                        if ((i & 1) == 0)
                        {
                            *p3++ = (ushort)(index - 2);
                            *p3++ = index;
                            *p3++ = (ushort)(index - 1);
                            index++;
                            //*p3++ = temp++;
                            //*p3++ = temp++;
                            //*p3++ = index++;
                        }
                        else
                        {
                            *p3++ = (ushort)(index - 1);
                            *p3++ = index;
                            *p3++ = (ushort)(index - 2);
                            index++;
                            //*p3++ = temp--;
                            //*p3++ = temp++;
                            //*p3++ = index++;
                        }
                    }

                    break;

                case GXListCommand.DrawLines:
                    count = *(bushort*)pData;
                    for (int i = 0; i < count; i++)
                        *p2++ = index++;
                    break;

                case GXListCommand.DrawLineStrip:
                    count = *(bushort*)pData;
                    for (int i = 1; i < count; i++)
                    {
                        *p2++ = index++;
                        *p2++ = index;
                    }
                    index++;
                    break;

                case GXListCommand.DrawPoints:
                    count = *(bushort*)pData;
                    for (int i = 0; i < count; i++)
                        *p1++ = index++;
                    break;

                default:
                    return;
            }

            pData += 2 + count * desc.Stride;
            goto Top;

        }

        private void CalcStride()
        {
            _stride = 0;
            for (int i = 0; i < 2; i++)
                if (_faceData[i] != null)
                    _stride += 12;
            for (int i = 2; i < 4; i++)
                if (_faceData[i] != null)
                    _stride += 4;
            for (int i = 4; i < 12; i++)
                if (_faceData[i] != null)
                    _stride += 8;
        }

        internal void UpdateStream(int index)
        {
            _dirty[index] = false;

            if (_faceData[index] == null)
                return;

            //Set starting address
            byte* pOut = (byte*)_graphicsBuffer.Address;
            for (int i = 0; i < index; i++)
                if (_faceData[i] != null)
                {
                    if (i < 2)
                        pOut += 12;
                    else if (i < 4)
                        pOut += 4;
                    else
                        pOut += 8;
                }

            if (index == 0)
            {
                ushort* pIn = (ushort*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    *(Vector3*)pOut = _vertices[*pIn++].WeightedPosition;
            }
            else if (index == 1)
            {
                Vector3* pIn = (Vector3*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    *(Vector3*)pOut = *pIn++;
            }
            else if (index < 4)
            {
                RGBAPixel* pIn = (RGBAPixel*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    *(RGBAPixel*)pOut = *pIn++;
            }
            else
            {
                Vector2* pIn = (Vector2*)_faceData[index].Address;
                for (int i = 0; i < _pointCount; i++, pOut += _stride)
                    *(Vector2*)pOut = *pIn++;
            }
        }

        internal unsafe void PrepareStream(GLContext ctx)
        {
            CalcStride();
            int bufferSize = _stride * _pointCount;

            if ((_graphicsBuffer != null) && (_graphicsBuffer.Length != bufferSize))
            {
                _graphicsBuffer.Dispose();
                _graphicsBuffer = null;
            }

            if (_graphicsBuffer == null)
            {
                _graphicsBuffer = new UnsafeBuffer(bufferSize);
                for (int i = 0; i < 12; i++)
                    _dirty[i] = true;
            }

            byte* pData = (byte*)_graphicsBuffer.Address;
            for (int i = 0; i < 12; i++)
            {
                if (_dirty[i])
                    UpdateStream(i);

                if (_faceData[i] == null)
                    continue;

                switch (i)
                {
                    case 0:
                        ctx.glEnableClientState(GLArrayType.VERTEX_ARRAY);
                        ctx.glVertexPointer(3, GLDataType.Float, _stride, pData);
                        pData += 12;
                        break;

                    case 1:
                        ctx.glEnableClientState(GLArrayType.NORMAL_ARRAY);
                        ctx.glNormalPointer(GLDataType.Float, _stride, pData);
                        pData += 12;
                        break;

                    case 2:
                        ctx.glEnableClientState(GLArrayType.COLOR_ARRAY);
                        ctx.glColorPointer(4, GLDataType.Byte, _stride, pData);
                        pData += 4;
                        break;

                    case 3:
                        pData += 4;
                        break;

                    default:
                        pData += 8;
                        break;

                }
            }
        }

        internal unsafe void DetachStreams(GLContext ctx)
        {
            ctx.glDisableClientState(GLArrayType.COLOR_ARRAY);
            ctx.glDisableClientState(GLArrayType.NORMAL_ARRAY);
            ctx.glDisableClientState(GLArrayType.VERTEX_ARRAY);
            ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
            ctx.glDisable((uint)GLEnableCap.Texture2D);
        }

        internal void Render(GLContext ctx, int texId)
        {
            if ((texId >= 0) && (_faceData[texId += 4] != null))
            {
                byte* pData = (byte*)_graphicsBuffer.Address;
                for (int i = 0; i < texId; i++)
                    if (_faceData[i] != null)
                    {
                        if (i < 2)
                            pData += 12;
                        else if (i < 4)
                            pData += 4;
                        else
                            pData += 8;
                    }

                ctx.glEnable(GLEnableCap.Texture2D);
                ctx.glEnableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
                ctx.glTexCoordPointer(2, GLDataType.Float, _stride, pData);
            }
            else
            {
                ctx.glDisable((uint)GLEnableCap.Texture2D);
                ctx.glDisableClientState(GLArrayType.TEXTURE_COORD_ARRAY);
            }

            if (_faces != null)
                _faces.Render(ctx);
            if (_lines != null)
                _lines.Render(ctx);
            if (_points != null)
                _points.Render(ctx);
        }

        internal void Weight()
        {
            foreach (Vertex3 v in _vertices)
                v.Weight();
            _dirty[0] = true;
        }
    }
}
