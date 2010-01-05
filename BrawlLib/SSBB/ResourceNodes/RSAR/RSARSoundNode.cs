using System;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using System.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARSoundNode : RSAREntryNode, IAudioSource
    {
        internal INFOSoundEntry* Header { get { return (INFOSoundEntry*)WorkingUncompressed.Address; } }
        internal override int StringId { get { return Header->_stringId; } }

        public override ResourceType ResourceType { get { return ResourceType.RSARFile; } }

        INFOSoundPart1 _part1;
        INFOSoundPart2 _part2;

        //[Category("RSAR Sound")]
        //public int StringId { get { return Header->_stringId; } }
        [Category("RSAR Sound")]
        public int FileId { get { return Header->_fileId; } }
        [Category("RSAR Sound")]
        public int Unknown1 { get { return Header->_unk1; } }
        [Category("RSAR Sound")]
        public byte Flag1 { get { return Header->_flag1; } }
        [Category("RSAR Sound")]
        public byte Flag2 { get { return Header->_flag2; } }
        [Category("RSAR Sound")]
        public byte Flag3 { get { return Header->_flag3; } }
        [Category("RSAR Sound")]
        public byte Flag4 { get { return Header->_flag4; } }
        [Category("RSAR Sound")]
        public int Unknown2 { get { return Header->_unk2; } }
        [Category("RSAR Sound")]
        public int Unknown3 { get { return Header->_unk3; } }
        [Category("RSAR Sound")]
        public int Unknown4 { get { return Header->_unk4; } }

        [Category("RSAR Sound Part 1")]
        public int UNK1 { get { return _part1._unk1; } }
        [Category("RSAR Sound Part 1")]
        public int UNK2 { get { return _part1._unk2; } }
        [Category("RSAR Sound Part 1")]
        public int UNK3 { get { return _part1._unk3; } }

        [Category("RSAR Sound Part 2")]
        public int PackIndex { get { return _part2._soundIndex; } }
        [Category("RSAR Sound Part 2")]
        public int Unk1 { get { return _part2._unk1; } }
        [Category("RSAR Sound Part 2")]
        public int Unk2 { get { return _part2._unk2; } }
        [Category("RSAR Sound Part 2")]
        public int Unk3 { get { return _part2._unk3; } }


        protected override bool OnInitialize()
        {
            base.OnInitialize();

            INFOHeader* info = RSARNode.Header->INFOBlock;
            _part1 = *Header->GetPart1(&info->_collection);
            _part2 = *Header->GetPart2(&info->_collection);

            return false;
        }


        public IAudioStream CreateStream()
        {
            return null;
        }
    }
}
