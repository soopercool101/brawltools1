using System;
using BrawlLib.SSBBTypes;
using System.Audio;
using BrawlLib.Wii.Audio;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RWSDSoundNode : RWSDEntryNode, IAudioSource
    {
        internal RWSD_WAVEEntry* Header { get { return (RWSD_WAVEEntry*)WorkingUncompressed.Address; } }

        internal VoidPtr _dataAddr;

        protected override bool OnInitialize()
        {
            _dataAddr = ((RWSDNode)_parent._parent)._audioSource.Address + Header->_offset;

            if (_name == null)
                _name = string.Format("Audio[{0:X2}]", Index);

            return false;
        }

        public IAudioStream CreateStream()
        {
            return new ADPCMStream(Header, _dataAddr);
        }
    }
}
