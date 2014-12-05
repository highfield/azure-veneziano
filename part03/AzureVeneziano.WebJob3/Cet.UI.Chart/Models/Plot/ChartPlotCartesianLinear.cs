using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public class ChartPlotCartesianLinear
        : ChartPlotCartesianBase
    {

        public List<Point> Points { get; set; }
        public bool IsStairStep { get; set; }


        #region PROP LineColor

        private Color _lineColor = Colors.Black;

        public Color LineColor
        {
            get { return this._lineColor; }
            set
            {
                if (this._lineColor != value)
                {
                    this._lineColor = value;
                    this.OnPropertyChanged("LineColor");

                    this.InvalidateRender();
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
            var axisY = cm.Axes.Single(_ => _.InstanceId == this.VerticalAxisId);

            //create the pen to be used for drawing
            var pen = new Pen(
                new SolidColorBrush(this.LineColor),
                2.0
                );

            pen.Freeze();

            const double areaLeft = 0;
            const double areaTop = 0;
            double x0 = double.NaN;
            double y0 = double.NaN;
            double x1, y1 = double.NaN;

            double lowestRelative = axisX.GetLowestRelative();
            double hightestRelative = axisX.GetHightestRelative();

            if (double.IsNaN(lowestRelative) == false)
            {
                //calculate the very first point (lowest progression)
                x0 = Math.Round(lowestRelative * dc.ClientArea.Width + areaLeft);
            }

            //draw segments up to the complete viewport
            for (int i = 0, count = this.Points.Count; i < count; i++)
            {
                Point pt = this.Points[i];
                double xrel = axisX.ConvertToRelative(pt.X);
                double yrel = axisY.ConvertToRelative(pt.Y);

                x1 = Math.Round(xrel * dc.ClientArea.Width + areaLeft);
                y1 = Math.Round((1.0 - yrel) * dc.ClientArea.Height + areaTop);

                if (double.IsNaN(x0) == false &&
                    x0 < (dc.ClientArea.Width + areaLeft) &&
                    x1 >= areaLeft)
                {
                    if (double.IsNaN(y0))
                    {
                        //estrapolazione del valore PRIMA del primo campione disponibile
                        DrawingHelpers.DrawDashedLine(
                            dc,
                            pen,
                            new Point(x0, y1),
                            new Point(x1, y1)
                            );
                    }
                    else if (this.IsStairStep)
                    {
                        dc.DrawLine(
                            pen,
                            new Point(x0, y0),
                            new Point(x1, y0)
                            );

                        dc.DrawLine(
                            pen,
                            new Point(x1, y0),
                            new Point(x1, y1)
                            );
                    }
                    else
                    {
                        dc.DrawLine(
                            pen,
                            new Point(x0, y0),
                            new Point(x1, y1)
                            );
                    }
                }

                x0 = x1;
                y0 = y1;
            }

            if (double.IsNaN(y0) == false &&
                double.IsNaN(hightestRelative) == false)
            {
                //calculate the very last point (hightest progression)
                x1 = Math.Round(hightestRelative * dc.ClientArea.Width + areaLeft);

                //estrapolazione del valore DOPO l'ultimo campione disponibile
                DrawingHelpers.DrawDashedLine(
                    dc,
                    pen,
                    new Point(x0, y1),
                    new Point(x1, y1)
                    );
            }
        }


        private class XPointComparer
            : Comparer<Point>
        {

            public override int Compare(Point a, Point b)
            {
                return a.X.CompareTo(b.X);
            }

        }

    }
}
