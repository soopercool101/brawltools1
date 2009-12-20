using System;
using BrawlLib.SSBBTypes;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class RBNKNode : RSARFileNode
    {
        internal RBNKHeader* Header { get { return (RBNKHeader*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.RBNK; } }

        protected override bool OnInitialize()
        {
            RSARNode parent;

            //Find bank entry in rsar
            if ((_name == null) && ((parent = RSARNode) != null))
            {
                RSARHeader* rsar = parent.Header;
                RuintList* list = rsar->INFOBlock->Banks;
                VoidPtr offset = &rsar->INFOBlock->_collection;
                SYMBHeader* symb = rsar->SYMBBlock;

                int count = list->_numEntries;
                for (int i = 0; i < count; i++)
                {
                    INFOBankEntry* bank = (INFOBankEntry*)list->Get(offset, i);
                    if (bank->_fileId == _fileIndex)
                    {
                        _name = symb->GetStringEntry(bank->_stringId);
                        break;
                    }
                }
            }
            
            base.OnInitialize();

            return true;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((RBNKHeader*)source.Address)->_header._tag == RBNKHeader.Tag ? new RBNKNode() : null; }
    }
}
