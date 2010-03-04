using System;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct PString
    {
        private byte* _address;

        public PString(byte* address) { _address = address; }

        public static implicit operator int(PString p) { return (int)p._address; }
        //public static implicit operator PString(int p) { return new PString((sbyte*)p); }
        public static implicit operator uint(PString p) { return (uint)p._address; }
        //public static implicit operator PString(uint p) { return new PString((sbyte*)p); }
        public static implicit operator sbyte*(PString p) { return (sbyte*)p._address; }
        public static implicit operator PString(sbyte* p) { return *(PString*)&p; }
        public static implicit operator byte*(PString p) { return p._address; }
        public static implicit operator PString(byte* p) { return *(PString*)&p; }
        public static implicit operator VoidPtr(PString p) { return *(VoidPtr*)&p; }
        public static implicit operator PString(VoidPtr p) { return *(PString*)&p; }

        public static explicit operator string(PString p) { return new String(p); }

        public static PString operator +(PString p, int amount)
        {
            p._address += amount;
            return p;
        }

        public static bool operator ==(PString p, string s)
        {
            byte* pStr1 = p._address;
            fixed (char* pChar = s)
            {
                char* pStr2 = pChar;
                do { if (*pStr1 != *pStr2++) return false; }
                while (*pStr1++ != 0);
            }
            return true;
        }
        public static bool operator !=(PString p, string s) { return !(p == s); }

        public override string ToString() { return new String(this); }

        public byte this[int index]
        {
            get { return _address[index]; }
            set { _address[index] = value; }
        }

        public int Length
        {
            get
            {
                int len = 0;
                byte* p = _address;
                while (*p++ != 0) len++;
                return len;
            }
        }

        public void Write(string s) { Write(s, 0); }
        public void Write(string s, int offset) { Write(s, offset, s.Length); }
        public void Write(string s, int offset, int len) { fixed (char* p = s) Write(p, offset, len); }

        public void Write(char* p, int offset, int len)
        {
            byte* s = _address;
            p += offset;
            for (int i = 0; i < len; i++)
                *s++ = (byte)*p++;
        }
        public void Write(byte* p, int offset, int len)
        {
            byte* s = _address;
            p += offset;
            for (int i = 0; i < len; i++)
                *s++ = *p++;
        }

        //internal static unsafe bool Equals(sbyte* pStr, string str)
        //{
        //    int c1, c2;
        //    fixed (char* p = str)
        //    {
        //        char* pStr2 = p;
        //        do
        //        {
        //            c1 = *pStr++;
        //            c2 = *pStr2++;
        //            if (c1 != c2)
        //                return false;
        //        } while ((c1 != 0) && (c2 != 0));
        //        return true;
        //    }
        //}
    }
}
