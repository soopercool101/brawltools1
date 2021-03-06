﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Models
{
    public unsafe class VertexCodec : IDisposable
    {
        private const float _maxError = 0.0005f;

        public Vector3 _min, _max;
        public bool _hasZ;
        public WiiVertexComponentType _type;

        public int _srcElements, _srcCount;
        public int _dstElements, _dstCount, _dstStride;

        public int _scale;
        public int _dataLen;

        private Remapper _remap;

        private GCHandle _handle;
        private float* _pData;

        private VertexCodec() { }
        ~VertexCodec() { Dispose(); }
        public void Dispose()
        {
            if (_handle.IsAllocated)
                _handle.Free();
            _pData = null;
        }


        public VertexCodec(Vector3[] vertices, bool removeZ)
        {
            _srcCount = vertices.Length;
            _srcElements = 3;
            _handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            _pData = (float*)_handle.AddrOfPinnedObject();
            Evaluate(removeZ);
        }
        public VertexCodec(Vector2[] vertices)
        {
            _srcCount = vertices.Length;
            _srcElements = 2;
            _handle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            _pData = (float*)_handle.AddrOfPinnedObject();
            Evaluate(false);
        }
        public VertexCodec(Vector3* sPtr, int count, bool removeZ)
        {
            _srcCount = count;
            _srcElements = 3;
            _pData = (float*)sPtr;
            Evaluate(removeZ);
        }
        public VertexCodec(Vector2* sPtr, int count)
        {
            _srcCount = count;
            _srcElements = 2;
            _pData = (float*)sPtr;
            Evaluate(false);
        }

        private void Evaluate(bool removeZ)
        {
            float* fPtr;
            int bestScale = 0;
            bool sign;

            Vector3 min = new Vector3(float.MaxValue), max = new Vector3(float.MinValue);
            float* pMin = (float*)&min, pMax = (float*)&max;
            float vMin = float.MaxValue, vMax = float.MinValue, vDist;
            float val;

            //Currently, remapping provides to benefit for vertices/normals
            //Maybe when merging is supported this will change

            _remap = new Remapper();
            //if (elements == 3)
            //    _remap.Remap<Vector3>(new MemoryList<Vector3>(sPtr, count), null);
            if (_srcElements == 2)
                _remap.Remap<Vector2>(new MemoryList<Vector2>(_pData, _srcCount), null);

            int[] imp = null;
            int impLen = _srcCount;

            //Remapping is useless if there is no savings
            if (_remap._impTable != null && _remap._impTable.Length < _srcCount)
            {
                imp = _remap._impTable;
                impLen = imp.Length;
            }

            _min = new Vector3(float.MaxValue);
            _max = new Vector3(float.MinValue);
            _dstCount = impLen;
            _type = 0;

            //Get extents
            fPtr = _pData;
            for (int i = 0; i < impLen; i++)
            {
                if (imp != null)
                    fPtr = &_pData[imp[i] * _srcElements];
                for (int x = 0; x < _srcElements; x++)
                {
                    val = *fPtr++;
                    if (val < pMin[x]) pMin[x] = val;
                    if (val > pMax[x]) pMax[x] = val;
                    if (val < vMin) vMin = val;
                    if (val > vMax) vMax = val;
                }
            }

            _min = min;
            _max = max;
            if (removeZ && (_srcElements == 3) && (min._z == 0) && (max._z == 0))
                _dstElements = 2;
            else
                _dstElements = _srcElements;

            _hasZ = _dstElements > 2;

            vDist = Math.Max(Math.Abs(vMin), Math.Abs(vMax));

            //Is signed? If so, increase type
            if (sign = (vMin < 0))
                _type++;

            int divisor = 0;
            float rMin = 0.0f, rMax;
            for (int i = 0; i < 2; i++)
            {
                float bestError = _maxError;
                float scale, maxVal;

                if (i == 0)
                    if (sign)
                    { rMax = 127.0f; rMin = -128.0f; }
                    else
                        rMax = 255.0f;
                else
                    if (sign)
                    { rMax = 32767.0f; rMin = -32768.0f; }
                    else
                        rMax = 65535.0f;

                maxVal = rMax / vDist;
                while ((divisor < 32) && ((scale = VQuant.QuantTable[divisor]) <= maxVal))
                {
                    float worstError = float.MinValue;

                    fPtr = _pData;
                    for (int y = 0; y < impLen; y++)
                    {
                        if (imp != null)
                            fPtr = &_pData[imp[i] * _srcElements];
                        for (int z = 0; z < _srcElements; z++)
                        {
                            if ((val = *fPtr++) == 0)
                                continue;

                            val *= scale;
                            if (val > rMax) val = rMax;
                            else if (val < rMin) val = rMin;

                            int step = (int)((val * scale) + (val > 0 ? 0.5f : -0.5f));
                            float error = Math.Abs((step / scale) - val);

                            if (error > worstError)
                                worstError = error;

                            if (error > bestError)
                                goto Check;
                        }
                    }

                    //for (fPtr = sPtr; fPtr < fCeil; )
                //{
                //    if ((val = *fPtr++) == 0)
                //        continue;

                    //    val *= scale;
                //    if (val > rMax) val = rMax;
                //    else if (val < rMin) val = rMin;

                    //    int step = (int)((val * scale) + (val > 0 ? 0.5f : -0.5f));
                //    float error = Math.Abs((step / scale) - val);

                    //    if (error > worstError)
                //        worstError = error;

                    //    if (error > bestError)
                //        break;
                //}

                Check:

                    if (worstError < bestError)
                    {
                        bestScale = divisor;
                        bestError = worstError;
                        if (bestError == 0)
                            goto Next;
                    }

                    divisor++;
                }

                if (bestError < _maxError)
                    goto Next;

                _type += 2;
            }

            _type = WiiVertexComponentType.Float;
            _scale = 0;

        Next:

            _scale = bestScale;
            _dstStride = _dstElements << ((int)_type >> 1);
            _dataLen = _dstCount * _dstStride;
        }

        private delegate void VertEncoder(float value, ref byte* pOut);
        private static readonly VertEncoder _byteEncoder = (float value, ref byte* pOut) =>
        {
            int val = (int)(value + (value > 0 ? 0.5f : -0.5f));
            *(byte*)pOut = (byte)val;
            pOut++;
        };
        private static readonly VertEncoder _shortEncoder = (float value, ref byte* pOut) =>
        {
            int val = (int)(value + (value > 0 ? 0.5f : -0.5f));
            *(bushort*)pOut = (ushort)val;
            pOut += 2;
        };
        private static readonly VertEncoder _floatEncoder = (float value, ref byte* pOut) =>
        {
            *(bfloat*)pOut = value;
            pOut += 4;
        };

        public void Write(byte* pOut)
        {
            try
            {
                int[] imp = _remap._impTable;
                float scale = VQuant.QuantTable[_scale];
                VertEncoder enc;
                switch (_type)
                {
                    case WiiVertexComponentType.Int8:
                    case WiiVertexComponentType.UInt8:
                        enc = _byteEncoder;
                        break;
                    case WiiVertexComponentType.Int16:
                    case WiiVertexComponentType.UInt16:
                        enc = _shortEncoder;
                        break;
                    default:
                        enc = _floatEncoder;
                        break;
                }

                //Copy elements using encoder
                float* pTemp = _pData;
                for (int i = 0; i < _dstCount; i++)
                {
                    if (imp != null)
                        pTemp = &_pData[imp[i] * _srcElements];
                    for (int x = 0; x < _srcElements; x++, pTemp++)
                        if (x < _dstElements)
                            enc(*pTemp * scale, ref pOut);
                }

                //Zero remaining
                for (int i = _dataLen; (i & 0x1F) != 0; i++)
                    *pOut++ = 0;
            }
            finally
            {
                Dispose();
            }
        }
        //public void Write(Vector2[] vertices, byte* pOut)
        //{
        //    fixed (Vector2* p = vertices)
        //        Write((float*)p, pOut);
        //}
        //public void Write(Vector3[] vertices, byte* pOut)
        //{
        //    fixed (Vector3* p = vertices)
        //        Write((float*)p, pOut);
        //}
        //public void Write(float* pIn, byte* pOut)
        //{
        //    int[] imp = _remap._impTable;
        //    byte* pCeil = pOut + _dataLen.Align(0x20);
        //    float scale = VQuant.QuantTable[_scale];
        //    VertEncoder enc;
        //    switch (_type)
        //    {
        //        case WiiVertexComponentType.Int8:
        //        case WiiVertexComponentType.UInt8:
        //            enc = _byteEncoder;
        //            break;
        //        case WiiVertexComponentType.Int16:
        //        case WiiVertexComponentType.UInt16:
        //            enc = _shortEncoder;
        //            break;
        //        default:
        //            enc = _floatEncoder;
        //            break;
        //    }

        //    //Copy elements using encoder
        //    float* pTemp = pIn;
        //    for (int i = 0; i < _dstCount; i++)
        //    {
        //        if (imp != null)
        //            pTemp = &pIn[imp[i] * _srcElements];
        //        for (int x = 0; x < _srcElements; x++, pTemp++)
        //            if (x < _dstElements)
        //                enc(*pTemp * scale, ref pOut);
        //    }

        //    //Zero remaining
        //    while (pOut < pCeil)
        //        *pOut++ = 0;
        //}

        #region Decoding

        public static UnsafeBuffer Decode(MDL0UVData* header)
        {
            int count = header->_numEntries;
            float scale = VQuant.DeQuantTable[header->_divisor];
            int type = ((header->_isST == 0) ? (int)ElementCodec.CodecType.S : (int)ElementCodec.CodecType.ST) + (int)header->_format;
            ElementDecoder decoder = ElementCodec.Decoders[type];
            UnsafeBuffer buffer = new UnsafeBuffer(count * 8);

            byte* pIn = (byte*)header->Entries, pOut = (byte*)buffer.Address;
            for (int i = 0; i < count; i++)
                decoder(ref pIn, ref pOut, scale);

            return buffer;
        }
        public static UnsafeBuffer Decode(MDL0VertexData* header)
        {
            int count = header->_numVertices;
            float scale = VQuant.DeQuantTable[header->_divisor];
            int type = ((header->_isXYZ == 0) ? (int)ElementCodec.CodecType.XY : (int)ElementCodec.CodecType.XYZ) + (int)header->_type;
            ElementDecoder decoder = ElementCodec.Decoders[type];
            UnsafeBuffer buffer = new UnsafeBuffer(count * 12);

            byte* pIn = (byte*)header->Data, pOut = (byte*)buffer.Address;
            for (int i = 0; i < count; i++)
                decoder(ref pIn, ref pOut, scale);

            return buffer;
        }
        public static UnsafeBuffer Decode(MDL0NormalData* header)
        {
            int count = header->_numVertices;
            float scale = VQuant.DeQuantTable[header->_divisor]; //Should always be zero?
            int type = (int)ElementCodec.CodecType.XYZ + (int)header->_type;
            ElementDecoder decoder = ElementCodec.Decoders[type];
            UnsafeBuffer buffer;

            if (header->_isNBT != 0)
                count *= 3; //Format is the same, just with three Vectors each

            buffer = new UnsafeBuffer(count * 12);

            byte* pIn = (byte*)header->Data, pOut = (byte*)buffer.Address;
            for (int i = 0; i < count; i++)
                decoder(ref pIn, ref pOut, scale);

            return buffer;
        }

        #endregion
    }
}
