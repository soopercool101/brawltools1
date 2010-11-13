﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.IO;
using System.Windows.Forms;
using BrawlLib.Wii.Models;
using BrawlLib.Imaging;

namespace BrawlLib.Modeling
{
    public static unsafe partial class Collada
    {
        public static MDL0Node ImportModel(string filePath)
        {
            MDL0Node model = new MDL0Node() { _name = Path.GetFileNameWithoutExtension(filePath), _origPath = filePath };
            model.InitGroups();

            using (DecoderShell shell = DecoderShell.Import(filePath))
                try
                {
                    //Extract images, removing duplicates
                    foreach (ImageEntry img in shell._images)
                    {
                        string name;
                        TextureRef tex;

                        if (img._path != null)
                        {
                            int ind1 = img._path.LastIndexOf('/') + 1;
                            int ind2 = img._path.LastIndexOf('.');

                            if (ind2 >= 0)
                                name = img._path.Substring(ind1, ind2 - ind1);
                            else
                                name = img._path.Substring(ind1);
                        }
                        else
                            name = img._name != null ? img._name : img._id;

                        tex = model._textures.FindOrCreate(name);
                        img._node = tex;
                    }

                    //Extract materials
                    foreach (MaterialEntry mat in shell._materials)
                    {
                        MDL0MaterialNode matNode = new MDL0MaterialNode();
                        matNode._name = mat._name != null ? mat._name : mat._id;
                        mat._node = matNode;
                        model._matList.Add(matNode);

                        //Find effect
                        if (mat._effect != null)
                            foreach (EffectEntry eff in shell._effects)
                            {
                                if (eff._id == mat._effect)
                                {
                                    //Attach textures and effects to material
                                    foreach (LightEffectEntry l in eff._shader._effects)
                                    {
                                        if (l._type == LightEffectType.diffuse && l._texture != null)
                                        {
                                            foreach (ImageEntry img in shell._images)
                                                if (img._id == l._texture)
                                                {
                                                    MDL0MaterialRefNode mr = new MDL0MaterialRefNode();

                                                    (mr._texture = img._node as TextureRef)._texRefs.Add(mr);
                                                    mr._name = mr._texture.Name;

                                                    matNode._children.Add(mr);
                                                    mr._parent = matNode;

                                                    break;
                                                }

                                        }
                                    }
                                }
                            }
                    }

                    //Extract scenes
                    foreach (SceneEntry scene in shell._scenes)
                    {
                        foreach (NodeEntry node in scene._nodes)
                        {
                            EnumNode(node, model._boneGroup, scene, model, shell);
                        }
                    }

                    model.CleanGroups();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.ToString());
                    model = null;
                }
                finally
                {
                    //Clean up the mess we've made
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                }

            return model;
        }
        private static void EnumNode(NodeEntry node, ResourceNode parent, SceneEntry scene, MDL0Node model, DecoderShell shell)
        {
            PrimitiveManager manager = null;
            MDL0BoneNode bone = null;
            Influence inf = null;

            if (node._type == NodeType.JOINT)
            {
                bone = new MDL0BoneNode();
                bone._name = node._name != null ? node._name : node._id;
                bone._bindState = bone._frameState = node._transform;
                node._node = bone;

                parent._children.Add(bone);
                bone._parent = parent;

                bone.RecalcBindState();

                foreach (NodeEntry e in node._children)
                    EnumNode(e, bone, scene, model, shell);

                inf = new Influence(bone);
                model._influences._influences.Add(inf);

            }

            foreach (InstanceEntry inst in node._instances)
            {
                if (inst._isController)
                {
                    foreach (SkinEntry skin in shell._skins)
                        if (skin._id == inst._url)
                        {
                            foreach (GeometryEntry g in shell._geometry)
                                if (g._id == skin._skinSource)
                                {
                                    manager = DecodePrimitivesWeighted(g, skin, scene, model._influences);
                                    break;
                                }

                            break;
                        }
                }
                else
                {
                    foreach (GeometryEntry g in shell._geometry)
                        if (g._id == inst._url)
                        {
                            manager = DecodePrimitivesUnweighted(g);
                            break;
                        }
                }

                if (manager != null)
                {
                    MDL0PolygonNode poly = new MDL0PolygonNode() { _manager = manager };
                    poly._name = node._name != null ? node._name : node._id;

                    //Attach single-bind
                    if (bone != null)
                        poly._singleBind = inf;

                    //Attach material
                    if (inst._material != null)
                    {
                        foreach(MaterialEntry mat in shell._materials)
                            if (mat._id == inst._material._target)
                            {
                                (poly._material = (mat._node as MDL0MaterialNode))._polygons.Add(poly);
                                break;
                            }
                    }

                    model._polyList.Add(poly);
                }

            }
        }

