using System;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;
using System.Collections.Generic;
using BrawlLib.Modeling;

namespace BrawlLib.Wii.Models
{
    public static unsafe class ModelDecoder
    {
        //public static void Decode(MDL0Node model)
        //{
        //    MDL0GroupNode grp;
        //    ModelLinker linker = ModelLinker.Read(model);
        //    IMatrixProvider[] nodeCache = model._nodes = new IMatrixProvider[model._numNodes];

        //    //Defs: link/scatter bones, nodes, polygon materials
        //    ParseDefs(linker, nodeCache);

        //    //Polygons: link bone, vertices, normals, colors, uvs.
        //    ParsePolygons(linker, nodeCache);

        //    //Textures: get texture list from texture/decal groups. Dump them in dummy 'Textures' group.
        //    ParseTextures(linker);

        //    //Materials: attach shaders and textures/decals
        //    ParseMaterials(linker);

        //    //Add groups to child list
        //    for (int i = 0; i < ModelLinker.BankLen; i++)
        //        if ((grp = linker.Groups[i]) != null)
        //            (grp._parent = model)._children.Add(grp);

        //    //Add node groups to list
        //    //for (int i = 0; i < nodeCount; i++)
        //    //    if (nodeCache[i] is NodeRef)
        //    //        model._nodeGroups.Add(nodeCache[i] as NodeRef);
        //}


        //private static void ParseDefs(ModelLinker linker, IMatrixProvider[] nodes)
        //{
        //    MDL0GroupNode bGrp = linker.Groups[(int)MDLResourceType.Bones];
        //    MDL0GroupNode matGrp = linker.Groups[(int)MDLResourceType.Materials];
        //    MDL0GroupNode plyGrp = linker.Groups[(int)MDLResourceType.Polygons];
        //    MDL0BoneNode bone;
        //    ResourceNode[] bCache;
        //    ResourceGroup* group = linker.Defs;
        //    ResourceEntry* entry;
        //    MDL0PolygonNode poly;
        //    MDL0MaterialNode mat;
        //    MDL0NodeType3Entry* nEntry;
        //    NodeRef nr;
        //    sbyte* p;
        //    int count, index, id;

        //    if (group == null)
        //        return;

        //    entry = &group->_first + 1;
        //    count = group->_numEntries;
        //    for (int i = 0; i < count; i++, entry++)
        //    {
        //        p = (sbyte*)group + entry->_stringOffset;
        //        if (PString.Equals(p, "NodeTree"))
        //        {
        //            linker.Model._hasTree = true;
        //            p = (sbyte*)group + entry->_dataOffset;

        //            linker.BoneCache = bCache = bGrp._children.ToArray();
        //            bGrp._children.Clear();
        //            while (*p == 2)
        //            {
        //                index = *(bushort*)(p + 1);
        //                id = *(bushort*)(p + 3);
        //                bone = bCache[index] as MDL0BoneNode;

        //                nodes[bone._nodeIndex] = bone;
        //                if (index == 0)
        //                    bGrp._children.Add(bone);
        //                else
        //                    (bone._parent = nodes[id] as MDL0BoneNode)._children.Add(bone);

        //                p += 5;
        //            }
        //        }
        //        else if (PString.Equals(p, "NodeMix"))
        //        {
        //            linker.Model._hasMix = true;
        //            p = (sbyte*)group + entry->_dataOffset;

        //        Next:
        //            switch (*p)
        //            {
        //                case 3:
        //                    index = *(bushort*)(p + 1);
        //                    nodes[index] = nr = new NodeRef() { _index = index };
        //                    nEntry = (MDL0NodeType3Entry*)(p + 4);
        //                    for (int x = *(p + 3); x-- > 0; nEntry++)
        //                        nr._entries.Add(new NodeWeight(nodes[nEntry->_id], nEntry->_value));

        //                    p = (sbyte*)nEntry;
        //                    goto Next;
        //                case 5:
        //                    p += 5;
        //                    goto Next;
        //            }
        //        }
        //        else
        //        {
        //            if (PString.Equals(p, "DrawOpa"))
        //                linker.Model._hasOpa = true;
        //            else
        //                linker.Model._hasXlu = true;
        //            p = (sbyte*)group + entry->_dataOffset;

        //            //What do we do with Xlu nodes? How are they different?
        //            while (*p == 4)
        //            {
        //                poly = plyGrp._children[*(bushort*)(p + 3)] as MDL0PolygonNode;
        //                mat = matGrp._children[*(bushort*)(p + 1)] as MDL0MaterialNode;
        //                poly._material = mat;
        //                mat._polygons.Add(poly);
        //                p += 8;
        //            }
        //        }
        //    }

        //}

