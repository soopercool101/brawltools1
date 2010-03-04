using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib
{
    public unsafe class StringTable
    {
        SortedList<string, IntPtr> _table = new SortedList<string, IntPtr>(StringComparer.Ordinal);

        public void Add(string s)
        {
            if ((!String.IsNullOrEmpty(s)) && (!_table.ContainsKey(s)))
                _table.Add(s, IntPtr.Zero);
        }

        public int GetTotalSize()
        {
            int len = 0;
            foreach (string s in _table.Keys)
                len += (s.Length + 5).Align(4);
            return len;
        }

        public void Clear() { _table.Clear(); }

        public BRESString* this[string s] { get { return (BRESString*)_table[s]; } }

        public void WriteTable(VoidPtr address)
        {
            BRESString* entry = (BRESString*)address;
            for (int i = 0; i < _table.Count; i++)
            {
                string s = _table.Keys[i];
                _table[s] = (IntPtr)entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}
