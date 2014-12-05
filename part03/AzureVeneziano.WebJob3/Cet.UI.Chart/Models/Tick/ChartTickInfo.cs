using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    /// <summary>
    /// Raccoglie i dati relativi ad una demarcazione
    /// </summary>
    public class ChartTickInfo
    {
        public double PixelPosition { get; internal set; }
        public int Priority { get; internal set; }
        public string MajorText { get; internal set; }
        public string MinorText { get; internal set; }

        internal int TopIndex { get; set; }

    }
}
