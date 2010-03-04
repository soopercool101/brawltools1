using System;
using BrawlLib.Modeling;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Graphics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BrawlLib.OpenGL;

namespace BrawlLib.Wii.Models
{
    public class VertexGroup
    {
        public Influence _nodeRef;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ElementFlags
    {
        public bool PosNormMatrixIndex;
        public fixed bool TexMatrixIndex[8];
        public XFDataFormat PositionFormat;
        public XFDataFormat NormalFormat;
        public fixed byte ColorFormat[2];
        public fixed byte UVFormat[8];
        public int Stride;

        public ElementFlags(int elemFlags, int texFlags)
        {
            if (PosNormMatrixIndex = (elemFlags & 1) != 0)
                Stride = 1;
            else
                Stride = 0;

            fixed (bool* p = TexMatrixIndex)
                for (int i = 0; i < 8; )
                    if (p[i] = (elemFlags & (1 << ++i)) != 0)
                        Stride += 1;

            switch (PositionFormat = (XFDataFormat)((elemFlags >> 9) & 3))
            {
                case XFDataFormat.Index8: Stride += 1; break;
                case XFDataFormat.Index16: Stride += 2; break;
                case XFDataFormat.Direct: throw new NotSupportedException("Polygon format cannot use direct mode!"); break;
            }

            switch (NormalFormat = (XFDataFormat)((elemFlags >> 11) & 3))
            {
                case XFDataFormat.Index8: Stride += 1; break;
                case XFDataFormat.Index16: Stride += 2; break;
                case XFDataFormat.Direct: throw new NotSupportedException("Polygon format cannot use direct mode!"); break;
            }

            fixed (byte* p = ColorFormat)
                for (int i = 0; i < 2; i++)
                    switch (p[i] = (byte)((elemFlags >> (i * 2 + 13)) & 3))
                    {
                        case 1: throw new NotSupportedException("Polygon format cannot use direct mode!"); break;
                        case 2: Stride += 1; break;
                        case 3: Stride += 2; break;
                    }

            fixed (byte* p = UVFormat)
                for (int i = 0; i < 8; i++)
                    switch (p[i] = (byte)((texFlags >> (i * 2)) & 3))
                    {
                        case 1: throw new NotSupportedException("Polygon format cannot use direct mode!"); break;
                        case 2: Stride += 1; break;
                        case 3: Stride += 2; break;
                    }
        }
    }

    public enum PrimScriptCommand : byte
    {
        ReadNull8,
        ReadNull16,
        Read8,
        Read16,
        Read8To16,
        ReadWeight
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PrimitiveScript
    {
        public int _stride;
        private byte _numCommands;
        private fixed byte _commands[31];
        private fixed ushort _nodes[16];

        public PrimitiveScript(MDL0Polygon* polygon)
        {
            ElementFlags elemFlags = new ElementFlags(polygon->_elemFlags, polygon->_texFlags);
            _stride = elemFlags.Stride;
            _numCommands = 0;
            fixed (byte* p = _commands)
            {
                if (elemFlags.PosNormMatrixIndex) p[_numCommands++] = (byte)PrimScriptCommand.ReadWeight;
                else p[_numCommands++] = (byte)PrimScriptCommand.ReadNull16;

                for (int i = 0; i < 8; i++)
                    if (elemFlags.TexMatrixIndex[i]) p[_numCommands++] = (byte)PrimScriptCommand.Read8;
                    else p[_numCommands++] = (byte)PrimScriptCommand.ReadNull8;

                XFDataFormat* fmt = &elemFlags.PositionFormat;
                for (int i = 0; i < 12; i++)
                    switch (*fmt++)
                    {
                        case XFDataFormat.Index16: p[_numCommands++] = (byte)PrimScriptCommand.Read16; break;
                        case XFDataFormat.Index8: p[_numCommands++] = (byte)PrimScriptCommand.Read8To16; break;
                        default: p[_numCommands++] = (byte)PrimScriptCommand.ReadNull16; break;
                    }
            }
        }

        public void AddCommand(PrimScriptCommand cmd)
        {
            fixed (byte* p = _commands)
                p[_numCommands++] = (byte)cmd;
        }
        public void Clear() { _numCommands = 0; }
        public void Run(ref byte* pIn, byte* pOut)
        {
            fixed (byte* c = _commands)
            {
                ushort* n = (ushort*)(c + 31);
                for (int i = 0; i < _numCommands; i++)
                {
                    switch ((PrimScriptCommand)c[i])
                    {
                        case PrimScriptCommand.ReadNull8: *pOut++ = 0xFF; break;
                        case PrimScriptCommand.ReadNull16: *(ushort*)pOut = 0xFFFF; pOut += 2; break;
                        case PrimScriptCommand.Read8: *pOut++ = *pIn++; break;
                        case PrimScriptCommand.Read16: *(ushort*)pOut = *(bushort*)pIn; pOut += 2; pIn += 2; break;
                        case PrimScriptCommand.Read8To16: *(ushort*)pOut = *pIn++; pOut += 2; break;
                        case PrimScriptCommand.ReadWeight: *(ushort*)pOut = n[*pIn++ / 3]; pOut += 2; break;
                    }
                }
            }
        }
        public void SetNode(ref byte* pIn)
        {
            ushort node = *(bushort*)pIn;
            int index = (*(bushort*)(pIn + 2) & 0xFFF) / 12;
            fixed (ushort* n = _nodes)
                n[index] = node;
            pIn += 4;
        }
    }

