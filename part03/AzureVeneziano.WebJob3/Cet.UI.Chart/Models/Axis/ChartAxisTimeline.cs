using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public class ChartAxisTimeline
        : ChartAxisBase<long>
    {
        public ChartAxisTimeline()
        {
            this.Formatter = new TimelineFormatter(this);
        }


        public override double ConvertToRelative(object value)
        {
            var absolute = Convert.ToInt64(value);
            return (absolute - this.LowerBound) / (double)(this.UpperBound - this.LowerBound);
        }


        public override object ConvertToAbsolute(double relative)
        {
            return this.LowerBound + (long)(relative * (this.UpperBound - this.LowerBound));
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


        internal TimelineCalculateTickArgs CreateCalculationArgs()
        {
            var si = new TimelineCalculateSegmentTickArgs();
            si.LowerBound = this.LowerBound;
            si.UpperBound = this.UpperBound;

            var ta = new TimelineCalculateTickArgs();
            ta.Formatter = TimelineHelper.StandardFormatter;
            //ta.LowestProgression = dt;
            ta.SegmentInfo = si;

            return ta;
        }


        public override void OnRender(CanvasDrawingContext dc)
        {
            var ta = this.CreateCalculationArgs();
            ta.SegmentInfo.StartPixel = 0.0;
            ta.SegmentInfo.EndPixel = dc.ClientArea.Width;

            this.Ticks = TimelineHelper.CalculateTicks(ta);

            DrawingHelpers.DrawTicks(
                dc,
                dc.ClientArea,
                this.Ticks,
                Dock.Top
                );
        }

    }
}
