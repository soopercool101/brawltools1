using System;
using System.Collections.Generic;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelEncoder
    {
        public static int CalcSize(ModelLinker linker, bool force)
        {
            MDL0GroupNode group;
            int headerLen, groupLen = 0x18, tableLen = 0, texLen = 0, dataLen = 0;
            int treeLen = 0, mixLen = 0, opaLen = 0, xluLen = 0;
            int texCount = 0, decCount = 0;
            int entries = 0;
            int index;

            switch (linker.Version)
            {
                case 0x09: headerLen = 0x80; break;
                default: headerLen = 0; break;
            }

            tableLen = linker.Model._nodes.Length * 4 + 4;

            for (int i = 0; i < ModelLinker.BankLen; i++)
            {
                if ((group = linker.Groups[i]) == null)
                    continue;

                entries = group.Children.Count;
                switch ((MDLResourceType)i)
                {
                    case MDLResourceType.Bones:

                        entries = linker.BoneCache.Length;
                        treeLen = entries * 5 + 1;

                        index = 0;
                        foreach (MDL0EntryNode c in linker.BoneCache)
                        {
                            c._entryIndex = index++;
                            dataLen += c.CalculateSize(force);
                        }

                      //  goto default;
                        break;

                    case MDLResourceType.Textures:

                        foreach (MDL0TextureNode tex in group.Children)
                        {
                            if (tex._texRefs.Count > 0)
                            { texCount++; texLen += tex._texRefs.Count * 8 + 4; }
                            if (tex._decRefs.Count > 0)
                            { decCount++; texLen += tex._decRefs.Count * 8 + 4; }
                        }
                        entries = texCount;
                        if (decCount > 0)
                            groupLen += 0x18 + (decCount * 0x10);

                        break;

                    case MDLResourceType.Shaders:
                        entries = linker.Groups[(int)MDLResourceType.Materials]._children.Count;
                        goto default;

                    case MDLResourceType.Polygons:

                        foreach (MDL0PolygonNode poly in group.Children)
                        {
                            if (poly._material != null)
                                if (poly._material._unk1 != 0) //When do we use Xlu?
                                    xluLen += 8;
                                else
                                    opaLen += 8;
                            if (poly._singleBind != null)
                                mixLen = 1;
                        }

                        //Add terminate byte
                        if (opaLen > 0) opaLen++;
                        if (xluLen > 0) xluLen++;

                        goto default;

                    default:

                        index = 0;
                        foreach (MDL0EntryNode c in group._children)
                        {
                            c._entryIndex = index++;
                            dataLen += c.CalculateSize(force);
                        }
                        break;

                }

                if (entries > 0)
                    groupLen += 0x18 + (entries * 0x10);
            }

            if (mixLen != 0)
            {
                mixLen = 0;

                foreach (IMatrixProvider m in linker.Model._nodes)
                {
                    if (m is MDL0BoneNode)
                        mixLen += 5;
                    else
                        mixLen += ((m as NodeRef)._entries.Count * 6) + 4;
                }
            }

            if (linker.Model._hasTree = (treeLen > 0)) groupLen += 0x10;
            if (linker.Model._hasMix = (mixLen > 0)) groupLen += 0x10;
            if (linker.Model._hasOpa = (opaLen > 0)) groupLen += 0x10;
            if (linker.Model._hasXlu = (xluLen > 0)) groupLen += 0x10;

            dataLen += (treeLen + mixLen + opaLen + xluLen).Align(4);

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
            int nodes = linker.Model._nodes.Length;
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

            //Write textures
            WriteTextures(linker, ref groupAddr);

            //Store group offsets
            linker.Finish();

            //Set props
            *props = new MDL0Props(vertices, faces, nodes, linker.Model._unk1, linker.Model._unk2, linker.Model._unk3, linker.Model._unk4, linker.Model._unk5, min, max);
        }

        private static void WriteNodeTable(ModelLinker linker)
        {
            bint* ptr = (bint*)((byte*)linker.Header + linker._headerLen);
            IMatrixProvider[] nodes = linker.Model._nodes;
            int len = nodes.Length;

            *ptr++ = len;

            //Write table and assign ids to bones
            for (int i = 0; i < len; i++)
            {
                if (nodes[i] is MDL0BoneNode)
                {
                    MDL0BoneNode bone = nodes[i] as MDL0BoneNode;
                    bone._nodeIndex = i;
                    *ptr++ = bone._entryIndex;
                }
                else
                    *ptr++ = -1;
            }
        }

        private static void WriteDefs(ModelLinker linker, ref byte* pGroup, ref byte* pData)
        {
            MDL0GroupNode polyGroup = linker.Groups[(int)MDLResourceType.Polygons];
            MDL0MaterialNode mat;
            ResourceGroup* pGrp = (ResourceGroup*)pGroup;
            ResourceEntry* pEntry = &pGrp->_first + 1;
            int index = 0;
            int count = 0;

            linker.Defs = pGrp;

            //NodeTree
            if (linker.Model._hasTree)
            {
                (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                count++;

                foreach (MDL0BoneNode bone in linker.BoneCache)
                {
                    *pData = 2;
                    *(bushort*)(pData + 1) = (ushort)bone._entryIndex;
                    if (bone._parent is MDL0BoneNode)
                        *(bushort*)(pData + 3) = (ushort)((MDL0BoneNode)bone._parent)._nodeIndex;
                    else
                        *(bushort*)(pData + 3) = 0;

                    pData += 5;
                }
                *pData++ = 1;
            }

            //NodeMix
            if (linker.Model._hasMix)
            {
                (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                count++;

                index = 0;
                foreach (object o in linker.Model._nodes)
                {
                    if (o is MDL0BoneNode)
                    {
                        *pData = 5;
                        *(bushort*)(pData + 1) = (ushort)(index++);
                        *(bushort*)(pData + 3) = (ushort)((MDL0BoneNode)o)._entryIndex;
                        pData += 5;
                    }
                    else
                    {
                        NodeRef nr = (NodeRef)o;
                        *pData = 3;
                        *(bushort*)&pData[1] = (ushort)(index++);
                        pData[3] = (byte)nr._entries.Count;
                        pData += 4;
                        foreach (NodeWeight w in nr._entries)
                        {
                            *(bushort*)pData = (ushort)w.Node._nodeIndex;
                            *(bfloat*)(pData + 2) = w.Weight;
                            pData += 6;
                        }
                    }
                }
                *pData++ = 1;
            }

            //DrawOpa
            if (linker.Model._hasOpa)
            {
                (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                count++;

                foreach (MDL0PolygonNode poly in polyGroup._children)
                    if (((mat = poly._material) != null) && (mat._unk1 == 0))
                    {
                        *pData = 4;
                        *(bushort*)(pData + 1) = (ushort)mat._entryIndex;
                        *(bushort*)(pData + 3) = (ushort)poly._entryIndex;
                        *(bushort*)(pData + 5) = 0; //Bone index?
                        pData[7] = 0;
                        pData += 8;
                    }
                *pData++ = 1;
            }

            //DrawXlu
            if (linker.Model._hasXlu)
            {
                (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                count++;

                foreach (MDL0PolygonNode poly in polyGroup._children)
                    if (((mat = poly._material) != null) && (mat._unk1 != 0))
                    {
                        *pData = 4;
                        *(bushort*)(pData + 1) = (ushort)mat._entryIndex;
                        *(bushort*)(pData + 3) = (ushort)poly._entryIndex;
                        *(bushort*)(pData + 5) = 0; //Bone index?
                        pData[7] = 0;
                        pData += 8;
                    }
                *pData++ = 1;
            }

            //Align data?
            while (((uint)pData & 3) != 0)
                *pData++ = 0;

            //Set header
            *pGrp = new ResourceGroup(count);

            //Advance group poiner
            pGroup += pGrp->_totalSize;
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