        private class ColladaEntry : IDisposable
        {
            internal string _id, _name;
            internal object _node;

            ~ColladaEntry() { Dispose(); }
            public virtual void Dispose() { GC.SuppressFinalize(this); }
        }
        private class ImageEntry : ColladaEntry
        {
            internal string _path;
        }
        private class MaterialEntry : ColladaEntry
        {
            internal string _effect;
        }
        private class EffectEntry : ColladaEntry
        {
            internal EffectShaderEntry _shader;
        }
        private class GeometryEntry : ColladaEntry
        {
            internal List<SourceEntry> _sources = new List<SourceEntry>();
            internal List<PrimitiveEntry> _primitives = new List<PrimitiveEntry>();

            internal int _faces, _lines;

            internal string _verticesId;
            internal InputEntry _verticesInput;

            public override void Dispose()
            {
                foreach (SourceEntry p in _sources)
                    p.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private class SourceEntry : ColladaEntry
        {
            internal SourceType _arrayType;
            internal string _arrayId;
            internal int _arrayCount;
            internal object _arrayData; //Parser must dispose!

            internal string _accessorSource;
            internal int _accessorCount;
            internal int _accessorStride;

            public override void Dispose()
            {
                if (_arrayData is UnsafeBuffer)
                    ((UnsafeBuffer)_arrayData).Dispose();
                _arrayData = null;
                GC.SuppressFinalize(this);
            }
        }
        private class InputEntry : ColladaEntry
        {
            internal SemanticType _semantic;
            internal int _set;
            internal int _offset;
            internal string _source;
            internal int _outputOffset;
        }
        private class PrimitiveEntry
        {
            internal PrimitiveType _type;

            internal string _material;
            internal int _entryCount;
            internal int _entryStride;
            internal int _faceCount, _pointCount;

            internal List<InputEntry> _inputs = new List<InputEntry>();

            internal List<PrimitiveFace> _faces = new List<PrimitiveFace>();
        }
        private class PrimitiveFace
        {
            internal int _pointCount;
            internal int _faceCount;
            internal ushort[] _pointIndices;
        }
        private class SkinEntry : ColladaEntry
        {
            internal string _skinSource;
            internal Matrix _bindMatrix = Matrix.Identity;

            //internal Matrix _bindShape;
            internal List<SourceEntry> _sources = new List<SourceEntry>();
            internal List<InputEntry> _jointInputs = new List<InputEntry>();

            internal List<InputEntry> _weightInputs = new List<InputEntry>();
            internal int _weightCount;
            internal int[][] _weights;

            public override void Dispose()
            {
                foreach (SourceEntry src in _sources)
                    src.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        private class SceneEntry : ColladaEntry
        {
            internal List<NodeEntry> _nodes = new List<NodeEntry>();

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (NodeEntry node in _nodes)
                    if ((n = DecoderShell.FindNodeInternal(name, node)) != null)
                        return n;
                return null;
            }
        }
        private class NodeEntry : ColladaEntry
        {
            internal NodeType _type = NodeType.NODE;
            internal FrameState _transform;
            //internal NodeEntry _parent;
            internal List<NodeEntry> _children = new List<NodeEntry>();
            internal List<InstanceEntry> _instances = new List<InstanceEntry>();
        }
        private class InstanceEntry : ColladaEntry
        {
            internal bool _isController;
            internal string _url;
            internal InstanceMaterial _material;
        }
        private class InstanceMaterial : ColladaEntry
        {
            internal string _symbol, _target;
            internal List<VertexBind> _vertexBinds = new List<VertexBind>();
        }
        private class VertexBind : ColladaEntry
        {
            internal string _semantic;
            internal string _inputSemantic;
            internal int _inputSet;
        }
        private class EffectShaderEntry : ColladaEntry
        {
            internal ShaderType _type;

            internal float _shininess, _reflectivity, _transparency;

            internal List<LightEffectEntry> _effects = new List<LightEffectEntry>();
        }
        private class LightEffectEntry : ColladaEntry
        {
            internal LightEffectType _type;
            internal RGBAPixel _color;

            internal string _texture;
            internal string _texCoord;
        }
        private enum ShaderType
        {
            None,
            phong,
            lambert,
            blinn
        }
        private enum LightEffectType
        {
            None,
            ambient,
            diffuse,
            emission,
            reflective,
            specular,
            transparent
        }
        private enum PrimitiveType
        {
            None,
            polygons,
            polylist,
            triangles,
            trifans,
            tristrips,
            lines,
            linestrips
        }
        private enum SemanticType
        {
            None,
            POSITION,
            VERTEX,
            NORMAL,
            TEXCOORD,
            COLOR,
            WEIGHT,
            JOINT,
            INV_BIND_MATRIX
        }
        private enum SourceType
        {
            None,
            Float,
            Int,
            Name
        }
        private enum NodeType
        {
            NODE,
            JOINT
        }

        private class DecoderShell : IDisposable
        {
            internal List<ImageEntry> _images = new List<ImageEntry>();
            internal List<MaterialEntry> _materials = new List<MaterialEntry>();
            internal List<EffectEntry> _effects = new List<EffectEntry>();
            internal List<GeometryEntry> _geometry = new List<GeometryEntry>();
            internal List<SkinEntry> _skins = new List<SkinEntry>();
            internal List<SceneEntry> _scenes = new List<SceneEntry>();
            internal XmlReader _reader;

            public static DecoderShell Import(string path)
            {
                using (FileMap map = FileMap.FromFile(path))
                using (XmlReader reader = new XmlReader(map.Address, map.Length))
                    return new DecoderShell(reader);
            }
            ~DecoderShell() { Dispose(); }
            public void Dispose()
            {
                foreach (GeometryEntry geo in _geometry)
                {

                }
            }

            private DecoderShell(XmlReader reader)
            {
                _reader = reader;

                while (reader.BeginElement())
                {
                    if (reader.Name.Equals("COLLADA", true))
                        ParseMain();

                    reader.EndElement();
                }

                _reader = null;
            }

            public NodeEntry FindNode(string name)
            {
                NodeEntry n;
                foreach (SceneEntry scene in _scenes)
                    foreach (NodeEntry node in scene._nodes)
                        if ((n = FindNodeInternal(name, node)) != null)
                            return n;
                return null;
            }
            internal static NodeEntry FindNodeInternal(string name, NodeEntry node)
            {
                NodeEntry e;

                if (node._name == name)
                    return node;

                foreach (NodeEntry n in node._children)
                    if ((e = FindNodeInternal(name, n)) != null)
                        return e;

                return null;
            }

            private void ParseMain()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("asset", true))
                        ParseAsset();
                    else if (_reader.Name.Equals("library_images", true))
                        ParseLibImages();
                    else if (_reader.Name.Equals("library_materials", true))
                        ParseLibMaterials();
                    else if (_reader.Name.Equals("library_effects", true))
                        ParseLibEffects();
                    else if (_reader.Name.Equals("library_geometries", true))
                        ParseLibGeometry();
                    else if (_reader.Name.Equals("library_controllers", true))
                        ParseLibControllers();
                    else if (_reader.Name.Equals("library_visual_scenes", true))
                        ParseLibScenes();

                    _reader.EndElement();
                }
            }

