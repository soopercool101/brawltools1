using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using BrawlLib.IO;

namespace BrawlLib.Modeling
{
    public static unsafe partial class Collada
    {
        public static MDL0Node ImportModel(string filePath)
        {
            MDL0Node model = new MDL0Node() { _name = Path.GetFileNameWithoutExtension(filePath) };
            model.InitGroups();

            using (FileMap map = FileMap.FromFile(filePath))
            {
                XmlReader reader = new XmlReader(map.Address, map.Length);
                while (reader.BeginElement())
                {
                    if (reader.Name.Equals("COLLADA", StringComparison.OrdinalIgnoreCase))
                    {
                        ParseCollada(model, reader);
                    }
                    reader.EndElement();
                }
            }

            model.CleanGroups();
            return model;
        }

        private class ColladaEntry
        {
            internal string _id, _name;
        }
        private class ImageEntry : ColladaEntry
        {
            internal string _path;
        }
        private class MaterialEntry : ColladaEntry
        {
            internal string _effect;
        }

        private class DecoderShell
        {
            internal List<ImageEntry> _textures = new List<ImageEntry>();
            internal List<MaterialEntry> _materials = new List<MaterialEntry>();
            internal XmlReader _reader;

            public static DecoderShell Import(string path)
            {
                using (FileMap map = FileMap.FromFile(path))
                    return new DecoderShell(new XmlReader(map.Address, map.Length));
            }

            private DecoderShell(XmlReader reader)
            {
                _reader = reader;

                while (reader.BeginElement())
                {
                    if (reader.Name.Equals("COLLADA", StringComparison.OrdinalIgnoreCase))
                        ParseMain();

                    reader.EndElement();
                }

                _reader = null;
            }

            private void ParseMain()
            {
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("asset", StringComparison.OrdinalIgnoreCase))
                        ParseAsset();
                    else if (_reader.Name.Equals("library_images", StringComparison.OrdinalIgnoreCase))
                        ParseLibImages();
                    else if (_reader.Name.Equals("library_materials", StringComparison.OrdinalIgnoreCase))
                        ParseLibMaterials();
                    else if (_reader.Name.Equals("library_effects", StringComparison.OrdinalIgnoreCase))
                        ParseLibEffects();
                    else if (_reader.Name.Equals("library_geometry", StringComparison.OrdinalIgnoreCase))
                        ParseLibGeometry();

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
                    if (_reader.Name.Equals("image", StringComparison.OrdinalIgnoreCase))
                    {
                        img = new ImageEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                                img._id = String.Copy(_reader.Value);
                            else if (_reader.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                                img._name = String.Copy(_reader.Value);
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("init_from", StringComparison.OrdinalIgnoreCase))
                                img._path = _reader.ReadString();

                            _reader.EndElement();
                        }

                    }
                    _reader.EndElement();
                }
            }
            private void ParseLibMaterials()
            {
                MaterialEntry mat;
                while (_reader.BeginElement())
                {
                    if (_reader.Name.Equals("material", StringComparison.OrdinalIgnoreCase))
                    {
                        mat = new MaterialEntry();
                        while (_reader.ReadAttribute())
                        {
                            if (_reader.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                                mat._id = String.Copy(_reader.Value);
                            else if (_reader.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                                mat._name = String.Copy(_reader.Value);
                        }

                        while (_reader.BeginElement())
                        {
                            if (_reader.Name.Equals("instance_effect", StringComparison.OrdinalIgnoreCase))
                                while (_reader.ReadAttribute())
                                    if (_reader.Name.Equals("url", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (_reader.Value.StartsWith("#"))
                                            mat._effect = _reader.Value.Substring(1);
                                        else
                                            mat._effect = String.Copy(_reader.Value);
                                    }

                            _reader.EndElement();
                        }
                    }

                    _reader.EndElement();
                }
            }
            private void ParseLibEffects()
            {
            }
            private void ParseLibGeometry()
            {
                while (_reader.BeginElement())
                {
                    _reader.EndElement();
                }
            }
        }

        public static void ParseCollada(MDL0Node model, XmlReader reader)
        {
            while (reader.BeginElement())
            {
                if (reader.Name.Equals("asset", StringComparison.OrdinalIgnoreCase))
                {
                }
                else if (reader.Name.Equals("library_images", StringComparison.OrdinalIgnoreCase))
                {
                    ParseImages(model, reader);
                }
                reader.EndElement();
            }
        }

        public static void ParseAsset(MDL0Node model, XmlReader reader)
        {

        }
        public static void ParseImages(MDL0Node model, XmlReader reader)
        {
            while (reader.BeginElement())
            {
                if (reader.Name.Equals("image", StringComparison.OrdinalIgnoreCase))
                {
                    string id = null, name = null;
                    while (reader.ReadAttribute())
                    {
                        if (reader.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                            id = String.Copy(reader.Value);
                        else if (reader.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                            name = String.Copy(reader.Value);
                    }

                    if (id != null)
                    {
                        MDL0TextureNode tex = new MDL0TextureNode();
                        tex._name = name != null ? name : id;
                        model._texList.Add(tex);
                    }
                }
                reader.EndElement();
            }
        }
    }
}
