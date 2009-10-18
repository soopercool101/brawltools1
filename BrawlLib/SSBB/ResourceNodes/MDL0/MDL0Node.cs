using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.SSBBTypes;
using BrawlLib.OpenGL;
using System.IO;
using BrawlLib.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0Node : BRESEntryNode
    {

        internal MDL0* Header { get { return (MDL0*)WorkingSource.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.MDL0; } }

        public override int DataAlign { get { return 0x20; } }

        private int _unk1, _unk2, _unk3, _unk4, _version;
        private int _numVertices, _numFaces, _numNodes;
        private Vector3 _min, _max;

        [Category("MDL0 Def")]
        public int Unknown1 { get { return _unk1; } set { _unk1 = value; } }
        [Category("MDL0 Def")]
        public int Unknown2 { get { return _unk2; } set { _unk2 = value; } }
        [Category("MDL0 Def")]
        public int NumVertices { get { return _numVertices; } set { _numVertices = value; } }
        [Category("MDL0 Def")]
        public int NumFaces { get { return _numFaces; } set { _numFaces = value; } }
        [Category("MDL0 Def")]
        public int Unknown3 { get { return _unk3; } set { _unk3 = value; } }
        [Category("MDL0 Def")]
        public int NumNodes { get { return _numNodes; } set { _numNodes = value; } }
        [Category("MDL0 Def")]
        public int Version { get { return _version; } set { _version = value; } }
        [Category("MDL0 Def")]
        public int Unknown4 { get { return _unk4; } set { _unk4 = value; } }
        [Category("MDL0 Def")]
        public BVec3 BoxMin { get { return _min; } set { _min = value; } }
        [Category("MDL0 Def")]
        public BVec3 BoxMax { get { return _max; } set { _max = value; } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            _unk1 = Header->_modelDef._unk1;
            _unk2 = Header->_modelDef._unk2;
            _numVertices = Header->_modelDef._numVertices;
            _numFaces = Header->_modelDef._numFaces;
            _unk3 = Header->_modelDef._unk3;
            _numNodes = Header->_modelDef._numNodes;
            _version = Header->_modelDef._version;
            _unk4 = Header->_modelDef._unk4;
            _min = Header->_modelDef._minExtents;
            _max = Header->_modelDef._maxExtents;

            return true;
        }

        protected override void OnPopulate()
        {
            ResourceGroup* group;
            for (int i = 0; i < 11; i++)
                if ((group = Header->GetEntry(i)) != null)
                    new MDL0GroupNode().Initialize(this, new DataSource(group, 0), i);
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (MDL0GroupNode n in Children)
                n.GetStrings(table);
        }

        public GLModel GetModel()
        {
            return new GLModel(this);
        }

        public override unsafe void Export(string outPath)
        {
            //get total size
            int len = WorkingRawSource.Length;
            int stringOffset;

            StringTable table = new StringTable();
            this.GetStrings(table);

            stringOffset = len = len.Align(4);
            len += table.GetTotalSize();
            //len.Align(0x20);

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(len);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    Memory.Move(map.Address, WorkingRawSource.Address, (uint)WorkingRawSource.Length);
                    table.WriteTable(map.Address + stringOffset);
                }
            }
            table.Clear();
        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((MDL0*)address)->_entry._tag == MDL0.Tag ? new MDL0Node() : null; }
    }
}