    public unsafe class PrimitiveCodec
    {
        //public static Primitive[] Decode(MDL0Polygon* header)
        //{
        //    UnsafeBuffer d3, d2, d1;
        //    //DecodeFaces(header, out d3, out d2, out d1);

        //    //Is weighted?
        //    if ((header->_elemFlags & 1) != 0)
        //    {
        //        //If so, pull out vertex/weight pairs
        //    }
        //    else
        //    {
        //        //Otherwise, copy vertices directly

        //    }

        //    return new Primitive[]{};
        //    //Create vertices using face references
        //    //If not weighted, this is very easy
        //}

        //public static void DecodeFaces(MDL0Polygon* header, out List<Vertex> triangles, out List<Vertex> lines, out List<Vertex> points)
        //{
        //    Vertex v1 = new Vertex(), v2;
        //    Vertex* pd3, pd2, pd1;
        //    PrimitiveScript script = new PrimitiveScript(header);
        //    int count;
        //    byte* pData = (byte*)header->PrimitiveData;

        //    //Get element count for each primitive type
        //    int d3 = 0, d2 = 0, d1 = 0;

        //CountTop:
        //    switch ((GXListCommand)(*pData++))
        //    {
        //        case GXListCommand.LoadIndexA:
        //        case GXListCommand.LoadIndexB:
        //        case GXListCommand.LoadIndexC:
        //        case GXListCommand.LoadIndexD:
        //            pData += 4;
        //            goto CountTop;

        //        case GXListCommand.DrawQuads:
        //            count = *(bushort*)pData;
        //            d3 += count / 2 * 3;
        //            break;

        //        case GXListCommand.DrawTriangles:
        //            count = *(bushort*)pData;
        //            d3 += count;
        //            break;

        //        case GXListCommand.DrawTriangleFan:
        //        case GXListCommand.DrawTriangleStrip:
        //            count = *(bushort*)pData;
        //            d3 += (count - 2) * 3;
        //            break;

        //        case GXListCommand.DrawLines:
        //            count = *(bushort*)pData;
        //            d2 += count;
        //            break;

        //        case GXListCommand.DrawLineStrip:
        //            count = *(bushort*)pData;
        //            d2 += (count - 1) * 2;
        //            break;

        //        case GXListCommand.DrawPoints:
        //            count = *(bushort*)pData;
        //            d1 += count;
        //            break;

        //        default: goto Next;
        //    }
        //    pData += 2 + count * script._stride;
        //    goto CountTop;

        //    //Create buffers
        //Next:
        //    pData = (byte*)header->PrimitiveData;

        //    triangles = new List<Vertex>();
        //    if (d3 > 0)
        //    {

