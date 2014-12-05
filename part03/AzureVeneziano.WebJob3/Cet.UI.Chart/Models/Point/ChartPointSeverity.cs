using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public class ChartPointSeverity
    {
        public double X;
        public IList<ChartPointSeverityItem> Items;
    }


    public class ChartPointSeverityItem
    {
        public string Description;
        public Color Color;
    }
}
