using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0ShaderNode : MDL0EntryNode
    {
        internal MDL0Shader* Header { get { return (MDL0Shader*)WorkingUncompressed.Address; } }

        internal List<MDL0MaterialNode> _materials = new List<MDL0MaterialNode>();

        internal override void GetStrings(StringTable table) { }

        protected override bool OnInitialize()
        {
            //if (_name == null)
                _name = String.Format("Shader{0}", Index);

            return false;
        }
    }
}
