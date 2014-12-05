using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.UI.Chart
{
    public class TimelineFormatter
        : IAxisFormatter, ICustomFormatter
    {
        public TimelineFormatter(ChartAxisTimeline axis)
        {
            this._axis = axis;
        }

        private ChartAxisTimeline _axis;


        public string MeasureUnit
        {
            get { return string.Empty; }
        }


        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }


        public string Format(string fmt, object arg, IFormatProvider formatProvider)
        {
            //provide default formatting if arg is not an Int64. 
            if (arg.GetType() != typeof(long))
            {
                try
                {
                    return HandleOtherFormats(fmt, arg);
                }
                catch (FormatException e)
                {
                    throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
                }
            }

            var ta = this._axis.CreateCalculationArgs();
            var dt = TimelineHelper.CalculateActualTimestamp(ta, (long)arg);

            var result = string.Format("{0}", dt);
            return result;
        }


        private string HandleOtherFormats(string format, object arg)
        {
            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }

    }
}
