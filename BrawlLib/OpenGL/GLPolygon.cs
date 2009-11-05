using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.Wii.Models;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.OpenGL
{
    public class GLPolygon
    {
        public List<GLPrimitive> _primitives = new List<GLPrimitive>();
        //public int _nodeIndex;
        public GLNode _node;

        //internal ushort[] _weights;
        internal VertexBuffer _vertices;
        internal VertexBuffer _normals;
        internal ColorBuffer _colors1, _colors2;
        internal VertexBuffer[] _uvData = new VertexBuffer[8];

        public int _index;
        public bool _enabled = true, _wireframe = false;
        public GLModel _model;

        public string _name;

        public List<GLMaterial> _materials = new List<GLMaterial>();

        public override string ToString() { return _name; }

        //public GLPolygon(MDL0PolygonNode node)
        //{
        //}

        //public unsafe GLPolygon(GLModel model, MDL0PolygonNode polygon)
        //{
        //    _model = model;

        //    GLPrimitive prim;
        //    _index = polygon.ItemId;
        //    _nodeIndex = polygon.NodeId;

        //    VoidPtr address = polygon.Data->;
        //    ModelEntrySize e = new ModelEntrySize(polygon->_flags);

        //    p._vertices = ExtractVertices(polygon->VertexData);
        //    if (polygon->_normalId != -1)
        //        p._normals = ExtractNormals(polygon->NormalData);
        //    if (polygon->_colorId1 != -1)
        //        p._colors1 = ExtractColors(polygon->ColorData1);
        //    if (polygon->_colorId2 != -1)
        //        p._colors2 = ExtractColors(polygon->ColorData2);

        //    MDL0UVData* uvPtr;
        //    for (int i = 0; (i < 8) && ((uvPtr = polygon->GetUVData(i)) != null); i++)
        //        p._uvData[i] = ExtractUVs(uvPtr);

        //    ushort[] nodeBuffer = new ushort[16];
        //    while ((prim = ExtractPrimitive(ref address, e, p, nodeBuffer)) != null)
        //        p._primitives.Add(prim);
        //}

        private uint[] _textureIds = new uint[8];
        internal unsafe void Render(GLContext context)
        {
            if (!_enabled)
                return;

            if (_materials.Count != 0)
            {
                _materials[0].Bind(context, _textureIds);
            }
            else
            {
                return;
            }

            context.glEnable(GLEnableCap.Texture2D);

            if (_wireframe)
                context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Line);
            else
                context.glPolygonMode(GLFace.FrontAndBack, GLPolygonMode.Fill);

            foreach (GLPrimitive prim in _primitives)
                prim.Render(context, _textureIds);

            context.glDisable((uint)GLEnableCap.Texture2D);
        }

        internal void Rebuild()
        {
            foreach (GLPrimitive prim in _primitives)
                prim.Rebuild();
        }
    }
}
