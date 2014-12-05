using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public sealed class MyAxisResources
    {
        #region Singleton pattern

        private MyAxisResources() { }


        private static MyAxisResources _instance;


        public static MyAxisResources Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MyAxisResources();

                return _instance;
            }
        }

        #endregion


        public ChartAxisBase AddWhenRequired(ChartModelBase cm, string id)
        {
            ChartAxisBase axis = null;

            if (string.IsNullOrWhiteSpace(id) == false &&
                cm.Axes.Any(_ => _.InstanceId == id) == false)
            {
                axis = GetAxis(id);
                cm.Axes.Add(axis);
            }

            return axis;
        }


        public ChartAxisBase GetAxis(string id)
        {
            var fi = this.GetType().GetField(id, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            var fn = (Func< ChartAxisBase>)fi.GetValue(null);
            ChartAxisBase instance = fn();
            return instance;
        }


        #region Available axes

        private static readonly Func<ChartAxisBase> TL = () => new ChartAxisTimeline()
        {
            InstanceId = "TL",
            Orientation = ChartModelXY.AxisOrientationHorizontal,
        };


        private static readonly Func<ChartAxisBase> BX = () => new ChartAxisBoolean()
        {
            InstanceId = "BX",
        };


        private static readonly Func<ChartAxisBase> AX1 = () => new ChartAxisDouble()
        {
            InstanceId = "AX1",
            Orientation = ChartModelXY.AxisOrientationVertical,
            LowerBound = 0.0,
            UpperBound = 100.0,
            Description = "First axis",
            Formatter = new TemperatureFormatter(),
        };


        private static readonly Func<ChartAxisBase> AX2 = () => new ChartAxisDouble()
        {
            InstanceId = "AX2",
            Orientation = ChartModelXY.AxisOrientationVertical,
            LowerBound = -25.0,
            UpperBound = 150.0,
            Description = "Second axis",
            Formatter = new TemperatureFormatter(),
        };

        #endregion

    }
}
