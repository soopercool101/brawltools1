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
            SYMBHeader* symb = Header->SYMBBlock;
            sbyte* offset = (sbyte*)symb + 8;
            buint* stringOffsets = symb->StringOffsets;

            VoidPtr gOffset = (VoidPtr)Header->INFOBlock + 8;
            ruint* groups = (ruint*)gOffset;
            for (int i = 0; i < 5; i++)
            {
                ruint* list = (ruint*)((uint)groups + groups[i] + 4);

                Type t = null;
                switch (i)
                {
                    case 0: t = typeof(RSARSoundNode); break;
                    case 1: t = typeof(RSARBankNode); break;
                    case 2: t = typeof(RSARTypeNode); break;
                    case 3: continue;// t = typeof(RSARSetNode); break;
                    case 4: t = typeof(RSARGroupNode); break;
                }

                int count = *((bint*)list - 1);
                sbyte* str, end;
                for (int x = 0; x < count; x++)
                {
                    ResourceNode parent = this;
                    RSAREntryNode n = Activator.CreateInstance(t) as RSAREntryNode;
                    n._origSource = n._uncompSource = new DataSource(gOffset + list[x], 0);

                    str = offset + stringOffsets[n.StringId];

                    for (end = str; *end != 0; end++) ;
                    while ((--end > str) && (*end != '_')) ;

                    if (end > str)
                    {
                        parent = CreatePath(parent, str, (int)end - (int)str);
                        n._name = new String(end + 1);
                    }
                    else
                    {
                        n._name = new String(str);
                    }
                    n.Initialize(parent, gOffset + list[x], 0);
                }
            }
        }

        private ResourceNode CreatePath(ResourceNode parent, sbyte* str, int length)
        {
            ResourceNode current;

            int len;
            char* cPtr;
            sbyte* start, end;
            sbyte* ceil = str + length;
            while (str < ceil)
            {
                for (end = str; ((end < ceil) && (*end != '_')); end++) ;
                len = (int)end - (int)str;

                current = null;
                foreach (ResourceNode n in parent._children)
                {
                    if ((n._name.Length != len) || !(n is RSARFolderNode))
                        continue;

                    fixed (char* p = n._name)
                        for (cPtr = p, start = str; (start < end) && (*start == *cPtr); start++, cPtr++) ; ;

                    if (start == end)
                    {
                        current = n;
                        break;
                    }
                }
                if (current == null)
                {
                    current = new RSARFolderNode();
                    current._name = new String(str, 0, len);
                    current._parent = parent;
                    parent._children.Add(current);
                }

                str = end + 1;
                parent = current;
            }

            return parent;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((RSARHeader*)source.Address)->_commonHeader._tag == RSARHeader.Tag ? new RSARNode() : null; }
    }
}
