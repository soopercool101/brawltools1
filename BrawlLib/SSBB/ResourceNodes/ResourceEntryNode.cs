using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ResourceEntryNode : ResourceNode
    {
        internal ResourceEntry* EntryData { get { return _parent != null ? (ResourceEntry*)(&((IResourceGroupNode)_parent).Group->First[Index]) : null; } }

        protected short _prev, _next, _id;

        [Category("Resource Entry")]
        public short SortNext { get { return _prev; } set { _prev = value; } }
        [Category("Resource Entry")]
        public short NodeNext { get { return _next; } set { _next = value; } }
        [Category("Resource Entry")]
        public short EntryId { get { return _id; } set { _id = value; } }

        protected override bool OnInitialize()
        {
            if ((_parent != null) && (!_initialized))
            {
                ResourceEntry* entry = EntryData;

                Name = entry->GetName();
                _prev = entry->_leftIndex;
                _next = entry->_rightIndex;
                _id = entry->_id;
            }
            return false;
        }
    }
}
