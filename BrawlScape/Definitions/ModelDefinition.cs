using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.OpenGL;

namespace BrawlScape
{
    public class ModelDefinition : ResourceDefinition<ModelReference>
    {
        public ModelDefinition(string relativePath, string nodePath) : base(relativePath, nodePath) { }

        public override string ToString()        {            return Text;        }

        public GLModel Model { get { return _nodeRef == null ? null : _nodeRef.Model; } }
    }
}
