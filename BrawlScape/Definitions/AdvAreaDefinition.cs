using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlScape
{
    public class AdvAreaDefinition : ResourceDefinition<NodeReference>, IListSource<ModelDefinition>, IListSource<TextureDefinition>
    {
        private string _path;

        public AdvAreaDefinition(string name)
            : base(String.Format("stage\\adventure\\{0}.pac", name), null)
        {
            Text = name;
            _path = String.Format("stage\\adventure\\{0}.pac", name);
        }

        private ModelDefinition[] _models;
        public ModelDefinition[] ListItems
        {
            get
            {
                if (_models == null)
                {
                    ResourceNode[] nodes = ResourceCache.FindNodeByType(_path, null, ResourceType.MDL0);
                    if (nodes != null)
                    {
                        ModelDefinition[] models = new ModelDefinition[nodes.Length];
                        for (int i = 0; i < nodes.Length; i++)
                            models[i] = new ModelDefinition(_path, nodes[i].TreePath);
                        _models = models;
                    }
                }
                return _models;
            }
        }

        private TextureDefinition[] _textures;
        TextureDefinition[] IListSource<TextureDefinition>.ListItems
        {
            get
            {
                if (_textures == null)
                {
                    ResourceNode[] nodes = ResourceCache.FindNodeByType(_path, null, ResourceType.TEX0);
                    if (nodes != null)
                    {
                        TextureDefinition[] textures = new TextureDefinition[nodes.Length];
                        for (int i = 0; i < nodes.Length; i++)
                            textures[i] = new TextureDefinition(_path, nodes[i].TreePath);

                        _textures = textures;
                    }
                }
                return _textures;
            }
        }
    }
}
