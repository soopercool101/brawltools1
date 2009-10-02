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
