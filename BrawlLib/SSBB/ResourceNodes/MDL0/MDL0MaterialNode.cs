using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0MaterialNode : MDL0EntryNode
    {
        internal MDL0Material* Data { get { return (MDL0Material*)WorkingSource.Address; } }
        //internal List<MDL0Data7Part3> _part3Entries = new List<MDL0Data7Part3>();
        internal List<string> _part4Entries = new List<string>();

        [Category("Material")]
        public int TotalLen { get { return Data->_dataLen; } }
        [Category("Material")]
        public int MDL0Offset { get { return Data->_mdl0Offset; } }
        [Category("Material")]
        public int StringOffset { get { return Data->_stringOffset; } }
        [Category("Material")]
        public int ID { get { return Data->_index; } }
        [Category("Material")]
        public int Unknown1 { get { return Data->_unk1; } }
        [Category("Material")]
        public byte Flag1 { get { return Data->_flag1; } }
        [Category("Material")]
        public byte Flag2 { get { return Data->_flag2; } }
        [Category("Material")]
        public byte Flag3 { get { return Data->_flag3; } }
        [Category("Material")]
        public byte Flag4 { get { return Data->_flag4; } }
        [Category("Material")]
        public int Type { get { return Data->_type; } }
        [Category("Material")]
        public byte Flag5 { get { return Data->_flag5; } }
        [Category("Material")]
        public byte Flag6 { get { return Data->_flag6; } }
        [Category("Material")]
        public byte Flag7 { get { return Data->_flag7; } }
        [Category("Material")]
        public byte Flag8 { get { return Data->_flag8; } }

        [Category("Material")]
        public int Unknown3 { get { return Data->_unk3; } }
        [Category("Material")]
        public int Unknown4 { get { return Data->_unk4; } }

        [Category("Material")]
        public int Data8Offset { get { return Data->_data8Offset; } }

        [Category("Material")]
        public int NumTextures { get { return Data->_numTextures; } }
        [Category("Material")]
        public int Part3Offset { get { return Data->_part3Offset; } }
        [Category("Material")]
        public int Part4Offset { get { return Data->_part4Offset; } }
        [Category("Material")]
        public int Part5Offset { get { return Data->_part5Offset; } }

        [Category("Material")]
        public int Unknown6 { get { return Data->_unk6; } }

        //[Category("Material Part3")]
        //public List<MDL0Data7Part3> Part3Entries { get { return _part3Entries; } }
        [Category("Material Part4")]
        public List<string> Part4Entries { get { return _part4Entries; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            if (!_initialized)
            {

                MDL0Data7Part4* part4 = Data->Part4;
                if (part4 != null)
                {
                    ResourceGroup* group = part4->Group;
                    for (int i = 0; i < group->_numEntries; i++)
                    {
                        _part4Entries.Add(group->First[i].GetName());
                    }
                }

                _origSource.Length = _uncompSource.Length = TotalLen;
                return Data->_numTextures != 0;
            }
            return false;
        }

        protected override void OnPopulate()
        {
            MDL0Data7Part3* part3 = Data->Part3;
            for (int i = 0; i < Data->_numTextures; i++)
                new MDL0MaterialRefNode().Initialize(this, part3++, MDL0Data7Part3.Size);
        }

        internal override void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            strings[Name] = 0;
            foreach (string s in _part4Entries)
                strings[s] = 0;
        }
    }
}
