using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace BrawlLib.SSBBTypes
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct ROOTHeader
    {
        public const int Tag = 0x746F6F72;

        public uint _tag;
        public bint _size;
        public ResourceGroup _master;

        //public BRESHeader* BRESHeader
        //{
        //    get
        //    {
        //        uint* ptr;
        //        fixed (ROOTHeader* p = &this)
        //            for (ptr = (uint*)p; *ptr != SmashBox.BRESHeader.Tag; ptr--) ;
        //        return (BRESHeader*)ptr;
        //    }
        //}

        public ROOTHeader(int size, int numEntries)
        {
            _tag = Tag;
            _size = size;
            _master = new ResourceGroup(numEntries);
        }
    }

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public unsafe struct ROOTGroup// : IEnumerable<VoidPtr>
    //{
    //    public buint _totalSize;
    //    public buint _numEntries;
    //    public ROOTGroupEntry _first;

    //    public ROOTGroup(uint numEntries)
    //    {
    //        _totalSize = (numEntries * 0x10) + 0x18;
    //        _numEntries = numEntries;
    //        _first = new ROOTGroupEntry() { _id = 0xFFFF, _prev = 1 };
    //    }

    //    private ROOTGroup* Address { get { fixed (ROOTGroup* ptr = &this)return ptr; } }
    //    public ROOTGroupEntry* First { get { return &this.Address->_first + 1; } }

        //public ROOTHeader* ROOTHeader
        //{
        //    get
        //    {
        //        foreach (uint* ptr in new PointerEnumerator(this.Address, 0x1000, -4))
        //            if (*ptr == SmashBox.ROOTHeader.Tag)
        //                return (ROOTHeader*)ptr;
        //        return null;
        //    }
        //}
        //public ROOTGroup* Master { get { return &ROOTHeader->_master; } }
        //public bool IsMaster { get { return this.Address == Master; } }

        //public ROOTGroupEntry*[] GetResourceGroup(ushort id)
        //{
        //    List<VoidPtr> list = new List<VoidPtr>();

        //    foreach (ROOTGroupEntry* mEntry in *this.Master)
        //        foreach (ROOTGroupEntry* gEntry in *((ROOTGroup*)mEntry->ResourceData))
        //            if (gEntry->_id == id) list.Add(gEntry);

        //    ROOTGroupEntry*[] arr = new ROOTGroupEntry*[list.Count];
        //    for (int i = 0; i < list.Count; i++)
        //        arr[i] = list[i];
        //    return arr;
        //}
        //public ROOTGroupEntry*[] GetStringGroup(VoidPtr stringAddr)
        //{
        //    List<VoidPtr> list = new List<VoidPtr>();

        //    foreach (ROOTGroupEntry* mEntry in *this.Master)
        //    {
        //        if (mEntry->ResourceStringAddress == stringAddr) list.Add(mEntry);
        //        foreach (ROOTGroupEntry* gEntry in *((ROOTGroup*)mEntry->ResourceData))
        //            if (gEntry->ResourceStringAddress == stringAddr) list.Add(gEntry);
        //    }

        //    ROOTGroupEntry*[] arr = new ROOTGroupEntry*[list.Count];
        //    for (int i = 0; i < list.Count; i++)
        //        arr[i] = list[i];
        //    return arr;
        //}
        //public void RemoveEntry(int index)
        //{
        //    ROOTGroupEntry* start = &First[index];
        //    ROOTGroupEntry* end = start + 1;
        //    ROOTGroupEntry* last = &First[_numEntries];

        //    Util.MoveMemory(start, end, (uint)last - (uint)end);
        //    _numEntries--;
        //    //_totalSize -= 0x10;
        //}

        //IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
        //public IEnumerator<VoidPtr> GetEnumerator() { return new PointerEnumerator(this.First, (int)this._numEntries, 0x10); }
    //}

    //[StructLayout(LayoutKind.Sequential, Pack = 1)]
    //public unsafe struct ROOTGroupEntry
    //{
    //    public const uint Size = 0x10;

    //    public bushort _id;
    //    public bushort _unk1;
    //    public bushort _prev;
    //    public bushort _next;
    //    public buint _stringOffset;
    //    public buint _dataOffset;

    //    private ROOTGroupEntry* Address { get { fixed (ROOTGroupEntry* ptr = &this)return ptr; } }

    //    public ROOTGroupEntry(ushort id, ushort prev, ushort next, uint dataOffset)
    //    {
    //        _id = id;
    //        _unk1 = 0;
    //        _prev = prev;
    //        _next = next;
    //        _stringOffset = 0;
    //        _dataOffset = dataOffset;
    //    }

        //public ROOTGroupEntry*[] GetGroup()
        //{
        //    return Parent->GetResourceGroup(_id);
        //}
        //public ROOTGroupEntry*[] GetStringGroup()
        //{
        //    return Parent->GetStringGroup(this.ResourceStringAddress);
        //}

    //    public string ResourceString { get { return new String((sbyte*)ResourceStringAddress); } }
    //    public VoidPtr ResourceStringAddress
    //    {
    //        get { return (VoidPtr)Parent + _stringOffset; }
    //        set { _stringOffset = (uint)value - (uint)Parent; }
    //    }
    //    public VoidPtr ResourceData
    //    {
    //        get { return (VoidPtr)Parent + _dataOffset; }
    //        set { _dataOffset = (uint)value - (uint)Parent; }
    //    }
    //    public MemoryBlock BRESEntryBlock { get { return new MemoryBlock(ResourceData, ((BRESCommonHeader*)ResourceData)->_size); } }
    //    public MemoryBlock ROOTGroupBlock { get { return new MemoryBlock(ResourceData, ((ROOTGroup*)ResourceData)->_totalSize); } }

    //    public ROOTGroup* Parent
    //    {
    //        get
    //        {
    //            foreach (ROOTGroupEntry* ptr in new PointerEnumerator(this.Address - 1, 0x1000, -16))
    //                if (ptr->_id == 0xFFFF) return (ROOTGroup*)((uint)ptr - 8);
    //            return null;
    //        }
    //    }
    //}
}
