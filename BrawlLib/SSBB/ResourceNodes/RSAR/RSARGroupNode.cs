using System;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARGroupNode : RSAREntryNode
    {
        internal INFOData5* Header { get { return (INFOData5*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_id; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARGroup; } }

        private int _id;
        private int _magic;
        private int _unk1, _unk2;

        public int Id { get { return _id; } }
        public int Magic { get { return _magic; } }
        public int Unknown1 { get { return _unk1; } }
        public int Unknown2 { get { return _unk2; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _id = Header->_id;
            _magic = Header->_magic;
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;

            return false;
        }
    }
}
