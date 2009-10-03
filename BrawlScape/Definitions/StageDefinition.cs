using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlScape
{
    public class StageDefinition : TextureDefinition
    {
        public static List<StageDefinition> List = new List<StageDefinition>();
        static StageDefinition()
        {
            List.Add(new StageDefinition("Battlefield", "STGBATTLEFIELD", 1));
            List.Add(new StageDefinition("Final Destination", "STGFINAL", 2));
            List.Add(new StageDefinition("Delfino Plaza", "STGDOLPIC", 3));
            List.Add(new StageDefinition("Luigi's Mansion", "STGMANSION", 4));
            List.Add(new StageDefinition("Mushroomy Kingdom", "STGMARIOPAST_00", 5));
            List.Add(new StageDefinition("Mario Circuit", "STGKART", 6));
            List.Add(new StageDefinition("75 m", "STGDONKEY", 7));
            List.Add(new StageDefinition("Rumble Falls", "STGJUNGLE", 8));
            List.Add(new StageDefinition("Pirate Ship", "STGPIRATES", 9));
            List.Add(new StageDefinition("Bridge of Eldin", "STGOLDIN", 10));
            List.Add(new StageDefinition("Norfair", "STGNORFAIR", 11));
            List.Add(new StageDefinition("Frigate Orpheon", "STGORPHEON", 12));
            List.Add(new StageDefinition("Yoshi's Island", "STGCRAYON", 13));
            List.Add(new StageDefinition("Halberd", "STGHALBERD", 14));
            List.Add(new StageDefinition("Lylat Cruise", "STGSTARFOX_SPACE", 15));
            List.Add(new StageDefinition("Pokemon Stadium 2", "STGSTADIUM", 16));
            List.Add(new StageDefinition("Spear Pillar", "STGTENGAN", 17));
            List.Add(new StageDefinition("Port Town Aero Dive", "STGFZERO", 18));
            List.Add(new StageDefinition("Summit", "STGICE", 19));
            List.Add(new StageDefinition("Flat Zone 2", "STGGW", 20));
            List.Add(new StageDefinition("Castle Siege", "STGEMBLEM", 21));
            List.Add(new StageDefinition("Wario Ware, Inc.", "STGMADEIN", 22));
            List.Add(new StageDefinition("Distant Planet", "STGEARTH", 23));
            List.Add(new StageDefinition("Skyworld", "STGPALUTENA", 24));
            List.Add(new StageDefinition("Mario Bros.", "STGFAMICOM", 25));
            List.Add(new StageDefinition("New Pork City", "STGNEWPORK", 26));
            List.Add(new StageDefinition("Smashville", "STGVILLAGE", 27));
            List.Add(new StageDefinition("Shadow Moses Island", "STGMETALGEAR", 28));
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
        }

        private int _index;
        private string _stageName;


        private StageDefinition(string name, string stageName, int index)
            : base("system\\common5.pac", String.Format("sc_selmap_en/Type1[80]/Textures(NW4R)/MenSelmapIcon.{0:00}", index))
        {
            _index = index;
            _stageName = stageName;
            Text = name;
        }
    }
}
