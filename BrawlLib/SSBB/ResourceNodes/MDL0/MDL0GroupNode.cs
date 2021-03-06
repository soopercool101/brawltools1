﻿using System;
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

        internal void Parse(MDL0Node model)
        {
            Influence inf;
            ModelLinker linker = model._linker;
            switch (_type)
            {
                case MDLResourceType.Bones:
                    //Break if there are no bones defined
                    if (linker.Bones == null)
                        break;

                    //Parse bones from raw data (flat list).
                    //Bones re-assign parents in their Initialize block, so parents are true.
                    ExtractGroup(linker.Bones, typeof(MDL0BoneNode));
 
                    //Cache flat list
                    linker.BoneCache = _children.ToArray();

                    //Reset children so we can rebuild
                    _children.Clear();

                    //Populate node cache
                    MDL0BoneNode bone = null;
                    int index;
                    int count = linker.BoneCache.Length;
                    for (int i = 0; i < count; i++)
                    {
                        //bone = linker.BoneCache[i] as MDL0BoneNode;
                        linker.NodeCache[(bone = linker.BoneCache[i] as MDL0BoneNode)._nodeIndex] = bone;
                    }

                    //Now that bones and primary influences have been cached, we can create weighted influences.

                    foreach (ResourcePair p in *linker.Defs)
                        if (p.Name == "NodeTree")
                        {
                            //Use node tree to rebuild bone heirarchy
                            byte* pData = (byte*)p.Data;

                        Top:
                            if (*pData == 2)
                            {
                                bone = linker.BoneCache[*(bushort*)(pData + 1)] as MDL0BoneNode;
                                index = *(bushort*)(pData + 3);

                                if (bone.Header->_parentOffset == 0)
                                    _children.Add(bone);
                                else
                                    (bone._parent = linker.NodeCache[index] as ResourceNode)._children.Add(bone);

                                pData += 5;
                                goto Top;
                            }
                        }
                        else if (p.Name == "NodeMix")
                        {
                            //Use node mix to create weight groups
                            byte* pData = (byte*)p.Data;

                        Top:
                            switch (*pData)
                            {
                                    //Type 3 is for weighted influences
                                case 3:
                                    //Get index/count fields
                                    index = *(bushort*)(pData + 1);
                                    count = pData[3];
                                    //Get data pointer (offset of 4)
                                    MDL0NodeType3Entry* nEntry = (MDL0NodeType3Entry*)(pData + 4);

                                    //Create influence with specified count
                                    inf = new Influence(count);
                                    //Iterate through weights, adding each to the influence
                                    //Here, we are referring back to the NodeCache to grab the bone.
                                    //Note that the weights do not reference other influences, only bones. There is a good reason for this.
                                    for (int i = 0; i < count; i++, nEntry++)
                                        inf._weights[i] = new BoneWeight(linker.NodeCache[nEntry->_id] as MDL0BoneNode, nEntry->_value);

                                    //Add influence to model object, while adding it to the cache.
                                    linker.NodeCache[index] = model._influences.AddOrCreate(inf);

                                    //Move data pointer to next entry
                                    pData = (byte*)nEntry;
                                    goto Top;

                                    //Type 5 is for primary influences
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
                    //Break if no polygons defined
                    if (linker.Polygons == null)
                        break;

                    //Extract
                    ExtractGroup(linker.Polygons, typeof(MDL0PolygonNode));

                    //Attach materials to polygons.
                    //This assumes that materials have already been parsed.

                    List<ResourceNode> matList = ((MDL0Node)_parent)._matList;
                    MDL0PolygonNode poly;
                    MDL0MaterialNode mat;

                    //Find DrawOpa or DrawXlu entry in Definition list
                    foreach (ResourcePair p in *linker.Defs)
                        if ((p.Name == "DrawOpa") || (p.Name == "DrawXlu"))
                        {
                            byte* pData = (byte*)p.Data;
                            while (*pData++ == 4)
                            {
                                //Get polygon from index
                                poly = _children[*(bushort*)(pData + 2)] as MDL0PolygonNode;
                                //Get material from index
                                mat = matList[*(bushort*)pData] as MDL0MaterialNode;
                                //Assign material to polygon
                                poly._material = mat;
                                //Add polygon to material reference list
                                mat._polygons.Add(poly);
                                //Increment pointer
                                pData += 7;
                            }
                        }
                    break;
            }
        }

        //Extracts resources from a group, using the specified type
        private void ExtractGroup(ResourceGroup* pGroup, Type t)
        {
            //If using shaders, cache results instead of unique entries
            //This is because shaders can appear multiple times, but with different names
            bool useCache = t == typeof(MDL0ShaderNode);

            MDL0CommonHeader* pHeader;
            ResourceNode node;
            int* offsetCache = stackalloc int[128];
            int offsetCount = 0, offset, x;

            foreach (ResourcePair p in *pGroup)
            {
                //Get data offset
                offset = (int)p.Data;
                if (useCache)
                {
                    //search for entry within offset cache
                    for (x = 0; (x < offsetCount) && (offsetCache[x] != offset); x++) ;
                    //If found, skip to next entry
                    if (x < offsetCount)
                        continue;
                    //Otherwise, store offset
                    offsetCache[offsetCount++] = offset;
                }

                //Create resource instance
                pHeader = (MDL0CommonHeader*)p.Data;
                node = Activator.CreateInstance(t) as ResourceNode;

                //Initialize
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
