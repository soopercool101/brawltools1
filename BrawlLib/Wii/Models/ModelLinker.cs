using System;
using System.Runtime.InteropServices;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using MR = BrawlLib.Wii.Models.MDLResourceType;

namespace BrawlLib.Wii.Models
{
    public enum MDLResourceType : int
    {
        Defs,
        Bones,
        Vertices,
        Normals,
        Colors,
        UVs,
        Materials,
        Shaders,
        Polygons,
        Textures,
        Decals
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe class ModelLinker
    {
        #region Linker lists
        internal const int BankLen = 11;

        internal static readonly Type[] TypeBank = new Type[] 
        { 
            null,
            typeof(MDL0BoneNode),
            typeof(MDL0VertexNode),
            typeof(MDL0NormalNode),
            typeof(MDL0ColorNode),
            typeof(MDL0UVNode),
            typeof(MDL0MaterialNode),
            typeof(MDL0ShaderNode),
            typeof(MDL0PolygonNode),
            null,
            null
        };

        internal static readonly List<MR>[] IndexBank = new List<MR>[]{
            null, //0
            null, //1
            null, //2
            null, //3
            null, //4
            null, //5
            null, //6
            null, //7
            null, //8
            new List<MR>( new MR[]{MR.Defs, MR.Bones, MR.Vertices, MR.Normals, MR.Colors, MR.UVs, MR.Materials, MR.Shaders, MR.Polygons, MR.Textures, MR.Decals})
        };
        #endregion

        public MDL0Header* Header;
        public int Version;
        public ResourceGroup* Defs;
        public ResourceGroup* Bones;
        public ResourceGroup* Vertices;
        public ResourceGroup* Normals;
        public ResourceGroup* Colors;
        public ResourceGroup* UVs;
        public ResourceGroup* Materials;
        public ResourceGroup* Shaders;
        public ResourceGroup* Polygons;
        public ResourceGroup* Textures;
        public ResourceGroup* Decals;
        //public int StringOffset;

        public MDL0GroupNode[] Groups = new MDL0GroupNode[BankLen];
        //public MDL0Props* Properties;

        public MDL0Node Model;
        public int _headerLen, _tableLen, _groupLen, _texLen, _dataLen;
        public int _texCount, _decCount;
        public ResourceNode[] BoneCache;

        private ModelLinker() { }
        public static ModelLinker Read(MDL0Node model)
        {
            byte* header = (byte*)model.Header;
            ModelLinker linker = new ModelLinker();
            ResourceGroup* pGroup;
            ResourceEntry* pEntry;
            MDL0CommonHeader* pHeader;
            MDL0GroupNode objGroup;
            ResourceNode node;
            int offset;
            List<MDLResourceType> iList = IndexBank[model._version];
            int groupCount = iList.Count, entryCount;
            MDLResourceType resType;
            bint* offsets = (bint*)(header + 0x10);
            Type objType;

            int* shaderTable = stackalloc int[128];
            for (int i = 0; i < 128; )
                shaderTable[i++] = 0;

            linker.Model = model;
            linker.Header = (MDL0Header*)header;
            linker.Version = model._version;

            fixed (ResourceGroup** gList = &linker.Defs)
                for (int i = 0; i < groupCount; i++)
                    if ((offset = offsets[i]) != 0)
                    {
                        resType = iList[i];
                        gList[(int)resType] = pGroup = (ResourceGroup*)((byte*)header + offset);
                        if ((objType = TypeBank[(int)resType]) != null)
                        {
                            linker.Groups[(int)resType] = objGroup = new MDL0GroupNode() { _name = resType.ToString("G") };
                            entryCount = pGroup->_numEntries;
                            pEntry = &pGroup->_first + 1;
                            for (int x = 0; x < entryCount; x++, pEntry++)
                            {
                                offset = pEntry->_dataOffset;
                                pHeader = (MDL0CommonHeader*)((byte*)pGroup + offset);
                                if (resType == MDLResourceType.Shaders)
                                {
                                    for (int y = 0; shaderTable[y] != offset; y++)
                                        if (shaderTable[y] == 0)
                                        {
                                            shaderTable[y] = offset;
                                            goto Create;
                                        }
                                    continue;
                                }
                            Create:
                                node = Activator.CreateInstance(objType) as ResourceNode;
                                node.Initialize(objGroup, pHeader, pHeader->_size);
                            }
                        }

                    }

            return linker;
        }

        public static ModelLinker Prepare(MDL0Node model)
        {
            ModelLinker linker = new ModelLinker();

            linker.Model = model;
            linker.Version = model._version;

            MDLResourceType resType;
            int index;
            List<MDLResourceType> iList = IndexBank[model._version];

            foreach (MDL0GroupNode group in model.Children)
            {
                resType = (MDLResourceType)Enum.Parse(typeof(MDLResourceType), group.Name);
                if (resType == MDLResourceType.Bones)
                    linker.BoneCache = group.FindChildrenByType(null, ResourceType.MDL0Bone);
                if ((index = iList.IndexOf(resType)) >= 0)
                    linker.Groups[(int)resType] = group;
            }

            return linker;
        }

        public void Write(ref byte* pGroup, ref byte* pData, bool force)
        {
            MDL0GroupNode group;
            ResourceGroup* pGrp;
            ResourceEntry* pEntry;
            //MDLResourceType resType;
            int len;

            fixed (ResourceGroup** pOut = &Defs)
                for (int i = 0; i < BankLen; i++)
                {
                    if (((group = Groups[i]) == null) || (TypeBank[i] == null))
                        continue;

                    pOut[i] = pGrp = (ResourceGroup*)pGroup;
                    pEntry = &pGrp->_first + 1;
                    if (i == (int)MDLResourceType.Bones)
                    {
                        *pGrp = new ResourceGroup(BoneCache.Length);
                        foreach (ResourceNode e in BoneCache)
                        {
                            (pEntry++)->_dataOffset = (int)pData - (int)pGroup;

                            len = e._calcSize;
                            e.Rebuild(pData, len, true);
                            pData += len;
                        }
                    }
                    else if (i == (int)MDLResourceType.Shaders)
                    {
                        MDL0GroupNode mats = Groups[(int)MDLResourceType.Materials];
                        MDL0Material* mHeader;

                        *pGrp = new ResourceGroup(mats._children.Count);
                        //Write data without headers
                        foreach (ResourceNode e in group._children)
                        {
                            len = e._calcSize;
                            e.Rebuild(pData, len, force);
                            pData += len;
                        }
                        //Write one header for each material, using same order.
                        foreach (MDL0MaterialNode mat in mats._children)
                        {
                            mHeader = mat.Header;
                            len = (int)mat._shader.Header;
                            mHeader->_shaderOffset = len - (int)mHeader;
                            (pEntry++)->_dataOffset = len - (int)pGrp;
                        }
                    }
                    else
                    {
                        *pGrp = new ResourceGroup(group._children.Count);
                        foreach (ResourceNode e in group._children)
                        {
                            (pEntry++)->_dataOffset = (int)pData - (int)pGroup;

                            len = e._calcSize;
                            e.Rebuild(pData, len, force);
                            pData += len;
                        }
                    }

                    pGroup += pGrp->_totalSize;
                }
        }

        //Write stored offsets to MDL header
        public void Finish()
        {
            List<MDLResourceType> iList = IndexBank[Version];
            MDLResourceType resType;
            bint* pOffset = (bint*)Header + 4;
            int count = iList.Count, offset;

            fixed (ResourceGroup** pGroup = &Defs)
                for (int i = 0; i < count; i++)
                {
                    resType = iList[i];
                    if ((offset = (int)pGroup[(int)resType]) > 0)
                        offset -= (int)Header;
                    pOffset[i] = offset;
                }
        }
    }
}