        //        //triangles = new List<Vertex>(d3);// new UnsafeBuffer(d3 * sizeof(Vertex));
        //        //d3 = 0;
        //        //pd3 = (Vertex*)triangles.Address;
        //    }
        //    else
        //    {
        //        triangles = null;
        //        //pd3 = null;
        //    }

        //    lines = new List<Vertex>(d2);
        //    //if (d2 > 0)
        //    //{
        //        //lines = new List<Vertex>(d2);
        //        //d2 = 0;
        //        //lines = new UnsafeBuffer(d2 * sizeof(Vertex));
        //        //pd2 = (Vertex*)lines.Address;
        //    //}
        //    //else
        //    //{
        //        //lines = null;
        //        //pd2 = null;
        //    //}

        //    points = new List<Vertex>(d1);
        //    //if (d1 > 0)
        //    //{
        //        //points = new List<Vertex>(d1);
        //        //points = new UnsafeBuffer(d1 * sizeof(Vertex));
        //        //pd1 = (Vertex*)points.Address;
        //    //}
        //    //else
        //    //{
        //        //points = null;
        //        //pd1 = null;
        //    //}

        //    //Extract Vertices
        //Top:
        //    switch ((GXListCommand)(*pData++))
        //    {
        //        case GXListCommand.LoadIndexA:
        //            script.SetNode(ref pData);
        //            goto Top;

        //        case GXListCommand.LoadIndexB:
        //        case GXListCommand.LoadIndexC:
        //        case GXListCommand.LoadIndexD:
        //            pData += 4;
        //            goto Top;

        //        case GXListCommand.DrawQuads:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            for (int i = 0; i < count; i++)
        //            {
        //                if ((i & 3) == 2)
        //                {
        //                    script.Run(ref pData, (byte*)&v2);
        //                    *pd3++ = v2;
        //                    *pd3++ = v2;
        //                }
        //                else
        //                    script.Run(ref pData, (byte*)&v1);

        //                *pd3++ = v1;
        //            }

        //            goto Top;

        //        case GXListCommand.DrawTriangles:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            for (int i = 0; i < count; i++)
        //                script.Run(ref pData, (byte*)(pd3++));

        //            goto Top;

        //        case GXListCommand.DrawTriangleStrip:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            script.Run(ref pData, (byte*)&v1);
        //            script.Run(ref pData, (byte*)&v2);
        //            for (int i = 2; i < count; i++)
        //            {
        //                if ((i & 1) == 0)
        //                {
        //                    *pd3++ = v1;
        //                    *pd3++ = v2;
        //                    script.Run(ref pData, (byte*)&v1);
        //                    *pd3++ = v1;
        //                }
        //                else
        //                {
        //                    *pd3++ = v1;
        //                    *pd3++ = v2;
        //                    script.Run(ref pData, (byte*)&v2);
        //                    *pd3++ = v2;
        //                }
        //            }

        //            goto Top;

        //        case GXListCommand.DrawTriangleFan:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            script.Run(ref pData, (byte*)&v1);
        //            script.Run(ref pData, (byte*)&v2);
        //            for (int i = 0; i < count; i++)
        //            {
        //                *pd3++ = v1;
        //                *pd3++ = v2;
        //                script.Run(ref pData, (byte*)&v2);
        //                *pd3++ = v2;
        //            }

        //            goto Top;

        //        case GXListCommand.DrawLines:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            for (int i = 0; i < count; i++)
        //                script.Run(ref pData, (byte*)(pd2++));

        //            goto Top;

        //        case GXListCommand.DrawLineStrip:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            script.Run(ref pData, (byte*)&v1);
        //            for (int i = 0; i < count; i++)
        //            {
        //                *pd2++ = v1;
        //                script.Run(ref pData, (byte*)&v1);
        //                *pd2++ = v1;
        //            }

        //            goto Top;

        //        case GXListCommand.DrawPoints:
        //            count = *(bushort*)pData;
        //            pData += 2;

        //            for (int i = 0; i < count; i++)
        //                script.Run(ref pData, (byte*)(pd1++));

        //            goto Top;

        //        default: break;
        //    }
        //}

    }
}
