using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class EFLSNode : ARCEntryNode
    {
        internal EFLSHeader* Header { get { return (EFLSHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.EFLS; } }

        private int _unk1, _unk2, _unk3;

        [Category("EFLS")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("EFLS")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("EFLS")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;

            return Header->_numEntries > 0;
        }

        protected override int OnCalculateSize(bool force)
        {
            int size = 0x20;
            foreach (EFLSEntryNode e in Children)
                size += e._name.Length + 0x11;
            return size;
        }

        protected override void OnPopulate()
        {
            EFLSHeader* header = Header;
            for (int i = 0; i < header->_numEntries; i++)
                new EFLSEntryNode() { _name = header->GetString(i) }.Initialize(this, &header->Entries[i], 0);
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            int count = Children.Count;

            EFLSHeader* header = (EFLSHeader*)address;
            *header = new EFLSHeader(count + 1, _unk1, _unk2, _unk3);

            EFLSEntry* entry = (EFLSEntry*)((int)header + 0x10);
            byte* dPtr = (byte*)entry + (count * 0x10);
            foreach (EFLSEntryNode n in Children)
            {
                int offset = n._name.Equals("<null>", StringComparison.OrdinalIgnoreCase) ? 0 : (int)dPtr - (int)header;
                *entry++ = new EFLSEntry(n._unk1, n._unk2, offset, n._unk3, n._unk4);

                if (offset > 0)
                {
                    int len = n._name.Length;
                    fixed (char* cPtr = n._name)
                        for (int i = 0; i < len; i++)
                            *dPtr++ = (byte)cPtr[i];
                    *dPtr++ = 0;
                }
            }
        }


        internal static ResourceNode TryParse(DataSource source) { return ((EFLSHeader*)source.Address)->_tag == EFLSHeader.Tag ? new EFLSNode() : null; }
    }

    public unsafe class EFLSEntryNode : ResourceNode
    {
        internal EFLSEntry* Header { get { return (EFLSEntry*)WorkingUncompressed.Address; } }
        public override bool AllowNullNames { get { return true; } }

        internal int _unk1, _unk2, _unk3, _unk4;

        [Category("EFLS Entry")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; SignalPropertyChange(); } }
        [Category("EFLS Entry")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; SignalPropertyChange(); } }
        [Category("EFLS Entry")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; SignalPropertyChange(); } }
        [Category("EFLS Entry")]
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; SignalPropertyChange(); } }

        protected override bool OnInitialize()
        {
            _unk1 = Header->_unk1;
            _unk2 = Header->_unk2;
            _unk3 = Header->_unk3;
            _unk4 = Header->_unk4;

            return false;
        }
    }
}
