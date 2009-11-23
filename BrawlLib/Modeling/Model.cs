using System;
using BrawlLib.OpenGL;
using System.Collections.Generic;
using System.Drawing;

namespace BrawlLib.Modeling
{
    public class Model : RenderedObject
    {
        internal bool _drawBones = true;

        internal Animation _currentAnim;
        public Animation CurrentAnimation
        {
            get { return _currentAnim; }
            set 
            {
                if (_currentAnim == value)
                    return;


            }
        }

        internal List<BoneNode> _bones = new List<BoneNode>(); //Nodes
        internal List<Polygon> _polygons = new List<Polygon>(); //Geometries
        internal List<Material> _materials = new List<Material>();
        //Effects

        //internal BoneNode _rootNode;
        //public BoneNode RootNode
        //{
        //    get { return _rootNode; }
        //    set { _rootNode = value; }
        //}

        private uint _nodeDisplayList = 0;


        public override void Prepare(GLContext ctx)
        {
            if (_drawBones)
            {
                ctx.glDisable((uint)GLEnableCap.Lighting);
                ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);

                foreach (BoneNode bone in _bones)
                    bone.RenderSkeleton(ctx, ref _nodeDisplayList, BoneNode.DefaultBoneColor, BoneNode.DefaultNodeColor);
            }
            base.Prepare(ctx);
        }

        public override void OnRender(GLContext ctx)
        {
        }

        //public override void OnRender(GLContext ctx)
        //{
        //    if (_drawBones)
        //    {
        //        ctx.glDisable((uint)GLEnableCap.Lighting);
        //        ctx.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);

        //        ctx.glColor(0.0f, 0.0f, 1.0f);
        //        foreach (BoneNode bone in _bones)
        //            bone.RenderSkeleton(ctx, ref _nodeDisplayList, BoneNode.DefaultBoneColor, BoneNode.DefaultNodeColor);
        //    }
        //}


        public override void SetFrame(object state, int index)
        {
            base.SetFrame(state, index);
            foreach (BoneNode n in _bones)
                n.SetFrame(_currentAnim, index);
        }

        public override void Unbind(GLContext ctx)
        {
            if (_nodeDisplayList != 0)
            {
                ctx.glDeleteLists(_nodeDisplayList, 1);
                _nodeDisplayList = 0;
            }
            base.Unbind(ctx);
        }
    }
}
