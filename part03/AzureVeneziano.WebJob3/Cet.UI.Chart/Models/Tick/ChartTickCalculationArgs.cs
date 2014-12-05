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
    public class ChartTickCalculationArgs
    {
        /// <summary>
        /// Rappresenta l'ampiezza minima di una divisione (in pixel)
        /// </summary>
        internal const double DefaultMinimumDivisionWidth = 50.0;


        public ChartTickCalculationArgs()
        {
            this.MinimumDivisionLength = ChartTickCalculationArgs.DefaultMinimumDivisionWidth;
        }


        private double _minimumDivisionWidth;

        public Action<IEnumerable<ChartTickInfo>> Formatter { get; set; }


        /// <summary>
        /// Indica la distanza minima, in pixel, tra una divisione e l'altra
        /// (sono esclusi i casi con indicazioni particolari)
        /// </summary>
        public double MinimumDivisionLength
        {
            get { return this._minimumDivisionWidth; }
            set
            {
                this._minimumDivisionWidth = Math.Max(
                    value,
                    5);
            }
        }

    }
}
