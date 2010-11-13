using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;
using System.Collections;
using BrawlLib.Imaging;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelEncoder
    {
        //This assumes influence list has already been cleaned
        public static void AssignNodeIndices(ModelLinker linker)
        {
            MDL0Node model = linker.Model;
            int index = 0;
            int count = model._influences._influences.Count + linker.BoneCache.Length;

            linker._nodeCount = count;
            linker.NodeCache = new IMatrixNode[count];

            //Add referenced primaries
            foreach (MDL0BoneNode bone in linker.BoneCache)
            {
                if (bone._refCount > 0)
                    linker.NodeCache[bone._nodeIndex = index++] = bone;
                else
                    bone._nodeIndex = -1;
                bone._weightCount = 0;
            }

            //Add weight groups
            foreach (Influence i in model._influences._influences)
            {
                linker.NodeCache[i._index = index++] = i;
                foreach (BoneWeight b in i._weights)
                    b.Bone._weightCount++;
            }

            //Add remaining bones
            foreach (MDL0BoneNode bone in linker.BoneCache)
                if (bone._nodeIndex == -1)
                    linker.NodeCache[bone._nodeIndex = index++] = bone;
        }

        public static int CalcSize(ModelLinker linker)
        {
            MDL0Node model = linker.Model;
            int headerLen, groupLen = 0x18, tableLen = 0, texLen = 0, dataLen = 0;
            int treeLen = 0, mixLen = 0, opaLen = 0, xluLen = 0;
            int texCount = 0, decCount = 0;
            int aInd, aLen, temp;

            //Get header length
            switch (linker.Version)
            {
                case 0x09: headerLen = 0x80; break;
                default: headerLen = 0; break;
            }

            //Assign node indices
            AssignNodeIndices(linker);

            //Get table length
            tableLen = (linker._nodeCount + 1) << 2;

            //Get group/data length
            List<MDLResourceType> iList = ModelLinker.IndexBank[linker.Version];
            foreach (MDLResourceType resType in iList)
            {
                IEnumerable entryList = null;
                int entries = 0;

                //entries = 0;
                switch (resType)
                {
                    case MDLResourceType.Defs:
                        {
                            //NodeTree
                            treeLen = linker.BoneCache.Length * 5;

                            //NodeMix
                            foreach (IMatrixNode i in linker.NodeCache)
                                if (!i.IsPrimaryNode)
                                    mixLen += ((Influence)i)._weights.Length * 8 + 4;
                                else if (i.ReferenceCount > 0)
                                    mixLen += 5;

                            //DrawOpa, DrawXlu
                            //Get assigned materials and categorize
                            foreach (MDL0PolygonNode poly in linker.Model.PolygonList)
                            {
                                if (poly._material == null)
                                    continue;

                                //Is material transparent?
                                if (poly._material.EnableBlend)
                                    opaLen += 8;
                                else
                                    xluLen += 8;
                            }

                            //Add terminate byte and set model def flags
                            if (linker.Model._hasTree = (treeLen > 0))
                            { treeLen++; entries++; }
                            if (linker.Model._hasMix = (mixLen > 0))
                            { mixLen++; entries++; }
                            if (linker.Model._hasOpa = (opaLen > 0))
                            { opaLen++; entries++; }
                            if (linker.Model._hasXlu = (xluLen > 0))
                            { xluLen++; entries++; }

                            //Align data
                            dataLen += (treeLen + mixLen + opaLen + xluLen).Align(4);

                            break;
                        }

                    case MDLResourceType.Vertices:
                        aInd = 0;
                        aLen = 1;

                    EvalAssets:

                        List<ResourceNode> polyList = linker.Model._polyList;
                        if (polyList == null)
                            break;

                        //What about merging?

                        //Create asset lists
                        IList aList;
                        switch (aInd)
                        {
                            case 0: aList = linker._vertices = new List<VertexCodec>(polyList.Count); break;
                            case 1: aList = linker._normals = new List<VertexCodec>(polyList.Count); break;
                            case 2: aList = linker._colors = new List<ColorCodec>(polyList.Count); break;
                            default: aList = linker._uvs = new List<VertexCodec>(polyList.Count); break;
                        }

                        aLen += aInd;
                        temp = polyList.Count;
                        for (int i = 0; i < temp; i++)
                        {
                            MDL0PolygonNode p = polyList[i] as MDL0PolygonNode;
                            //object c;
                            for (int x = aInd; x < aLen; x++)
                                if (p._manager._faceData[x] != null)
                                {
                                    p._elementIndices[x] = (short)aList.Count;
                                    switch (aInd)
                                    {
                                        case 0:
                                            VertexCodec vert;
                                            aList.Add(vert = new VertexCodec(p._manager.RawPoints, true));
                                            dataLen += vert._dataLen.Align(0x20) + 0x40;
                                            break;

                                        case 1:
                                            aList.Add(vert = new VertexCodec((Vector3*)p._manager._faceData[x].Address, p._manager._pointCount, false));
                                            dataLen += vert._dataLen.Align(0x20) + 0x20;
                                            break;

                                        case 2:
                                            ColorCodec col;
                                            aList.Add(col = new ColorCodec((RGBAPixel*)p._manager._faceData[x].Address, p._manager._pointCount));
                                            dataLen += col._dataLen.Align(0x20) + 0x20;
                                            break;

                                        default:
                                            aList.Add(vert = new VertexCodec((Vector2*)p._manager._faceData[x].Address, p._manager._pointCount));
                                            dataLen += vert._dataLen.Align(0x20) + 0x40;
                                            break;
                                    }
                                }
                                else
                                    p._elementIndices[x] = -1;
                        }
                        entries = aList.Count;

                        break;

                    case MDLResourceType.Normals:
                        aInd = 1;
                        aLen = 1;
                        goto EvalAssets;

                    case MDLResourceType.Colors:
                        aInd = 2;
                        aLen = 2;
                        goto EvalAssets;

                    case MDLResourceType.UVs:
                        aInd = 4;
                        aLen = 8;
                        goto EvalAssets;

                    case MDLResourceType.Bones: entryList = linker.BoneCache; break;
                    case MDLResourceType.Materials: entryList = linker.Model._matList; break;
                    case MDLResourceType.Polygons: entryList = linker.Model._polyList; break;
                    case MDLResourceType.Shaders:
                        if ((entryList = linker.Model._shadList) != null)
                            entries = linker.Model._shadList.Count;
                        break;

                    case MDLResourceType.Textures:
                        TextureRef[] tex = linker.Model._textures.GetTextures();
                        entries = tex.Length;

                        foreach (TextureRef t in tex)
                        { texCount++; texLen += t._texRefs.Count * 8 + 4; }

                        break;

                    case MDLResourceType.Decals:
                        TextureRef[] dec = linker.Model._textures.GetDecals();
                        entries = dec.Length;

                        foreach (TextureRef t in dec)
                        { decCount++; texLen += t._decRefs.Count * 8 + 4; }

                        break;

                }

                if (entryList != null)
                {
                    int index = 0;
                    foreach (MDL0EntryNode e in entryList)
                    {
                        e._entryIndex = index++;
                        dataLen += e.CalculateSize(true);
                    }
                    if (entries == 0)
                        entries = index;
                }

                if (entries > 0)
                    groupLen += entries * 0x10 + 0x18;

            }

            linker._headerLen = headerLen;
            linker._tableLen = tableLen;
            linker._groupLen = groupLen;
            linker._texLen = texLen;
            linker._texCount = texCount;
            linker._decCount = decCount;
            linker._dataLen = dataLen;

            return headerLen + tableLen + groupLen + texLen + dataLen;
        }

        internal static unsafe void Build(ModelLinker linker, MDL0Header* header, int length, bool force)
        {
            int vertices = 0, faces = 0;
            Vector3 min = new Vector3(), max = new Vector3();

            MDL0Props* props;
            byte* groupAddr = (byte*)header + linker._headerLen + linker._tableLen;
            byte* dataAddr = groupAddr + linker._groupLen + linker._texLen;

            linker.Header = header;

            *header = new MDL0Header(length, linker.Version);
            props = header->Properties;

            //Write node table, assign node ids
            WriteNodeTable(linker);

            //Write def table
            WriteDefs(linker, ref groupAddr, ref dataAddr);

            //Write groups
            linker.Write(ref groupAddr, ref dataAddr, force);

            //Write assets
            WriteAssets(linker, ref groupAddr, ref dataAddr);

            //Write textures
            WriteTextures(linker, ref groupAddr);

            //Store group offsets
            linker.Finish();

            //Set props
            *props = new MDL0Props(vertices, faces, linker._nodeCount, linker.Model._unk1, linker.Model._unk2, linker.Model._unk3, linker.Model._unk4, linker.Model._unk5, min, max);
        }

        private static void WriteNodeTable(ModelLinker linker)
        {
            bint* ptr = (bint*)((byte*)linker.Header + linker._headerLen);
            int len = linker._nodeCount;
            int i = 0;

            //Set length
            *ptr++ = len;

            //Write indices
            while (i < len)
            {
                IMatrixNode n = linker.NodeCache[i++];
                if (n.IsPrimaryNode)
                    *ptr++ = ((MDL0BoneNode)n)._entryIndex;
                else
                    *ptr++ = -1;
            }
        }

        private static void WriteDefs(ModelLinker linker, ref byte* pGroup, ref byte* pData)
        {
            MDL0Node mdl = linker.Model;

            //This should never happen!
            if (!mdl._hasMix && !mdl._hasOpa && !mdl._hasTree && !mdl._hasXlu)
                return;

            IList polyList = mdl._polyList;
            MDL0PolygonNode poly;
            MDL0MaterialNode mat;
            int polyCount = polyList.Count;
            int entryCount = 0;
            byte* floor = pData;
            int dataLen;

            ResourceGroup* group = (ResourceGroup*)pGroup;
            ResourceEntry* entry = &group->_first + 1;

            //Set def offset
            linker.Defs = group;

            //NodeTree
            if (mdl._hasTree)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                int bCount = linker.BoneCache.Length;
                for (int i = 0; i < bCount; i++)
                {
                    MDL0BoneNode bone = linker.BoneCache[i] as MDL0BoneNode;

                    *pData = 2; //Entry tag
                    *(bushort*)(pData + 1) = (ushort)bone._entryIndex;
                    if (bone._parent is MDL0BoneNode)
                        *(bushort*)(pData + 3) = (ushort)((MDL0BoneNode)bone._parent)._nodeIndex;
                    else
                        *(bushort*)(pData + 3) = 0;

                    pData += 5; //Advance
                }
                *pData++ = 1; //Terminate
            }

            //NodeMix
            //Only weight references go here.
            //First list bones used by weight groups, in bone order
            //Then list weight groups that use bones. Ordered by entry count.
            if (mdl._hasMix)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                //Add bones first (using flat bone list)
                foreach (MDL0BoneNode b in linker.BoneCache)
                    if (b._weightCount > 0)
                    {
                        *pData = 5; //Tag
                        *(bushort*)(pData + 1) = (ushort)b._nodeIndex;
                        *(bushort*)(pData + 3) = (ushort)b._entryIndex;
                        pData += 5; //Advance
                    }

                //Add weight groups (using sorted influence list)
                foreach (Influence i in mdl._influences._influences)
                {
                    *pData = 3; //Tag
                    *(bushort*)&pData[1] = (ushort)i._index;
                    pData[3] = (byte)i._weights.Length;
                    pData += 4; //Advance
                    foreach (BoneWeight w in i._weights)
                    {
                        *(bushort*)pData = (ushort)w.Bone._nodeIndex;
                        *(bfloat*)(pData + 2) = w.Weight;
                        pData += 6; //Advance
                    }
                }

                *pData++ = 1; //Terminate
            }

            //DrawOpa
            if (mdl._hasOpa)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                for (int i = 0; i < polyCount; i++)
                {
                    poly = polyList[i] as MDL0PolygonNode;
                    if (((mat = poly._material) != null) && (!mat._blendMode.EnableBlend))
                    {
                        *pData = 4; //Tag
                        *(bushort*)(pData + 1) = (ushort)mat._entryIndex;
                        *(bushort*)(pData + 3) = (ushort)poly._entryIndex;
                        *(bushort*)(pData + 5) = 0; //Bone index?
                        pData[7] = 0;
                        pData += 8; //Advance
                    }
                }

                *pData++ = 1; //Terminate
            }

            //DrawXlu
            if (mdl._hasOpa)
            {
                //Write group entry
                entry[entryCount++]._dataOffset = (int)(pData - pGroup);

                for (int i = 0; i < polyCount; i++)
                {
                    poly = polyList[i] as MDL0PolygonNode;
                    if (((mat = poly._material) != null) && (mat._blendMode.EnableBlend))
                    {
                        *pData = 4; //Tag
                        *(bushort*)(pData + 1) = (ushort)mat._entryIndex;
                        *(bushort*)(pData + 3) = (ushort)poly._entryIndex;
                        *(bushort*)(pData + 5) = 0; //Bone index?
                        pData[7] = 0;
                        pData += 8; //Advance
                    }
                }
                *pData++ = 1; //Terminate
            }

            //Align data
            dataLen = (int)(pData - floor);
            while ((dataLen++ & 3) != 0)
                *pData++ = 0;

            //Set header
            *group = new ResourceGroup(entryCount);

            //Advance group poiner
            pGroup += group->_totalSize;
        }

        private static void WriteAssets(ModelLinker linker, ref byte* pGroup, ref byte* pData)
        {
            ResourceGroup* group;
            ResourceEntry* entry;
            int index;

            if (linker._vertices != null)
            {
                linker.Vertices = group = (ResourceGroup*)pGroup;
                *group = new ResourceGroup(linker._vertices.Count);
                entry = &group->_first + 1;

                index = 0;
                foreach (VertexCodec c in linker._vertices)
                {
                    (entry++)->_dataOffset = (int)(pData - pGroup);

                    MDL0VertexData* header = (MDL0VertexData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x40;
                    header->_dataOffset = 0x40;
                    header->_index = index;
                    header->_isXYZ = c._hasZ ? 1 : 0;
                    header->_type = (int)c._type;
                    header->_divisor = (byte)c._scale;
                    header->_entryStride = (byte)c._dstStride;
                    header->_numVertices = (short)c._dstCount;
                    header->_eMin = c._min;
                    header->_eMax = c._max;
                    header->_pad1 = header->_pad2 = 0;

                    c.Write(pData + 0x40);
                    pData += header->_dataLen;
                }

                pGroup += group->_totalSize;
            }

            if (linker._normals != null)
            {
                linker.Normals = group = (ResourceGroup*)pGroup;
                *group = new ResourceGroup(linker._normals.Count);
                entry = &group->_first + 1;

                index = 0;
                foreach (VertexCodec c in linker._normals)
                {
                    (entry++)->_dataOffset = (int)(pData - pGroup);

                    MDL0NormalData* header = (MDL0NormalData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x20;
                    header->_dataOffset = 0x20;
                    header->_index = index;
                    header->_isNBT = 0;
                    header->_type = (int)c._type;
                    header->_divisor = (byte)c._scale;
                    header->_entryStride = (byte)c._dstStride;
                    header->_numVertices = (ushort)c._dstCount;

                    c.Write(pData + 0x20);
                    pData += header->_dataLen;
                }

                pGroup += group->_totalSize;
            }

            if (linker._colors != null)
            {
                linker.Colors = group = (ResourceGroup*)pGroup;
                *group = new ResourceGroup(linker._colors.Count);
                entry = &group->_first + 1;

                index = 0;
                foreach (ColorCodec c in linker._colors)
                {
                    (entry++)->_dataOffset = (int)(pData - pGroup);

                    MDL0ColorData* header = (MDL0ColorData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x20;
                    header->_dataOffset = 0x20;
                    header->_index = index;
                    header->_isRGBA = c._hasAlpha ? 1 : 0;
                    header->_format = (int)c._outType;
                    header->_entryStride = (byte)c._dstStride;
                    header->_scale = 0;
                    header->_numEntries = (ushort)c._dstCount;

                    c.Write(pData + 0x20);
                    pData += header->_dataLen;
                }
                pGroup += group->_totalSize;
            }

            if (linker._uvs != null)
            {
                linker.UVs = group = (ResourceGroup*)pGroup;
                *group = new ResourceGroup(linker._uvs.Count);
                entry = &group->_first + 1;

                index = 0;
                foreach (VertexCodec c in linker._uvs)
                {
                    (entry++)->_dataOffset = (int)(pData - pGroup);

                    MDL0UVData* header = (MDL0UVData*)pData;
                    header->_dataLen = c._dataLen.Align(0x20) + 0x40;
                    header->_dataOffset = 0x40;
                    header->_index = index;
                    header->_format = (int)c._type;
                    header->_divisor = (byte)c._scale;
                    header->_isST = 1;
                    header->_entryStride = (byte)c._dstStride;
                    header->_numEntries = (ushort)c._dstCount;
                    header->_min = (Vector2)c._min;
                    header->_max = (Vector2)c._max;
                    header->_pad1 = header->_pad2 = header->_pad3 = header->_pad4 = 0;

                    c.Write(pData + 0x40);
                    pData += header->_dataLen;
                }

                pGroup += group->_totalSize;
            }
        }

        //Do this after materials, before textures
        //private static void WriteShaders(ModelLinker linker, bool force, ref byte* pGroup, ref byte* pData)
        //{
        //    MDL0Material* mHeader;
        //    MDL0GroupNode mats = linker.Groups[(int)MDLResourceType.Materials];
        //    MDL0GroupNode shaders = linker.Groups[(int)MDLResourceType.Shaders];
        //    ResourceGroup* pGrp = (ResourceGroup*)pGroup;
        //    ResourceEntry* pEntry = &pGrp->_first + 1;
        //    int num;

        //    if (mats == null || shaders == null)
        //        return;

        //    *pGrp = new ResourceGroup(mats._children.Count);
        //    linker.Shaders = pGrp;

        //    //Write data
        //    foreach (MDL0ShaderNode s in shaders._children)
        //    {
        //        num = s._calcSize;
        //        s.Rebuild(pData, num, force);
        //        pData += num;
        //    }

        //    //Create headers, one for each material
        //    //Also link materials to shader using offset
        //    foreach (MDL0MaterialNode mat in mats._children)
        //    {
        //        mHeader = mat.Header;
        //        num = (int)mat._shader.Header;
        //        mHeader->_shaderOffset = num - (int)mHeader;
        //        (pEntry++)->_dataOffset = num - (int)pGrp;
        //    }

        //    //Advance group
        //    pGroup += pGrp->_totalSize;
        //}

        //Materials must already be written. Do this last!
        private static void WriteTextures(ModelLinker linker, ref byte* pGroup)
        {
            MDL0GroupNode texGrp = linker.Groups[(int)MDLResourceType.Textures];

            if (texGrp == null)
                return;

            ResourceGroup* pTexGroup = null;
            ResourceEntry* pTexEntry = null;
            if (linker._texCount > 0)
            {
                linker.Textures = pTexGroup = (ResourceGroup*)pGroup;
                *pTexGroup = new ResourceGroup(linker._texCount);

                pTexEntry = &pTexGroup->_first + 1;
                pGroup += pTexGroup->_totalSize;
            }

            ResourceGroup* pDecGroup = null;
            ResourceEntry* pDecEntry = null;
            if (linker._decCount > 0)
            {
                linker.Decals = pDecGroup = (ResourceGroup*)pGroup;
                *pDecGroup = new ResourceGroup(linker._decCount);
                pDecEntry = &pDecGroup->_first + 1;
                pGroup += pDecGroup->_totalSize;
            }

            bint* pData = (bint*)pGroup;
            int offset;

            //Textures
            if (pTexGroup != null)
                foreach (MDL0TextureNode t in texGrp._children)
                    if (t._texRefs.Count > 0)
                    {
                        offset = (int)pData;
                        (pTexEntry++)->_dataOffset = offset - (int)pTexGroup;
                        *pData++ = t._texRefs.Count;
                        foreach (MDL0MaterialRefNode mat in t._texRefs)
                        {
                            *pData++ = (int)mat._parent.WorkingUncompressed.Address - offset;
                            *pData++ = (int)mat.WorkingUncompressed.Address - offset;
                        }
                    }

            //Decals
            if (pDecGroup != null)
                foreach (MDL0TextureNode t in texGrp._children)
                    if (t._decRefs.Count > 0)
                    {
                        offset = (int)pData;
                        (pDecEntry++)->_dataOffset = offset - (int)pDecGroup;
                        *pData++ = t._decRefs.Count;
                        foreach (MDL0MaterialRefNode mat in t._decRefs)
                        {
                            *pData++ = (int)mat._parent.WorkingUncompressed.Address - offset;
                            *pData++ = (int)mat.WorkingUncompressed.Address - offset;
                        }
                    }
        }
    }
}
