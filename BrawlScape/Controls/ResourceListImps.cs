using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlScape
{
    //public class ResourceListBase : ListView { }
    public class CharacterList : ResourceList<CharacterDefinition> { }
    public class CostumeList : ResourceList<CostumeDefinition> { }
    public class ItemList : ResourceList<ItemDefinition> { }
    public class TextureList : ResourceList<TextureDefinition> { }
    public class ModelList : ResourceList<ModelDefinition> { }
    public class StageList : ResourceList<StageDefinition> { }
    public class SSEStageList : ResourceList<AdvStageDefinition> { }
    public class SSEAreaList : ResourceList<AdvAreaDefinition> { }
}
