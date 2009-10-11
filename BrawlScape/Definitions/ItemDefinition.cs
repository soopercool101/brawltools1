using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Imaging;

namespace BrawlScape
{
    public class ItemDefinition : TextureDefinition, IListSource<ModelDefinition>
    {
        public static List<ItemDefinition> List = new List<ItemDefinition>();
        static ItemDefinition()
        {
            List.Add(new ItemDefinition("Assist Trophy", "Assist", 2));
            List.Add(new ItemDefinition("Badge", "Badge", 48));
            List.Add(new ItemDefinition("Banana", "Banana", 42));
            List.Add(new ItemDefinition("Beam Sword", "BeamSword", 20));
            List.Add(new ItemDefinition("Ba-bomb", "Bombhei", 31));
            List.Add(new ItemDefinition("Box", "Box", 4));
            List.Add(new ItemDefinition("Bumper", "Bumper", 43));
            List.Add(new ItemDefinition("Chewing", "Chewing", 33));
            List.Add(new ItemDefinition("Clacker", "Clacker", 30));
            List.Add(new ItemDefinition("Curry", "Curry", 17));
            List.Add(new ItemDefinition("Deku Nut", "Deku", 35));
            List.Add(new ItemDefinition("Mr. Saturn", "Doseisan", 40));
            List.Add(new ItemDefinition("Dragoon", "Dragoon", 10));
            List.Add(new ItemDefinition("Fan", "Harisen", 22));
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
            List.Add(new ItemDefinition("Pasaran", "Pasaran", 39));
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
            List.Add(new ItemDefinition("Spring", "Spring", 44));
            List.Add(new ItemDefinition("Star Rod", "StarRod", 24));
            List.Add(new ItemDefinition("SoccerBall", "SoccerBall", 46));
            List.Add(new ItemDefinition("Super Scope", "SuperScope", 27));
            List.Add(new ItemDefinition("Super Star", "SuperStar", 14));
            List.Add(new ItemDefinition("Food", "Tabemono", 7));
            List.Add(new ItemDefinition("Team Healing", "TeamHealing", 47));
            List.Add(new ItemDefinition("Thunder", "Thunder", 19));
            List.Add(new ItemDefinition("Unira", "Unira", 45));
            List.Add(new ItemDefinition("Rabbit Hat", "UsagiHat", 16));
            List.Add(new ItemDefinition("Warp Star", "Warpstar", 13));
            List.Add(new ItemDefinition("Slow", "Slow", 18));
            List.Add(new ItemDefinition("Barrel", "Barrel", -1));
            List.Add(new ItemDefinition("Bill", "Bill", -1));
            List.Add(new ItemDefinition("Capsule", "Capsule", -1));
            List.Add(new ItemDefinition("CD", "Cd", -1));
            List.Add(new ItemDefinition("Coin", "Coin", -1));
            List.Add(new ItemDefinition("Dragoon Sight", "DragoonSight", -1));
            List.Add(new ItemDefinition("Figure", "Figure", -1));
            List.Add(new ItemDefinition("Party Ball", "Kusudama", -1));
            List.Add(new ItemDefinition("Lip Stick Flower", "RipStickFlower", -1));
            List.Add(new ItemDefinition("Seal", "Seal", -1));
            List.Add(new ItemDefinition("Seed", "Seed", -1));
            List.Add(new ItemDefinition("Snake's Box", "CBox", -1));
            List.Add(new ItemDefinition("Snake's Grenade", "SnakeGrenade", -1));
            List.Add(new ItemDefinition("Diddy's Peanuts", "DiddyPeanuts", -1));
            List.Add(new ItemDefinition("Link's Bomb", "LinkBomb", -1));
            List.Add(new ItemDefinition("Toon Link's Bomb", "ToonLinkBomb", -1));
            List.Add(new ItemDefinition("Peach's Turnips", "PeachDaikon", -1));
        }

        private string _resName, _nodePath;
        private int _iconIndex;

        private ItemDefinition(string name, string resourceName, int iconIndex)
            : base(iconIndex == -1 ? null : "system\\common5.pac", iconIndex == -1 ? null : String.Format("sc_selcharacter2_en/MenuRule_en/ModelData[0]/Textures(NW4R)/MenMainItem.{0:00}", iconIndex))
        {
            Text = name;
            _resName = resourceName;
            _iconIndex = iconIndex;
            _nodePath = String.Format("ItmCommonBrres/Itm{0}Brres", _resName);
        }

        //public Image GetIcon()
        //{
        //    ResourceNode n = ResourceCache.FindNode("system\\common5.pac", String.Format("sc_selcharacter2_en/MenuRule_en/ModelData[0]/Textures(NW4R)/MenMainItem.{0:00}", _iconIndex));
        //    if (n is IImageSource)
        //        return ((IImageSource)n).GetImage(0);
        //    return null;
        //}

        //private TextureDefinition[] _textures;
        //public TextureDefinition[] ListItems
        //{
        //    get
        //    {
        //        if (_textures == null)
        //        {
        //            ResourceNode[] nodes = ResourceCache.FindNodeByType("system\\common3.pac", String.Format("ItmCommonBrres/Itm{0}Brres", _resName), ResourceType.TEX0);
        //            if (nodes != null)
        //            {
        //                _textures = new TextureDefinition[nodes.Length];
        //                for (int i = 0; i < nodes.Length; i++)
        //                    _textures[i] = new TextureDefinition("system\\common3.pac", nodes[i].TreePath);
        //            }
        //        }
        //        return _textures;
        //    }
        //}

        private ModelDefinition[] _models;
        public ModelDefinition[] ListItems
        {
            get
            {
                if (_models == null)
                {
                    ResourceNode[] nodes = ResourceCache.FindNodeByType("system\\common3.pac", _nodePath, ResourceType.MDL0);
                    if (nodes != null)
                    {
                        ModelDefinition[] models = new ModelDefinition[nodes.Length];
                        for (int i = 0; i < nodes.Length; i++)
                            models[i] = new ModelDefinition("system\\common3.pac", nodes[i].TreePath);
                        _models = models;
                    }
                }
                return _models;
            }
        }
    }
}
