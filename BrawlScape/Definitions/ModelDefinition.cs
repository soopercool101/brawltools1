using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlScape
{
    public class ModelDefinition : ResourceDefinition<ModelReference>, IListSource<TextureDefinition>
    {
        public ModelDefinition(string relativePath, string nodePath) : base(relativePath, nodePath) { }

        public override string ToString() { return Text; }

        public GLModel Model { get { return _nodeRef == null ? null : _nodeRef.Model; } }

        private TextureDefinition[] _textures;
        public TextureDefinition[] ListItems
        {
            get 
            {
                if ((_textures == null) && (_nodeRef != null) && (_nodeRef.Node != null) && (_nodeRef.Model != null))
                {
                    ResourceNode node = _nodeRef.Node;
                    ResourceNode root = node.RootNode;
                    ResourceTree tree = _nodeRef.Tree;
                    GLModel model = Model;

                    List<TextureDefinition> list = new List<TextureDefinition>();

                    for (int i = 0; i < 2; i++)
                    {
                        ResourceNode tex = node.FindChild("Textures" + i.ToString(), false);
                        if (tex != null)
                        {
                            foreach (ResourceNode n in tex.Children)
                            {
                                ResourceNode tNode = root.FindChild("Textures(NW4R)/" + n.Name, true);
                                if (tNode is TEX0Node)
                                {
                                    TextureDefinition tdef = new TextureDefinition(tree.RelativePath, tNode.TreePath);
                                    tdef.Changed += TextureChanged;
                                    list.Add(tdef);
                                }
                            }
                            _textures = list.ToArray();
                            break;
                        }
                    }
                }
                return _textures;
            }
        }

        public void AttachTextures()
        {
            GLModel model = Model;
            if ((ListItems != null) && (model != null))
            {
                foreach (TextureDefinition tex in _textures)
                {
                    if (tex.Text == "TShadow1")
                        continue;

                    //model.AttachTexture(tex.Text, tex.Texture);
                }
            }
        }

        private void TextureChanged(ResourceDefinition<TextureReference> tex)
        {
            GLModel model = Model;
            if (model != null)
            {
                //model.AttachTexture(tex.Text, (tex as TextureDefinition).Texture);
                MainForm.ActiveForm.Invalidate(true);
            }
        }
    }
}
