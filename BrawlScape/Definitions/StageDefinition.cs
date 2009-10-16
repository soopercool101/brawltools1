using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlScape
{
    public class StageDefinition : TextureDefinition, IListSource<ModelDefinition>, IListSource<TextureDefinition>
    {
        public static List<StageDefinition> List = new List<StageDefinition>();
        static StageDefinition()
        {
            List.Add(new StageDefinition("Battlefield", "STGBATTLEFIELD", 1));
            List.Add(new StageDefinition("Final Destination", "STGFINAL", 2));
            List.Add(new StageDefinition("Delfino Plaza", "STGDOLPIC", 3));
            List.Add(new StageDefinition("Luigi's Mansion", "STGMANSION", 4));
            List.Add(new StageDefinition("Mushroomy Kingdom (Exterior)", "STGMARIOPAST_00", 5));
            List.Add(new StageDefinition("Mushroomy Kingdom (Interior)", "STGMARIOPAST_01", 5));
            List.Add(new StageDefinition("Mario Circuit", "STGKART", 6));
            List.Add(new StageDefinition("75 m", "STGDONKEY", 7));
            List.Add(new StageDefinition("Rumble Falls", "STGJUNGLE", 8));
            List.Add(new StageDefinition("Pirate Ship", "STGPIRATES", 9));
            List.Add(new StageDefinition("Bridge of Eldin", "STGOLDIN", 10));
            List.Add(new StageDefinition("Norfair", "STGNORFAIR", 11));
            List.Add(new StageDefinition("Frigate Orpheon", "STGORPHEON", 12));
            List.Add(new StageDefinition("Yoshi's Island", "STGCRAYON", 13));
            List.Add(new StageDefinition("Halberd", "STGHALBERD", 14));
            List.Add(new StageDefinition("Lylat Cruise (Asteroid)", "STGSTARFOX_ASTEROID", 15));
            List.Add(new StageDefinition("Lylat Cruise (Battleship)", "STGSTARFOX_BATTLESHIP", 15));
            List.Add(new StageDefinition("Lylat Cruise (Corneria)", "STGSTARFOX_CORNERIA", 15));
            List.Add(new StageDefinition("Lylat Cruise (GDiff)", "STGSTARFOX_GDIFF", 15));
            List.Add(new StageDefinition("Lylat Cruise (Space)", "STGSTARFOX_SPACE", 15));
            List.Add(new StageDefinition("Pokemon Stadium 2", "STGSTADIUM", 16));
            List.Add(new StageDefinition("Spear Pillar (Dialga)", "STGTENGAN_1", 17));
            List.Add(new StageDefinition("Spear Pillar (Palkia)", "STGTENGAN_2", 17));
            List.Add(new StageDefinition("Spear Pillar (Crecelia)", "STGTENGAN_3", 17));
            List.Add(new StageDefinition("Port Town Aero Dive", "STGFZERO", 18));
            List.Add(new StageDefinition("Summit", "STGICE", 19));
            List.Add(new StageDefinition("Flat Zone 2", "STGGW", 20));
            List.Add(new StageDefinition("Castle Siege (Exterior)", "STGEMBLEM_00", 21));
            List.Add(new StageDefinition("Castle Siege (Interior)", "STGEMBLEM_01", 21));
            List.Add(new StageDefinition("Castle Siege (Cavern)", "STGEMBLEM_02", 21));
            List.Add(new StageDefinition("Wario Ware, Inc.", "STGMADEIN", 22));
            List.Add(new StageDefinition("Distant Planet", "STGEARTH", 23));
            List.Add(new StageDefinition("Skyworld", "STGPALUTENA", 24));
            List.Add(new StageDefinition("Mario Bros.", "STGFAMICOM", 25));
            List.Add(new StageDefinition("New Pork City", "STGNEWPORK", 26));
            List.Add(new StageDefinition("Smashville[0]", "STGVILLAGE_00", 27));
            List.Add(new StageDefinition("Smashville[1]", "STGVILLAGE_01", 27));
            List.Add(new StageDefinition("Smashville[2]", "STGVILLAGE_02", 27));
            List.Add(new StageDefinition("Smashville[3]", "STGVILLAGE_03", 27));
            List.Add(new StageDefinition("Smashville[4]", "STGVILLAGE_04", 27));
            List.Add(new StageDefinition("Shadow Moses Island (Gekko)", "STGMETALGEAR_00", 28));
            List.Add(new StageDefinition("Shadow Moses Island (Ray)", "STGMETALGEAR_01", 28));
            List.Add(new StageDefinition("Shadow Moses Island (Rex)", "STGMETALGEAR_02", 28));
            List.Add(new StageDefinition("Green Hill Zone", "STGGREENHILL", 29));
            List.Add(new StageDefinition("Pictochat", "STGPICTCHAT", 30));
            List.Add(new StageDefinition("Hanenbow", "STGPLANKTON", 31));

            List.Add(new StageDefinition("Temple", "STGDXSHRINE", 50));
            List.Add(new StageDefinition("Yoshi's Island", "STGDXYORSTER", 51));
            List.Add(new StageDefinition("Jungle Japes", "STGDXGARDEN", 52));
            List.Add(new StageDefinition("Onett", "STGDXONETT", 53));
            List.Add(new StageDefinition("Green Greens", "STGDXGREENS", 54));
            List.Add(new StageDefinition("Rainbow Cruise", "STGDXRCRUISE", 55));
            List.Add(new StageDefinition("Corneria", "STGDXCORNERIA", 56));
            List.Add(new StageDefinition("Big Blue", "STGDXBIGBLUE", 57));
            List.Add(new StageDefinition("Brinstar", "STGDXZEBES", 58));
            List.Add(new StageDefinition("Pokemon Stadium", "STGDXPSTADIUM", 59));

            List.Add(new StageDefinition("Stage Edit 1", "STGEDIT_0", 102));
            List.Add(new StageDefinition("Stage Edit 2", "STGEDIT_1", 102));
            List.Add(new StageDefinition("Stage Edit 3", "STGEDIT_2", 102));
            List.Add(new StageDefinition("Credits", "STGCHARAROLL", -1));
            List.Add(new StageDefinition("Config Test", "STGCONFIGTEST", -1));
            List.Add(new StageDefinition("All-Star Healing Room", "STGHEAL", -1));
            List.Add(new StageDefinition("Homerun Contest", "STGHOMERUN", -1));
            List.Add(new StageDefinition("Online Training", "STGONLINETRAINING", -1));
            List.Add(new StageDefinition("Results", "STGRESULT", -1));
            List.Add(new StageDefinition("Target Practice 1", "STGTARGETLV1", -1));
            List.Add(new StageDefinition("Target Practice 2", "STGTARGETLV2", -1));
            List.Add(new StageDefinition("Target Practice 3", "STGTARGETLV3", -1));
            List.Add(new StageDefinition("Target Practice 4", "STGTARGETLV4", -1));
            List.Add(new StageDefinition("Target Practice 5", "STGTARGETLV5", -1));
        }

        private int _index;
        private string _stageName;
        private string _path;


        private StageDefinition(string name, string stageName, int index)
            : base(index == -1 ? null : "system\\common5.pac", index == -1 ? null : String.Format("sc_selmap_en/Type1[80]/Textures(NW4R)/MenSelmapIcon.{0:00}", index))
        {
            _index = index;
            _stageName = stageName;
            _path = String.Format("stage\\melee\\{0}.PAC", _stageName);
            Text = name;
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
