using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BrawlLib.Wii.Compression
{
    [global::System.Serializable]
    public class InvalidCompressionException : Exception
    {
        public InvalidCompressionException() { }
        public InvalidCompressionException(string message) : base(message) { }
        public InvalidCompressionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCompressionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public static unsafe class Compressor
    {
        public static bool IsDataCompressed(VoidPtr addr, int length)
        {
            CompressionHeader* cmpr = (CompressionHeader*)addr;

            if (cmpr->ExpandedSize < length)
                return false;

            switch (cmpr->Algorithm)
            {
                case CompressionType.LZ77: return cmpr->Parameter == 0;
                default: return false;
            }
        }
        public static void Expand(CompressionHeader* header, VoidPtr dstAddr, int dstLen)
        {
            switch (header->Algorithm)
            {
                case CompressionType.LZ77: { LZ77.Expand(header, dstAddr, dstLen); break; }
                case CompressionType.Huffman:
                case CompressionType.RunLength:
                case CompressionType.Differential:
                default:
                    throw new InvalidCompressionException("Unknown compression type.");
            }
        }
        internal static unsafe void Compact(CompressionType type, VoidPtr srcAddr, int srcLen, Stream outStream)
        {
            switch (type)
            {
                case CompressionType.LZ77: { LZ77.Compact(srcAddr, srcLen, outStream, null); break; }
            }
        }
    }
}
