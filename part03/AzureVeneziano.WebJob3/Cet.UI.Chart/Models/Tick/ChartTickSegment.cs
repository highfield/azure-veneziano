using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public class ChartTickSegment
        : Collection<ChartTickInfo>
    {
        public bool IsValid { get; internal set; }
        public int PriorityCount { get; internal set; }

    }
}
