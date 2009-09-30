using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;

namespace BrawlScape
{
    public class CharacterDefinition : ListViewItem
    {
        public static List<CharacterDefinition> List = new List<CharacterDefinition>();
        static CharacterDefinition()
        {
            List.Add(new CharacterDefinition("Mario", "Mario", 0));
            List.Add(new CharacterDefinition("Donkey Kong", "Donkey", 1));
            List.Add(new CharacterDefinition("Link", "Link", 2));
            List.Add(new CharacterDefinition("Samus", "Samus", 3));
            List.Add(new CharacterDefinition("Yoshi", "Yoshi", 4));
            List.Add(new CharacterDefinition("Kirby", "Kirby", 5));
            List.Add(new CharacterDefinition("Fox", "Fox", 6));
            List.Add(new CharacterDefinition("Pikachu", "Pikachu", 7));
            List.Add(new CharacterDefinition("Luigi", "Luigi", 8));
            List.Add(new CharacterDefinition("Captain Falcon", "Captain", 9));
            List.Add(new CharacterDefinition("Ness", "Ness", 10));
            List.Add(new CharacterDefinition("Bowser", "Koopa", 11));
            List.Add(new CharacterDefinition("Peach", "Peach", 12));
            List.Add(new CharacterDefinition("Zelda", "Zelda", 13));
            List.Add(new CharacterDefinition("Sheik", "Sheik", 14));
            List.Add(new CharacterDefinition("Ice Climbers", "Popo", 15));
            List.Add(new CharacterDefinition("Marth", "Marth", 16));
            List.Add(new CharacterDefinition("Game and Watch", "GameWatch", 17));
            List.Add(new CharacterDefinition("Falco", "Falco", 18));
            List.Add(new CharacterDefinition("Ganondorf", "Ganon", 19));
            List.Add(new CharacterDefinition("Meta Knight", "Metaknight", 21));
            List.Add(new CharacterDefinition("Pit", "Pit", 22));
            List.Add(new CharacterDefinition("Zero Suit Samus", "SZeroSuit", 23));
            List.Add(new CharacterDefinition("Olimar", "Pikmin", 24));
            List.Add(new CharacterDefinition("Lucas", "Lucas", 25));
            List.Add(new CharacterDefinition("Diddy Kong", "Diddy", 26));
            List.Add(new CharacterDefinition("Pokemon Trainer", "PokeTrainer", 27));
            List.Add(new CharacterDefinition("Charizard", "PokeLizardon", 28));
            List.Add(new CharacterDefinition("Squirtle", "PokeZenigame", 29));
            List.Add(new CharacterDefinition("Ivysaur", "PokeFushigisou", 30));
            List.Add(new CharacterDefinition("King Dedede", "Dedede", 31));
            List.Add(new CharacterDefinition("Lucario", "Lucario", 32));
            List.Add(new CharacterDefinition("Ike", "Ike", 33));
            List.Add(new CharacterDefinition("ROB", "Robot", 34));
            List.Add(new CharacterDefinition("Jigglypuff", "Purin", 36));
            List.Add(new CharacterDefinition("Wario", "Wario", 37));
            List.Add(new CharacterDefinition("Toon Link", "ToonLink", 40));
            List.Add(new CharacterDefinition("Wolf", "Wolf", 43));
            List.Add(new CharacterDefinition("Snake", "Snake", 45));
            List.Add(new CharacterDefinition("Sonic", "Sonic", 46));
        }

        //private ResourceNode _csfNode;
        //private ResourceNode CSFNode 
        //{
        //    get
        //    {
        //        return ResourceCache.FindNode("system\\common5_en.pac", String.Format("sc_selcharacter_en/Type1[70]/MenSelchrChrFace.{0:000}", _csfIndex));
        //    }
        //}

        private string _fitPath;
        public string FighterPath { get { return _fitPath; } }

        private int _index;
        public int CharacterIndex { get { return _index; } }
        //public int CSFIndex { get{return _csfIndex;} }

        public TEX0Node CSFNode
        {
            get
            {
                return ResourceCache.FindNode("system\\common5.pac", String.Format("sc_selcharacter_en/Type1[70]/MenSelchrChrFace.{0:000}", _index + 1)) as TEX0Node;
            }
        }

        public BRESNode CSPNode
        {
            get
            {
                foreach (BRESNode node in ResourceCache.FindNode("system\\common5.pac", "sc_selcharacter_en/char_bust_tex_lz77").Children)
                    if (node.FileIndex == _index)
                        return node;
                return null;
            }
        }

