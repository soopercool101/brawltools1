﻿using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARListNode : ResourceNode
    {
        internal RSARHeader* Header { get { return (RSARHeader*)WorkingUncompressed.Address; } }

        private int _index;
        public RSARListNode(int index) { _index = index; }

        protected override bool OnInitialize()
        {
            switch (_index)
            {
                //case 0: _name = "Sounds"; break;
                case 1: _name = "Banks"; break;
                case 2: _name = "Types"; break;
                case 3: _name = "Sets"; break;
                case 4: _name = "Groups"; break;
            }

            return true;
        }

        protected override void OnPopulate()
        {
            Type t;
            switch (_index)
            {
                //case 0: t = typeof(RSARSoundNode); break;
                case 1: t = typeof(RSARBankNode); break;
                case 2: t = typeof(RSARTypeNode); break;
                case 3: t = typeof(RSARSetNode); break;
                case 4: t = typeof(RSARGroupNode); break;
                default: return;
            }

            ruint* groups = Header->INFOBlock->GroupList;
            ruint* list = (ruint*)((uint)groups + groups[_index] + 4);

            int count = *((bint*)list - 1);
            for (int i = 0; i < count; i++)
                ((ResourceNode)Activator.CreateInstance(t)).Initialize(this, new DataSource((VoidPtr)groups + list[i], 0));
        }
    }

    public unsafe class RSAREntryNode : ResourceNode
    {
        internal RSARNode RSARNode
        {
            get
            {
                ResourceNode n = this;
                while (((n = n.Parent) != null) && !(n is RSARNode)) ;
                return n as RSARNode;
            }
        }
        internal virtual int StringId { get { return 0; } }

        //protected override bool OnInitialize()
        //{
        //    RSARNode p = RSARNode;
        //    if (p != null)
        //        _name = p.Header->SYMBBlock->GetStringEntry(StringId);
        //    else
        //        _name = String.Format("Entry{0}", StringId);

        //    return false;
        //}
    }
}
