using System;
using System.Runtime.InteropServices;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using System.Collections.Generic;
using MR = BrawlLib.Wii.Models.MDLResourceType;
using BrawlLib.Modeling;

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
    public unsafe class ModelLinker// : IDisposable
    {
        #region Linker lists
        internal const int BankLen = 11;

        internal static readonly Type[] TypeBank = new Type[] 
        { 
            null,
            typeof(MDL0BoneNode),
            null,
            null,
            null,
            null,
            typeof(MDL0MaterialNode),
            typeof(MDL0ShaderNode),
            typeof(MDL0PolygonNode),
            null,
            null
        };

        internal static readonly MR[] OrderBank = new MR[]
        {
            MR.Textures,
            MR.Decals,
            MR.Defs,
            MR.Bones,
            MR.Materials,
            MR.Shaders,
            MR.Polygons,
            MR.Vertices,
            MR.Normals,
            MR.Colors,
            MR.UVs
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
        public int _texCount, _decCount, _nodeCount;
        public ResourceNode[] BoneCache;
        public IMatrixNode[] NodeCache;

        public List<VertexCodec> _vertices;
        public List<VertexCodec> _normals;
        public List<ColorCodec> _colors;
        public List<VertexCodec> _uvs;

        private ModelLinker() { }

        public ModelLinker(MDL0Header* pModel)
        {
            Header = pModel;
            Version = pModel->_header._version;
            NodeCache = new IMatrixNode[pModel->Properties->_numNodes];

            bint* offsets = (bint*)((byte*)pModel + 0x10);
            List<MDLResourceType> iList = IndexBank[Version];
            int groupCount = iList.Count;
            int offset;

            //Extract resource addresses
            fixed (ResourceGroup** gList = &Defs)
                for (int i = 0; i < groupCount; i++)
                    if ((offset = offsets[i]) != 0)
                        gList[(int)iList[i]] = (ResourceGroup*)((byte*)pModel + offset);
        }
        //~ModelLinker()
        //{
        //    Dispose();
        //}
        //public void Dispose()
        //{
        //    if (_vertices != null)
        //    {
        //        foreach (VertexCodec c in _vertices)
        //            c.Dispose();
        //        _vertices = null;
        //    }
        //    if (_normals != null)
        //    {
        //        foreach (VertexCodec c in _normals)
        //            c.Dispose();
        //        _normals = null;
        //    }
        //    if (_colors != null)
        //    {
        //        //foreach (ColorCodec c in _colors)
        //        //    c.Dispose();
        //        _colors = null;
        //    }
        //    if (_uvs != null)
        //    {
        //        foreach (VertexCodec c in _uvs)
        //            c.Dispose();
        //        _uvs = null;
        //    }
        //    GC.SuppressFinalize(this);
        //}


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

                //Get flattened bone list and assign it to bone cache
                if (resType == MDLResourceType.Bones)
                    linker.BoneCache = group.FindChildrenByType(null, ResourceType.MDL0Bone);

                //If version contains resource type, add it to group list
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
            int len;

            fixed (ResourceGroup** pOut = &Defs)
                foreach(MDLResourceType resType in OrderBank)
                {
                    if (((group = Groups[(int)resType]) == null) || (TypeBank[(int)resType] == null))
                        continue;

                    pOut[(int)resType] = pGrp = (ResourceGroup*)pGroup;
                    pEntry = &pGrp->_first + 1;
                    if (resType == MDLResourceType.Bones)
                    {
                        *pGrp = new ResourceGroup(BoneCache.Length);
                        foreach (ResourceNode e in BoneCache)
                        {
                            (pEntry++)->_dataOffset = (int)(pData - pGroup);

                            len = e._calcSize;
                            e.Rebuild(pData, len, true);
                            pData += len;
                        }
                    }
                    else if (resType == MDLResourceType.Shaders)
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
                            (pEntry++)->_dataOffset = (int)(pData - pGroup);

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