        //Needs node list for bone
        private static void ParsePolygons(ModelLinker linker, IMatrixProvider[] nodes)
        {
            MDL0GroupNode group = linker.Groups[(int)MDLResourceType.Polygons];
            MDL0GroupNode vert = linker.Groups[(int)MDLResourceType.Vertices];
            MDL0GroupNode norm = linker.Groups[(int)MDLResourceType.Normals];
            MDL0GroupNode col = linker.Groups[(int)MDLResourceType.Colors];
            MDL0GroupNode uv = linker.Groups[(int)MDLResourceType.UVs];

            MDL0Polygon* pHeader;
            int offset;

            if (group == null)
                return;

            foreach (MDL0PolygonNode p in group._children)
            {
                pHeader = p.Header;

                //Get node references for use in vertex groups
                bushort* pNode = (bushort*)((byte*)pHeader + pHeader->_part1Offset) + 1;
                int count = *pNode++;
                for (int i = 0; i < count; i++)
                {
                    int index = *pNode++;
                    IMatrixProvider m = nodes[index];
                    if (m is MDL0BoneNode)
                        p._nodeList.Add(new Influence(m as MDL0BoneNode));
                    else
                        p._nodeList.Add(((Influence)m).Clone());
                }

                ////Bone
                //if ((offset = pHeader->_nodeId) >= 0)
                //    (p._singleBind = nodes[offset] as MDL0BoneNode)._polygons.Add(p);
                ////Vertices
                //if ((offset = pHeader->_vertexId) >= 0)
                //    (p._vertexNode = vert._children[offset] as MDL0VertexNode)._polygons.Add(p);
                ////Normals
                //if ((offset = pHeader->_normalId) >= 0)
                //    (p._normalNode = norm._children[offset] as MDL0NormalNode)._polygons.Add(p);
                ////Colors
                //for (int i = 0; i < 2; i++)
                //    if ((offset = pHeader->ColorIds[i]) >= 0)
                //        (p._colorSet[i] = col._children[offset] as MDL0ColorNode)._polygons.Add(p);
                ////UVs
                //for (int i = 0; i < 8; i++)
                //    if ((offset = pHeader->UVIds[i]) >= 0)
                //        (p._uvSet[i] = uv._children[offset] as MDL0UVNode)._polygons.Add(p);
            }
        }

        //private static void ParseMaterials(ModelLinker linker)
        //{
        //    MDL0GroupNode matGrp = linker.Groups[(int)MDLResourceType.Materials];
        //    MDL0GroupNode shdGrp = linker.Groups[(int)MDLResourceType.Shaders];
        //    MDL0GroupNode texGrp = linker.Groups[(int)MDLResourceType.Textures];
        //    MDL0Material* mHeader;
        //    MDL0Shader* sHeader;
        //    sbyte* p;
        //    int offset;

        //    if (matGrp == null)
        //        return;

        //    foreach (MDL0MaterialNode mat in matGrp._children)
        //    {
        //        //Attach shader
        //        mHeader = mat.Header;
        //        sHeader = (MDL0Shader*)((byte*)mHeader + mHeader->_shaderOffset);
        //        foreach (MDL0ShaderNode shd in shdGrp.Children)
        //            if (sHeader == shd.Header)
        //            { (mat._shader = shd)._materials.Add(mat); break; }

        //        //Attach textures/decals
        //        foreach (MDL0MaterialRefNode mr in mat.Children)
        //        {
        //            offset = mr.Header->_secondaryOffset;
        //            p = offset == 0 ? null : (sbyte*)mr.Header + offset;

        //            foreach (MDL0TextureNode t in texGrp._children)
        //            {
        //                if (mr._name == t._name)
        //                    (mr._texture = t)._texRefs.Add(mr);
        //                if ((p != null) && PString.Equals(p, t._name))
        //                    (mr._decal = t)._decRefs.Add(mr);
        //            }
        //        }
        //    }
        //}

        ////Creates texture linker objects and adds them to 'Textures' group
        //private static void ParseTextures(ModelLinker linker)
        //{
        //    MDL0GroupNode texGrp = null;
        //    ResourceGroup* pGroup = linker.Textures;
        //    ResourceEntry* pEntry;
        //    int eCount;

        //    pGroup = linker.Textures;
        //    if (pGroup != null)
        //    {
        //        texGrp = new MDL0GroupNode() { _name = "Textures" };
        //        eCount = pGroup->_numEntries;
        //        pEntry = &pGroup->_first + 1;
        //        for (int i = 0; i < eCount; i++, pEntry++)
        //            texGrp._children.Add(new MDL0TextureNode() { _name = new String((sbyte*)pGroup + pEntry->_stringOffset) });
        //    }

        //    pGroup = linker.Decals;
        //    if (pGroup != null)
        //    {
        //        if (texGrp == null)
        //            texGrp = new MDL0GroupNode() { _name = "Textures" };
        //        eCount = pGroup->_numEntries;
        //        pEntry = &pGroup->_first + 1;
        //        for (int i = 0; i < eCount; i++, pEntry++)
        //        {
        //            sbyte* p = (sbyte*)pGroup + pEntry->_stringOffset;
        //            foreach (MDL0TextureNode n in texGrp._children)
        //                if (PString.Equals(p, n._name))
        //                { p = null; break; }

        //            if (p != null)
        //                texGrp._children.Add(new MDL0TextureNode() { _name = new String(p) });
        //        }
        //    }

        //    linker.Groups[(int)MDLResourceType.Textures] = texGrp;
        //}
    }
}
