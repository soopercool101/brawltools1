using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;

namespace BrawlScape
{
    class ItemDefinition : ListViewItem
    {
        public static List<ItemDefinition> List = new List<ItemDefinition>();
        static ItemDefinition()
        {
            List.Add(new ItemDefinition("Assist Trophy", "Assist", 2));
            List.Add(new ItemDefinition("Badge", "Badge", 48));
            List.Add(new ItemDefinition("Banana", "Banana", 42));
            //List.Add(new ItemDefinition("Barrel", "Barrel", 48));
            List.Add(new ItemDefinition("Beam Sword", "BeamSword", 20));
            //List.Add(new ItemDefinition("Bill", "Bill", 48));
            List.Add(new ItemDefinition("Ba-bomb", "Bombhei", 31));
            List.Add(new ItemDefinition("Box", "Box", 4));
            List.Add(new ItemDefinition("Bumper", "Bumper", 43));
            //List.Add(new ItemDefinition("Capsule", "Capsule", 48));
            //List.Add(new ItemDefinition("CD", "Cd", 48));
            //List.Add(new ItemDefinition("Chewing", "Chewing", 48));
            //List.Add(new ItemDefinition("Clacker", "Clacker", 48));
            //List.Add(new ItemDefinition("Coin", "Coin", 48));
            List.Add(new ItemDefinition("Curry", "Curry", 17));
            List.Add(new ItemDefinition("Deku Nut", "Deku", 35));
            List.Add(new ItemDefinition("Mr. Saturn", "Doseisan", 40));
            List.Add(new ItemDefinition("Dragoon", "Dragoon", 10));
            //List.Add(new ItemDefinition("Dragoon Sight", "DragoonSight", 48));
            //List.Add(new ItemDefinition("Figure", "Figure", 48));
            List.Add(new ItemDefinition("Fire Flower", "FireFlower", 29));
            List.Add(new ItemDefinition("Freezer", "Freezer", 36));
            List.Add(new ItemDefinition("Golden Hammer", "GoldenHammer", 26));
            List.Add(new ItemDefinition("Green Shell", "GreenShell", 41));
            List.Add(new ItemDefinition("Hammer", "Hammer", 25));
            List.Add(new ItemDefinition("Heart", "Heart", 9));
            List.Add(new ItemDefinition("Home Run Bat", "HomeRunBat", 21));
            List.Add(new ItemDefinition("Maxim Tomato", "MaximTomato", 8));
            List.Add(new ItemDefinition("Poison Mushroom", "MushD", 12));
            List.Add(new ItemDefinition("Mushroom", "Mushroom", 11));
            List.Add(new ItemDefinition("Metal Block", "MetalBlock", 15));
            List.Add(new ItemDefinition("Pitfall", "Pitfall", 38));
            List.Add(new ItemDefinition("Poke Ball", "PokeBall", 3));
            List.Add(new ItemDefinition("Powder Box", "PowderBox", 5));
            List.Add(new ItemDefinition("Ray Gun", "RayGun", 28));
            List.Add(new ItemDefinition("Lip Stick", "RipStick", 23));
            List.Add(new ItemDefinition("Sandbag", "Sandbag", 6));
            List.Add(new ItemDefinition("Screw Attack", "Screw", 49));
            List.Add(new ItemDefinition("Sensor Bomb", "SensorBomb", 32));
            List.Add(new ItemDefinition("Smart Bomb", "SmartBomb", 34));
            List.Add(new ItemDefinition("Smash Ball", "SmashBall", 1));
            List.Add(new ItemDefinition("Smoke Screen", "SmokeScreen", 37));
            List.Add(new ItemDefinition("Star Rod", "StarRod", 24));
            List.Add(new ItemDefinition("SoccerBall", "SoccerBall", 46));
            List.Add(new ItemDefinition("Super Scope", "SuperScope", 27));
            List.Add(new ItemDefinition("Super Star", "SuperStar", 14));
            List.Add(new ItemDefinition("Food", "Tabemono", 7));
            List.Add(new ItemDefinition("Team Healing", "TeamHealing", 47));
            List.Add(new ItemDefinition("Thunder", "Thunder", 19));
            List.Add(new ItemDefinition("Rabbit Hat", "UsagiHat", 16));
            List.Add(new ItemDefinition("Unira", "Unira", 45));
            List.Add(new ItemDefinition("Warp Star", "Warpstar", 13));
            List.Add(new ItemDefinition("Slow", "Slow", 18));
            List.Add(new ItemDefinition("Fan", "Harisen", 22));
            List.Add(new ItemDefinition("Clacker", "Clacker", 30));
            List.Add(new ItemDefinition("Pasaran", "Pasaran", 39));
            List.Add(new ItemDefinition("Spring", "Spring", 44));
            List.Add(new ItemDefinition("Chewing", "Chewing", 33));
        }

        private string _resName;
        private int _iconIndex;

        private ItemDefinition(string name, string resourceName, int iconIndex)
        {
            Text = name;
            _resName = resourceName;
            _iconIndex = iconIndex;
        }

        public Image GetIcon()
        {
            ResourceNode n = ResourceCache.FindNode("system\\common5.pac", String.Format("sc_selcharacter2_en/MenuRule_en/ModelData[0]/Textures(NW4R)/MenMainItem.{0:00}", _iconIndex));
            if (n is IImageSource)
                return ((IImageSource)n).GetImage(0);
            return null;
        }

        private TextureDefinition[] _textures;
        public TextureDefinition[] Textures
        {
            get
            {
                if (_textures == null)
                {
                    ResourceNode[] nodes = ResourceCache.FindNodeByType("system\\common3.pac", String.Format("ItmCommonBrres/Itm{0}Brres", _resName), ResourceType.TEX0);
                    _textures = new TextureDefinition[nodes.Length];
                    for (int i = 0; i < nodes.Length; i++)
                        _textures[i] = new TextureDefinition("system\\common3.pac", nodes[i].TreePath);
                }
                return _textures;
            }
        }
    }
}
