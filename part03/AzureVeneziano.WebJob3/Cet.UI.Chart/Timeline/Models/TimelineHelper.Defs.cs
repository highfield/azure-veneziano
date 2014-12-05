using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public static partial class TimelineHelper
    {
        /// <summary>
        /// Indica l'unita' di misura di uno snapper
        /// </summary>
        internal enum SnapperUnit
        {
            Millisecond,
            Second,
            Minute,
            Hour,
            Day,
            DayOfWeek,
            Month,
            Year,
        }


        static TimelineHelper()
        {
            //definisce tutti gli snapper possibili
            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms")
                };
                var snapper = new TimelineSnapperDecimal(1, SnapperUnit.Millisecond, offsets);
                snapper.Description = "1ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(1), "1ms"),
                };
                var snapper = new TimelineSnapperDecimal(2, SnapperUnit.Millisecond, offsets);
                snapper.Description = "2ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(1), "1ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(2), "2ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(3), "3ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(4), "4ms"),
                };
                var snapper = new TimelineSnapperDecimal(5, SnapperUnit.Millisecond, offsets);
                snapper.Description = "5ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(5), "5ms"),
                };
                var snapper = new TimelineSnapperDecimal(10, SnapperUnit.Millisecond, offsets);
                snapper.Description = "10ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(5), "5ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(10), "10ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(15), "15ms"),
                };
                var snapper = new TimelineSnapperDecimal(20, SnapperUnit.Millisecond, offsets);
                snapper.Description = "20ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(10), "10ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(20), "20ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(30), "30ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(40), "40ms"),
                };
                var snapper = new TimelineSnapperDecimal(50, SnapperUnit.Millisecond, offsets);
                snapper.Description = "50ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(50), "50ms"),
                };
                var snapper = new TimelineSnapperDecimal(100, SnapperUnit.Millisecond, offsets);
                snapper.Description = "100ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(50), "50ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(100), "100ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(150), "150ms"),
                };
                var snapper = new TimelineSnapperDecimal(200, SnapperUnit.Millisecond, offsets);
                snapper.Description = "200ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(100), "100ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(200), "200ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(300), "300ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(400), "400ms"),
                };
                var snapper = new TimelineSnapperDecimal(500, SnapperUnit.Millisecond, offsets);
                snapper.Description = "500ms";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0ms"),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(500), "500ms"),
                };
                var snapper = new TimelineSnapperDecimal(1000, SnapperUnit.Second, offsets);
                snapper.Description = "1\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(0), "0.0\""),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(500), "0.5\""),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(1000), "1.0\""),
                    new TimelineSpanIndication(TimeSpan.FromMilliseconds(1500), "1.5\""),
                };
                var snapper = new TimelineSnapperDecimal(2000, SnapperUnit.Second, offsets);
                snapper.Description = "2\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(1), "1\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(2), "2\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(3), "3\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(4), "4\""),
                };
                var snapper = new TimelineSnapperDecimal(5000, SnapperUnit.Second, offsets);
                snapper.Description = "5\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(5), "5\""),
                };
                var snapper = new TimelineSnapperDecimal(10000, SnapperUnit.Second, offsets);
                snapper.Description = "10\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(5), "5\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(10), "10\""),
                };
                var snapper = new TimelineSnapperDecimal(15000, SnapperUnit.Second, offsets);
                snapper.Description = "15\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(5), "5\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(10), "10\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(15), "15\""),
                };
                var snapper = new TimelineSnapperDecimal(20000, SnapperUnit.Second, offsets);
                snapper.Description = "20\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(10), "10\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(20), "20\""),
                };
                var snapper = new TimelineSnapperDecimal(30000, SnapperUnit.Second, offsets);
                snapper.Description = "30\"";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(10), "10\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(20), "20\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(30), "30\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(40), "40\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(50), "50\""),
                };
                var snapper = new TimelineSnapperDecimal(60000, SnapperUnit.Minute, offsets);
                snapper.Description = "1'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromSeconds(0), "0\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(30), "30\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(60), "60\""),
                    new TimelineSpanIndication(TimeSpan.FromSeconds(90), "90\""),
                };
                var snapper = new TimelineSnapperDecimal(2 * 60000, SnapperUnit.Minute, offsets);
                snapper.Description = "2'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(1), "1'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(2), "2'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(3), "3'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(4), "4'"),
                };
                var snapper = new TimelineSnapperDecimal(5 * 60000, SnapperUnit.Minute, offsets);
                snapper.Description = "5'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(5), "5'"),
                };
                var snapper = new TimelineSnapperDecimal(10 * 60000, SnapperUnit.Minute, offsets);
                snapper.Description = "10'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(5), "5'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(10), "10'"),
                };
                var snapper = new TimelineSnapperDecimal(15 * 60000, SnapperUnit.Minute, offsets);
                snapper.Description = "15'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(5), "5'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(10), "10'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(15), "15'"),
                };
                var snapper = new TimelineSnapperDecimal(20 * 60000, SnapperUnit.Minute, offsets);
                snapper.Description = "20'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(10), "10'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(20), "20'"),
                };
                var snapper = new TimelineSnapperDecimal(30 * 60000, SnapperUnit.Minute, offsets);
                snapper.Description = "30'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(10), "10'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(20), "20'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(30), "30'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(40), "40'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(50), "50'"),
                };
                var snapper = new TimelineSnapperDecimal(60 * 60000, SnapperUnit.Hour, offsets);
                snapper.Description = "60'";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromMinutes(0), "0'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(30), "30'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(60), "60'"),
                    new TimelineSpanIndication(TimeSpan.FromMinutes(90), "90'"),
                };
                var snapper = new TimelineSnapperDecimal(2 * 60 * 60000, SnapperUnit.Hour, offsets);
                snapper.Description = "2h";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromHours(0), "0h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(1), "1h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(2), "2h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(3), "3h"),
                };
                var snapper = new TimelineSnapperDecimal(4 * 60 * 60000, SnapperUnit.Hour, offsets);
                snapper.Description = "4h";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromHours(0), "0h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(1), "1h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(2), "2h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(3), "3h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(4), "4h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(5), "5h"),
                };
                var snapper = new TimelineSnapperDecimal(6 * 60 * 60000, SnapperUnit.Hour, offsets);
                snapper.Description = "6h";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromHours(0), "0h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(3), "3h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(6), "6h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(9), "9h"),
                };
                var snapper = new TimelineSnapperDecimal(12 * 60 * 60000, SnapperUnit.Hour, offsets);
                snapper.Description = "12h";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromHours(0), "0h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(6), "6h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(12), "12h"),
                    new TimelineSpanIndication(TimeSpan.FromHours(18), "18h"),
                };
                var snapper = new TimelineSnapperDecimal(24 * 60 * 60000, SnapperUnit.Day, offsets);
                snapper.Description = "24h";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(1), "1d"),
                };
                var snapper = new TimelineSnapperDecimal(48 * 60 * 60000, SnapperUnit.Day, offsets);
                snapper.Description = "2d";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(1), "1d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(2), "2d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(3), "3d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(4), "4d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(5), "5d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(6), "6d"),
                };
                var snapper = new TimelineSnapperWeek(offsets);
                snapper.Description = "7d";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(5), "5d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(10), "10d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(15), "15d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(20), "20d"),
                    new TimelineSpanIndication(TimeSpan.FromDays(25), "25d"),
                };
                var snapper = new TimelineSnapperMonth(1, offsets);
                snapper.Description = "1mo";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0mo"),
                };
                var snapper = new TimelineSnapperMonth(2, offsets);
                snapper.Description = "2mo";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0mo"),
                };
                var snapper = new TimelineSnapperMonth(3, offsets);
                snapper.Description = "3mo";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0mo"),
                };
                var snapper = new TimelineSnapperMonth(6, offsets);
                snapper.Description = "6mo";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0mo"),
                };
                var snapper = new TimelineSnapperYear(1, offsets);
                snapper.Description = "12mo";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0y"),
                };
                var snapper = new TimelineSnapperYear(2, offsets);
                snapper.Description = "2y";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0y"),
                };
                var snapper = new TimelineSnapperYear(5, offsets);
                snapper.Description = "5y";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0y"),
                };
                var snapper = new TimelineSnapperYear(10, offsets);
                snapper.Description = "10y";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0y"),
                };
                var snapper = new TimelineSnapperYear(20, offsets);
                snapper.Description = "20y";
                _snappers.Add(snapper);
            }

            {
                var offsets = new TimelineSpanIndication[]
                {
                    new TimelineSpanIndication(TimeSpan.FromDays(0), "0y"),
                };
                var snapper = new TimelineSnapperYear(50, offsets);
                snapper.Description = "50y";
                _snappers.Add(snapper);
            }
        }


        private static List<TimelineSnapper> _snappers = new List<TimelineSnapper>();


        public static IEnumerable<TimelineSnapper> Snappers
        {
            get { return _snappers; }
        }

    }
}
