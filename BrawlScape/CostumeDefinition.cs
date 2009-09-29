using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.Drawing;
using System.IO;
using BrawlLib.Wii.Compression;

namespace BrawlScape
{
    public class CostumeDefinition : TextureDefinition
    {
        private CharacterDefinition _character;
        private int _index;

        public CostumeDefinition(CharacterDefinition character, int index) : base("system\\common5.pac", character.CSPNode.FindChild("Textures(NW4R)", false).Children[index].TreePath)
        {
            _character = character;
            _index = index;
        }

        private TextureDefinition[] _textures;
        public TextureDefinition[] Textures
        {
            get
            {
                if (_textures == null)
                {
                    try
                    {
                        //Find outfit in fighter folder
                        string path = _character.GetCostumePath(_index);

                        ARCNode node = ResourceCache.FindNode(path, null) as ARCNode;
                        node.IsPair = true;

                        ResourceNode[] nodes = node.FindChildrenByType(null, ResourceType.TEX0);
                        TextureDefinition[] textures = new TextureDefinition[nodes.Length];
                        for (int i = 0; i < nodes.Length; i++)
                            textures[i] = new TextureDefinition(path, nodes[i].TreePath);

                        _textures = textures;
                    }
                    catch (Exception x) { MessageBox.Show(x.Message); return new TextureDefinition[0]; }
                }
                return _textures;
            }
        }

        internal void ExportCostume()
        {
            string outFile;
            string path = _character.GetCostumePath(_index);
            int filter = (Program.SaveFile(Filters.CostumeExportFilter, Path.GetFileName(path), out outFile, false));
            if (filter == 0)
                return;

            ResourceNode node = ResourceCache.FindNode(path, null);
            if (node.IsDirty)
                node.Rebuild(false);

            switch (filter)
            {
                case 1: { ((ARCNode)node).ExportPair(outFile); break; }
                case 2: { ((ARCNode)node).ExportPAC(outFile); break; }
                case 3: { ((ARCNode)node).ExportPCS(outFile); break; }
            }
        }

        internal void ImportCostume()
        {
            //string inFile;
            //string path = _character.GetCostumePath(_index);
            //if (Program.OpenFile(Filters.CostumeImportFilter, out inFile, false) == 0)
            //    return;
        }

        internal void RestoreCostume()
        {

        }
    }
}
