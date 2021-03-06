﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class INFONode : ResourceNode
    {
        internal SYMBHeader* Header { get { return (SYMBHeader*)WorkingUncompressed.Address; } }

        protected override bool OnInitialize()
        {
            _name = "INFO";
            return true;
        }

        protected override void OnPopulate()
        {
        }
    }
}
