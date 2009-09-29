using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ResourceEntryNode : ResourceNode
    {
        internal ResourceEntry* EntryData { get { return _parent != null ? (ResourceEntry*)(&((IResourceGroupNode)_parent).Group->First[Index]) : null; } }

        protected short _prev, _next, _id;

        protected override bool OnInitialize()
        {
            ResourceEntry* entry = EntryData;

            Name = entry->GetName();
            _prev = entry->_prev;
            _next = entry->_next;
            _id = entry->_id;

            return false;
        }
    }
}
