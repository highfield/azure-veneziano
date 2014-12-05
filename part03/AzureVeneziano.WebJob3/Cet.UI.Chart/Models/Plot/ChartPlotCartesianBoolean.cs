using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public class ChartPlotCartesianBoolean
        : ChartPlotCartesianLinear
    {
        private const double VerticalPadding = 0.2; //relative


        public ChartPlotCartesianBoolean()
        {
            this.IsStairStep = true;
        }


        public override void OnRender(CanvasDrawingContext dc)
        {
            //draw upper/lower bound lines
            double dashPeriod = 4.0;
            double dashDuty = 0.5;

            double yd;

            yd = Math.Round(dc.ClientArea.Height * (1.0 - VerticalPadding));
            DrawingHelpers.DrawDashedLine(
                dc,
                dc.GetTickPen(0),
                new Point(0.0, yd),
                new Point(dc.ClientArea.Width, yd),
                dashPeriod,
                dashDuty
                );

            yd = Math.Round(dc.ClientArea.Height * VerticalPadding);
            DrawingHelpers.DrawDashedLine(
                dc,
                dc.GetTickPen(0),
                new Point(0.0, yd),
                new Point(dc.ClientArea.Width, yd),
                dashPeriod,
                dashDuty
                );

            //draw plot
            dc.PushTransform(
                new TranslateTransform(0.0, dc.ClientArea.Height * VerticalPadding)
                );

            dc.PushTransform(
                new ScaleTransform(1.0, 1.0 - VerticalPadding * 2)
                );

            base.RenderCore(dc);

            dc.Pop();
            dc.Pop();
        }

    }
}
