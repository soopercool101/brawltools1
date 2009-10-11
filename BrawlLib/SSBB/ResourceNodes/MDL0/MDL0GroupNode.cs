using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0EntryNode : ResourceEntryNode
    {
        internal virtual void GetStrings(IDictionary<string, VoidPtr> strings) { strings[Name] = 0; }
    }

    public unsafe class MDL0GroupNode : ResourceNode, IResourceGroupNode
    {
        private int _index;
        private int _topNode, _mRight;

        internal ResourceGroup* Header { get { return (ResourceGroup*)WorkingSource.Address; } }
        ResourceGroup* IResourceGroupNode.Group { get { return Header; } }

        [Category("MDL0 Group")]
        public int TopNode { get { return _topNode; } set { _topNode = value; } }

        internal void GetStrings(IDictionary<string, VoidPtr> strings)
        {
            foreach (MDL0EntryNode n in Children)
                n.GetStrings(strings);
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
                case 0: Name = "Definitions"; break;
                case 1: Name = "Bones"; break;
                case 2: Name = "Vertices"; break;
                case 3: Name = "Normals"; break;
                case 4: Name = "Colors"; break;
                case 5: Name = "UV Points"; break;
                case 6: Name = "Materials1"; break;
                case 7: Name = "Materials2"; break;
                case 8: Name = "Polygons"; break;
                case 9: Name = "Textures1"; break;
                case 10: Name = "Textures2"; break;
            }
            _topNode = Header->_first._leftIndex;

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
        }
    }
}
