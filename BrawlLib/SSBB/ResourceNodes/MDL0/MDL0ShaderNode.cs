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

            //Attach to materials
            MDL0Node model = Model;
            byte* pHeader = (byte*)Header;
            if ((model != null) && (model._matList != null))
                foreach (MDL0MaterialNode mat in model._matList)
                {
                    MDL0Material* mHeader = mat.Header;
                    if (((byte*)mHeader + mHeader->_shaderOffset) == pHeader)
                    {
                        mat._shader = this;
                        _materials.Add(mat);
                    }
                }

            return false;
        }
    }
}
