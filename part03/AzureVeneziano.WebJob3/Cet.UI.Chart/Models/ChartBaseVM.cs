using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public abstract class ChartBaseVM
        : INotifyPropertyChanged
    {
        protected ChartBaseVM()
        {
        }


        public ChartBaseVM Owner { get; internal set; }
        public string InstanceId { get; set; }  //TODO: internal set


        #region PROP Description

        private string _description;

        public string Description
        {
            get { return this._description; }
            set
            {
                if (this._description != value)
                {
                    this._description = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }

        #endregion


        #region PROP RenderTriggerId

        private int _renderTriggerId;

        public int RenderTriggerId
        {
            get { return this._renderTriggerId; }
            private set
            {
                if (this._renderTriggerId != value)
                {
                    this._renderTriggerId = value;
                    this.OnPropertyChanged("RenderTriggerId");
                }
            }
        }

        #endregion


        public void InvalidateRender()
        {
            this.RenderTriggerId++;
        }


        public abstract void OnRender(CanvasDrawingContext dc);


        public virtual bool EvaluateRenderingFilter(string id)
        {
            return true;
        }


        #region EVT PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(
                    this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
