using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.Wii.Models;
using BrawlLib.SSBBTypes;
using BrawlLib.Modeling;
using BrawlLib.OpenGL;
using System.Drawing;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Compression;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0BoneNode : MDL0EntryNode, IMatrixProvider
    {
        private List<string> _entries = new List<string>();

        internal MDL0Bone* Header { get { return (MDL0Bone*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MDL0Bone; } }

        //protected override int DataLength { get { return Header->_headerLen; } }

        internal BoneFlags _flags;
        internal List<MDL0PolygonNode> _polygons = new List<MDL0PolygonNode>();

        internal FrameState _bindState;
        internal Matrix _bindMatrix, _inverseBindMatrix;

        internal FrameState _frameState;
        internal Matrix _frameMatrix;

        private Vector3 _bMin, _bMax;
        private Matrix43 _transform, _transformInvert;

        internal int _nodeIndex;

        [Browsable(false)]
        public Matrix FrameMatrix { get { return _frameMatrix; } }
        [Browsable(false)]
        public Matrix InverseBindMatrix { get { return _inverseBindMatrix; } }

        //[Category("Bone")]
        //public int HeaderLen { get { return Header->_headerLen; } }
        //[Category("Bone")]
        //public int MDL0Offset { get { return Header->_mdl0Offset; } }
        //[Category("Bone")]
        //public int StringOffset { get { return Header->_stringOffset; } }

        //public List<MDL0PolygonNode> Polygons { get { return _polygons; } }

        [Category("Bone")]
        public int BoneIndex { get { return Header->_index; } }

        [Category("Bone")]
        public int NodeId { get { return Header->_nodeId; } }
        [Category("Bone")]
        public BoneFlags Flags { get { return _flags; } }
        [Category("Bone")]
        public uint Pad1 { get { return Header->_pad1; } }
        [Category("Bone")]
        public uint Pad2 { get { return Header->_pad2; } }

        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Scale { get { return _bindState._scale; } set { _bindState.Scale = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotation { get { return _bindState._rotate; } set { _bindState.Rotate = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Translation { get { return _bindState._translate; } set { _bindState.Translate = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMin { get { return _bMin; } set { _bMin = value; SignalPropertyChange(); } }
        [Category("Bone"), TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 BoxMax { get { return _bMax; } set { _bMax = value; SignalPropertyChange(); } }

        //[Category("Bone")]
        //public int ParentOffset { get { return Header->_parentOffset / 0xD0; } }
        //[Category("Bone")]
        //public int FirstChildOffset { get { return Header->_firstChildOffset / 0xD0; } }
        //[Category("Bone")]
        //public int NextOffset { get { return Header->_nextOffset / 0xD0; } }
        //[Category("Bone")]
        //public int PrevOffset { get { return Header->_prevOffset / 0xD0; } }
        [Category("Bone")]
        public int Part2Offset { get { return Header->_part2Offset; } }

        [Category("Data2 Part2")]
        public List<string> Entries { get { return _entries; } }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);

            foreach (MDL0BoneNode n in Children)
                n.GetStrings(table);

            foreach (string s in _entries)
                table.Add(s);
        }

        protected override bool OnInitialize()
        {
            MDL0Bone* header = Header;

            //SetSizeInternal(header->_headerLen);

            //Assign parent node based on offset. Will scatter after all bones are processed
            int offset = header->_parentOffset;
            if (offset < 0)
            {
                MDL0Bone* pHeader = (MDL0Bone*)((byte*)header + offset);
                foreach (MDL0BoneNode bone in _parent._children)
                    if (pHeader == bone.Header)
                    {
                        _parent = bone;
                        break;
                    }
            }

            if ((_name == null) && (header->_stringOffset != 0))
                _name = header->ResourceString;

            _flags = (BoneFlags)(uint)header->_flags;
            _nodeIndex = header->_nodeId;

            _bindState = _frameState = new FrameState(header->_scale, header->_rotation, header->_translation);
            _bindMatrix = _frameMatrix = header->_transform;
            _inverseBindMatrix = header->_transformInv;

            _bMin = header->_boxMin;
            _bMax = header->_boxMax;

            if (header->_part2Offset != 0)
            {
                MDL0Data7Part4* part4 = (MDL0Data7Part4*)((byte*)header + header->_part2Offset);
                ResourceGroup* group = part4->Group;
                ResourceEntry* pEntry = &group->_first + 1;
                int count = group->_numEntries;
                for (int i = 0; i < count; i++)
                    _entries.Add(new String((sbyte*)group + (pEntry++)->_stringOffset));
            }

            return false;
        }

        //Use MoveRaw without processing children.
        //Prevents addresses from changing before completion.
        //internal override void MoveRaw(VoidPtr address, int length)
        //{
        //    Memory.Move(address, WorkingSource.Address, (uint)length);
        //    DataSource newsrc = new DataSource(address, length);
        //    if (_compression == CompressionType.None)
        //    {
        //        _replSrc.Close();
        //        _replUncompSrc.Close();
        //        _replSrc = _replUncompSrc = newsrc;
        //    }
        //    else
        //    {
        //        _replSrc.Close();
        //        _replSrc = newsrc;
        //    }
        //}

        protected override int OnCalculateSize(bool force)
        {
            int len = 0xD0;
            if (_entries.Count > 0)
                len += 0x1C + (_entries.Count * 0x2C);
            return len;
        }

        protected internal override void OnRebuild(VoidPtr address, int length, bool force)
        {
            MDL0Bone* header = (MDL0Bone*)address;
            MDL0BoneNode bone;
            int index = 0, offset;

            header->_headerLen = length;
            header->_index = _entryIndex;
            header->_nodeId = _nodeIndex;
            header->_flags = (uint)_flags;
            header->_pad1 = 0;
            header->_pad2 = 0;
            header->_scale = _bindState._scale;
            header->_rotation = _bindState._rotate;
            header->_translation = _bindState._translate;
            header->_boxMin = _bMin;
            header->_boxMax = _bMax;

            header->_transform = _bindMatrix;
            header->_transformInv = _inverseBindMatrix;

            //Sub-entries
            if (_entries.Count > 0)
            {
                header->_part2Offset = 0xD0;
                *(bint*)((byte*)address + 0xD0) = 0x1C + (_entries.Count * 0x2C);
                ResourceGroup* pGroup = (ResourceGroup*)((byte*)address + 0xD4);
                ResourceEntry* pEntry = &pGroup->_first + 1;
                byte* pData = (byte*)pGroup + pGroup->_totalSize;

                *pGroup = new ResourceGroup(_entries.Count);

                foreach (string s in _entries)
                {
                    (pEntry++)->_dataOffset = (int)pData - (int)pGroup;
                    MDL0Data7Part4Entry* p = (MDL0Data7Part4Entry*)pData;
                    *p = new MDL0Data7Part4Entry(1);
                    pData += 0x1C;
                }
            }
            else
                header->_part2Offset = 0;

            //Set first child
            if (_children.Count > 0)
                header->_firstChildOffset = length;
            else
                header->_firstChildOffset = 0;

            if (_parent != null)
            {
                index = _parent._children.IndexOf(this);

                //Parent
                if (_parent is MDL0BoneNode)
                    header->_parentOffset = (int)_parent.WorkingUncompressed.Address - (int)address;
                else
                    header->_parentOffset = 0;

                //Prev
                if (index == 0)
                    header->_prevOffset = 0;
                else
                {
                    //Link to prev
                    bone = _parent._children[index - 1] as MDL0BoneNode;
                    offset = (int)bone.Header - (int)address;
                    header->_prevOffset = offset;
                    bone.Header->_nextOffset = -offset;
                }

                //Next
                if (index == (_parent._children.Count - 1))
                    header->_nextOffset = 0;
            }
        }

        protected internal override void PostProcess(VoidPtr mdlAddress, VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Bone* header = (MDL0Bone*)dataAddress;
            header->_mdl0Offset = (int)mdlAddress - (int)dataAddress;
            header->_stringOffset = ((int)stringTable[Name] + 4) - (int)dataAddress;

            //Entry strings
            if (_entries.Count > 0)
            {
                ResourceGroup* pGroup = (ResourceGroup*)((byte*)header + header->_part2Offset + 4);
                ResourceEntry* pEntry = &pGroup->_first;
                int count = pGroup->_numEntries;
                (*pEntry++) = new ResourceEntry(0xFFFF, 0, 0, 0, 0);

                for (int i = 0; i < count; i++)
                {
                    MDL0Data7Part4Entry* entry = (MDL0Data7Part4Entry*)((byte*)pGroup + (pEntry++)->_dataOffset);
                    entry->_stringOffset = (int)stringTable[_entries[i]] + 4 - (int)entry;

                    ResourceEntry.Build(pGroup, i + 1, entry, (BRESString*)stringTable[_entries[i]]);
                }
            }
        }

        //Change has been made to bind state, need to recalculate matrices
        internal void RecalcBindState()
        {
            if (_parent is MDL0BoneNode)
            {
                _bindMatrix = ((MDL0BoneNode)_parent)._bindMatrix * _bindState._transform;
                _inverseBindMatrix = _bindState._iTransform * ((MDL0BoneNode)_parent)._inverseBindMatrix;
            }
            else
            {
                _bindMatrix = _bindState._transform;
                _inverseBindMatrix = _bindState._iTransform;
            }

            foreach (MDL0BoneNode bone in Children)
                bone.RecalcBindState();
        }

        public void CalcBase() { }
        public void CalcWeighted() { }

        #region Rendering

        public static Color DefaultBoneColor = Color.FromArgb(0, 0, 128);
        public static Color DefaultNodeColor = Color.FromArgb(0, 128, 0);

        internal Color _boneColor = Color.Transparent;
        internal Color _nodeColor = Color.Transparent;

        const float _nodeRadius = 0.15f;
        const float _nodeAdj = 0.10f;

        private static readonly Vector3[] _nodeVertices = new Vector3[] { 
            new Vector3(-_nodeRadius, 0.0f, 0.0f),

            new Vector3(-_nodeAdj, 0.0f, -_nodeAdj),
            new Vector3(-_nodeAdj, _nodeAdj, 0.0f),
            new Vector3(-_nodeAdj, 0.0f, _nodeAdj),
            new Vector3(-_nodeAdj, -_nodeAdj, 0.0f),
            new Vector3(-_nodeAdj, 0.0f, -_nodeAdj),
            
            //new Vector3(0.0f, 0.0f, -_nodeRadius),
            //new Vector3(0.0f, -_nodeRadius, 0.0f),
            //new Vector3(0.0f, 0.0f, _nodeRadius),
            //new Vector3(0.0f, _nodeRadius, 0.0f),
            //new Vector3(0.0f, 0.0f, -_nodeRadius),

            
            new Vector3(_nodeRadius, 0.0f, 0.0f),

            new Vector3(_nodeAdj, 0.0f, -_nodeAdj),
            new Vector3(_nodeAdj, _nodeAdj, 0.0f),
            new Vector3(_nodeAdj, 0.0f, _nodeAdj),
            new Vector3(_nodeAdj, -_nodeAdj, 0.0f),
            new Vector3(_nodeAdj, 0.0f, -_nodeAdj),
            
            new Vector3(0.0f, _nodeRadius, 0.0f),

            new Vector3(0.0f, _nodeAdj, -_nodeAdj),
            new Vector3(_nodeAdj, _nodeAdj, 0.0f),
            new Vector3(0.0f, _nodeAdj, _nodeAdj),
            new Vector3(-_nodeAdj, _nodeAdj, 0.0f),
            new Vector3(0.0f, _nodeAdj, -_nodeAdj),
            
            //new Vector3(-_nodeRadius, 0.0f, 0.0f),
            //new Vector3(0.0f, 0.0f, _nodeRadius),
            //new Vector3(_nodeRadius, 0.0f, 0.0f),
            //new Vector3(0.0f,0.0f,-_nodeRadius),
            //new Vector3(-_nodeRadius, 0.0f, 0.0f),
            
            new Vector3(0.0f, -_nodeRadius, 0.0f),

            new Vector3(0.0f, -_nodeAdj, -_nodeAdj),
            new Vector3(_nodeAdj, -_nodeAdj, 0.0f),
            new Vector3(0.0f, -_nodeAdj, _nodeAdj),
            new Vector3(-_nodeAdj, -_nodeAdj, 0.0f),
            new Vector3(0.0f, _nodeAdj, -_nodeAdj),
            
            new Vector3(0.0f, 0.0f, -_nodeRadius),

            new Vector3(0.0f, _nodeAdj, -_nodeAdj),
            new Vector3(-_nodeAdj, 0.0f, -_nodeAdj),
            new Vector3(0.0f, -_nodeAdj, -_nodeAdj),
            new Vector3(_nodeAdj, 0.0f, -_nodeAdj),
            new Vector3(0.0f, _nodeAdj, -_nodeAdj),
            
            new Vector3(0.0f, 0.0f, _nodeRadius),

            new Vector3(0.0f, _nodeAdj, _nodeAdj),
            new Vector3(-_nodeAdj, 0.0f, _nodeAdj),
            new Vector3(0.0f, -_nodeAdj, _nodeAdj),
            new Vector3(_nodeAdj, 0.0f, _nodeAdj),
            new Vector3(0.0f, _nodeAdj, _nodeAdj)
        };

        public bool _render = true;
        internal unsafe void Render(GLContext ctx)
        {
            if (!_render)
                return;

            if (_boneColor != Color.Transparent)
                ctx.glColor(_boneColor.R, _boneColor.G, _boneColor.B, _boneColor.A);
            else
                ctx.glColor(DefaultBoneColor.R, DefaultBoneColor.G, DefaultBoneColor.B, DefaultBoneColor.A);

            Vector3 v = _frameState._translate;

            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex3v((float*)&v);

            ctx.glEnd();

            ctx.glPushMatrix();

            fixed (Matrix* m = &_frameState._transform)
                ctx.glMultMatrix((float*)m);


            //Render node
            GLDisplayList ndl = ctx.FindOrCreate<GLDisplayList>("NodeOrb", CreateNodeOrb);
            if (_nodeColor != Color.Transparent)
                ctx.glColor(_nodeColor.R, _nodeColor.G, _nodeColor.B, _nodeColor.A);
            else
                ctx.glColor(DefaultNodeColor.R, DefaultNodeColor.G, DefaultNodeColor.B, DefaultNodeColor.A);
            ndl.Call();
            DrawNodeOrients(ctx);

            //Render children
            foreach (MDL0BoneNode n in Children)
                n.Render(ctx);

            ctx.glPopMatrix();
        }

        internal void ApplyCHR0(CHR0Node node, int index)
        {
            CHR0EntryNode e;

            if ((node == null) || (index == 0)) //Reset to bind pose
                _frameState = _bindState;
            else if ((e = node.FindChild(Name, false) as CHR0EntryNode) != null) //Set to anim pose
                _frameState = new FrameState(e.GetAnimFrame(index - 1));
            else //Set to neutral pose
                _frameState = _bindState;

            if (_parent is MDL0BoneNode)
                _frameMatrix = ((MDL0BoneNode)_parent)._frameMatrix * _frameState._transform;
            else
                _frameMatrix = _frameState._transform;

            foreach (MDL0BoneNode b in Children)
                b.ApplyCHR0(node, index);
        }

        private static GLDisplayList CreateNodeOrb(GLContext ctx)
        {
            GLDisplayList list = new GLDisplayList(ctx);

            list.Begin();
            DrawNodeOrb(ctx);
            list.End();

            return list;
        }
        private static void DrawNodeOrb(GLContext ctx)
        {
            fixed (Vector3* p = _nodeVertices)
            {
                Vector3* vp = p;
                for (int x = 0; x < 6; x++)
                {
                    ctx.glBegin(GLPrimitiveType.TriangleFan);
                    for (int i = 0; i < 6; i++)
                        ctx.glVertex3v((float*)vp++);
                    ctx.glEnd();
                }
            }
        }
        private static void DrawNodeOrients(GLContext ctx)
        {
            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glColor(1.0f, 0.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(_nodeRadius * 2, 0.0f, 0.0f);

            ctx.glColor(0.0f, 1.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, _nodeRadius * 2, 0.0f);

            ctx.glColor(0.0f, 0.0f, 1.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, _nodeRadius * 2);

            ctx.glEnd();
        }

        internal override void Bind(GLContext ctx)
        {
            _render = true;
            _boneColor = Color.Transparent;
            _nodeColor = Color.Transparent;
        }

        #endregion

    }
}
