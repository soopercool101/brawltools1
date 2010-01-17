﻿using System;
using System.Collections.Generic;
using BrawlLib.SSBBTypes;

namespace BrawlLib
{
    public unsafe class StringTable
    {
        SortedList<string, VoidPtr> _table = new SortedList<string, VoidPtr>(StringComparer.Ordinal);

        public void Add(string s)
        {
            if ((!String.IsNullOrEmpty(s)) && (!_table.ContainsKey(s)))
                _table.Add(s, 0);
        }

        public int GetTotalSize()
        {
            int len = 0;
            foreach (string s in _table.Keys)
                len += (s.Length + 5).Align(4);
            return len;
        }

        public void Clear() { _table.Clear(); }

        public VoidPtr this[string s] { get { return _table[s]; } }

        public void WriteTable(VoidPtr address)
        {
            BRESString* entry = (BRESString*)address;
            for (int i = 0; i < _table.Count; i++)
            {
                string s = _table.Keys[i];
                _table[s] = entry;
                entry->Value = s;
                entry = entry->Next;
            }
        }
    }
}
