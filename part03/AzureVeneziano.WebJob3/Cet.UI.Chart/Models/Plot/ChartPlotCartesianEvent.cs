using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public class ChartPlotCartesianEvent
        : ChartPlotCartesianBase
    {

        public List<ChartPointEvent> Points { get; set; }


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

            //create the pen to be used for drawing
            var pen = new Pen(
                new SolidColorBrush(this.LineColor),
                1.0
                );

            pen.Freeze();

            //draw segments up to the complete viewport
            const double areaLeft = 0.0;
            const double areaTop = 0.0;
            double y0 = areaTop;
            double y1 = areaTop + dc.ClientArea.Height;

            double yt = areaTop + 0.7 * dc.ClientArea.Height;

            for (int i = 0, count = this.Points.Count; i < count; i++)
            {
                ChartPointEvent pt = this.Points[i];
                double xrel = axisX.ConvertToRelative(pt.X);
                double x = Math.Round(xrel * dc.ClientArea.Width + areaLeft);

                if (x < (dc.ClientArea.Width + areaLeft) &&
                    x >= areaLeft)
                {
                    dc.DrawLine(
                        pen,
                        new Point(x, y0),
                        new Point(x, y1)
                        );

                    //TODO: controllare sovrapposizione scritte
                    if (string.IsNullOrWhiteSpace(pt.Description) == false)
                    {
                        var fface = dc.GetTickFace(0);
                        var fsize = 10.0;

                        var sz = dc.MeasureText(
                            pt.Description,
                            fface,
                            fsize
                            );

                        var pos = new Point(
                            x - 0.0 - sz.Height,
                            yt - sz.Width * 0.5
                            );

                        dc.PushTransform(
                            new TranslateTransform(pos.X, pos.Y)
                            );

                        dc.PushTransform(
                            new RotateTransform(-90.0)
                            );

                        dc.DrawText(
                            pt.Description,
                            fface,
                            fsize,
                            dc.GetTickBrush(0),
                            new Point()
                            );

                        dc.Pop();
                        dc.Pop();
                    }
                }
            }
        }

    }
}
