using System;

namespace System.IO
{
    public unsafe class XmlReader
    {
        private byte* _base, _ptr;
        private int _length, _position, _depth;
        private byte[] _buffer = new byte[512];
        private bool _inTag, _inString;

        public string Name { get { return null; } }
        public string Value { get { return null; } }

        public XmlReader(void* pSource, int length)
        {
            _position = 0;
            _length = 0;
            _base = _ptr = (byte*)pSource;

            //Find start of Xml file


        }

        //Read next non-whitespace byte. Returns 0 on EOF
        private int ReadByte()
        {
            byte b;
            if (_position < _length)
            {
                b = _base[_position++];
                if (b >= 0x20)
                    return b;
            }
            return -1;

            //byte b;
            //while (_position < _length)
            //{
            //    b = _base[_position];
            //    if ((b == 0x3C) || (b == 0x3E))
            //        return false;

            //    *p = b;
            //    _position++;
            //    return true;
            //}
            //return false;
        }

        public string ReadElement()
        {
            byte b;
            //while (ReadByte(&b)) ;
            return null;

        }

        public string ReadString()
        {
            //Find next bracket or space

            return null;
        }

        //Search the beginning of the file for the Xml tag
        internal bool Validate()
        {
            int maxLen = Math.Min(_length, 256);
            int b;
            for (int i = 0; i < maxLen; i++)
            {
                if ((b = ReadByte()) == 0)
                    break;
                if (b == 0x3C)
                {

                }
            }
            return false;
        }

        internal bool BeginElement()
        {
            throw new NotImplementedException();
        }

        internal void EndElement()
        {
            throw new NotImplementedException();
        }

        internal bool ReadAttribute()
        {
            throw new NotImplementedException();
        }
    }
}
