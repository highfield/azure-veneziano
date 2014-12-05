using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public abstract class ChartPlotBase
        : ChartBaseVM
    {
        protected ChartPlotBase()
        {
        }


        #region PROP IsEmpty

        private bool _isEmpty;

        public bool IsEmpty
        {
            get { return this._isEmpty; }
            protected set
            {
                if (this._isEmpty != value)
                {
                    this._isEmpty = value;
                    this.OnPropertyChanged("IsEmpty");
                }
            }
        }

        #endregion

    }
}
