using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARTypeNode : RSAREntryNode
    {
        internal INFOData3* Header { get { return (INFOData3*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_typeId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARType; } }

        private uint _flags;
        private int _typeId;

        public uint Flags { get { return _flags; } set { _flags = value; SignalPropertyChange(); } }
        public int TypeId { get { return _typeId; } set { _typeId = value; SignalPropertyChange(); } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _flags = Header->_flags;
            _typeId = Header->_typeId;

            return false;
        }
    }
}
