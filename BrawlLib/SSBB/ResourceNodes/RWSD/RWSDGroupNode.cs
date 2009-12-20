using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDEntryNode : ResourceNode
    {
        internal VoidPtr _offset;
    }

    public unsafe class RWSDGroupNode : ResourceNode
    {
        internal RWSD_DATAHeader* Header { get { return (RWSD_DATAHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RWSDGroup; } }

        int _index;

        protected override bool OnInitialize()
        {
            _index = Index;
            if (_index == 0)
                _name = "Sounds";
            else
                _name = "Audio";

            return Header->_list._numEntries > 0;
        }

        protected override void OnPopulate()
        {
            VoidPtr offset = &Header->_list;
            int count = Header->_list._numEntries;

            if (_index == 0)
            {
                LabelItem[] list = ((RWSDNode)_parent)._labels; //Get labels from parent
                ((RWSDNode)_parent)._labels = null; //Clear labels, no more use for them!

                for (int i = 0; i < count; i++)
                {
                    RWSDDataNode node = new RWSDDataNode();
                    if (list != null)
                    {
                        node._soundIndex = list[i].Tag;
                        node._name = list[i].String;
                    }
                    node.Initialize(this, Header->_list.Get(offset, i), 0);
                }
            }
            else
                for (int i = 0; i < count; i++)
                    new RWSDSoundNode().Initialize(this, Header->_list.Get(offset, i), 0);

        }
    }
}
