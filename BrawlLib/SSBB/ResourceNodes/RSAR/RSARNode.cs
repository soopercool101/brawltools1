using System;
using BrawlLib.SSBBTypes;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSARNode : ResourceNode
    {
        internal RSARHeader* Header { get { return (RSARHeader*)WorkingSource.Address; } }

        public override ResourceType ResourceType { get { return ResourceType.RSAR; } }

        protected override bool OnInitialize()
        {
            if (_origPath != null)
                _name = Path.GetFileNameWithoutExtension(this._origPath);

            return true;
        }

        protected override void OnPopulate()
        {

        }

        internal static ResourceNode TryParse(VoidPtr address) { return ((RSARHeader*)address)->_commonHeader._tag == RSARHeader.Tag ? new RSARNode() : null; }
    }
}
