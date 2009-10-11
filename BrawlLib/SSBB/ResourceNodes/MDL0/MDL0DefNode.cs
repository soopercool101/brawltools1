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

        [Category("MDL0 Nodes")]//, Editor(typeof(EasyCollectionEditor), typeof(UITypeEditor))]
        public List<object> Items { get { return _items; } }

        protected override bool OnInitialize()
        {
            base.OnInitialize();

            VoidPtr addr = WorkingSource.Address;
            object n = null;
            while ((n = MDL0NodeClass.Create(ref addr)) != null)
                _items.Add(n);

            return false;
        }
    }
}
