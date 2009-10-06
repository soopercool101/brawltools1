using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlScape
{
    public class ModelReference : NodeReference
    {
        private GLModel _model;
        public GLModel Model
        {
            get
            {
                if (_model == null)
                {
                    try { _model = ((MDL0Node)Node).GetModel(); }
                    catch { }
                }
                return _model;
            }
        }
    }
}
