using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public class ChartPlotCartesianSeverity
        : ChartPlotCartesianBase
    {

        public List<ChartPointSeverity> Points { get; set; }


        #region PROP DefaultColor

        private Color _defaultColor = Colors.Gray;

        public Color DefaultColor
        {
            get { return this._defaultColor; }
            set
            {
                if (this._defaultColor != value)
                {
                    this._defaultColor = value;
                    this.OnPropertyChanged("DefaultColor");
                }
            }
        }

        #endregion


        public override void OnRender(CanvasDrawingContext dc)
        {
            this.RenderCore(dc);
        }


        protected void RenderCore(CanvasDrawingContext dc)
        {
            var cm = this.Owner as ChartModelXY;

            this.IsEmpty =
                cm == null ||
                this.Points == null ||
                this.Points.Count == 0;

            if (this.IsEmpty)
            {
                return;
            }

            var axisX = cm.Axes.Single(_ => _.InstanceId == this.HorizontalAxisId);

            //create the pen to be used for drawing
            var pen = new Pen(
                Brushes.DimGray,
                1.0
                );

            pen.Freeze();

            const double areaLeft = 0;
            const double areaTop = 0;
            double x0 = double.NaN;
            double x1 = double.NaN;
            Color color0 = this.DefaultColor;
            Color color1 = this.DefaultColor;
            //int level0 = 0;
            //int level1 = 0;

            double lowestRelative = axisX.GetLowestRelative();
            double hightestRelative = axisX.GetHightestRelative();

            if (double.IsNaN(lowestRelative) == false)
            {
                //calculate the very first point (lowest progression)
                x0 = Math.Round(lowestRelative * dc.ClientArea.Width + areaLeft);
            }

            //draw segments up to the complete viewport
            double y0 = areaTop;
            double y1 = areaTop + dc.ClientArea.Height;

            for (int i = 0, count = this.Points.Count; i < count; i++)
            {
                ChartPointSeverity pt = this.Points[i];
                double xrel = axisX.ConvertToRelative(pt.X);

                x1 = Math.Round(xrel * dc.ClientArea.Width + areaLeft);
                
                var topmost = (pt.Items ?? Enumerable.Empty<ChartPointSeverityItem>())
                    .FirstOrDefault();

                color1 = topmost != null
                    ? topmost.Color
                    : this.DefaultColor;
                //level1 = pt.Level;

                if (double.IsNaN(x0) == false &&
                    x0 < (dc.ClientArea.Width + areaLeft) &&
                    x1 >= areaLeft)
                {
                    dc.DrawRectangle(
                        //this.GetLevelBrush(level0),
                        new SolidColorBrush(color0),
                        pen,
                        new Rect(x0, y0, x1 - x0, y1 - y0)
                        );
                }

                x0 = x1;
                color0 = color1;
                //level0 = level1;
            }

            if (double.IsNaN(hightestRelative) == false)
            {
                var penext = new Pen(
                    Brushes.DimGray,
                    1.0
                    );

                penext.DashStyle = DashStyles.Dash;
                penext.Freeze();

                //calculate the very last point (hightest progression)
                x1 = Math.Round(hightestRelative * dc.ClientArea.Width + areaLeft);

                //estrapolazione del valore DOPO l'ultimo campione disponibile
                dc.DrawRectangle(
                    //this.GetLevelBrush(level0),
                    new SolidColorBrush(color0),
                    penext,
                    new Rect(x0, y0, x1 - x0, y1 - y0)
                    );
            }
        }

    }
}
