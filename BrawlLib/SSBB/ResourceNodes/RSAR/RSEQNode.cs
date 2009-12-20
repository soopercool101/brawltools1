using System;
using BrawlLib.SSBBTypes;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RSEQNode : RSARFileNode
    {
        internal RSEQHeader* Header { get { return (RSEQHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RSEQ; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            //RSARNode rsar = RSARNode;
            //if (rsar == null)
            //    _name = Path.GetFileNameWithoutExtension(_origPath);
            //else
            //    _name = String.Format("[0x{0:X}] Sequence", _fileIndex);
            return true;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((RSEQHeader*)source.Address)->_header._tag == RSEQHeader.Tag ? new RSEQNode() : null; }
    }
}
