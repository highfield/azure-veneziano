using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public class ChartAxisDouble
        : ChartAxisBase<double>
    {
        public ChartAxisDouble()
        {
            this.LowestBound = double.NaN;
            this.HightestBound = double.NaN;
        }


        private int VerticalMajorDivisionCount = 2; //TEMP
        private int VerticalMinorDivisionCount = 5; //TEMP


        public override double ConvertToRelative(object value)
        {
            var absolute = Convert.ToDouble(value);
            return (absolute - this.LowerBound) / (this.UpperBound - this.LowerBound);
        }


        public override object ConvertToAbsolute(double relative)
        {
            return this.LowerBound + relative * (this.UpperBound - this.LowerBound);
        }


        public override double GetLowestRelative()
        {
            if (this.LowestBound > 0)
            {
                return this.ConvertToRelative(this.LowestBound);
            }
            else
            {
                return double.NaN;
            }
        }


        public override double GetHightestRelative()
        {
            if (this.HightestBound > 0)
            {
                return this.ConvertToRelative(this.HightestBound);
            }
            else
            {
                return double.NaN;
            }
        }


        public override void OnRender(CanvasDrawingContext dc)
        {
            this.Ticks = new ChartTickSegmentCollection(Guid.Empty);    //TEMP!!

            var segm = new ChartTickSegment();
            segm.IsValid = true;
            this.Ticks.Add(segm);

            if (this.VerticalMajorDivisionCount > 1 &&
                this.VerticalMinorDivisionCount > 1)
            {
                //draw ticks
                int totalDivisionCount = this.VerticalMajorDivisionCount * this.VerticalMinorDivisionCount;

                for (int major = 0; major <= this.VerticalMajorDivisionCount; major++)
                {
                    double relativePosition = (double)major / this.VerticalMajorDivisionCount;
                    var pattern = "{0:S}";
                    //var pattern = major == 0//this.VerticalMajorDivisionCount
                    //    ? "{0:SU}"
                    //    : "{0:S}";

                    var tick = new ChartTickInfo();
                    tick.PixelPosition = dc.ClientArea.Height * relativePosition;
                    tick.Priority = 1;
                    tick.MajorText = string.Format(this.Formatter, pattern, this.ConvertToAbsolute(relativePosition));
                    segm.Add(tick);

                    for (int minor = 1; minor < this.VerticalMinorDivisionCount; minor++)
                    {
                        relativePosition = (double)(major * this.VerticalMinorDivisionCount + minor) / totalDivisionCount;
                        if (relativePosition > 1.001)
                            break;

                        tick = new ChartTickInfo();
                        tick.PixelPosition = dc.ClientArea.Height * relativePosition;
                        tick.Priority = 0;
                        tick.MajorText = string.Format(this.Formatter, "{0:S}", this.ConvertToAbsolute(relativePosition));
                        segm.Add(tick);
                    }
                }
            }

            DrawingHelpers.DrawTicks(
                dc,
                dc.ClientArea,
                this.Ticks,
                Dock.Right
                );


            if (this.Formatter != null)
            {
                //indicazione unità di misura
                //var attr = this.Formatter
                //    .GetType()
                //    .GetCustomAttributes(false)
                //    .OfType<MeasureUnitAttribute>()
                //    .FirstOrDefault();

                //if (attr != null)
                {
                    var fface = dc.GetTickFace(1);
                    var fsize = 13.0;

                    var sz = dc.MeasureText(
                        this.Formatter.MeasureUnit ?? string.Empty,
                        fface,
                        fsize
                        );

                    var y = (segm[segm.Count - 2].PixelPosition + segm[segm.Count - 1].PixelPosition) / 2.0;

                    var pt = new Point(
                        dc.ClientArea.Width - 6.0 - sz.Width,
                        y - sz.Height * 0.5
                        );

                    dc.DrawText(
                        this.Formatter.MeasureUnit ?? string.Empty,
                        fface,
                        fsize,
                        dc.GetTickBrush(1),
                        pt
                        );
                }
            }
            {
                var fface = dc.GetTickFace(1);
                var fsize = 13.0;

                var sz = dc.MeasureText(
                    this.Description,
                    fface,
                    fsize
                    );

                var pt = new Point(
                    dc.ClientArea.Width - 16.0 - sz.Width,
                    dc.ClientArea.Height + 12.0
                    );

                dc.DrawText(
                    this.Description,
                    fface,
                    fsize,
                    dc.GetTickBrush(1),
                    pt
                    );
            }
        }

    }
}
