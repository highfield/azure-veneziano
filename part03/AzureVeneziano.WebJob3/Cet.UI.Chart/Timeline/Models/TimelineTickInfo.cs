using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    /// <summary>
    /// Raccoglie i dati relativi ad una demarcazione
    /// lungo l'asse temporale
    /// </summary>
    public class TimelineTickInfo
        : ChartTickInfo
    {
        public DateTime UserTimestamp { get; internal set; }
        public long? Gap { get; internal set; }

    }
}
