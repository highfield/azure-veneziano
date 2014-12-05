using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;


namespace Cet.UI.Chart
{
    public class ColorToBrushConverter
        : IValueConverter
    {

        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            string s;
            if (value is Color)
            {
                var color = (Color)value;
                return new SolidColorBrush(color);
            }
            else if (value is Brush)
            {
                return value;
            }
            else if (string.IsNullOrWhiteSpace(s = value as string) == false)
            {
                return (Color)ColorConverter.ConvertFromString(s);
            }

            return null;
        }


        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            var brush = value as SolidColorBrush;

            if (brush != null)
            {
                return brush.Color;
            }

            return default(Color);
        }

    }
}
