using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using BrawlLib.SSBBTypes;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class MDL0DefNode : MDL0EntryNode
    {
        internal List<object> _items = new List<object>();

        int _len;
        protected override int DataLength { get { return _len; } }

        [Category("MDL0 Nodes")]
        public List<object> Items { get { return _items; } }

        protected override bool OnInitialize()
        {
            VoidPtr addr = WorkingUncompressed.Address;
            object n = null;
            while ((n = MDL0NodeClass.Create(ref addr)) != null)
                _items.Add(n);

            _len = addr - WorkingUncompressed.Address;
            base.OnInitialize();

            return false;
        }
    }
}
