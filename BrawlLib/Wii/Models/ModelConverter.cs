using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Textures;
using BrawlLib.Imaging;
using System.Runtime.InteropServices;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelConverter
    {

        public static GLPolygon ExtractPolygon(MDL0Polygon* polygon)
        {
            GLPolygon p = new GLPolygon();
            GLPrimitive prim;
            p._index = polygon->_index;

            VoidPtr address = polygon->PrimitiveData;
            ModelEntrySize e = new ModelEntrySize(polygon->_flags);

            p._vertices = ExtractVertices(polygon->VertexData, true);
            p._normals = ExtractNormals(polygon->NormalData);
            if (polygon->_colorId1 != -1)
                p._colors1 = ExtractColors(polygon->ColorData1);
            if (polygon->_colorId2 != -1)
                p._colors2 = ExtractColors(polygon->ColorData2);

            MDL0UVData* uvPtr;
            for(int i = 0 ; (i < 8) && ((uvPtr = polygon->GetUVData(i)) != null) ; i++)
                p._uvData[i] = ExtractUVs(uvPtr);

            while((prim = ExtractPrimitive(ref address, e)) != null)
                p._primitives.Add(prim);

            return p;
        }

        public static VertexBuffer ExtractVertices(MDL0VertexData* vertices, bool isXYZ)
        {
            int count = vertices->_numVertices;
            int elements = isXYZ ? count * 3 : count * 2;
            float divisor = (float)vertices->_divisor;

            VertexBuffer buffer = new VertexBuffer(count, isXYZ);
            float* dPtr = (float*)buffer.Address;

            switch (vertices->Type)
            {
                case WiiVertexComponentType.UInt8:
                    {
                        byte* sPtr = (byte*)vertices->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Int8:
                    {
                        sbyte* sPtr = (sbyte*)vertices->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.UInt16:
                    {
                        bushort* sPtr = (bushort*)vertices->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Int16:
                    {
                        bshort* sPtr = (bshort*)vertices->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Float:
                    {
                        bfloat* sPtr = (bfloat*)vertices->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++;
                        break;
                    }
            }

            if (isXYZ)
            {
                Vector3* vec = (Vector3*)buffer.Address;
                for (int i = 0; i < count; i++, vec++)
                {
                    //float x = vec->_x;
                    //vec->_x = vec->_z;
                    //vec->_z = vec->_y;
                    //vec->_y = x;
                }
            }
            return buffer;
        }

        public static ColorBuffer ExtractColors(MDL0ColorData* colors)
        {
            int count = colors->_numEntries;

            ColorBuffer buffer = new ColorBuffer(count);
            ARGBPixel* dPtr = (ARGBPixel*)buffer.Address;

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
            return buffer;
        }

        public static VertexBuffer ExtractNormals(MDL0NormalData* normals)
        {
            int count = normals->_numVertices;
            int elements = count * 3;
            float divisor = (float)normals->_divisor;

            VertexBuffer buffer = new VertexBuffer(count, true);
            float* dPtr = (float*)buffer.Address;

            switch (normals->Type)
            {
                case WiiVertexComponentType.UInt8:
                    {
                        byte* sPtr = (byte*)normals->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Int8:
                    {
                        sbyte* sPtr = (sbyte*)normals->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.UInt16:
                    {
                        bushort* sPtr = (bushort*)normals->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Int16:
                    {
                        bshort* sPtr = (bshort*)normals->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Float:
                    {
                        bfloat* sPtr = (bfloat*)normals->Data;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++;
                        break;
                    }
            }
            return buffer;
        }

        public static VertexBuffer ExtractUVs(MDL0UVData* uvs)
        {
            int count = uvs->_numEntries;
            int elements = count * 2;
            float divisor = (float)uvs->_divisor;

            VertexBuffer buffer = new VertexBuffer(count, false);
            float* dPtr = (float*)buffer.Address;

            switch (uvs->Type)
            {
                case WiiVertexComponentType.UInt8:
                    {
                        byte* sPtr = (byte*)uvs->Entries;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Int8:
                    {
                        sbyte* sPtr = (sbyte*)uvs->Entries;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.UInt16:
                    {
                        bushort* sPtr = (bushort*)uvs->Entries;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Int16:
                    {
                        bshort* sPtr = (bshort*)uvs->Entries;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++ / divisor;
                        break;
                    }
                case WiiVertexComponentType.Float:
                    {
                        bfloat* sPtr = (bfloat*)uvs->Entries;
                        for (int i = 0; i < elements; i++)
                            *dPtr++ = *sPtr++;
                        break;
                    }
            }
            return buffer;
        }

        private delegate ushort IndexParser(VoidPtr addr);
        private static IndexParser ByteParser = x => *(byte*)x;
        private static IndexParser UShortParser = x => *(bushort*)x;
        public static GLPrimitive ExtractPrimitive(ref VoidPtr address, ModelEntrySize entryInfo)
        {
            PrimitiveHeader* header = (PrimitiveHeader*)address;
            GLPrimitiveType type;
            switch (header->Type)
            {
                case WiiPrimitiveType.BoneDef1:
                case WiiPrimitiveType.BoneDef2:
                case WiiPrimitiveType.BoneDef3: 
                    { address += 5; return new GLBoneDef(header); }
                case WiiPrimitiveType.Lines: { type = GLPrimitiveType.Lines; break; }
                case WiiPrimitiveType.LineStrip: { type = GLPrimitiveType.LineStrip; break; }
                case WiiPrimitiveType.Points: { type = GLPrimitiveType.Points; break; }
                case WiiPrimitiveType.Quads: { type = GLPrimitiveType.Quads; break; }
                case WiiPrimitiveType.TriangleFan: { type = GLPrimitiveType.TriangleFan; break; }
                case WiiPrimitiveType.Triangles: { type = GLPrimitiveType.Triangles; break; }
                case WiiPrimitiveType.TriangleStrip: { type = GLPrimitiveType.TriangleStrip; break; }
                default: return null;
            }

            GLPrimitive primitive = new GLPrimitive();
            primitive._type = type;
            primitive._elements = header->Entries;

            int entries = primitive._elements;
            int stride = entryInfo._totalLen;
            VoidPtr data = header->Data;

            //Extra data
            data += entryInfo._extraLen;

            //Vertex Data
            primitive._vertexIndices = ParseIndices(data, entries, entryInfo._vertexLen, stride);
            data += entryInfo._vertexLen;

            //Normal Data
            primitive._normalIndices = ParseIndices(data, entries, entryInfo._normalLen, stride);
            data += entryInfo._normalLen;

            //Color Data
            for (int i = 0; i < 2; data += entryInfo._colorLen[ i++])
                primitive._colorIndices[i] = ParseIndices(data, entries, entryInfo._colorLen[i], stride);

            //UV Data
            for (int i = 0; i < 8; data += entryInfo._uvLen[i++])
                primitive._uvIndices[i] = ParseIndices(data, entries, entryInfo._uvLen[i], stride);

            address += (entryInfo._totalLen * entries) + 3;

            return primitive;
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
