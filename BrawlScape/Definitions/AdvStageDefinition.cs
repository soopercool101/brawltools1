using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlScape
{
    public class AdvStageDefinition : TextureDefinition, IListSource<AdvAreaDefinition>
    {
        public static List<AdvStageDefinition> List = new List<AdvStageDefinition>();
        static AdvStageDefinition()
        {
            List.Add(new AdvStageDefinition("Midair Stadium", 0));
            List.Add(new AdvStageDefinition("Skyworld", 1));
            List.Add(new AdvStageDefinition("Sea of Clouds", 2));
            List.Add(new AdvStageDefinition("The Jungle", 3));
            List.Add(new AdvStageDefinition("The Plain", 4));
            List.Add(new AdvStageDefinition("The Lake", 5));
            List.Add(new AdvStageDefinition("The Ruined Zoo", 6));
            List.Add(new AdvStageDefinition("The Battlefield Fortress", 7));
            List.Add(new AdvStageDefinition("The Forest", 8));
            List.Add(new AdvStageDefinition("The Research Facility (1)", 9));
            List.Add(new AdvStageDefinition("The Lake Shore", 10));
            List.Add(new AdvStageDefinition("The Path to the Ruins", 11));
            List.Add(new AdvStageDefinition("The Cave", 12));
            List.Add(new AdvStageDefinition("The Ruins", 13));
            List.Add(new AdvStageDefinition("The Wilds (1)", 14));
            List.Add(new AdvStageDefinition("The Ruined Hall", 15));
            List.Add(new AdvStageDefinition("The Wilds (2)", 16));
            List.Add(new AdvStageDefinition("The Swamp", 17));
            List.Add(new AdvStageDefinition("The Research Facility (2)", 18));
            List.Add(new AdvStageDefinition("Outside the Ancient Ruins", 19));
            List.Add(new AdvStageDefinition("The Glacial Peak", 20));
            List.Add(new AdvStageDefinition("The Canyon", 21));
            List.Add(new AdvStageDefinition("Battleship Halberd Interior", 22));
            List.Add(new AdvStageDefinition("Battleship Halberd Exterior", 23));
            List.Add(new AdvStageDefinition("Battleship Halberd Bridge", 24));
            List.Add(new AdvStageDefinition("The Subspace Bomb Factory (1)", 25));
            List.Add(new AdvStageDefinition("The Subspace Bomb Factory (2)", 26));
            List.Add(new AdvStageDefinition("Entrance to Subspace", 27));
            List.Add(new AdvStageDefinition("Subspace (1)", 28));
            List.Add(new AdvStageDefinition("Subspace (2)", 29));
            List.Add(new AdvStageDefinition("The Great Maze (1)", 30));
            List.Add(new AdvStageDefinition("The Great Maze (2)", 31));
            List.Add(new AdvStageDefinition("The Great Maze (3)", 32));
            List.Add(new AdvStageDefinition("The Great Maze (4)", 33));
        }

        private int _index;

        public AdvStageDefinition(string name, int index)
             : base("menu\\adventure\\map_sel.brres", String.Format("Textures(NW4R)/AdvSelmapPrev.{0:00}", index + 1))
        {
            Text = name;
            _index = index;
        }

        private static string[][] _areaIds = new string[][]
        {
            new string[]{"010001","030001","030101"},//Midair Stadium
            new string[]{"040001", "040101", "040201", "040201a"},//Skyworld
            new string[]{"050001", "050102", "050102a", "050103"},//Sea of Clouds
            new string[]{"060001", "060002", "060002a", "060003", "060004", "060004a"},//The Jungle
            new string[]{"070001", "070001a", "070002"},//The Plain
            new string[]{"080001", "080101", "080102", "080103", "080103a", "080103b", "080104", "080104a", "080104b", "080105", "080105a", "080201", "080301"},//The Lake
            new string[]{"090001", "090101", "090201", "090202", "090203", "090203a"},//The Ruined Zoo
            new string[]{"100001", "100001a", "100002", "100101", "100201", "100202", "100202a", "100203", "100205"},//The Battlefield Fortress
            new string[]{"120001", "120001a", "120002", "120003"},//The Forest
            new string[]{"140001", "140005", "140101", "140102", "140103", "140104", "140105", "140106"},//The Research Facility (1)
            new string[]{"160001", "160002", "160101", "160102", "160201", "160202", "160301", "160301a", "160301b", "160301c"},//The Lake Shore
            new string[]{"180001", "180001a", "180002", "180003", "180101"},//The Path to the Ruins
            new string[]{"200001","200001a", "200002", "200003", "200003a"},//The Cave
            new string[]{"220001", "220002", "220002a", "220003", "220003a", "220101"},//The Ruins
            new string[]{"240001", "240001a", "240002", "240002a", "240002b", "240101"},//The Wilds (1)
            new string[]{"250001"},//The Ruined Hall
            new string[]{"260001", "260001a", "260002"},//The Wilds (2)
            new string[]{"270001", "270002", "270002a", "270101", "270201", "270202", "270202a", "270203"},//The Swamp
            new string[]{"280002", "280002a", "280003", "280101", "280201", "280202", "280202a", "280203", "280204", "280301"},//The Research Facility (2)
            new string[]{"290001", "290001a","290001b"},//Outside the Ancient Ruins
            new string[]{"300001"},//The Glacial Peak
            new string[]{"310001", "310002", "310003", "310003a", "310003b", "310303", "320001"},//The Canyon
            new string[]{"330001", "330002", "330002a", "330101", "330101a", "330102", "330103", "330104", "330201"},//Battleship Halberd Interior
            new string[]{"340001", "340002", "340003", "340004", "340005"},//Battleship Halberd Exterior
            new string[]{"350001"},//Battleship Halberd Bridge
            new string[]{"360001", "360001a", "360001b", "360001c", "360002"},//The Subspace Bomb Factory (1)
            new string[]{"370001", "370001a", "370002", "370101", "370201", "370202", "370203", "370301"},//The Subspace Bomb Factory (2)
            new string[]{"390001"},//Entrance to Subspace
            new string[]{"400001", "400002", "400003", "400004", "400005", "400006", "400007", "400008", "400009", "400101"},//Subspace (1)
            new string[]{"410001", "410002", "410003"},//Subspace (2)
            new string[]{"420001a", "420001b", "420001c", "420001d", "420002a", "420002b", "420002c", "420002d", "420002e", "420003a", "420005a", "420005b", "420005c", "420007a", "420007b", "420007c", "420009a", "420009b", "420009c", "420011a", "420013a", "420013b", "420015a", "420017a", "420017b", "420017c", "420019a", "420019b", "420021a", "420021c", "420021d", "420021e", "420023a", "420023b", "420023c", "420025a", "420025b", "420025c", "420027a", "420027b", "420027c", "420029b", "420031a", "420031b", "420031c", "420033a", "420033b", "420035a", "420037a", "420041", "420042", "420043", "420044", "420045", "420046", "420047", "420051", "420052", "420053", "420054", "420055", "420056", "420057", "420058", "420059", "420060", "420061", "420062", "420063", "420064", "420065", "420066", "420067", "420068", "420069", "420070", "420071", "420072", "420073", "420074", "420075", "420076", "420077", "420078", "420079", "420080", "420081", "420101"},//The Great Maze (1)
            new string[]{"900001", "900101", "900201"},//The Great Maze (2)
            new string[]{"910101"},//The Great Maze (3)
            new string[]{"920001", "920101", "920201", "920301", "920401", "920501", "920601", "920701", "920801"}//The Great Maze (4)
        };

        private AdvAreaDefinition[] _areas;
        public AdvAreaDefinition[] ListItems
        {
            get
            {
                if (_areas == null)
                {
                    int count = _areaIds[_index].Length;
                    _areas = new AdvAreaDefinition[count];
                    for (int i = 0; i < count; i++)
                        _areas[i] = new AdvAreaDefinition(_areaIds[_index][i]);
                }
                return _areas;
            }
        }
    }
}
