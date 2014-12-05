using Cet.UI.Chart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.WebJob
{
    public class PercentFormatter
        : IAxisFormatter, ICustomFormatter
    {

        public string MeasureUnit
        {
            get { return "%"; }
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
            // Provide default formatting if arg is not an Double. 
            if (arg.GetType() != typeof(Double))
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

            // Provide default formatting for unsupported format strings. 
            string ufmt = fmt.ToUpper(CultureInfo.InvariantCulture);

            if (string.IsNullOrWhiteSpace(ufmt))
                ufmt = "LU";

            string result;
            switch (ufmt[0])
            {
                case 'L':
                    result = string.Format("{0:F3}", arg);
                    break;

                case 'S':
                    result = string.Format("{0:F1}", arg);
                    break;

                default:
                    try
                    {
                        return HandleOtherFormats(fmt, arg);
                    }
                    catch (FormatException e)
                    {
                        throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
                    }
            }

            if (ufmt.Length > 1 &&
                ufmt[1] == 'U')
            {
                result += " " + this.MeasureUnit;
            }

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
