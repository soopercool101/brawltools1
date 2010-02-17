using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrawlLib.Modeling
{
    public interface IMatrixProvider
    {
        Matrix FrameMatrix { get; }
        Matrix InverseBindMatrix { get; }
        void CalcBase();
        void CalcWeighted();
    }
}
