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

        public int Flag { get { return Header->_flag; } }
        public int Res0 { get { return Header->_res0; } }
        public int Res1 { get { return Header->_res1; } }
        public int Res2 { get { return Header->_res2; } }
        public int Ref0 { get { return Header->_ref0; } }
        public int Ref1 { get { return Header->_ref1; } }
        public int Ref2 { get { return Header->_ref2; } }
        public int Ref3 { get { return Header->_ref3; } }
        public int Ref4 { get { return Header->_ref4; } }
        public int Ref5 { get { return Header->_ref5; } }
        public int Ref6 { get { return Header->_ref6; } }
        public int Ref7 { get { return Header->_ref7; } }
        public int Pad0 { get { return Header->_pad0; } }
        public int Pad1 { get { return Header->_pad1; } }

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
