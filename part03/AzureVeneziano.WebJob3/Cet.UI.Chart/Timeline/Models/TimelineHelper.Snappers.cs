using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cet.UI.Chart
{
    public static partial class TimelineHelper
    {
        #region CLS TimelineSnapper

        public abstract class TimelineSnapper
        {
            internal TimelineSnapper(
                SnapperUnit unit,
                IEnumerable<TimelineSpanIndication> offsets)
            {
                this.Unit = unit;
                this.Offsets = offsets;
            }


            internal SnapperUnit Unit { get; private set; }
            public IEnumerable<TimelineSpanIndication> Offsets { get; private set; }
            public abstract long NominalExtent { get; }
            public string Description { get; internal set; }
            internal abstract bool CanBeUsed(TimelineSnapper reference);
            internal abstract long Snap(long value);


            public TimelineSpanIndication GetIndication()
            {
                return new TimelineSpanIndication(
                    TimeSpan.FromTicks(this.NominalExtent),
                    this.Description);
            }
        }

        #endregion


        #region CLS TimelineSnapperDecimal

        public class TimelineSnapperDecimal
            : TimelineSnapper
        {
            internal TimelineSnapperDecimal(
                int milliseconds,
                SnapperUnit unit,
                IEnumerable<TimelineSpanIndication> offsets)
                : base(unit, offsets)
            {
                this._extent = milliseconds * 10000L;
                this.Milliseconds = milliseconds;
            }


            private long _extent;

            internal int Milliseconds { get; private set; }


            public override long NominalExtent
            {
                get { return this._extent; }
            }


            internal override bool CanBeUsed(TimelineSnapper reference)
            {
                var decimalStepper = reference as TimelineSnapperDecimal;

                if (decimalStepper != null)
                {
                    return (this.Milliseconds % decimalStepper.Milliseconds) == 0;
                }
                else
                {
                    return true;
                }
            }


            internal override long Snap(long value)
            {
                return (long)(this._extent * Math.Round(value / (double)this._extent));
            }
        }

        #endregion


        #region CLS TimelineSnapperWeek

        public class TimelineSnapperWeek
            : TimelineSnapper
        {
            internal TimelineSnapperWeek(IEnumerable<TimelineSpanIndication> offsets)
                : base(SnapperUnit.DayOfWeek, offsets)
            {
            }


            private long _extent = TimeSpan.FromDays(7).Ticks;


            public override long NominalExtent
            {
                get { return this._extent; }
            }


            internal override bool CanBeUsed(TimelineSnapper reference)
            {
                return false;
            }


            internal override long Snap(long value)
            {
                return (long)(this._extent * Math.Round(value / (double)this._extent));
            }
        }

        #endregion


        #region CLS TimelineSnapperMonth

        public class TimelineSnapperMonth
            : TimelineSnapper
        {
            private const double AverageDaysInMonth = (365 * 4 + 1) / 48.0;


            internal TimelineSnapperMonth(
                int months,
                IEnumerable<TimelineSpanIndication> offsets)
                : base(SnapperUnit.Month, offsets)
            {
                this.Months = months;
                this._extent = months * TimeSpan.FromDays(TimelineSnapperMonth.AverageDaysInMonth).Ticks;
            }


            private long _extent;

            internal int Months { get; private set; }


            public override long NominalExtent
            {
                get { return this._extent; }
            }


            internal override bool CanBeUsed(TimelineSnapper reference)
            {
                var monthStepper = reference as TimelineSnapperMonth;

                if (monthStepper != null)
                {
                    return false;// (this.Months % monthStepper.Months) == 0;
                }
                else
                {
                    return true;
                }
            }


            internal override long Snap(long value)
            {
                var timestamp = new DateTime(value);

                switch (this.Months)
                {
                    case 1:
                        return this.SnapMonth1(timestamp);

                    case 2:
                        return this.SnapMonth2(timestamp);

                    case 3:
                        return this.SnapMonth3(timestamp);

                    case 6:
                        return this.SnapMonth6(timestamp);
                }

                throw new NotImplementedException();
            }


            private long SnapMonth1(DateTime timestamp)
            {
                //arrotonda al mese
                int days = DateTime.DaysInMonth(
                    timestamp.Year,
                    timestamp.Month);
                timestamp += TimeSpan.FromDays(days / 2.0);

                return new DateTime(
                    timestamp.Year,
                    timestamp.Month,
                    1).Ticks;
            }


            private long SnapMonth2(DateTime timestamp)
            {
                //arrotonda al bimestre
                if ((timestamp.Month % 2) != 0)
                {
                    return new DateTime(
                        timestamp.Year,
                        timestamp.Month,
                        1).Ticks;
                }
                else if (timestamp.Month < 12)
                {
                    return new DateTime(
                        timestamp.Year,
                        timestamp.Month + 1,
                        1).Ticks;
                }
                else
                {
                    return new DateTime(
                        timestamp.Year + 1,
                        1,
                        1).Ticks;
                }
            }


            private long SnapMonth3(DateTime timestamp)
            {
                //arrotonda al trimestre
                int month = timestamp.Month;

                switch (month)
                {
                    case 2:
                    case 5:
                    case 8:
                    case 11:
                        int days = DateTime.DaysInMonth(
                            timestamp.Year,
                            month);

                        bool isEven = ((days & 1) == 0);
                        int threshold = (days >> 1) + 1;

                        if (timestamp.Day > threshold)
                        {
                            month++;
                        }
                        else if (timestamp.Day < threshold)
                        {
                            month--;
                        }
                        else if (isEven || timestamp.Hour >= 12)
                        {
                            month++;
                        }
                        else
                        {
                            month--;
                        }
                        break;
                }

                switch (month)
                {
                    case 1:
                        return new DateTime(
                            timestamp.Year,
                            1,
                            1).Ticks;

                    case 3:
                        return new DateTime(
                            timestamp.Year,
                            4,
                            1).Ticks;

                    case 4:
                        return new DateTime(
                            timestamp.Year,
                            4,
                            1).Ticks;

                    case 6:
                        return new DateTime(
                            timestamp.Year,
                            7,
                            1).Ticks;

                    case 7:
                        return new DateTime(
                            timestamp.Year,
                            7,
                            1).Ticks;

                    case 9:
                        return new DateTime(
                            timestamp.Year,
                            10,
                            1).Ticks;

                    case 10:
                        return new DateTime(
                            timestamp.Year,
                            10,
                            1).Ticks;

                    case 12:
                        return new DateTime(
                            timestamp.Year + 1,
                            1,
                            1).Ticks;
                }

                throw new NotImplementedException();
            }


            private long SnapMonth6(DateTime timestamp)
            {
                //arrotonda al semestre
                if (timestamp.Month <= 3)
                {
                    return new DateTime(
                        timestamp.Year,
                        1,
                        1).Ticks;
                }
                else if (timestamp.Month >= 10)
                {
                    return new DateTime(
                        timestamp.Year + 1,
                        1,
                        1).Ticks;
                }
                else
                {
                    return new DateTime(
                        timestamp.Year,
                        7,
                        1).Ticks;
                }
            }
        }

        #endregion


        #region CLS TimelineSnapperYear

        public class TimelineSnapperYear
            : TimelineSnapper
        {
            private const double AverageDaysInYear = (365 * 4 + 1) / 4.0;


            internal TimelineSnapperYear(
                int years,
                IEnumerable<TimelineSpanIndication> offsets)
                : base(SnapperUnit.Year, offsets)
            {
                this.Years = years;
                this._extent = years * TimeSpan.FromDays(TimelineSnapperYear.AverageDaysInYear).Ticks;
            }


            private long _extent;

            internal int Years { get; private set; }


            public override long NominalExtent
            {
                get { return this._extent; }
            }


            internal override bool CanBeUsed(TimelineSnapper reference)
            {
                var yearStepper = reference as TimelineSnapperYear;

                if (yearStepper != null)
                {
                    return false; // (this.Years % yearStepper.Years) == 0;
                }
                else
                {
                    return true;
                }
            }


            internal override long Snap(long value)
            {
                var timestamp = new DateTime(value);

                double year = timestamp.Year + (timestamp.Month - 1) / 12.0;
                year = this.Years * Math.Round(year / (double)this.Years);

                return new DateTime(
                    (int)year,
                    1,
                    1).Ticks;
            }
        }

        #endregion

    }
}
