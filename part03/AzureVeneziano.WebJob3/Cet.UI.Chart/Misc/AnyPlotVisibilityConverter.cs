using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public class AnyPlotVisibilityConverter
        : IValueConverter
    {
        public object Convert(
            object value, 
            Type targetType, 
            object parameter, 
            System.Globalization.CultureInfo culture)
        {
            var list = value as IEnumerable<ChartPlotBase>;
            return (list != null && list.Any())
                ? Visibility.Visible
                : Visibility.Collapsed;
        }


        public object ConvertBack(
            object value, 
            Type targetType, 
            object parameter, 
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
