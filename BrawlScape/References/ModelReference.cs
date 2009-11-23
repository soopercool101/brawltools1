using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Modeling;

namespace BrawlScape
{
    public class ModelReference : NodeReference
    {
        private MDL0Node _model;
        public MDL0Node Model
        {
            get
            {
                if (_model == null)
                {
                    try { _model = ((MDL0Node)Node); }
                    catch { }
                }
                return _model;
            }
        }
    }
}
