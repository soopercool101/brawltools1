using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Runtime.InteropServices;
using BrawlLib.Modeling;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelConverter
    {
        //public static Model Extract(MDL0Node node)
        //{
        //    ResourceNode n;
        //    Model m = new Model();

        //    //Cache nodes
        //    object[] nodeCache = new object[node.NumNodes];

        //    //Extract bones
        //    if ((n = node.FindChild("Bones", false)) != null)
        //        foreach (MDL0BoneNode bone in n.Children)
        //            m._bones.Add(ParseBone(bone, nodeCache));

        //    //Extract polygons
        //    //if ((n = node.FindChild("Polygons", false)) != null)
        //    //    foreach (MDL0PolygonNode poly in n.Children)
        //    //        m._polygons.Add(ParsePolygon(poly));



        //    return m;
        //}
        //private static BoneNode ParseBone(MDL0BoneNode node, object[] cache)
        //{
        //    BoneNode bone = new BoneNode();
        //    bone._name = node._name;
        //    bone.BaseTransform = new FrameState(node.Scale, node.Rotation, node.Translation);

        //    cache[node.NodeId] = bone;

        //    foreach (MDL0BoneNode n in node.Children)
        //        bone._children.Add(ParseBone(n, cache));

        //    return bone;
        //}
        //private static Polygon ParsePolygon(MDL0PolygonNode poly)
        //{
        //    Polygon p = new Polygon();
        //    p._name = poly.Name;

        //    //Extract raw data
        //    MDL0VertexNode vNode = poly.GetVertexNode();
        //    if(vNode != null)
        //        p._vertices = vNode.GetVertices();

        //    MDL0Polygon* polygon = poly.Header;
        //    VoidPtr address = polygon->PrimitiveData;
        //    ModelEntrySize e = new ModelEntrySize(polygon->_flags);

        //    p._vertices = ExtractVertices(polygon->VertexData);
        //    if (polygon->_normalId != -1)
        //        p._normals = ExtractNormals(polygon->NormalData);
        //    if (polygon->_colorId1 != -1)
        //        p._colors1 = ExtractColors(polygon->ColorData1);
        //    if (polygon->_colorId2 != -1)
        //        p._colors2 = ExtractColors(polygon->ColorData2);

        //    MDL0UVData* uvPtr;
        //    for (int i = 0; (i < 8) && ((uvPtr = polygon->GetUVData(i)) != null); i++)
        //        p._uvData[i] = ExtractUVs(uvPtr);

        //    int nodeIndex = 0;
        //    ushort[] nodeBuffer = new ushort[16];
        //    while ((prim = ExtractPrimitive(ref address, e, p, nodeBuffer, ref nodeIndex)) != null)
        //        p._primitives.Add(prim);

        //    return p;
        //}

        //public static GLPolygon ExtractPolygon(GLModel model, MDL0Polygon* polygon)
        //{
        //    GLPolygon p = new GLPolygon();
        //    GLPrimitive prim;
        //    p._name = polygon->ResourceString;
        //    p._model = model;
        //    p._index = polygon->_index;
        //    //p._nodeIndex = polygon->_nodeId;
        //    if (polygon->_nodeId != -1)
        //        p._node = model._nodes[polygon->_nodeId];

        //    VoidPtr address = polygon->PrimitiveData;
        //    ModelEntrySize e = new ModelEntrySize(polygon->_flags);

        //    p._vertices = ExtractVertices(polygon->VertexData);
        //    if(polygon->_normalId != -1)
        //        p._normals = ExtractNormals(polygon->NormalData);
        //    if (polygon->_colorId1 != -1)
        //        p._colors1 = ExtractColors(polygon->ColorData1);
        //    if (polygon->_colorId2 != -1)
        //        p._colors2 = ExtractColors(polygon->ColorData2);

        //    MDL0UVData* uvPtr;
        //    for(int i = 0 ; (i < 8) && ((uvPtr = polygon->GetUVData(i)) != null) ; i++)
        //        p._uvData[i] = ExtractUVs(uvPtr);

        //    int nodeIndex = 0;
        //    ushort[] nodeBuffer = new ushort[16];
        //    while((prim = ExtractPrimitive(ref address, e, p, nodeBuffer,ref nodeIndex)) != null)
        //        p._primitives.Add(prim);

        //    return p;
        //}

        public static List<Primitive> ExtractPrimitives(MDL0Polygon* polygon)
        {
            List<Primitive> list = new List<Primitive>();

            VoidPtr dataAddr = polygon->PrimitiveData;
            ModelEntrySize e = new ModelEntrySize(polygon->_flags);

            int nodeIndex = 0;
            ushort[] nodeBuffer = new ushort[16];
            Primitive p;

            while ((p = ExtractPrimitive(ref dataAddr, e, nodeBuffer, ref nodeIndex)) != null)
                list.Add(p);

            return list;
        }

        private static Vector2[] ExtractPoints(VoidPtr address, int count, WiiVertexComponentType type, float divisor)
        {
            Vector2[] points = new Vector2[count];

            fixed (Vector2* p = points)
            {
                float* dPtr = (float*)p;
                for (int i = 0; i < count; i++)
                {
                    *dPtr++ = ReadValue(ref address, type, divisor);
                    *dPtr++ = ReadValue(ref address, type, divisor);
                }
            }
            return points;
        }
        private static Vector3[] ExtractVertices(VoidPtr address, int count, bool isXYZ, WiiVertexComponentType type, float divisor)
        {
            Vector3[] verts = new Vector3[count];
            fixed (Vector3* p = verts)
            {
                float* dPtr = (float*)p;
                for (int i = 0; i < count; i++)
                {
                    *dPtr++ = ReadValue(ref address, type, divisor);
                    *dPtr++ = ReadValue(ref address, type, divisor);
                    if (isXYZ)
                        *dPtr++ = ReadValue(ref address, type, divisor);
                    else
                        *dPtr++ = 0.0f;
                }
            }

            return verts;
        }
        private static float ReadValue(ref VoidPtr addr, WiiVertexComponentType type, float divisor)
        {
            switch (type)
            {
                case WiiVertexComponentType.UInt8: addr += 1; return ((byte*)addr)[-1] / divisor;
                case WiiVertexComponentType.Int8: addr += 1; return ((sbyte*)addr)[-1] / divisor;
                case WiiVertexComponentType.UInt16: addr += 2; return ((bushort*)addr)[-1] / divisor;
                case WiiVertexComponentType.Int16: addr += 2; return ((bshort*)addr)[-1] / divisor;
                case WiiVertexComponentType.Float: addr += 4; return ((bfloat*)addr)[-1];
            }
            return 0.0f;
        }

        public static ARGBPixel[] ExtractColors(MDL0ColorData* colors)
        {
            int count = colors->_numEntries;
            ARGBPixel[] c = new ARGBPixel[count];

            fixed (ARGBPixel* p = c)
            {
                ARGBPixel* dPtr = p;

                switch (colors->Type)
                {
                    case WiiColorComponentType.RGB565:
                        {
                            wRGB565Pixel* sPtr = (wRGB565Pixel*)colors->Data;
                            for (int i = 0; i < count; i++)
                                *dPtr++ = (ARGBPixel)(*sPtr++);
                            break;
                        }
                    case WiiColorComponentType.RGB8:
                        {
                            wRGBPixel* sPtr = (wRGBPixel*)colors->Data;
                            for (int i = 0; i < count; i++)
                                *dPtr++ = (ARGBPixel)(*sPtr++);
                            break;
                        }
                    case WiiColorComponentType.RGBX8:
                        {
                            wRGBXPixel* sPtr = (wRGBXPixel*)colors->Data;
                            for (int i = 0; i < count; i++)
                                *dPtr++ = (ARGBPixel)(*sPtr++);
                            break;
                        }
                    case WiiColorComponentType.RGBA4:
                        {
                            wRGBA4Pixel* sPtr = (wRGBA4Pixel*)colors->Data;
                            for (int i = 0; i < count; i++)
                                *dPtr++ = (ARGBPixel)(*sPtr++);
                            break;
                        }
                    case WiiColorComponentType.RGBA6:
                        {
                            wRGBA6Pixel* sPtr = (wRGBA6Pixel*)colors->Data;
                            for (int i = 0; i < count; i++)
                                *dPtr++ = (ARGBPixel)(*sPtr++);
                            break;
                        }
                    case WiiColorComponentType.RGBA8:
                        {
                            wRGBAPixel* sPtr = (wRGBAPixel*)colors->Data;
                            for (int i = 0; i < count; i++)
                                *dPtr++ = (ARGBPixel)(*sPtr++);
                            break;
                        }
                }
            }
            return c;
        }

        public static Vector3[] ExtractVertices(MDL0VertexData* vertices)
        {
            return ExtractVertices(vertices->Data, vertices->_numVertices, vertices->_isXYZ != 0, vertices->Type, (float)(1 << vertices->_divisor));
        }
        public static Vector3[] ExtractNormals(MDL0NormalData* normals)
        {
            return ExtractVertices(normals->Data, normals->_numVertices, true, normals->Type, (float)(1 << normals->_divisor));
        }
        public static Vector2[] ExtractUVs(MDL0UVData* uvs)
        {
            return ExtractPoints(uvs->Entries, uvs->_numEntries, uvs->Type, (float)(1 << uvs->_divisor));
        }

        private delegate ushort IndexParser(VoidPtr addr);
        private static IndexParser ByteParser = x => *(byte*)x;
        private static IndexParser UShortParser = x => *(bushort*)x;
        private static Primitive ExtractPrimitive(ref VoidPtr address, ModelEntrySize entryInfo, ushort[] nodeBuffer,ref int nodeIndex)
        {
            Top:
            PrimitiveHeader* header = (PrimitiveHeader*)address;
            GLPrimitiveType type;
            switch (header->Type)
            {
                case WiiPrimitiveType.BoneDef1:
                    {
                        if (*(bushort*)header->Data == 0xB000)
                            nodeIndex = 0;
                        nodeBuffer[nodeIndex++] = header->Entries;

                        //nodeBuffer[(*(bushort*)header->Data - 0xB000) / 0x0C] = header->Entries;
                        address += 5; 
                        goto Top;
                    }
                case WiiPrimitiveType.BoneDef2:
                case WiiPrimitiveType.BoneDef3:
                    { address += 5; goto Top; }
                case WiiPrimitiveType.Lines: { type = GLPrimitiveType.Lines; break; }
                case WiiPrimitiveType.LineStrip: { type = GLPrimitiveType.LineStrip; break; }
                case WiiPrimitiveType.Points: { type = GLPrimitiveType.Points; break; }
                case WiiPrimitiveType.Quads: { type = GLPrimitiveType.Quads; break; }
                case WiiPrimitiveType.TriangleFan: { type = GLPrimitiveType.TriangleFan; break; }
                case WiiPrimitiveType.Triangles: { type = GLPrimitiveType.Triangles; break; }
                case WiiPrimitiveType.TriangleStrip: { type = GLPrimitiveType.TriangleStrip; break; }
                default: return null;
            }

            Primitive primitive = new Primitive();
            primitive._type = type;
            ;

            int entries = primitive._elementCount = header->Entries;
            int stride = entryInfo._totalLen;
            VoidPtr data = header->Data;

            //Weight indices
            primitive._weightIndices = ParseWeights(data, entries, entryInfo._extraLen, stride, nodeBuffer);
            data += entryInfo._extraLen;

            //Vertex Data
            primitive._vertexIndices = ParseIndices(data, entries, entryInfo._vertexLen, stride);
            data += entryInfo._vertexLen;

            //Normal Data
            primitive._normalIndices = ParseIndices(data, entries, entryInfo._normalLen, stride);
            data += entryInfo._normalLen;

            //Color Data
            for (int i = 0; i < 2; data += entryInfo._colorLen[i++])
                primitive._colorIndices[i] = ParseIndices(data, entries, entryInfo._colorLen[i], stride);

            //UV Data
            for (int i = 0; i < 8; data += entryInfo._uvLen[i++])
                primitive._uvIndices[i] = ParseIndices(data, entries, entryInfo._uvLen[i], stride);

            address += (entryInfo._totalLen * entries) + 3;

            return primitive;
        }

        private static ushort[] ParseWeights(VoidPtr address, int elementCount, int elementLen, int stride, ushort[] nodeBuffer)
        {
            if (elementLen == 0)
                return null;

            ushort[] indices = new ushort[elementCount];
            byte* ptr = (byte*)address;

            for (int i = 0; i < elementCount; ptr += stride )
                indices[i++] = nodeBuffer[*ptr / 3];

            return indices;
        }

        private static ushort[] ParseIndices(VoidPtr address, int elementCount, int elementLen, int stride)
        {
            if (elementLen == 0)
                return null;

            ushort[] indices = new ushort[elementCount];
            byte* ptr = (byte*)address;

            if (elementLen == 1)
                for (int i = 0; i < elementCount; ptr += stride)
                    indices[i++] = *ptr;
            else
                for (int i = 0; i < elementCount; ptr += stride)
                    indices[i++] = *(bushort*)ptr;

            return indices;
        }
    }
}