            private void ParseAsset()
            {
            }

            private void ParseLibImages()
            {
                ImageEntry img;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("image", true))
                    {
                        img = new ImageEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", true))
                                img._id = (string)_reader.Value;
                            else if (_reader.Name.Equals("name", true))
                                img._name = (string)_reader.Value;
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("init_from", true))
                                img._path = _reader.ReadElementString();

                            _reader.EndElement();
                        }

                        _images.Add(img);
                    }
                    _reader.EndElement();
                }
            }
            private void ParseLibMaterials()
            {
                MaterialEntry mat;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("material", true))
                    {
                        mat = new MaterialEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", true))
                                mat._id = (string)_reader.Value;
                            else if (_reader.Name.Equals("name", true))
                                mat._name = (string)_reader.Value;
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("instance_effect", true))
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("url", true))
                                        mat._effect = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                            _reader.EndElement();
                        }

                        _materials.Add(mat);
                    }

                    _reader.EndElement();
                }
            }
            private void ParseLibEffects()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("effect", true))
                        _effects.Add(ParseEffect());
                    _reader.EndElement();
                }
            }
            private EffectEntry ParseEffect()
            {
                EffectEntry eff = new EffectEntry();

                while (_reader.ReadAttribute())
                {
                    if (_reader.Name.Equals("id", true))
                        eff._id = (string)_reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        eff._name = (string)_reader.Value;
                }

                while (_reader.BeginElement())
                {
                    //Only common is supported
                    if (_reader.Name.Equals("profile_COMMON", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("technique", true))
                            {
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("phong", true))
                                        eff._shader = ParseShader(ShaderType.phong);
                                    else if (_reader.Name.Equals("lambert", true))
                                        eff._shader = ParseShader(ShaderType.lambert);
                                    else if (_reader.Name.Equals("blinn", true))
                                        eff._shader = ParseShader(ShaderType.blinn);

                                    _reader.EndElement();
                                }
                            }
                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }



                return eff;
            }
            private EffectShaderEntry ParseShader(ShaderType type)
            {
                EffectShaderEntry s = new EffectShaderEntry();
                s._type = type;
                float v;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("ambient", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.ambient));
                    else if (_reader.Name.Equals("diffuse", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.diffuse));
                    else if (_reader.Name.Equals("emission", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.emission));
                    else if (_reader.Name.Equals("reflective", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.reflective));
                    else if (_reader.Name.Equals("specular", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.specular));
                    else if (_reader.Name.Equals("transparent", true))
                        s._effects.Add(ParseLightEffect(LightEffectType.transparent));

                    else if (_reader.Name.Equals("shininess", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._shininess = v;
                            _reader.EndElement();
                        }
                    }
                    else if (_reader.Name.Equals("reflectivity", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._reflectivity = v;
                            _reader.EndElement();
                        }
                    }
                    else if (_reader.Name.Equals("transparency", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("float", true))
                                if (_reader.ReadValue(&v))
                                    s._transparency = v;
                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return s;
            }
            private LightEffectEntry ParseLightEffect(LightEffectType type)
            {
                LightEffectEntry eff = new LightEffectEntry();
                eff._type = type;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("color", true))
                        eff._color = ParseColor();
                    else if (_reader.Name.Equals("texture", true))
                    {
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("texture", true))
                                eff._texture = (string)_reader.Value;
                            else if (_reader.Name.Equals("texcoord", true))
                                eff._texCoord = (string)_reader.Value;
                    }

                    _reader.EndElement();
                }

                return eff;
            }
            private void ParseLibGeometry()
            {
                GeometryEntry geo;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("geometry", true))
                    {
                        geo = new GeometryEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", true))
                                geo._id = (string)_reader.Value;
                            else if (_reader.Name.Equals("name", true))
                                geo._name = (string)_reader.Value;
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("mesh", true))
                            {
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("source", true))
                                        geo._sources.Add(ParseSource());
                                    else if (_reader.Name.Equals("vertices", true))
                                    {
                                        while (_reader.ReadAttribute())
                                            if (_reader.Name.Equals("id", true))
                                                geo._verticesId = (string)_reader.Value;

                                        while (_reader.BeginElement())
                                        {
                                            if (_reader.Name.Equals("input", true))
                                                geo._verticesInput = ParseInput();

                                            _reader.EndElement();
                                        }
                                    }
                                    else if (_reader.Name.Equals("polygons", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.polygons));
                                    else if (_reader.Name.Equals("polylist", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.polylist));
                                    else if (_reader.Name.Equals("triangles", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.triangles));
                                    else if (_reader.Name.Equals("tristrips", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.tristrips));
                                    else if (_reader.Name.Equals("trifans", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.trifans));
                                    else if (_reader.Name.Equals("lines", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.lines));
                                    else if (_reader.Name.Equals("linestrips", true))
                                        geo._primitives.Add(ParsePrimitive(PrimitiveType.linestrips));

                                    _reader.EndElement();
                                }
                            }
                            _reader.EndElement();
                        }

                        _geometry.Add(geo);
                    }
                    _reader.EndElement();
                }
            }
            private PrimitiveEntry ParsePrimitive(PrimitiveType type)
            {
                PrimitiveEntry prim = new PrimitiveEntry() { _type = type };
                PrimitiveFace p;
                int val;
                int stride = 0, elements = 0;

                switch (type)
                {
                    case PrimitiveType.trifans:
                    case PrimitiveType.tristrips:
                    case PrimitiveType.triangles:
                        stride = 3;
                        break;
                    case PrimitiveType.lines:
                    case PrimitiveType.linestrips:
                        stride = 2;
                        break;
                    case PrimitiveType.polygons:
                    case PrimitiveType.polylist:
                        stride = 4;
                        break;
                }

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("material", true))
                        prim._material = (string)_reader.Value;
                    else if (_reader.Name.Equals("count", true))
                        prim._entryCount = int.Parse((string)_reader.Value);

                prim._faces.Capacity = prim._entryCount;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("input", true))
                    {
                        prim._inputs.Add(ParseInput());
                        elements++;
                    }
                    else if (_reader.Name.Equals("p", true))
                    {
                        List<ushort> indices = new List<ushort>(stride * elements);

                        p = new PrimitiveFace();
                        //p._pointIndices.Capacity = stride * elements;
                        while (_reader.ReadValue(&val))
                            indices.Add((ushort)val);

                        p._pointCount = indices.Count / elements;
                        p._pointIndices = indices.ToArray();

                        switch (type)
                        {
                            case PrimitiveType.trifans:
                            case PrimitiveType.tristrips:
                            case PrimitiveType.polygons:
                            case PrimitiveType.polylist:
                                p._faceCount = p._pointCount - 2;
                                break;

                            case PrimitiveType.triangles:
                                p._faceCount = p._pointCount / 3;
                                break;

                            case PrimitiveType.lines:
                                p._faceCount = p._pointCount / 2;
                                break;

                            case PrimitiveType.linestrips:
                                p._faceCount = p._pointCount - 1;
                                break;
                        }

                        prim._faceCount += p._faceCount;
                        prim._pointCount += p._pointCount;
                        prim._faces.Add(p);
                    }

                    _reader.EndElement();
                }

                prim._entryStride = elements;

                return prim;
            }
            private InputEntry ParseInput()
            {
                InputEntry inp = new InputEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        inp._id = (string)_reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        inp._name = (string)_reader.Value;
                    else if (_reader.Name.Equals("semantic", true))
                        inp._semantic = (SemanticType)Enum.Parse(typeof(SemanticType), (string)_reader.Value, true);
                    else if (_reader.Name.Equals("set", true))
                        inp._set = int.Parse((string)_reader.Value);
                    else if (_reader.Name.Equals("offset", true))
                        inp._offset = int.Parse((string)_reader.Value);
                    else if (_reader.Name.Equals("source", true))
                        inp._source = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                return inp;
            }
            private SourceEntry ParseSource()
            {
                SourceEntry src = new SourceEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        src._id = (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("float_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Float;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string)_reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string)_reader.Value);

                            UnsafeBuffer buffer = new UnsafeBuffer(src._arrayCount * 4);
                            src._arrayData = buffer;

                            float* pOut = (float*)buffer.Address;
                            for (int i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadValue(pOut++))
                                    break;
                        }
                    }
                    else if (_reader.Name.Equals("int_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Int;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string)_reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string)_reader.Value);

                            UnsafeBuffer buffer = new UnsafeBuffer(src._arrayCount * 4);
                            src._arrayData = buffer;

                            int* pOut = (int*)buffer.Address;
                            for (int i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadValue(pOut++))
                                    break;
                        }
                    }
                    else if (_reader.Name.Equals("Name_array", true))
                    {
                        if (src._arrayType == SourceType.None)
                        {
                            src._arrayType = SourceType.Name;

                            while (_reader.ReadAttribute())
                                if (_reader.Name.Equals("id", true))
                                    src._arrayId = (string)_reader.Value;
                                else if (_reader.Name.Equals("count", true))
                                    src._arrayCount = int.Parse((string)_reader.Value);

                            string[] list = new string[src._arrayCount];
                            src._arrayData = list;

                            for (int i = 0; i < src._arrayCount; i++)
                                if (!_reader.ReadStringSingle())
                                    break;
                                else
                                    list[i] = (string)_reader.Value;
                        }
                    }
                    else if (_reader.Name.Equals("technique_common", true))
                    {
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("accessor", true))
                            {
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("source", true))
                                        src._accessorSource = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;
                                    else if (_reader.Name.Equals("count", true))
                                        src._accessorCount = int.Parse((string)_reader.Value);
                                    else if (_reader.Name.Equals("stride", true))
                                        src._accessorStride = int.Parse((string)_reader.Value);

                                //Ignore params
                            }

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return src;
            }

            private void ParseLibControllers()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("controller", false))
                    {
                        string id = null;
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("id", false))
                                id = (string)_reader.Value;

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("skin", false))
                                _skins.Add(ParseSkin(id));

                            _reader.EndElement();
                        }
                    }
                    _reader.EndElement();
                }
            }

            private SkinEntry ParseSkin(string id)
            {
                SkinEntry skin = new SkinEntry();
                skin._id = id;

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("source", false))
                        skin._skinSource = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_shape_matrix", false))
                        skin._bindMatrix = ParseMatrix();
                    else if (_reader.Name.Equals("source", false))
                        skin._sources.Add(ParseSource());
                    else if (_reader.Name.Equals("joints", false))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("input", false))
                                skin._jointInputs.Add(ParseInput());

                            _reader.EndElement();
                        }
                    else if (_reader.Name.Equals("vertex_weights", false))
                    {
                        while (_reader.ReadAttribute())
                            if (_reader.Name.Equals("count", false))
                                skin._weightCount = int.Parse((string)_reader.Value);

                        skin._weights = new int[skin._weightCount][];

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("input", false))
                                skin._weightInputs.Add(ParseInput());
                            else if (_reader.Name.Equals("vcount", false))
                            {
                                for (int i = 0; i < skin._weightCount; i++)
                                {
                                    int val;
                                    _reader.ReadValue(&val);
                                    skin._weights[i] = new int[val * skin._weightInputs.Count];
                                }
                            }
                            else if (_reader.Name.Equals("v", false))
                            {
                                for (int i = 0; i < skin._weightCount; i++)
                                {
                                    int[] weights = skin._weights[i];
                                    fixed (int* p = weights)
                                        for (int x = 0; x < weights.Length; x++)
                                            _reader.ReadValue(&p[x]);
                                }
                            }
                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }

                return skin;
            }

            private void ParseLibScenes()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("visual_scene", true))
                        _scenes.Add(ParseScene());

                    _reader.EndElement();
                }
            }
            private SceneEntry ParseScene()
            {
                SceneEntry sc = new SceneEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        sc._id = (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("node", true))
                        sc._nodes.Add(ParseNode());

                    _reader.EndElement();
                }

                return sc;
            }
            private NodeEntry ParseNode()
            {
                NodeEntry node = new NodeEntry();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("id", true))
                        node._id = (string)_reader.Value;
                    else if (_reader.Name.Equals("name", true))
                        node._name = (string)_reader.Value;
                    else if (_reader.Name.Equals("type", true))
                        node._type = (NodeType)Enum.Parse(typeof(NodeType), (string)_reader.Value, true);

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("matrix", true))
                        node._transform = ParseMatrix().Derive();
                    else if (_reader.Name.Equals("node", true))
                        node._children.Add(ParseNode());
                    else if (_reader.Name.Equals("instance_controller", true))
                        node._instances.Add(ParseInstance(true));
                    else if (_reader.Name.Equals("instance_geometry", true))
                        node._instances.Add(ParseInstance(false));

                    _reader.EndElement();
                }

                return node;
            }

            private InstanceEntry ParseInstance(bool controller)
            {
                InstanceEntry c = new InstanceEntry();
                c._isController = controller;

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("url", true))
                        c._url = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_material", true))
                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("technique_common", true))
                                while (_reader.BeginElement())
                                {
                                    if (_reader.Name.Equals("instance_material", true))
                                        c._material = ParseMatInstance();
                                    _reader.EndElement();
                                }
                            _reader.EndElement();
                        }

                    _reader.EndElement();
                }

                return c;
            }

            private InstanceMaterial ParseMatInstance()
            {
                InstanceMaterial mat = new InstanceMaterial();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("symbol", true))
                        mat._symbol = (string)_reader.Value;
                    else if (_reader.Name.Equals("target", true))
                        mat._target = _reader.Value[0] == '#' ? (string)(_reader.Value + 1) : (string)_reader.Value;

                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("bind_vertex_input", true))
                        mat._vertexBinds.Add(ParseVertexInput());
                    _reader.EndElement();
                }
                return mat;
            }
            private VertexBind ParseVertexInput()
            {
                VertexBind v = new VertexBind();

                while (_reader.ReadAttribute())
                    if (_reader.Name.Equals("semantic", true))
                        v._semantic = (string)_reader.Value;
                    else if (_reader.Name.Equals("input_semantic", true))
                        v._inputSemantic = (string)_reader.Value;
                    else if (_reader.Name.Equals("input_set", true))
                        v._inputSet = int.Parse((string)_reader.Value);

                return v;
            }

            private Matrix ParseMatrix()
            {
                Matrix m;
                float* pM = (float*)&m;
                for (int y = 0; y < 4; y++)
                    for (int x = 0; x < 4; x++)
                        _reader.ReadValue(&pM[x * 4 + y]);
                return m;
            }
            private RGBAPixel ParseColor()
            {
                float f;
                RGBAPixel c;
                byte* p = (byte*)&c;
                for (int i = 0; i < 4; i++)
                {
                    if (!_reader.ReadValue(&f))
                        p[i] = 255;
                    else
                        p[i] = (byte)(f * 255.0f + 0.5f);
                }
                return c;
            }
        }
    }
}
