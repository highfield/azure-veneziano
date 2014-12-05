using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    /// <summary>
    /// Raccoglie i parametri necessari per rappresentare
    /// un gap lungo l'asse temporale
    /// </summary>
    public class TimelineGapInfo
    {
        public TimelineGapInfo(
            long progression,
            DateTime timestamp)
        {
            this.StartProgression = progression;
            this.StartUserTimestamp = timestamp;
        }


        public long StartProgression { get; private set; }
        public DateTime StartUserTimestamp { get; private set; }
        public long Duration { get; set; }

    }
}
