using System;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using BrawlLib.IO;
using BrawlLib.Wii.Compression;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class ARCNode : ARCEntryNode
    {
        private bool _isPair;

        internal ARCHeader* Header { get { return (ARCHeader*)WorkingSource.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.ARC; } }
        public bool IsPair { get { return _isPair; } set { _isPair = value; } }

        protected override void OnPopulate()
        {
            ARCFileHeader* entry = Header->First;
            for (int i = 0 ; i < Header->_numFiles; i++, entry = entry->Next)
                if (NodeFactory.FromAddress(this, entry->Data, entry->Length) == null)
                    new ARCEntryNode().Initialize(this, entry->Data, entry->Length);
        }

        internal override void Initialize(ResourceNode parent, DataSource origSource, DataSource uncompSource)
        {
            base.Initialize(parent, origSource, uncompSource);
            if (_origPath != null)
            {
                string path = Path.Combine(Path.GetDirectoryName(_origPath), Path.GetFileNameWithoutExtension(_origPath));
                _isPair = File.Exists(path + ".pac") && File.Exists(path + ".pcs");
            }
        }

        protected override bool OnInitialize()
        {
            base.OnInitialize();
            _name = Header->Name;
            return Header->_numFiles > 0;
        }

        //public override void Replace(string fileName)
        //{
        //    base.Replace(fileName);
        //}

        protected override int OnCalculateSize(bool force)
        {
            int size = ARCHeader.Size + (Children.Count * 0x20);
            foreach (ResourceNode node in Children)
                size += node.CalculateSize(force).Align(0x20);
            return size;
        }

        internal protected override void OnRebuild(VoidPtr address, int size, bool force)
        {
            ARCHeader* header = (ARCHeader*)address;
            *header = new ARCHeader((ushort)Children.Count, Name);

            ARCFileHeader* entry = header->First;
            foreach(ARCEntryNode node in Children)
            {
                *entry = new ARCFileHeader(node.FileType, node.FileIndex, node._calcSize, node.FileFlags, node.FileId);
                if (node.IsCompressed)
                    node.WriteCompressed(entry->Data, entry->Length);
                else
                    node.Rebuild(entry->Data, entry->Length, force);
                entry = entry->Next;
            }

            _replSrc.Close();
            _replUncompSrc.Close();
            _replSrc = _replUncompSrc = new DataSource(address, size);
        }

        public void ExportPair(string path)
        {
            if (Path.HasExtension(path))
                path = path.Substring(0, path.LastIndexOf('.'));

            ExportPAC(path + ".pac");
            ExportPCS(path + ".pcs");
        }
        public void ExportPAC(string outPath)
        {
            ExportUncompressed(outPath);
        }
        public void ExportPCS(string outPath)
        {
            if (Compression == CompressionType.LZ77)
                Export(outPath);
            else
            {
                using (FileStream inStream = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 0x2000, FileOptions.SequentialScan | FileOptions.DeleteOnClose))
                using(FileStream outStream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 8, FileOptions.SequentialScan))
                {
                    Compressor.Compact(CompressionType.LZ77, WorkingSource.Address, WorkingSource.Length, inStream);
                    outStream.SetLength(inStream.Length);
                    using (FileMap map = FileMap.FromStream(inStream))
                    using(FileMap outMap = FileMap.FromStream(outStream))
                        Memory.Move(outMap.Address, map.Address, (uint)map.Length);
                }
            }
        }

        internal static ResourceNode TryParse(VoidPtr address) 
        { return ((ARCHeader*)address)->_tag == ARCHeader.Tag ? new ARCNode() : null; }
    }

    public unsafe class ARCEntryNode : ResourceNode
    {
        internal ARCFileHeader* FileHeader { get { return _parent == null ? null : (ARCFileHeader*)(WorkingRawSource.Address - 0x20); } }
        public override ResourceType ResourceType { get { return ResourceType.ARCEntry; } }

        internal ARCFileType _fileType;
        [Category("ARC Entry")]
        public ARCFileType FileType { get { return _fileType; } set { _fileType = value; } }

        internal short _fileIndex;
        [Category("ARC Entry")]
        public short FileIndex { get { return _fileIndex; } set { _fileIndex = value; } }

        internal short _fileFlags;
        [Category("ARC Entry")]
        public short FileFlags { get { return _fileFlags; } set { _fileFlags = value; } }

        internal short _fileId;
        [Category("ARC Entry")]
        public short FileId { get { return _fileId; } set { _fileId = value; } }

        protected override bool OnInitialize()
        {
            if (!IsBranch)
            {
                if (FileHeader != null)
                {
                    _fileType = FileHeader->FileType;
                    _fileIndex = FileHeader->_index;
                    _fileFlags = FileHeader->_flags;
                    _fileId = FileHeader->_id;
                    _name = String.Format("{0}[{1}]", _fileType, _fileIndex);
                }
                else
                    Name = Path.GetFileName(_origPath);
            }

            return false;
        }
    }
}
