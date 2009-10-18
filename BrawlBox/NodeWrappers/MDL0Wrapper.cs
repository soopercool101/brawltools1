using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlBox.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0)]
    class MDL0Wrapper : GenericWrapper
    {
        [NodeAction("Preview")]
        public void Preview()
        {
            using (ModelForm form = new ModelForm())
            {
                form.ShowDialog(((MDL0Node)_resource).GetModel());
            }
        }

        //protected internal override void OnDoubleClick()
        //{
        //    Preview();
        //}
    }
}
