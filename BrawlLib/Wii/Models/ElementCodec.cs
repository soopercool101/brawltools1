using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using BrawlLib.Wii.Graphics;
using System.Runtime.InteropServices;
using BrawlLib.Modeling;

namespace BrawlLib.Wii.Models
{
    public unsafe delegate void ElementDecoder(ref byte* pIn, ref byte* pOut, float scale);
    public unsafe class ElementCodec
    {
        [Flags]
        public enum CodecType : int
        {
            S = 0,
            ST = 5,
            XY = 10,
            XYZ = 15
        }

        #region Decoders

        public static ElementDecoder[] Decoders = new ElementDecoder[] 
        {
            Element_Byte_Float2, //S
            Element_SByte_Float2,
            Element_wUShort_Float2,
            Element_wShort_Float2,
            Element_wFloat_Float2,
            Element_Byte2_Float2, //ST
            Element_SByte2_Float2,
            Element_wUShort2_Float2,
            Element_wShort2_Float2,
            Element_wFloat2_Float2,
        Element_Byte2_Float3, //XY
        Element_SByte2_Float3,
        Element_wUShort2_Float3,
        Element_wShort2_Float3,
        Element_wFloat2_Float3,
        Element_Byte3_Float3, //XYZ
        Element_SByte3_Float3,
        Element_wUShort3_Float3,
        Element_wShort3_Float3,
        Element_wFloat3_Float3
        };

        public static void Element_Byte_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_SByte_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*(sbyte*)pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_wUShort_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(ushort)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_wShort_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }
        public static void Element_wFloat_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;
            p[3] = *pIn++;
            p[2] = *pIn++;
            p[1] = *pIn++;
            p[0] = *pIn++;

