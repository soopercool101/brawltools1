using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe abstract class MDL0EntryNode : ResourceNode
    {
        internal virtual void GetStrings(StringTable table) { table.Add(Name); }

        protected abstract int DataLength { get; }

        protected override bool OnInitialize()
        {
            if (IsBranch)
                if (IsCompressed)
                    _replUncompSrc.Length = DataLength;
                else
                    _replSrc.Length = _replUncompSrc.Length = DataLength;
            else
                if (IsCompressed)
                    _uncompSource.Length = DataLength;
                else
                    _origSource.Length = _uncompSource.Length = DataLength;

            return false;
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable) { }
    }

    public unsafe class MDL0GroupNode : ResourceNode
    {
        internal int _index;

        internal ResourceGroup* Header { get { return (ResourceGroup*)WorkingUncompressed.Address; } }

        internal void GetStrings(StringTable table)
        {
            foreach (MDL0EntryNode n in Children)
                n.GetStrings(table);
        }

        internal void Initialize(ResourceNode parent, DataSource source, int index)
        {
            _index = index;
            base.Initialize(parent, source);
        }

        protected override bool OnInitialize()
        {
            switch (_index)
            {
                case 0: _name = "Definitions"; break;
                case 1: _name = "Bones"; break;
                case 2: _name = "Vertices"; break;
                case 3: _name = "Normals"; break;
                case 4: _name = "Colors"; break;
                case 5: _name = "UV Points"; break;
                case 6: _name = "Materials1"; break;
                case 7: _name = "Materials2"; break;
                case 8: _name = "Polygons"; break;
                case 9: _name = "Textures1"; break;
                case 10: _name = "Textures2"; break;
            }
            //_topNode = Header->_first._leftIndex;

            return Header->_numEntries > 0;
        }

        protected override void OnPopulate()
        {
            Type t;
            switch (_index)
            {
                case 0: { t = typeof(MDL0DefNode); break; }
                case 1: { t = typeof(MDL0BoneNode); break; }
                case 2: { t = typeof(MDL0VertexNode); break; }
                case 3: { t = typeof(MDL0NormalNode); break; }
                case 4: { t = typeof(MDL0ColorNode); break; }
                case 5: { t = typeof(MDL0UVNode); break; }
                case 6: { t = typeof(MDL0MaterialNode); break; }
                case 7: { t = typeof(MDL0MaterialExtNode); break; }
                case 8: { t = typeof(MDL0PolygonNode); break; }
                case 9: { t = typeof(MDL0Data10Node); break; }
                case 10: { t = typeof(MDL0Data10Node); break; }
                default: return;
            }

            ResourceGroup* group = Header;
            for (int i = 0; i < group->_numEntries; i++)
                ((MDL0EntryNode)Activator.CreateInstance(t)).Initialize(this, new DataSource(group->First[i].DataAddress, 0));

            if(_index == 1)
            {
                List<ResourceNode> _roots = new List<ResourceNode>();
                MDL0BoneNode bone;
                int offset;
                for(int i = 0 ; i < _children.Count; i++)
                {
                    bone = _children[i] as MDL0BoneNode;
                    if ((offset = bone.ParentOffset) == 0)
                        _roots.Add(bone);
                    else 
                    {
                        bone._parent = _children[i + offset];
                        bone._parent.Children.Add(bone);
                    }
                }
                _children.Clear();
                _children = _roots;
            }
            else if ((_index >= 9) || (_index == 0) || (_index == 7))
            {
                for (int i = 0; i < group->_numEntries; i++)
                    _children[i]._name = new String((sbyte*)group + group->First[i]._stringOffset);
            }
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            ResourceGroup* group = (ResourceGroup*)dataAddress;
            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            if (_index == 1)
            {
                //Special processing for bones

                int index = 0;
                foreach (MDL0EntryNode n in Children)
                    PostProcessBone(n, group,ref index, stringTable);
            }
            else
            {
                int index = 1;
                foreach (MDL0EntryNode n in Children)
                {
                    dataAddress = (VoidPtr)group + (rEntry++)->_dataOffset;
                    ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                    n.PostProcess(dataAddress, stringTable);
                }
            }
        }

        private void PostProcessBone(MDL0EntryNode node, ResourceGroup* group, ref int index, StringTable stringTable)
        {
            VoidPtr dataAddress = (VoidPtr)group + group->First[index++]._dataOffset;
            ResourceEntry.Build(group, index, dataAddress, (BRESString*)stringTable[node.Name]);
            node.PostProcess(dataAddress, stringTable);

            foreach (MDL0EntryNode n in node.Children)
                PostProcessBone(n, group, ref index, stringTable);
        }
    }
}