        private CharacterDefinition(string name, string path, int index)
        {
            Text = name;
            _fitPath = path;
            _index = index;
        }

        public Image GetCSF()
        {
            ResourceNode n = CSFNode;
            if (n is IImageSource)
                return ((IImageSource)n).GetImage(0);
            return null;
        }

        CostumeDefinition[] _costumes;
        public CostumeDefinition[] Costumes
        {
            get
            {
                if (_costumes == null)
                {
                    try
                    {
                        ResourceNode node = CSPNode.FindChild("Textures(NW4R)", false);

                        _costumes = new CostumeDefinition[node.Children.Count];
                        for (int i = 0; i < node.Children.Count; i++)
                            _costumes[i] = new CostumeDefinition(this, i);
                    }
                    catch (Exception x) { MessageBox.Show(x.Message); _costumes = new CostumeDefinition[0]; }
                }
                return _costumes;
            }
        }

        private static readonly int[,] _costumeIds = new int[47,12] { 
        {0,6,3,4,5,2,0,0,0,0,0,0}, //mario
        {0,4,1,3,2,5,0,0,0,0,0,0}, //donkey
        {0,1,3,5,6,4,0,0,0,0,0,0}, //link
        {0,3,1,5,4,2,0,0,0,0,0,0}, //samus
        {0,1,3,4,5,6,0,0,0,0,0,0}, //yoshi
        {0,4,3,1,2,5,0,0,0,0,0,0}, //kirby
        {0,4,1,2,3,5,0,0,0,0,0,0}, //fox
        {0,1,2,3,0,0,0,0,0,0,0,0}, //pikachu
        {0,5,1,3,4,6,0,0,0,0,0,0}, //luigi
        {0,4,1,2,3,5,0,0,0,0,0,0}, //captain
        {0,5,4,2,3,6,0,0,0,0,0,0}, //ness
        {0,4,1,3,5,6,0,0,0,0,0,0}, //koopa
        {0,5,1,3,2,4,0,0,0,0,0,0}, //peach
        {0,1,3,5,2,4,0,0,0,0,0,0}, //zelda
        {0,1,3,5,2,4,0,0,0,0,0,0}, //sheik
        {0,1,3,4,2,5,0,0,0,0,0,0}, //popo
        {0,1,2,4,5,3,0,0,0,0,0,0}, //marth
        {0,0,0,0,0,0,0,0,0,0,0,0}, //gamewatch
        {0,5,3,1,2,4,0,0,0,0,0,0}, //falco
        {0,4,3,2,1,5,0,0,0,0,0,0}, //ganon
        {0,0,0,0,0,0,0,0,0,0,0,0},
        {0,4,1,2,3,5,0,0,0,0,0,0}, //metaknight
        {0,4,1,2,3,5,0,0,0,0,0,0}, //pit
        {0,3,1,5,4,2,0,0,0,0,0,0}, //szerosuit
        {0,4,1,5,2,3,0,0,0,0,0,0}, //pikmin
        {0,4,1,3,2,5,0,0,0,0,0,0}, //lucas
        {0,5,4,6,2,3,0,0,0,0,0,0}, //diddy
        {0,1,2,3,4,0,0,0,0,0,0,0}, //poketrainer
        {0,1,2,3,4,0,0,0,0,0,0,0}, //pokelizardon
        {0,1,2,3,4,0,0,0,0,0,0,0}, //pokezenigame
        {0,1,2,3,4,0,0,0,0,0,0,0}, //pokefushigisou
        {0,6,2,5,3,4,0,0,0,0,0,0}, //dedede
        {0,1,4,5,2,0,0,0,0,0,0,0}, //lucario
        {0,5,1,3,2,4,0,0,0,0,0,0}, //ike
        {0,6,5,4,3,2,0,0,0,0,0,0}, //robot
        {0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,1,4,3,2,0,0,0,0,0,0,0}, //purin
        {0,1,5,2,4,3,6,7,9,8,10,11}, //wario
        {0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,1,3,4,5,6,0,0,0,0,0,0}, //toonlink
        {0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,1,4,2,3,5,0,0,0,0,0,0}, //wolf
        {0,0,0,0,0,0,0,0,0,0,0,0}, 
        {0,1,3,4,2,5,0,0,0,0,0,0}, //snake
        {0,5,4,2,1,0,0,0,0,0,0,0}  //sonic
        };

        internal string GetCostumePath(int index)
        {
            return String.Format("fighter\\{0}\\Fit{1}{2:00}", _fitPath.ToLower(), _fitPath, _costumeIds[_index,index]);
        }
    }
}
