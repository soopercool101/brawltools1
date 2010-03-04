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
using BrawlLib.Wii.Graphics;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelConverter
    {
        public static List<Primitive> ExtractPrimitives(MDL0Polygon* polygon)
        {
            List<Primitive> list = new List<Primitive>();

            VoidPtr dataAddr = polygon->PrimitiveData;
            ElementFlags e = new ElementFlags(polygon->_elemFlags, polygon->_texFlags);
            //ModelEntrySize e = new ModelEntrySize(polygon->_flags);

            int nodeIndex = 0;
            ushort[] nodeBuffer = new ushort[16];
            Primitive p;

            while ((p = ExtractPrimitive(ref dataAddr, e, nodeBuffer, ref nodeIndex)) != null)
                list.Add(p);

            return list;
        }

        private delegate ushort IndexParser(VoidPtr addr);
        private static IndexParser ByteParser = x => *(byte*)x;
        private static IndexParser UShortParser = x => *(bushort*)x;
        private static Primitive ExtractPrimitive(ref VoidPtr address, ElementFlags entryInfo, ushort[] nodeBuffer, ref int nodeIndex)
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
            primitive._type = GLPrimitiveType.Triangles;
            

            int entries = primitive._elementCount = header->Entries;
            int stride = entryInfo.Stride;
            byte* data = (byte*)header->Data;

            //Pos matrices
            if (entryInfo.PosNormMatrixIndex)
            {
                primitive._weightIndices = ParseWeights(data, entries, stride, nodeBuffer);
                data += 1;
            }

            //Tex matrices
            for (int i = 0; i < 8; i++)
                if (entryInfo.TexMatrixIndex[i])
                    data++;

            XFDataFormat* fPtr = &entryInfo.PositionFormat;

            primitive._vertexIndices = ParseElement(ref data, *fPtr++, entries, stride);
            primitive._normalIndices = ParseElement(ref data, *fPtr++, entries, stride);

            //Color Data
            for (int i = 0; i < 2; )
                primitive._colorIndices[i++] = ParseElement(ref data, *fPtr++, entries, stride);

            //UV Data
            for (int i = 0; i < 8; )
                primitive._uvIndices[i++] = ParseElement(ref data, *fPtr++, entries, stride);

            address += stride * entries + 3;

            return primitive;
        }

        private static ushort[] ParseWeights(byte* pData, int elementCount, int stride, ushort[] nodeBuffer)
        {
            ushort[] indices = new ushort[elementCount];

            for (int i = 0; i < elementCount; pData += stride )
                indices[i++] = nodeBuffer[*pData / 3];

            return indices;
        }

        private static ushort[] ParseElement(ref byte* pData, XFDataFormat fmt, int count, int stride)
        {
            if (fmt == XFDataFormat.None)
                return null;

            ushort[] indices = new ushort[count];

            byte* tPtr = pData;
            if (fmt == XFDataFormat.Index8)
            {
                for (int i = 0; i < count; tPtr += stride)
                    indices[i++] = *tPtr;
                pData++;
            }
            else if (fmt == XFDataFormat.Index16)
            {
                for (int i = 0; i < count; tPtr += stride)
                    indices[i++] = *(bushort*)tPtr;
                pData += 2;
            }

            return indices;
        }

        private static ushort[] Parse8(byte* pData, int elements, int stride)
        {
            ushort[] indices = new ushort[elements];

            for (int i = 0; i < elements; pData += stride)
                indices[i++] = *pData;

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
