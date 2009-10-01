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
        private int _index, _portraitIndex;
        private string _path;

        public CostumeDefinition(CharacterDefinition character, int index) :
            base("system\\common5.pac", String.Format("sc_selcharacter_en/char_bust_tex_lz77/Type1[{0}]/Textures(NW4R)/MenSelchrFaceB.{1:000}", character.CharacterIndex, character.PortraitId + index))
        {
            _character = character;
            _index = index;
            _portraitIndex = _character.PortraitId + _index;
            _path = _character.GetCostumePath(index);
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
                        //string path = _character.GetCostumePath(_index);

                        ARCNode node = ResourceCache.FindNode(_path, null) as ARCNode;

                        ResourceNode[] nodes = node.FindChildrenByType(null, ResourceType.TEX0);
                        TextureDefinition[] textures = new TextureDefinition[nodes.Length];
                        for (int i = 0; i < nodes.Length; i++)
                            textures[i] = new TextureDefinition(_path, nodes[i].TreePath);

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
            //string path = _character.GetCostumePath(_index);
            int filter = (Program.SaveFile(Filters.CostumeExportFilter, Path.GetFileName(_path), out outFile, false));
            if (filter == 0)
                return;

            ResourceNode node = ResourceCache.FindNode(_path, null);
            if (node.IsDirty)
                node.Rebuild(false);

            switch (filter)
            {
                case 1: { ((ARCNode)node).ExportPair(outFile); break; }
                case 2: { ((ARCNode)node).ExportPAC(outFile); break; }
                case 3: { ((ARCNode)node).ExportPCS(outFile); break; }
            }
        }

        internal bool ImportCostume()
        {
            string inFile;
            //string path = _character.GetCostumePath(_index);
            if (Program.OpenFile(Filters.CostumeImportFilter, out inFile, false) == 0)
                return false;

            //Replace file in cache
            ResourceCache.LoadExternal(_path, inFile);
            return true;
        }

        internal bool RestoreCostume()
        {
            return ResourceCache.Restore(_path);
        }

        internal void Reset()
        {
            if (_textures != null)
            {
                foreach (TextureDefinition def in _textures)
                    def.Reset();
                _textures = null;
            }
        }

        private TextureReference _stockRef;
        public TextureReference StockPortrait 
        {
            get
            {
                if (_stockRef == null)
                {
                    _stockRef = NodeReference.Get<TextureReference>("system\\common5.pac", String.Format("sc_selcharacter_en/Type1[90]/Textures(NW4R)/InfStc.{0:000}", _portraitIndex));
                }
                return _stockRef;
            }
        }

        private TextureReference _gameRef;
        public TextureReference GamePortrait 
        {
            get
            {
                if (_gameRef == null)
                {
                    _gameRef = NodeReference.Get<TextureReference>(String.Format("info\\portrite\\InfFace{0:000}.brres", _portraitIndex), String.Format("Textures(NW4R)/InfFace.{0:000}", _portraitIndex));
                }
                return _gameRef;
            }
        }
    }
}
