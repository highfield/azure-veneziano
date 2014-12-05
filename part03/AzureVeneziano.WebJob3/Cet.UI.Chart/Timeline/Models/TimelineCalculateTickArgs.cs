using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    /// <summary>
    /// Raccoglie i parametri necessari all'algoritmo di calcolo
    /// delle suddivisioni dell'asse temporale
    /// </summary>
    public class TimelineCalculateTickArgs
        : ChartTickCalculationArgs
    {
        /// <summary>
        /// Rappresenta l'ampiezza minima di una divisione (in pixel)
        /// </summary>
        //internal const double DefaultMinimumDivisionWidth = 50.0;


        public TimelineCalculateTickArgs()
        {
            this.Formatter = TimelineHelper.StandardFormatter;
            //this.MinimumDivisionWidth = TimelineCalculateTickArgs.DefaultMinimumDivisionWidth;
        }


        //private double _minimumDivisionWidth;
        private List<TimelineGapInfo> _gaps;

        public DateTime LowestProgression { get; set; }
        public TimelineCalculateSegmentTickArgs SegmentInfo { get; set; }
        //public Action<IEnumerable<TimelineTickInfo>> Formatter { get; set; }


        /// <summary>
        /// Indica la distanza minima, in pixel, tra una divisione e l'altra
        /// (sono esclusi i casi con indicazioni particolari)
        /// </summary>
        //public double MinimumDivisionWidth
        //{
        //    get { return this._minimumDivisionWidth; }
        //    set
        //    {
        //        this._minimumDivisionWidth = Math.Max(
        //            value,
        //            5);
        //    }
        //}


        /// <summary>
        /// Restituisce un'enumerazione di oggetti 
        /// che definiscono i gaps presenti
        /// </summary>
        public IEnumerable<TimelineGapInfo> Gaps
        {
            get
            {
                if (this._gaps != null)
                {
                    return this._gaps;
                }
                else
                {
                    return Enumerable.Empty<TimelineGapInfo>();
                }
            }
        }


        /// <summary>
        /// Permette di impostare la lista di gaps 
        /// presenti nell'intervallo temporale corrente
        /// </summary>
        /// <param name="collection"></param>
        public void SetGaps(IEnumerable<TimelineGapInfo> collection)
        {
            //riordino in base all'istante iniziale
            var sorted = from gap in collection ?? Enumerable.Empty<TimelineGapInfo>()
                         //where this.LowerBound <= gap.Start.Ticks && gap.Start.Ticks <= this.UpperBound
                         orderby gap.StartProgression
                         select gap;

            //unisce eventuali sovrapposizioni


            //finalmente assegna il risultato alla lista locale
            this._gaps = sorted.ToList();
        }

    }
}
