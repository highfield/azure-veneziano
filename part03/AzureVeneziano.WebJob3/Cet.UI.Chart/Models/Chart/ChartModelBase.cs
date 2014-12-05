using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public abstract class ChartModelBase
        : ChartBaseVM
    {
        protected ChartModelBase()
        {
            this._plots = new ChartPlotCollection(this);
            this._axes = new ChartAxisCollection(this);
        }


        #region PROP IsValid

        private bool _isValid;

        public bool IsValid
        {
            get { return this._isValid; }
            private set
            {
                if (this._isValid != value)
                {
                    this._isValid = value;
                    this.OnPropertyChanged("IsValid");
                }
            }
        }

        #endregion
        

        #region PROP Plots

        private readonly ChartPlotCollection _plots;

        public ChartPlotCollection Plots
        {
            get { return this._plots; }
        }

        #endregion
        

        #region PROP Axes

        private readonly ChartAxisCollection _axes;


        public ChartAxisCollection Axes
        {
            get { return this._axes; }
        }

        #endregion

    }
}
