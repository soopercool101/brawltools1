using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe abstract class MDL0EntryNode : ResourceNode
    {
        internal virtual void GetStrings(StringTable table) { table.Add(Name); }

        internal int _entryIndex;

        internal MDL0Node Model
        {
            get
            {
                ResourceNode n = _parent;
                while (!(n is MDL0Node) && (n != null)) n = n._parent;
                return n as MDL0Node;
            }
        }

        internal virtual void Bind(GLContext ctx) { }
        internal virtual void Unbind(GLContext ctx) { }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable) { }
    }

    public unsafe class MDL0GroupNode : ResourceNode
    {
        internal ResourceGroup* Header { get { return (ResourceGroup*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MDL0Group; } }

        internal int _entryCount;
        internal int _index;
        internal List<ResourceNode> _nodeCache;

        internal void GetStrings(StringTable table)
        {
            foreach (MDL0EntryNode n in Children)
                n.GetStrings(table);
        }

        //internal void Initialize(ResourceNode parent, DataSource source, int index)
        //{
        //    _index = index;
        //    base.Initialize(parent, source);
        //}

        //protected override bool OnInitialize()
        //{
        //    switch (_index)
        //    {
        //        case 0: _name = "Definitions"; break;
        //        case 1: _name = "Bones"; break;
        //        case 2: _name = "Vertices"; break;
        //        case 3: _name = "Normals"; break;
        //        case 4: _name = "Colors"; break;
        //        case 5: _name = "UV Points"; break;
        //        case 6: _name = "Materials"; break;
        //        case 7: _name = "Shaders"; break;
        //        case 8: _name = "Polygons"; break;
        //        case 9: _name = "Textures"; break;
        //        case 10: _name = "Decals"; break;
        //    }
        //    //_topNode = Header->_first._leftIndex;

        //    return Header->_numEntries > 0;
        //}

        //protected override void OnPopulate()
        //{
        //    Type t;
        //    switch (_index)
        //    {
        //        case 0: { t = typeof(MDL0DefNode); break; }
        //        case 1: { t = typeof(MDL0BoneNode); break; }
        //        case 2: { t = typeof(MDL0VertexNode); break; }
        //        case 3: { t = typeof(MDL0NormalNode); break; }
        //        case 4: { t = typeof(MDL0ColorNode); break; }
        //        case 5: { t = typeof(MDL0UVNode); break; }
        //        case 6: { t = typeof(MDL0MaterialNode); break; }
        //        case 7: { t = typeof(MDL0ShaderNode); break; }
        //        case 8: { t = typeof(MDL0PolygonNode); break; }
        //        case 9: { t = typeof(MDL0TextureNode); break; }
        //        case 10: { t = typeof(MDL0TextureNode); break; }
        //        default: return;
        //    }

        //    ResourceGroup* group = Header;
        //    for (int i = 0; i < group->_numEntries; i++)
        //        ((MDL0EntryNode)Activator.CreateInstance(t)).Initialize(this, new DataSource(group->First[i].DataAddress, 0));

        //    if(_index == 1)
        //    {
        //        List<ResourceNode> _roots = new List<ResourceNode>();
        //        MDL0BoneNode bone;
        //        int offset;
        //        for(int i = 0 ; i < _children.Count; i++)
        //        {
        //            bone = _children[i] as MDL0BoneNode;
        //            if ((offset = bone.ParentOffset) == 0)
        //                _roots.Add(bone);
        //            else 
        //            {
        //                bone._parent = _children[i + offset];
        //                bone._parent.Children.Add(bone);
        //            }
        //        }
        //        _nodeCache = _children;
        //        _children = _roots;
        //    }
        //    else if ((_index >= 9) || (_index == 0) || (_index == 7))
        //    {
        //        for (int i = 0; i < group->_numEntries; i++)
        //            _children[i]._name = new String((sbyte*)group + group->First[i]._stringOffset);
        //    }
        //}

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            ResourceGroup* pGroup = (ResourceGroup*)dataAddress;
            ResourceEntry* rEntry = &pGroup->_first;
            int index = 1;
            (*rEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            if (_name == "Bones")
                foreach (MDL0EntryNode n in _children)
                    PostProcessBone(n, pGroup, ref index, stringTable);
            else if (_name == "Textures")
            {
                //ResourceGroup* dGroup = (ResourceGroup*)((byte*)pGroup + pGroup->_totalSize);
                bool hasDec = false;
                foreach (MDL0TextureNode n in _children)
                {
                    if (n._texRefs.Count > 0)
                        ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (rEntry++)->_dataOffset, (BRESString*)stringTable[n._name]);
                    if (n._decRefs.Count > 0)
                        hasDec = true;
                }
                if (hasDec)
                {
                    pGroup = (ResourceGroup*)((byte*)pGroup + pGroup->_totalSize);
                    rEntry = &pGroup->_first;
                    (*rEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
                    index = 1;
                    foreach (MDL0TextureNode n in _children)
                    {
                        if (n._decRefs.Count > 0)
                            ResourceEntry.Build(pGroup, index++, (byte*)pGroup + (rEntry++)->_dataOffset, (BRESString*)stringTable[n._name]);
                    }
                }
            }
            else
                foreach (MDL0EntryNode n in _children)
                {
                    dataAddress = (VoidPtr)pGroup + (rEntry++)->_dataOffset;
                    ResourceEntry.Build(pGroup, index++, dataAddress, (BRESString*)stringTable[n.Name]);
                    n.PostProcess(dataAddress, stringTable);
                }
        }

        private void PostProcessBone(MDL0EntryNode node, ResourceGroup* group, ref int index, StringTable stringTable)
        {
            VoidPtr dataAddress = (VoidPtr)group + (&group->_first)[index]._dataOffset;
            ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[node.Name]);
            node.PostProcess(dataAddress, stringTable);

            foreach (MDL0EntryNode n in node.Children)
                PostProcessBone(n, group, ref index, stringTable);
        }

        internal void Bind(GLContext ctx)
        {
            foreach (MDL0EntryNode e in Children)
                e.Bind(ctx);
        }
        internal void Unbind(GLContext ctx)
        {
            foreach (MDL0EntryNode e in Children)
                e.Unbind(ctx);
        }
    }
}
