using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Wii.Models
{
    public unsafe class VertexCodec : IDisposable
    {
        public Vector3 _min, _max;
        public bool _hasZ;
        public WiiVertexComponentType _type;

        public int _srcElements, _srcCount;
        public int _dstElements, _dstStride;

        public int _scale;
        public int _dataLen;

        public UnsafeBuffer _newVertices;
        public UnsafeBuffer _remapTable;

        private VertexCodec() { }
        ~VertexCodec() { Dispose(); }
        public void Dispose()
        {
            if (_newVertices != null)
            {
                _newVertices.Dispose();
                _newVertices = null;
            }
            if (_remapTable != null)
            {
                _remapTable.Dispose();
                _remapTable = null;
            }
            GC.SuppressFinalize(this);
        }

        private const float _maxError = 0.0005f;

        public static VertexCodec Evaluate(Vector3[] vertices, bool removeZ)
        {
            VertexCodec enc = new VertexCodec();
            fixed (Vector3* p = vertices)
                enc.Evaluate((float*)p, 3, vertices.Length, removeZ);
            return enc;
        }
        public static VertexCodec Evaluate(Vector2[] points)
        {
            VertexCodec enc = new VertexCodec();
            fixed (Vector2* p = points)
                enc.Evaluate((float*)p, 2, points.Length, false);
            return enc;
        }

        private void Evaluate(float* sPtr, int elements, int count, bool removeZ)
        {
            float* fPtr, fCeil = sPtr + (elements * count);
            int bestScale = 0;
            bool sign;

            Vector3 min = new Vector3(float.MaxValue), max = new Vector3(float.MinValue);
            float* pMin = (float*)&min, pMax = (float*)&max;
            float vMin = float.MaxValue, vMax = float.MinValue, vDist;
            float val;

            _min = new Vector3(float.MaxValue);
            _max = new Vector3(float.MinValue);
            _srcElements = elements;
            _srcCount = count;
            _type = 0;

            //Get extents
            fPtr = sPtr;
            for (int i = 0; i < count; i++)
                for (int x = 0; x < elements; x++)
                {
                    val = *fPtr++;
                    if (val < pMin[x]) pMin[x] = val;
                    if (val > pMax[x]) pMax[x] = val;
                    if (val < vMin) vMin = val;
                    if (val > vMax) vMax = val;
                }

            _min = min;
            _max = max;
            if (removeZ && (elements == 3) && (min._z == 0) && (max._z == 0))
                _dstElements = 2;
            else
                _dstElements = elements;

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
                    for (fPtr = sPtr; fPtr < fCeil; )
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
                            break;
                    }

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
            _dataLen = count * _dstStride;
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

        public void Write(Vector2[] vertices, byte* pOut)
        {
            fixed (Vector2* p = vertices)
                Write((float*)p, pOut);
        }
        public void Write(Vector3[] vertices, byte* pOut)
        {
            fixed (Vector3* p = vertices)
                Write((float*)p, pOut);
        }
        public void Write(float* pIn, byte* pOut)
        {
            byte* pCeil = pOut + _dataLen.Align(0x20);
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

            for (int i = 0; i < _srcCount; i++)
                for (int x = 0; x < _srcElements; x++, pIn++)
                    if (x < _dstElements)
                        enc(*pIn * scale, ref pOut);

            //Zero remaining
            while (pOut < pCeil)
                *pOut++ = 0;
        }

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

        //private delegate float VertexDecoder(ref byte* sPtr);
        //private static VertexDecoder _u8Decode = (ref byte* sPtr) => { return *sPtr++; };
        //private static VertexDecoder _s8Decode = (ref byte* sPtr) => { return *(sbyte*)(sPtr++); };
        //private static VertexDecoder _u16Decode = (ref byte* sPtr) => { bushort* p = (bushort*)sPtr; sPtr += 2; return *p; };
        //private static VertexDecoder _s16Decode = (ref byte* sPtr) => { bshort* p = (bshort*)sPtr; sPtr += 2; return *p; };
        //private static VertexDecoder _fDecode = (ref byte* sPtr) => { bfloat* p = (bfloat*)sPtr; sPtr += 4; return *p; };

        //private static void Extract(byte* sPtr, float* dPtr, int count, int sElem, int dElem, WiiVertexComponentType type, float scale)
        //{
        //    VertexDecoder decoder;
        //    switch (type)
        //    {
        //        case WiiVertexComponentType.UInt8: decoder = _u8Decode; break;
        //        case WiiVertexComponentType.Int8: decoder = _s8Decode; break;
        //        case WiiVertexComponentType.UInt16: decoder = _u16Decode; break;
        //        case WiiVertexComponentType.Int16: decoder = _s16Decode; break;
        //        default: decoder = _fDecode; break;
        //    }

        //    for (int i = 0; i < count; i++)
        //        for (int x = 0; x < dElem; x++)
        //            *dPtr++ = (x < sElem) ? decoder(ref sPtr) * scale : 0.0f;
        //}

        #endregion
    }
}
