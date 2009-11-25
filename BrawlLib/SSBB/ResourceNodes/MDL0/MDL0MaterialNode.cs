using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;
using System.Drawing;
using System.IO;
using BrawlLib.Imaging;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialNode : MDL0EntryNode
    {
        internal MDL0Material* Header { get { return (MDL0Material*)WorkingUncompressed.Address; } }
        protected override int DataLength { get { return Header->_dataLen; } }

        //internal List<MDL0Data7Part3> _part3Entries = new List<MDL0Data7Part3>();
        internal List<string> _part4Entries = new List<string>();

        private byte _numTextures;
        private byte _numLayers;
        private byte _flag3;
        private byte _flag4;
        private byte _flag5;
        private byte _flag6;
        private byte _flag7;
        private byte _flag8;
        private int _type;

        [Category("Material")]
        public int TotalLen { get { return Header->_dataLen; } }
        [Category("Material")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Material")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Material")]
        public int ID { get { return Header->_index; } }
        [Category("Material")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("Material")]
        public byte Textures { get { return _numTextures; } set { _numTextures = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Layers { get { return _numLayers; } set { _numLayers = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag3 { get { return _flag3; } set { _flag3 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag4 { get { return _flag4; } set { _flag4 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public int Type { get { return _type; } set { _type = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag5 { get { return _flag5; } set { _flag5 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag6 { get { return _flag6; } set { _flag6 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag7 { get { return _flag7; } set { _flag7 = value; SignalPropertyChange(); } }
        [Category("Material")]
        public byte Flag8 { get { return _flag8; } set { _flag8 = value; SignalPropertyChange(); } }

        [Category("Material")]
        public int Unknown3 { get { return Header->_unk3; } }
        [Category("Material")]
        public int Unknown4 { get { return Header->_unk4; } }

        [Category("Material")]
        public int Data8Offset { get { return Header->_data8Offset; } }

        [Category("Material")]
        public int NumTextures { get { return Header->_numTextures; } }
        [Category("Material")]
        public int Part3Offset { get { return Header->_part3Offset; } }
        [Category("Material")]
        public int Part4Offset { get { return Header->_part4Offset; } }
        [Category("Material")]
        public int Part5Offset { get { return Header->_part5Offset; } }

        [Category("Material")]
        public int Unknown6 { get { return Header->_unk6; } }

        //[Category("Material Part3")]
        //public List<MDL0Data7Part3> Part3Entries { get { return _part3Entries; } }
        [Category("Material Part4")]
        public List<string> Part4Entries { get { return _part4Entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _numTextures = Header->_flag1;
            _numLayers = Header->_numLayers;
            _flag3 = Header->_flag3;
            _flag4 = Header->_flag4;
            _flag5 = Header->_flag5;
            _flag6 = Header->_flag6;
            _flag7 = Header->_flag7;
            _flag8 = Header->_flag8;
            _type = Header->_type;

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            MDL0Data7Part4* part4 = Header->Part4;
            if (part4 != null)
            {
                ResourceGroup* group = part4->Group;
                for (int i = 0; i < group->_numEntries; i++)
                    _part4Entries.Add(group->First[i].GetName());
            }

            return Header->_numTextures != 0;
        }

        protected override void OnPopulate()
        {
            MDL0Data7Part3* part3 = Header->Part3;
            for (int i = 0; i < Header->_numTextures; i++)
                new MDL0MaterialRefNode().Initialize(this, part3++, MDL0Data7Part3.Size);
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (string s in _part4Entries)
                table.Add(s);

            foreach (MDL0MaterialRefNode n in Children)
                n.GetStrings(table);
        }

        //protected override int OnCalculateSize(bool force)
        //{

        //}

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Material* header = (MDL0Material*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            header->_flag1 = _numTextures;
            header->_numLayers = _numLayers;
            header->_flag3 = _flag3;
            header->_flag4 = _flag4;
            header->_flag5 = _flag5;
            header->_flag6 = _flag6;
            header->_flag7 = _flag7;
            header->_flag8 = _flag8;
            header->_type = _type;

            MDL0Data7Part4* part4 = header->Part4;
            if (part4 != null)
            {
                ResourceGroup* group = part4->Group;
                group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
                ResourceEntry* rEntry = group->First;

                for (int i = 0, x = 1; i < group->_numEntries; i++)
                {
                    MDL0Data7Part4Entry* entry = (MDL0Data7Part4Entry*)((int)group + (rEntry++)->_dataOffset);
                    ResourceEntry.Build(group, x++, entry, (BRESString*)stringTable[_part4Entries[i]]);
                    entry->ResourceStringAddress = stringTable[_part4Entries[i]] + 4;
                }
            }

            MDL0Data7Part3* part3 = header->Part3;
            foreach (MDL0MaterialRefNode n in Children)
                n.PostProcess(part3++, stringTable);
        }

        internal override void Bind(GLContext ctx) { foreach (MDL0MaterialRefNode m in Children) m.Bind(ctx); }
        internal override void Unbind(GLContext ctx) { foreach (MDL0MaterialRefNode m in Children) m.Unbind(ctx); }
    }
}
