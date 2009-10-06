using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BrawlLib.SSBBTypes;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;

namespace BrawlLib.OpenGL
{
    public unsafe class GLModel
    {
        public static int _index;
        public List<GLBone> _bones = new List<GLBone>();
        public List<GLPolygon> _polygons = new List<GLPolygon>();

        public List<GLMaterial> _materials = new List<GLMaterial>();
        public List<GLTexture> _textures = new List<GLTexture>();

        public Vector3 _min, _max;
        private bool _enabled = true;

        public GLModel(MDL0Node node)
        {
            ResourceNode n;
            ResourceGroup* group;

            _min = node.BoxMin;
            _max = node.BoxMax;

            //Extract textures
            foreach (MDL0Data10Node tex in node.FindChild("Textures1", false).Children)
                _textures.Add(new GLTexture(this, tex));

            //Extract materials
            foreach (MDL0MaterialNode mat in node.FindChild("Materials1", false).Children)
                _materials.Add(new GLMaterial(this, mat));

            //Extract polygons
            group = node.Header->PolygonGroup;
            for (int i = 0; i < group->_numEntries; i++)
            {
                GLPolygon poly = ModelConverter.ExtractPolygon((MDL0Polygon*)group->First[i].DataAddress);
                poly._model = this;
                _polygons.Add(poly);
            }

            //Link Opa
            MDL0NodeEntry opaNode = node.FindChild("Definitions/DrawOpa", false) as MDL0NodeEntry;
            if(opaNode != null)
            foreach (MDL0NodeType4 o in opaNode.Items)
                _polygons[o.PolygonId]._materials.Add(_materials[o.MaterialId]);

            //Link Xlu
            MDL0NodeEntry xluNode = node.FindChild("Definitions/DrawOpa", false) as MDL0NodeEntry;
            if (xluNode != null)
                foreach (MDL0NodeType4 o in xluNode.Items)
                    _polygons[o.PolygonId]._materials.Add(_materials[o.MaterialId]);

            //Link bones
            if ((n = node.FindChild("Bones", false)) != null)
            {
                foreach (MDL0BoneNode bone in n.Children)
                    _bones.Add(ParseBone(bone, opaNode));
            }
        }

        private GLBone ParseBone(MDL0BoneNode node, MDL0NodeEntry opaNode)
        {
            GLBone bone = new GLBone();
            bone._translation = node.Translation;
            bone._rotation = node.Rotation;
            bone._scale = node.Scale;
            bone._id = node.NodeId;
            bone._index = node.NodeIndex;

            //Link materials/polygons
            foreach (MDL0NodeType4 o in opaNode.Items)
            {
                if (o.BoneIndex == bone._id)
                {
                    bone._polygons.Add(_polygons[o.PolygonId]);
                }
            }

            foreach (MDL0BoneNode n in node.Children)
                bone._children.Add(ParseBone(n, opaNode));

            return bone;
        }

        public void AttachTexture(string texName, Bitmap bmp)
        {
            foreach (GLTexture tex in _textures)
                tex.Attach(bmp, texName);
        }

        internal unsafe void Render(GLContext context)
        {
            if (!_enabled)
                return;

            _index = 0;
            foreach (GLBone bone in _bones)
                bone.Render(context);

            //foreach (GLPolygon poly in _polygons)
            //    poly.Render(context);
        }
    }
}
