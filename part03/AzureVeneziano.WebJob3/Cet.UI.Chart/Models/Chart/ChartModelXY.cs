using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public class ChartModelXY
        : ChartModelBase
    {
        public const string AxisOrientationHorizontal = "H";
        public const string AxisOrientationVertical = "V";


        public ChartModelXY()
        {
            this.Plots.CollectionChanged += this.PlotsCollectionChanged;
            this.Axes.CollectionChanged += AxesCollectionChanged;
        }


        #region PROP StandardPlots

        private IEnumerable<ChartPlotBase> _standardPlots;

        public IEnumerable<ChartPlotBase> StandardPlots
        {
            get { return this._standardPlots; }
            private set
            {
                if (this._standardPlots != value)
                {
                    this._standardPlots = value;
                    this.OnPropertyChanged("StandardPlots");
                }
            }
        }

        #endregion


        #region PROP BooleanPlots

        private IEnumerable<ChartPlotBase> _booleanPlots;

        public IEnumerable<ChartPlotBase> BooleanPlots
        {
            get { return this._booleanPlots; }
            private set
            {
                if (this._booleanPlots != value)
                {
                    this._booleanPlots = value;
                    this.OnPropertyChanged("BooleanPlots");
                }
            }
        }

        #endregion


        #region PROP SeverityPlots

        private IEnumerable<ChartPlotBase> _severityPlots;

        public IEnumerable<ChartPlotBase> SeverityPlots
        {
            get { return this._severityPlots; }
            private set
            {
                if (this._severityPlots != value)
                {
                    this._severityPlots = value;
                    this.OnPropertyChanged("SeverityPlots");
                }
            }
        }

        #endregion


        #region PROP LegendPlots

        private IEnumerable<ChartPlotBase> _legendPlots;

        public IEnumerable<ChartPlotBase> LegendPlots
        {
            get { return this._legendPlots; }
            private set
            {
                if (this._legendPlots != value)
                {
                    this._legendPlots = value;
                    this.OnPropertyChanged("LegendPlots");
                }
            }
        }

        #endregion


        #region PROP HorizontalAxes

        private IEnumerable<ChartAxisBase> _horizontalAxes;

        public IEnumerable<ChartAxisBase> HorizontalAxes
        {
            get { return this._horizontalAxes; }
            private set
            {
                if (this._horizontalAxes != value)
                {
                    this._horizontalAxes = value;
                    this.OnPropertyChanged("HorizontalAxes");
                }
            }
        }

        #endregion


        #region PROP VerticalAxes

        private IEnumerable<ChartAxisBase> _verticalAxes;

        public IEnumerable<ChartAxisBase> VerticalAxes
        {
            get { return this._verticalAxes; }
            private set
            {
                if (this._verticalAxes != value)
                {
                    this._verticalAxes = value;
                    this.OnPropertyChanged("VerticalAxes");
                }
            }
        }

        #endregion


        void PlotsCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs e)
        {
            var severityPlots = new List<ChartPlotBase>();
            var booleanPlots = new List<ChartPlotBase>();
            var standardPlots = new List<ChartPlotBase>();
            var legendPlots = new List<ChartPlotBase>();

            foreach (var plot in this.Plots)
            {
                var standard = plot as ChartPlotCartesianBase;
                var @boolean = plot as ChartPlotCartesianBoolean;
                var severity = plot as ChartPlotCartesianSeverity;

                if (@boolean != null)
                {
                    booleanPlots.Add(plot);
                }
                else if (severity != null)
                {
                    severityPlots.Add(severity);
                    legendPlots.Add(severity);
                }
                else if (standard != null)
                {
                    standardPlots.Add(plot);
                    legendPlots.Add(standard);
                }
            }

            this.SeverityPlots = severityPlots;
            this.BooleanPlots = booleanPlots;
            this.StandardPlots = standardPlots;
            this.LegendPlots = legendPlots;
        }


        void AxesCollectionChanged(
            object sender,
            NotifyCollectionChangedEventArgs e)
        {
            this.HorizontalAxes = this
                .Axes
                .Where(_ => _.Orientation == AxisOrientationHorizontal)
                .ToList();

            this.VerticalAxes = this
                .Axes
                .Where(_ => _.Orientation == AxisOrientationVertical)
                .ToList();
        }


        public override void OnRender(CanvasDrawingContext dc)
        {
            //render horizontal axis divisions
            foreach (var axis in this.HorizontalAxes ?? Enumerable.Empty<ChartAxisBase>())
            {
                DrawingHelpers.DrawDivisions(
                    dc,
                    dc.ClientArea,
                    axis.Ticks,
                    Orientation.Vertical
                    );
                break;
            }

            //render vertical axis divisions
            foreach (var axis in this.VerticalAxes ?? Enumerable.Empty<ChartAxisBase>())
            {
                DrawingHelpers.DrawDivisions(
                    dc,
                    dc.ClientArea,
                    axis.Ticks,
                    Orientation.Horizontal
                    );
                break;
            }

            //draw plots, if applies
            foreach (var renderable in this.StandardPlots ?? Enumerable.Empty<ChartPlotBase>())
            {
                renderable.InvalidateRender();
            }

            foreach (var renderable in this.BooleanPlots ?? Enumerable.Empty<ChartPlotBase>())
            {
                renderable.InvalidateRender();
            }

            foreach (var renderable in this.SeverityPlots ?? Enumerable.Empty<ChartPlotBase>())
            {
                renderable.InvalidateRender();
            }
        }

    }
}
