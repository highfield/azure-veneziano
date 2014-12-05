using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    /// <summary>
    /// Rappresenta il risultato dell'algoritmo di suddivisione dell'asse temporale
    /// </summary>
    public class ChartTickSegmentCollection
        : Collection<ChartTickSegment>
    {
        public ChartTickSegmentCollection(Guid uid)
        {
            this.TickClassId = uid;
        }

        public Guid TickClassId { get; private set; }

    }
}
