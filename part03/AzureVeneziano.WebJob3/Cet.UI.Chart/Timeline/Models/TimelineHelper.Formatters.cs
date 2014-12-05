using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public static partial class TimelineHelper
    {
        /// <summary>
        /// Formattatore standard
        /// </summary>
        /// <param name="collection"></param>
        public static void StandardFormatter(IEnumerable<ChartTickInfo> collection)
        {
            foreach (TimelineTickInfo info in collection)
            {
                var unit = _snappers[info.TopIndex].Unit;

                switch (unit)
                {
                    case TimelineHelper.SnapperUnit.Millisecond:
                        var ticks = info.UserTimestamp.Ticks + 5000L;
                        info.MajorText = new DateTime(ticks).ToString("s'\"' fff");
                        break;

                    case TimelineHelper.SnapperUnit.Second:
                        info.MajorText = info.UserTimestamp.ToString("s'\"'");
                        break;

                    case TimelineHelper.SnapperUnit.Minute:
                        info.MajorText = info.UserTimestamp.ToString("m\"'\"");
                        break;

                    case TimelineHelper.SnapperUnit.Hour:
                        info.MinorText = info.UserTimestamp.ToString("tt");

                        if (string.IsNullOrEmpty(info.MinorText))
                        {
                            info.MajorText = info.UserTimestamp.ToString("%H'h'");
                        }
                        else
                        {
                            info.MajorText = info.UserTimestamp.ToString("%h");
                        }
                        break;

                    case TimelineHelper.SnapperUnit.Day:
                        info.MajorText = info.UserTimestamp.ToString("%d");
                        info.MinorText = info.UserTimestamp.ToString("MMM yy");
                        break;

                    case TimelineHelper.SnapperUnit.DayOfWeek:
                        info.MajorText = info.UserTimestamp.ToString("ddd");
                        info.MinorText = TimelineHelper.ShortMonthDayPattern(info.UserTimestamp);
                        break;

                    case TimelineHelper.SnapperUnit.Month:
                        info.MajorText = info.UserTimestamp.ToString("MMM");
                        info.MinorText = info.UserTimestamp.ToString("yyyy");
                        break;

                    case TimelineHelper.SnapperUnit.Year:
                        info.MajorText = info.UserTimestamp.ToString("MMM");
                        info.MinorText = info.UserTimestamp.ToString("yyyy");
                        break;
                }
            }
        }


        /// <summary>
        /// Formattatore ridotto
        /// </summary>
        /// <param name="collection"></param>
        public static void TinyFormatter(IEnumerable<ChartTickInfo> collection)
        {
            foreach (TimelineTickInfo info in collection)
            {
                var unit = _snappers[info.TopIndex].Unit;

                switch (unit)
                {
                    case TimelineHelper.SnapperUnit.Millisecond:
                        var ticks = info.UserTimestamp.Ticks + 5000L;
                        info.MajorText = new DateTime(ticks).ToString("s'\"' fff");
                        break;

                    case TimelineHelper.SnapperUnit.Second:
                        info.MajorText = info.UserTimestamp.ToString("s'\"'");
                        break;

                    case TimelineHelper.SnapperUnit.Minute:
                        info.MajorText = info.UserTimestamp.ToString("m\"'\"");
                        break;

                    case TimelineHelper.SnapperUnit.Hour:
                        var tt = info.UserTimestamp.ToString("tt");

                        if (string.IsNullOrEmpty(tt))
                        {
                            info.MajorText = info.UserTimestamp.ToString("%H'h'");
                        }
                        else
                        {
                            info.MajorText = info.UserTimestamp.ToString("%h tt");
                        }
                        break;

                    case TimelineHelper.SnapperUnit.Day:
                        info.MajorText = info.UserTimestamp.ToString("%d MMM");
                        break;

                    case TimelineHelper.SnapperUnit.DayOfWeek:
                        info.MajorText = TimelineHelper.ShortMonthDayPattern(info.UserTimestamp);
                        break;

                    case TimelineHelper.SnapperUnit.Month:
                        info.MajorText = info.UserTimestamp.ToString("MMM yy");
                        break;

                    case TimelineHelper.SnapperUnit.Year:
                        info.MajorText = info.UserTimestamp.ToString("MMM yy");
                        break;
                }
            }
        }

    }
}
