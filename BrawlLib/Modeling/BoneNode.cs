using System;
using System.Collections.Generic;
using BrawlLib.OpenGL;
using System.Drawing;

namespace BrawlLib.Modeling
{
    public class BoneNode : RenderedObject
    {
        //internal const float _nodeRadius = 0.5f;

        public static Color DefaultBoneColor = Color.FromArgb(0, 0, 128);
        public static Color DefaultNodeColor = Color.FromArgb(0, 128, 0);

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

        //internal int _frameIndex;

        internal Color _boneColor = Color.Transparent;
        public Color BoneColor
        {
            get { return _boneColor; }
            set
            {
                _boneColor = value;
                foreach (BoneNode n in _children)
                    if (n._boneColor != Color.Transparent)
                        n.BoneColor = value;
            }
        }

        internal Color _nodeColor = Color.Transparent;
        public Color NodeColor 
        { 
            get { return _nodeColor; } 
            set 
            {
                _nodeColor = value;
                foreach (BoneNode n in _children)
                    if (n._nodeColor != Color.Transparent)
                        n.NodeColor = value;
            } 
        }

        internal List<PolygonRef> _polygonRefs = new List<PolygonRef>();

        //internal FrameState _currentFrame;

        //internal List<FrameState> _stateList = new List<FrameState>();

        //public FrameState this[int index]
        //{
        //    get { return _stateList[index]; }
        //    set { SetFrame(value, index); }
        //}

        //internal AnimBone _animation;


        //public void SetFrame(FrameState state, int index)
        //{
        //    //state._isDirty = true;
        //    _stateList[index] = state;
        //}

        public override void OnRender(GLContext ctx)
        {
        }

        internal unsafe void RenderSkeleton(GLContext ctx, ref uint nodeDl, Color boneColor, Color nodeColor)
        {
            if (!_enabled)
                return;

            if (_boneColor != Color.Transparent)
                boneColor = _boneColor;
            if (_nodeColor != Color.Transparent)
                nodeColor = _nodeColor;

            ctx.glColor(boneColor.R, boneColor.G, boneColor.B, boneColor.A);

            Vector3 v = _frameTransform._translate;

            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex3v((float*)&v);

            ctx.glEnd();

            ctx.glPushMatrix();

            fixed (Matrix* m = &_frameTransform._transform)
                ctx.glMultMatrix((float*)m);

            //Render node
            if (nodeDl == 0)
                nodeDl = BuildNode(ctx);

            ctx.glColor(nodeColor.R, nodeColor.G, nodeColor.B, nodeColor.A);
            ctx.glCallList(nodeDl);

            //Render children
            foreach (BoneNode n in _children)
                n.RenderSkeleton(ctx, ref nodeDl, boneColor, nodeColor);

            ctx.glPopMatrix();
        }

        private unsafe uint BuildNode(GLContext ctx)
        {
            uint dl = ctx.glGenLists(1);
            ctx.glNewList(dl, GLListMode.COMPILE);

            fixed (Vector3* p = _nodeVertices)
            {
                //ctx.glColor(0.0f, 0.5f, 0.0f);

                Vector3* vp = p;
                for (int x = 0; x < 6; x++)
                {
                    ctx.glBegin(GLPrimitiveType.TriangleFan);
                    for (int i = 0; i < 6; i++)
                        ctx.glVertex3v((float*)vp++);
                    ctx.glEnd();
                }
            }

            //Render node orients
            ctx.glBegin(GLPrimitiveType.Lines);

            ctx.glColor(1.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(_nodeRadius * 2, 0.0f, 0.0f);

            ctx.glColor(0.0f, 1.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, _nodeRadius * 2, 0.0f);

            ctx.glColor(0.0f, 0.0f, 1.0f);
            ctx.glVertex(0.0f, 0.0f, 0.0f);
            ctx.glVertex(0.0f, 0.0f, _nodeRadius * 2);

            ctx.glEnd();

            ctx.glEndList();

            return dl;
        }

    }
}
