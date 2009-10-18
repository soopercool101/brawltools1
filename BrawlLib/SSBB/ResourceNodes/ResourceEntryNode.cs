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

        protected int _left, _right, _id;

        [Category("Resource Entry")]
        public int SortNext { get { return _left; } set { _left = value; } }
        [Category("Resource Entry")]
        public int NodeNext { get { return _right; } set { _right = value; } }
        [Category("Resource Entry")]
        public int EntryId { get { return _id; } set { _id = value; } }

        protected override bool OnInitialize()
        {
            if ((_parent != null) && (!_initialized))
            {
                ResourceEntry* entry = EntryData;

                Name = entry->GetName();
                //_left = entry->_leftIndex;
                //_right = entry->_rightIndex;
                //_id = entry->_id;
            }
            return false;
        }
    }
}
