using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;
using System.ComponentModel;
using BrawlLib.OpenGL;
using BrawlLib.Wii.Models;
using BrawlLib.Modeling;

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

        protected internal virtual void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable) { }
    }

    public unsafe class MDL0GroupNode : ResourceNode
    {
        internal ResourceGroup* Header { get { return (ResourceGroup*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MDL0Group; } }

        internal MDLResourceType _type;

        internal MDL0GroupNode(MDLResourceType type)
        {
            _type = type;
            _name = _type.ToString("g");
        }

        internal void GetStrings(StringTable table)
        {
            foreach (MDL0EntryNode n in Children)
                n.GetStrings(table);
        }

        public override void RemoveChild(ResourceNode child)
        {
            if ((_children != null) && (_children.Count == 1) && (_children.Contains(child)))
                _parent.RemoveChild(this);
            else
                base.RemoveChild(child);
        }

        internal void Parse(ModelLinker linker)
        {
            switch (_type)
            {
                case MDLResourceType.Bones:
                    if (linker.Bones == null)
                        break;
                    ExtractGroup(linker.Bones, typeof(MDL0BoneNode));

                    //Scatter bones
                    linker.BoneCache = _children.ToArray();
                    _children.Clear();
                    foreach (MDL0BoneNode bone in linker.BoneCache)
                    {
                        //Add to bone cache
                        linker.NodeCache[bone._nodeIndex] = new Influence(bone);

                        //Scatter. Keep the true followers! Exile the deserters!
                        if (bone._parent == this)
                            _children.Add(bone);
                        else
                            bone._parent._children.Add(bone);
                    }

                    //Create weight points
                    foreach (ResourcePair p in *linker.Defs)
                        if (p.Name == "NodeMix")
                        {
                            byte* pData = (byte*)p.Data;

                        Top:
                            switch (*pData)
                            {
                                case 3:
                                    int index = *(bushort*)(pData + 1);
                                    int count = pData[3];
                                    Influence nr = new Influence(count);
                                    MDL0NodeType3Entry* nEntry = (MDL0NodeType3Entry*)(pData + 4);

                                    linker.NodeCache[index] = nr;
                                    while (count-- > 0)
                                    {
                                        nr.Merge(linker.NodeCache[nEntry->_id], nEntry->_value);
                                        nEntry++;
                                    }

                                    pData = (byte*)nEntry;
                                    goto Top;

                                case 5:
                                    pData += 5;
                                    goto Top;
                            }
                        }

                    break;

                case MDLResourceType.Materials:
                    if (linker.Materials != null)
                        ExtractGroup(linker.Materials, typeof(MDL0MaterialNode));
                    break;

                case MDLResourceType.Shaders:
                    if (linker.Shaders != null)
                        ExtractGroup(linker.Shaders, typeof(MDL0ShaderNode));
                    break;

                case MDLResourceType.Polygons:
                    if (linker.Polygons == null)
                        break;
                    ExtractGroup(linker.Polygons, typeof(MDL0PolygonNode));

                    //Attach materials to polygons
                    List<ResourceNode> matList = ((MDL0Node)_parent)._matList;
                    MDL0PolygonNode poly;
                    MDL0MaterialNode mat;
                    foreach (ResourcePair p in *linker.Defs)
                        if ((p.Name == "DrawOpa") || (p.Name == "DrawXlu"))
                        {
                            byte* pData = (byte*)p.Data;
                            while (*pData++ == 4)
                            {
                                poly = _children[*(bushort*)(pData + 2)] as MDL0PolygonNode;
                                mat = matList[*(bushort*)pData] as MDL0MaterialNode;
                                poly._material = mat;
                                mat._polygons.Add(poly);
                                pData += 7;
                            }
                        }
                    break;
            }
        }

        //protected override bool OnInitialize()
        //{
        //    _children = new List<ResourceNode>(Header->_numEntries);

        //    //Pull out members
        //    ExtractGroup();

        //    ModelLinker linker = ((MDL0Node)_parent)._linker;

        //    //Type-specific processing
        //    switch (_type)
        //    {
        //        case MDLResourceType.Bones:

        //            //Scatter bones
        //            linker.BoneCache = _children.ToArray();
        //            _children.Clear();
        //            foreach (MDL0BoneNode bone in linker.BoneCache)
        //            {
        //                //Add to bone cache
        //                linker.NodeCache[bone._nodeIndex] = new Influence(bone);

        //                //Scatter. Keep the true followers! Exile the deserters!
        //                if (bone._parent == this)
        //                    _children.Add(bone);
        //                else
        //                    bone._parent._children.Add(bone);
        //            }

        //            //Create weight points
        //            foreach (ResourcePair p in *linker.Defs)
        //                if (p.Name == "NodeMix")
        //                {
        //                    byte* pData = (byte*)p.Data;

        //                Top:
        //                    switch (*pData)
        //                    {
        //                        case 3:
        //                            int index = *(bushort*)(pData + 1);
        //                            int count = pData[3];
        //                            Influence nr = new Influence(count);
        //                            MDL0NodeType3Entry* nEntry = (MDL0NodeType3Entry*)(pData + 4);

        //                            linker.NodeCache[index] = nr;
        //                            while (count-- > 0)
        //                            {
        //                                nr.Merge(linker.NodeCache[nEntry->_id], nEntry->_value);
        //                                nEntry++;
        //                            }

        //                            pData = (byte*)nEntry;
        //                            goto Top;

        //                        case 5:
        //                            pData += 5;
        //                            goto Top;
        //                    }
        //                }

        //            break;

        //        case MDLResourceType.Materials:
        //            break;

        //        case MDLResourceType.Shaders:
        //            break;

        //        case MDLResourceType.Polygons:

        //            //Attach materials to polygons
        //            List<ResourceNode> matList = ((MDL0Node)_parent)._matList;
        //            MDL0PolygonNode poly;
        //            MDL0MaterialNode mat;
        //            foreach (ResourcePair p in *linker.Defs)
        //                if ((p.Name == "DrawOpa") || (p.Name == "DrawXlu"))
        //                {
        //                    byte* pData = (byte*)p.Data;
        //                    while (*pData++ == 4)
        //                    {
        //                        poly = _children[*(bushort*)(pData + 2)] as MDL0PolygonNode;
        //                        mat = matList[*(bushort*)pData] as MDL0MaterialNode;
        //                        poly._material = mat;
        //                        mat._polygons.Add(poly);
        //                        pData += 7;
        //                    }
        //                }
        //            break;
        //    }

        //    //Return true so we keep the list
        //    return true;
        //}

        private void ExtractGroup(ResourceGroup* pGroup, Type t)
        {
            bool useCache = t == typeof(MDL0ShaderNode);

            MDL0CommonHeader* pHeader;
            ResourceNode node;
            int* offsetCache = stackalloc int[128];
            int offsetCount = 0, offset, x;

            foreach (ResourcePair p in *pGroup)
            {
                offset = (int)p.Data;
                if (useCache)
                {
                    //search for entry within offset cache
                    for (x = 0; (x < offsetCount) && (offsetCache[x] != offset); x++) ;
                    //If found, skip
                    if (x < offsetCount)
                        continue;
                    //Store offset
                    offsetCache[offsetCount++] = offset;
                }

                pHeader = (MDL0CommonHeader*)p.Data;
                node = Activator.CreateInstance(t) as ResourceNode;

                node.Initialize(this, pHeader, pHeader->_size);
            }
        }

        protected internal virtual void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            ResourceGroup* pGroup = (ResourceGroup*)dataAddress;
            ResourceEntry* rEntry = &pGroup->_first;
            int index = 1;
            (*rEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

            if (_name == "Bones")
                foreach (MDL0EntryNode n in _children)
                    PostProcessBone(mdlAddress, n, pGroup, ref index, stringTable);
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
                    n.PostProcess(mdlAddress, dataAddress, stringTable);
                }
        }

        private void PostProcessBone(VoidPtr mdlAddress, MDL0EntryNode node, ResourceGroup* group, ref int index, StringTable stringTable)
        {
            VoidPtr dataAddress = (VoidPtr)group + (&group->_first)[index]._dataOffset;
            ResourceEntry.Build(group, index++, dataAddress, (BRESString*)stringTable[node.Name]);
            node.PostProcess(mdlAddress, dataAddress, stringTable);

            foreach (MDL0EntryNode n in node.Children)
                PostProcessBone(mdlAddress, n, group, ref index, stringTable);
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
