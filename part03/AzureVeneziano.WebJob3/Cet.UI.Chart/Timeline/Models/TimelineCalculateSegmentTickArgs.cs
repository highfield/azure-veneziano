using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public class TimelineCalculateSegmentTickArgs
    {
        private long _lowerBound;
        private long _upperBound;
        private long _extent;

        private double _startPixel;
        private double _endPixel;
        private double _width;

        private bool _isValid;

        public long? GapDuration { get; internal set; }


        /// <summary>
        /// Indica l'estremo inferiore dell'intervallo temporale, in ticks
        /// </summary>
        public long LowerBound
        {
            get { return this._lowerBound; }
            set
            {
                this._lowerBound = value;
                this.Validate();
            }
        }


        /// <summary>
        /// Indica l'estremo superiore dell'intervallo temporale, in ticks
        /// </summary>
        public long UpperBound
        {
            get { return this._upperBound; }
            set
            {
                this._upperBound = value;
                this.Validate();
            }
        }


        /// <summary>
        /// Indica l'estremo inferiore del segmento grafico, in pixel
        /// </summary>
        public double StartPixel
        {
            get { return this._startPixel; }
            set
            {
                this._startPixel = value;
                this.Validate();
            }
        }


        /// <summary>
        /// Indica l'estremo superiore del segmento grafico, in pixel
        /// </summary>
        public double EndPixel
        {
            get { return this._endPixel; }
            set
            {
                this._endPixel = value;
                this.Validate();
            }
        }


        /// <summary>
        /// Restituisce l'estensione temporale effettiva
        /// </summary>
        public long Extent
        {
            get { return this._extent; }
        }


        /// <summary>
        /// Restituisce l'ampiezza effettiva del segmento grafico
        /// </summary>
        public double Width
        {
            get { return this._width; }
        }


        /// <summary>
        /// Indica se i parametri impostati possono essere ritenuti validi
        /// </summary>
        public bool IsValid
        {
            get { return this._isValid; }
        }


        private void Validate()
        {
            this._extent = this._upperBound - this._lowerBound;
            this._width = this._endPixel - this._startPixel + 1;

            this._isValid =
                this._extent > 0 &&
                this._width >= 2;
        }

    }
}
