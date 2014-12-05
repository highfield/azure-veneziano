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
    public class ChartAxisBoolean
        : ChartAxisBase<bool>
    {
        public ChartAxisBoolean()
        {
        }


        public override double ConvertToRelative(object value)
        {
            var absolute = Convert.ToBoolean(value);
            return absolute ? 1.0 : 0.0;
        }


        public override object ConvertToAbsolute(double relative)
        {
            return relative >= 0.5;
        }


        public override double GetLowestRelative()
        {
            return double.NaN;
        }


        public override double GetHightestRelative()
        {
            return double.NaN;
        }


        public override void OnRender(CanvasDrawingContext dc)
        {
            //do nothing
        }

    }
}
