using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace BrawlScape
{
    public class CharacterDefinition : TextureDefinition, IListSource<CostumeDefinition>
    {
        public static List<CharacterDefinition> List = new List<CharacterDefinition>();
        static CharacterDefinition()
        {
            List.Add(new CharacterDefinition("Mario", "Mario", 0, 6));
            List.Add(new CharacterDefinition("Donkey Kong", "Donkey", 1, 6));
            List.Add(new CharacterDefinition("Link", "Link", 2, 6));
            List.Add(new CharacterDefinition("Samus", "Samus", 3, 6));
            List.Add(new CharacterDefinition("Yoshi", "Yoshi", 4, 6));
            List.Add(new CharacterDefinition("Kirby", "Kirby", 5, 6));
            List.Add(new CharacterDefinition("Fox", "Fox", 6, 6));
            List.Add(new CharacterDefinition("Pikachu", "Pikachu", 7, 4));
            List.Add(new CharacterDefinition("Luigi", "Luigi", 8, 6));
            List.Add(new CharacterDefinition("Captain Falcon", "Captain", 9, 6));
            List.Add(new CharacterDefinition("Ness", "Ness", 10, 6));
            List.Add(new CharacterDefinition("Bowser", "Koopa", 11, 6));
            List.Add(new CharacterDefinition("Peach", "Peach", 12, 6));
            List.Add(new CharacterDefinition("Zelda", "Zelda", 13, 6));
            List.Add(new CharacterDefinition("Sheik", "Sheik", 14, 6));
            List.Add(new CharacterDefinition("Ice Climbers", "Popo", 15, 6));
            List.Add(new CharacterDefinition("Marth", "Marth", 16, 6));
            List.Add(new CharacterDefinition("Game & Watch", "GameWatch", 17, 6));
            List.Add(new CharacterDefinition("Falco", "Falco", 18, 6));
            List.Add(new CharacterDefinition("Ganondorf", "Ganon", 19, 6));
            List.Add(new CharacterDefinition("Meta Knight", "Metaknight", 21, 6));
            List.Add(new CharacterDefinition("Pit", "Pit", 22, 6));
            List.Add(new CharacterDefinition("Zero Suit Samus", "SZeroSuit", 23, 6));
            List.Add(new CharacterDefinition("Olimar", "Pikmin", 24, 6));
            List.Add(new CharacterDefinition("Lucas", "Lucas", 25, 6));
            List.Add(new CharacterDefinition("Diddy Kong", "Diddy", 26, 6));
            List.Add(new CharacterDefinition("Pokemon Trainer", "PokeTrainer", 27, 5));
            List.Add(new CharacterDefinition("Charizard", "PokeLizardon", 28, 5));
            List.Add(new CharacterDefinition("Squirtle", "PokeZenigame", 29, 5));
            List.Add(new CharacterDefinition("Ivysaur", "PokeFushigisou", 30, 5));
            List.Add(new CharacterDefinition("King Dedede", "Dedede", 31, 6));
            List.Add(new CharacterDefinition("Lucario", "Lucario", 32, 5));
            List.Add(new CharacterDefinition("Ike", "Ike", 33, 6));
            List.Add(new CharacterDefinition("ROB", "Robot", 34, 6));
            List.Add(new CharacterDefinition("Jigglypuff", "Purin", 36, 5));
            List.Add(new CharacterDefinition("Wario", "Wario", 37, 12));
            List.Add(new CharacterDefinition("Toon Link", "ToonLink", 40, 6));
            List.Add(new CharacterDefinition("Wolf", "Wolf", 43, 6));
            List.Add(new CharacterDefinition("Snake", "Snake", 45, 6));
            List.Add(new CharacterDefinition("Sonic", "Sonic", 46, 5));
        }


        private string _fitName;
        public string FighterName { get { return _fitName; } }

        private int _index;
        public int CharacterIndex { get { return _index; } }

        public int PortraitId { get { return (_index * 10) + 1; } }

        private int _costumeCount;
        public int CostumeCount { get { return _costumeCount; } }

        private TextureReference _charNameRef;
        public TextureReference NameReference { get { return _charNameRef; } }

        private CharacterDefinition(string name, string fitName, int index, int costumeCount)
            : base("system\\common5.pac", String.Format("sc_selcharacter_en/Type1[70]/Textures(NW4R)/MenSelchrChrFace.{0:000}", index + 1))
        {
            Text = name;
            _fitName = fitName;
            _index = index;
            _costumeCount = costumeCount;
            _charNameRef = NodeReference.Get<TextureReference>("system\\common5.pac", String.Format("sc_selcharacter_en/Type1[70]/Textures(NW4R)/MenSelchrChrNmS.{0:000}", index + 1));
            _charNameRef.DataChanged += OnChanged;
        }

        protected override void OnChanged(NodeReference r)
        {
            if (_texture != null)
            {
                _texture.Dispose();
                _texture = null;
            }
            base.OnChanged(r);
        }

        private Bitmap _texture;
        public override Bitmap Texture
        {
            get
            {
                if (_texture == null)
                {
                    Bitmap icon = base.Texture;
                    Bitmap name = _charNameRef.Texture;

                    _texture = new Bitmap(80, 56, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(_texture))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        if (icon != null)
                            g.DrawImageUnscaled(icon, 0, 0);
                        if (name != null)
                            g.DrawImage(name, 0, 0, 80, 16);
                    }
                }
                return _texture;
            }
        }

        CostumeDefinition[] _costumes;
        public CostumeDefinition[] ListItems
        {
            get
            {
                if (_costumes == null)
                {
                    _costumes = new CostumeDefinition[_costumeCount];
                    for (int i = 0; i < _costumeCount; i++)
                        _costumes[i] = new CostumeDefinition(this, i);
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
            return String.Format("fighter\\{0}\\Fit{1}{2:00}", _fitName.ToLower(), _fitName, _costumeIds[_index,index]);
        }


    }
}
