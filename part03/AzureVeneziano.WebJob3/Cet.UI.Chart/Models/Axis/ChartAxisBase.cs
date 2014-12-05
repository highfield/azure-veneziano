using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public abstract class ChartAxisBase
        : ChartBaseVM
    {
        protected ChartAxisBase() { }


        public string Orientation { get; set; }
        public IAxisFormatter Formatter { get; set; }


        #region PROP IsValid

        private bool _isValid;

        public bool IsValid
        {
            get { return this._isValid; }
            set
            {
                if (this._isValid != value)
                {
                    this._isValid = value;
                    this.OnPropertyChanged("IsValid");
                }
            }
        }

        #endregion


        #region PROP Ticks

        private ChartTickSegmentCollection _ticks;

        public ChartTickSegmentCollection Ticks
        {
            get { return this._ticks; }
            protected set
            {
                if (this._ticks != value)
                {
                    this._ticks = value;
                    this.OnPropertyChanged("Ticks");

                    this.Owner.InvalidateRender();
                }
            }
        }

        #endregion


        public abstract double GetLowestRelative();


        public abstract double GetHightestRelative();


        public abstract double ConvertToRelative(object value);


        public abstract object ConvertToAbsolute(double relative);


        #region EVT PropertyChanged

        //public event PropertyChangedEventHandler PropertyChanged;


        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    var handler = this.PropertyChanged;

        //    if (handler != null)
        //    {
        //        handler(
        //            this,
        //            new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        #endregion

    }


    public abstract class ChartAxisBase<TValue>
        : ChartAxisBase
    {
        protected ChartAxisBase() { }


        #region PROP LowerBound

        private TValue _lowerBound;

        public TValue LowerBound
        {
            get { return this._lowerBound; }
            set
            {
                if (object.Equals(this._lowerBound, value) == false)
                {
                    this._lowerBound = value;
                    this.OnPropertyChanged("LowerBound");

                    this.InvalidateRender();
                }
            }
        }

        #endregion


        #region PROP UpperBound

        private TValue _upperBound;

        public TValue UpperBound
        {
            get { return this._upperBound; }
            set
            {
                if (object.Equals(this._upperBound, value) == false)
                {
                    this._upperBound = value;
                    this.OnPropertyChanged("UpperBound");

                    this.InvalidateRender();
                }
            }
        }

        #endregion


        #region PROP LowestBound

        private TValue _lowestBound;

        public TValue LowestBound
        {
            get { return this._lowestBound; }
            set
            {
                if (object.Equals(this._lowestBound, value) == false)
                {
                    this._lowestBound = value;
                    this.OnPropertyChanged("LowestBound");
                }
            }
        }

        #endregion


        #region PROP HightestBound

        private TValue _hightestBound;

        public TValue HightestBound
        {
            get { return this._hightestBound; }
            set
            {
                if (object.Equals(this._hightestBound, value) == false)
                {
                    this._hightestBound = value;
                    this.OnPropertyChanged("HightestBound");
                }
            }
        }

        #endregion

    }
}
