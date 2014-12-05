using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.UI.Chart
{
    public abstract class ChartPlotCartesianBase
        : ChartPlotBase
    {
        public string HorizontalAxisId { get; set; }
        public string VerticalAxisId { get; set; }

    }
}