            ((float*)pOut)[0] = val * scale;
            ((float*)pOut)[1] = 0.0f;
            pOut += 8;
        }

        public static void Element_Byte2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*pIn++) * scale;
            ((float*)pOut)[1] = (float)(*pIn++) * scale;
            pOut += 8;
        }
        public static void Element_SByte2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(*(sbyte*)pIn++) * scale;
            ((float*)pOut)[1] = (float)(*(sbyte*)pIn++) * scale;
            pOut += 8;
        }
        public static void Element_wUShort2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(ushort)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = (float)(ushort)((*pIn++ << 8) | *pIn++) * scale;
            pOut += 8;
        }
        public static void Element_wShort2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            ((float*)pOut)[0] = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            ((float*)pOut)[1] = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            pOut += 8;
        }
        public static void Element_wFloat2_Float2(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;

            for (int i = 0; i < 2; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                ((float*)pOut)[i] = val * scale;
            }
            pOut += 8;
        }

        public static void Element_wShort2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float* f = (float*)pOut;

            *f++ = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            *f++ = (float)(short)((*pIn++ << 8) | *pIn++) * scale;
            *f = 0.0f;

            pOut += 12;
        }

        public static void Element_wShort3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            short temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = (float)temp * scale;
                pOut += 4;
            }
        }
        public static void Element_wUShort2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            ushort temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                if (i == 2)
                    *(float*)pOut = 0.0f;
                else
                {
                    p[1] = *pIn++;
                    p[0] = *pIn++;
                    *(float*)pOut = (float)temp * scale;
                }
                pOut += 4;
            }
        }
        public static void Element_wUShort3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            ushort temp;
            byte* p = (byte*)&temp;
            for (int i = 0; i < 3; i++)
            {
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = (float)temp * scale;
                pOut += 4;
            }
        }
        public static void Element_Byte2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (i == 2) ? 0.0f : (float)(*pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_Byte3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (float)(*pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_SByte2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (i == 2) ? 0.0f : (float)(*(sbyte*)pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_SByte3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            for (int i = 0; i < 3; i++)
            {
                *(float*)pOut = (float)(*(sbyte*)pIn++) * scale;
                pOut += 4;
            }
        }
        public static void Element_wFloat2_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;
            for (int i = 0; i < 3; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = (i == 2) ? 0.0f : val;
                pOut += 4;
            }
        }
        public static void Element_wFloat3_Float3(ref byte* pIn, ref byte* pOut, float scale)
        {
            float val;
            byte* p = (byte*)&val;
            for (int i = 0; i < 3; i++)
            {
                p[3] = *pIn++;
                p[2] = *pIn++;
                p[1] = *pIn++;
                p[0] = *pIn++;
                *(float*)pOut = val;
                pOut += 4;
            }
        }

        #endregion

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ElementDescriptor
    {
        public int Stride;
        public bool Weighted;

        public fixed byte Commands[31];
        public fixed int Defs[12];

        public UnsafeBuffer RemapTable;
        public int RemapSize;

        private fixed ushort Nodes[16];

        public ElementDescriptor(MDL0Polygon* polygon)
        {
            MDL0Header* model = (MDL0Header*)((byte*)polygon + polygon->_mdl0Offset);
            byte* pData = (byte*)polygon->DefList;
            byte* pCom;
            ElementDef* pDef;
            int fmtLo, fmtHi;
            int grp0, grp1, grp2;
            int format;

            RemapTable = new UnsafeBuffer(polygon->_numVertices * 4);
            RemapSize = 0;
            Stride = 0;

            //Read element descriptor from polygon display list
            //Use direct access instead!
            //May change depending on file version

            fmtLo = *(bint*)(pData + 12);
            fmtHi = *(bint*)(pData + 18);
            grp0 = *(bint*)(pData + 34);
            grp1 = *(bint*)(pData + 40);
            grp2 = *(bint*)(pData + 46);

            //grp1 = *(buint*)(pData + 40);
            //grp1 |= (ulong)(*(buint*)(pData + 46)) << 32;


            //Build extract script
            fixed (int* pDefData = Defs)
            fixed (byte* pComData = Commands)
            {
                pCom = pComData;
                pDef = (ElementDef*)pDefData;

                //Pos/Norm weight
                if (Weighted = (fmtLo & 1) != 0)
                {
                    *pCom++ = (byte)DecodeOp.PosWeight;
                    Stride++;
                }

                //Tex matrix
                for (int i = 0; i < 8; i++)
                    if (((fmtLo >> (i + 1)) & 1) != 0)
                    {
                        *pCom++ = (byte)(DecodeOp.TexMtx0 + i);
                        Stride++;
                    }

                //Positions
                format = ((fmtLo >> 9) & 3) - 1;
                if (format >= 0)
                {
                    pDef->Input = (byte)format;
                    pDef->Type = 0;
                    if (format == 0)
                    {
                        throw new NotSupportedException("Direct mode is not suported for polygons!");
                        //pDef->Scale = (byte)((grp0 >> 4) & 0x1F);
                        //pDef->Output = (byte)(((grp0 >> 1) & 0x7) + ((grp0 & 1) == 0 ? ElementCodec.CodecType.XY : ElementCodec.CodecType.XYZ));
                        //pCom[NumCommands++] = (byte)DecodeOp.ElementDirect;
                    }
                    else
                    {
                        Stride += format;
                        pDef->Output = 12;
                        *pCom++ = (byte)DecodeOp.ElementIndexed;
                    }
                    pDef++;
                }

                //Normals
                format = ((fmtLo >> 11) & 3) - 1;
                if (format >= 0)
                {
                    pDef->Input = (byte)format;
                    pDef->Type = 1;
                    if (format == 0)
                    {
                        throw new NotSupportedException("Direct mode is not suported for polygons!");
                        //pDef->Scale = 0; //Implied?
                        //pDef->Output = (byte)(((grp0 >> 10) & 0x7) + ((grp0 & (1 << 10)) == 0 ? ElementCodec.CodecType.XYZ : ElementCodec.CodecType.XYZ));
                        //pCom[NumCommands++] = (byte)DecodeOp.ElementDirect;
                    }
                    else
                    {
                        Stride += format;
                        pDef->Output = 12;
                        *pCom++ = (byte)DecodeOp.ElementIndexed;
                    }
                    pDef++;
                }

                //Colors
                for (int i = 0; i < 2; i++)
                {
                    format = ((fmtLo >> (i * 2 + 13)) & 3) - 1;
                    if (format >= 0)
                    {
                        pDef->Input = (byte)format;
                        pDef->Type = (byte)(i + 2);
                        if (format == 0)
                        {
                            throw new NotSupportedException("Direct mode is not suported for polygons!");
                            //pDef->Output = (byte)((grp0 >> (i * 4 + 14)) & 7);
                            //pCom[NumCommands++] = (byte)DecodeOp.ElementDirect;
                        }
                        else
                        {
                            Stride += format;
                            pDef->Output = 4;
                            *pCom++ = (byte)DecodeOp.ElementIndexed;
                        }
                        pDef++;
                    }
                }

                //UVs
                for (int i = 0; i < 8; i++)
                {
                    format = ((fmtHi >> (i * 2)) & 3) - 1;
                    if (format >= 0)
                    {
                        pDef->Input = (byte)format;
                        pDef->Type = (byte)(i + 4);
                        if (format == 0)
                        {
                            throw new NotSupportedException("Direct mode is not suported for polygons!");
                            //Needs work!
                            //if (i == 0)
                            //{
                            //    pDef->Output = (byte)(((grp0 >> 22) & 7) + ((grp0 & 22) == 0 ? ElementCodec.CodecType.S : ElementCodec.CodecType.ST));
                            //    pDef->Scale = (byte)((grp0 >> 25) & 0x1F);
                            //}
                            //else
                            //{
                            //    pDef->Output = (byte)((int)((grp1 >> (i * 9 + 1)) & 7) + ((grp1 & ((ulong)1 << (i * 9 + 1))) == 0 ? ElementCodec.CodecType.S : ElementCodec.CodecType.ST));
                            //    pDef->Scale = (byte)((grp1 >> (i * 9 + 4)) & 0x1F);
                            //}
                            //pCom[NumCommands++] = (byte)DecodeOp.ElementDirect;
                        }
                        else
                        {
                            Stride += format;
                            pDef->Output = 8;
                            *pCom++ = (byte)DecodeOp.ElementIndexed;
                        }
                        pDef++;
                    }
                }

                *pCom = 0;
            }
        }

        public void SetNode(ref byte* pIn)
        {
            ushort node = *(bushort*)pIn;
            int index = (*(bushort*)(pIn + 2) & 0xFFF) / 12;
            fixed (ushort* n = Nodes)
                n[index] = node;
            pIn += 4;
        }

        public void Run(ref byte* pIn, byte** pAssets, byte** pOut, int count)
        {
            int weight = 0;
            int index, outSize;
            DecodeOp o;
            ElementDef* pDef;
            byte* p;
            //int* pTexMtx = stackalloc int[8];

            byte* tIn, tOut;

            //Iterate commands in list
            fixed(ushort* pNode = Nodes)
            fixed (int* pDefData = Defs)
            fixed (byte* pCmd = Commands)
            {
                for (int i = 0; i < count; i++)
                {

                    pDef = (ElementDef*)pDefData;
                    p = pCmd;

                Top:
                    o = (DecodeOp)(*p++);
                    switch (o)
                    {

                        case DecodeOp.PosWeight:
                            weight = pNode[*pIn++ / 3];
                            goto Top;

                        case DecodeOp.TexMtx0:
                        case DecodeOp.TexMtx1:
                        case DecodeOp.TexMtx2:
                        case DecodeOp.TexMtx3:
                        case DecodeOp.TexMtx4:
                        case DecodeOp.TexMtx5:
                        case DecodeOp.TexMtx6:
                        case DecodeOp.TexMtx7:
                            index = (int)o - (int)DecodeOp.TexMtx0;
                            pIn++;
                            //*pOut++ = (byte)(*pIn++ / 3);
                            goto Top;

                        case DecodeOp.ElementDirect:

                            ElementCodec.Decoders[pDef->Output](ref pIn, ref pOut[pDef->Type], VQuant.DeQuantTable[pDef->Scale]);

                            goto Top;

                        case DecodeOp.ElementIndexed:

                            if (pDef->Input == 2)
                            {
                                index = *(bushort*)pIn;
                                pIn += 2;
                            }
                            else
                                index = *pIn++;

                            if (pDef->Type == 0) //Vertices?
                            {
                                //Match weight and index with remap table
                                int mapEntry = (weight << 16) | index;
                                int* pTmp = (int*)RemapTable.Address;

                                //Find matching index, starting at end of list
                                index = RemapSize;
                                while ((--index >= 0) && (pTmp[index] != mapEntry)) ;

                                //No match, create new entry
                                //Will be processed into Vertices at the end!
                                if (index < 0)
                                    pTmp[index = RemapSize++] = mapEntry;

                                //Write index
                                *(ushort*)pOut[pDef->Type] = (ushort)index;
                                pOut[pDef->Type] += 2;
                            }
                            else
                            {
                                //Copy data from buffer
                                outSize = pDef->Output;

                                tIn = pAssets[pDef->Type] + (index * outSize);
                                tOut = pOut[pDef->Type];

                                while (outSize-- > 0)
                                    *tOut++ = *tIn++;

                                pOut[pDef->Type] = tOut;
                            }

                            pDef++;
                            goto Top;

                        default: break; //End
                    }
                }
            }
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct ElementDef
        {
            public byte Input; //Input format
            public byte Output; //Output size/decoder
            public byte Type;
            public byte Scale;
        }

        public enum DecodeOp : int
        {
            End = 0,
            PosWeight,
            TexMtx0,
            TexMtx1,
            TexMtx2,
            TexMtx3,
            TexMtx4,
            TexMtx5,
            TexMtx6,
            TexMtx7,
            ElementDirect,
            ElementIndexed
        }

        internal unsafe List<Vertex3> Finish(Vector3* pVert, Influence[] nodeTable)
        {
            //Create vertex list from remap table
            List<Vertex3> list = new List<Vertex3>(RemapSize);

            if (Weighted)
            {
                ushort* pMap = (ushort*)RemapTable.Address;
                for (int i = 0; i < RemapSize; i++)
                    list.Add(new Vertex3(pVert[*pMap++], nodeTable[*pMap++].Clone()));
            }
            else
            {
                int* pMap = (int*)RemapTable.Address;
                for (int i = 0; i < RemapSize; i++)
                    list.Add(new Vertex3(pVert[*pMap++]));
            }

            RemapTable.Dispose();
            RemapTable = null;

            return list;
        }
    }
}
