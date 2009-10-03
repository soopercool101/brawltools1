using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlScape
{
    public class ModelDefinition : ResourceDefinition<ModelReference>
    {
        public ModelDefinition(string relativePath, string nodePath) : base(relativePath, nodePath) { }

        public override string ToString()        {            return Text;        }
    }
}
