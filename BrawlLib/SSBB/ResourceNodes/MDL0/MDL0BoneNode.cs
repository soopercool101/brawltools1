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

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0BoneNode : MDL0EntryNode, IMatrixProvider
    {
        private List<string> _entries = new List<string>();

        internal MDL0Bone* Header { get { return (MDL0Bone*)WorkingUncompressed.Address; } }
        public override ResourceType ResourceType { get { return ResourceType.MDL0Bone; } }

        protected override int DataLength { get { return Header->_headerLen; } }

        internal FrameState _bindState;
        internal Matrix _bindMatrix, _inverseBindMatrix;

        internal FrameState _frameState;
        internal Matrix _frameMatrix;

        private Vector3 _bMin, _bMax;
        private Matrix43 _transform, _transformInvert;

        [Category("Bone")]
        public Matrix FrameMatrix { get { return _frameMatrix; } }
        [Category("Bone")]
        public Matrix InverseBindMatrix { get { return _inverseBindMatrix; } }

        [Category("Bone")]
        public int HeaderLen { get { return Header->_headerLen; } }
        [Category("Bone")]
        public int MDL0Offset { get { return Header->_mdl0Offset; } }
        [Category("Bone")]
        public int StringOffset { get { return Header->_stringOffset; } }
        [Category("Bone")]
        public int BoneIndex { get { return Header->_index; } }

        [Category("Bone")]
        public int NodeId { get { return Header->_nodeId; } }
        [Category("Bone")]
        public uint Flags { get { return Header->_flags; } }
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

        [Category("Bone")]
        public int ParentOffset { get { return Header->_parentOffset / 0xD0; } }
        [Category("Bone")]
        public int FirstChildOffset { get { return Header->_firstChildOffset / 0xD0; } }
        [Category("Bone")]
        public int NextOffset { get { return Header->_nextOffset / 0xD0; } }
        [Category("Bone")]
        public int PrevOffset { get { return Header->_prevOffset / 0xD0; } }
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
            base.OnInitialize();

            if ((_name == null) && (Header->_stringOffset != 0))
                _name = Header->ResourceString;

            _bindState = _frameState = new FrameState(Header->_scale, Header->_rotation, Header->_translation);
            _bindMatrix = _frameMatrix = Header->_transform;
            _inverseBindMatrix = Header->_transformInv;

            _bMin = Header->_boxMin;
            _bMax = Header->_boxMax;

            if (Header->_part2Offset != 0)
            {
                MDL0Data7Part4* part4 = Header->Part2;
                if (part4 != null)
                {
                    ResourceGroup* group = part4->Group;
                    for (int i = 0; i < group->_numEntries; i++)
                    {
                        _entries.Add(group->First[i].GetName());
                    }
                }
            }
            return false;
        }

        protected internal override void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            MDL0Bone* header = (MDL0Bone*)dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;

            header->_boxMin = _bMin;
            header->_boxMax = _bMax;
            header->_scale = _bindState._scale;
            header->_rotation = _bindState._rotate;
            header->_translation = _bindState._translate;
            header->_transform = (bMatrix43)_bindMatrix;
            header->_transformInv = (bMatrix43)_inverseBindMatrix;

            MDL0Data7Part4* part4;
            if ((header->_part2Offset != 0) && ((part4 = header->Part2) != null))
            {
                ResourceGroup* group = part4->Group;
                group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
                ResourceEntry* rEntry = group->First;

                for (int i = 0, x = 1; i < group->_numEntries; )
                {
                    ResourceEntry.Build(group, x++, (VoidPtr)group + (rEntry++)->_dataOffset, (BRESString*)stringTable[_entries[i++]]);
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
