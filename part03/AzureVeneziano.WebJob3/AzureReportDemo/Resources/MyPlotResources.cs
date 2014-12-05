using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace Cet.UI.Chart
{
    public sealed class MyPlotResources
    {
        #region Singleton pattern

        private MyPlotResources() { }


        private static MyPlotResources _instance;


        public static MyPlotResources Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MyPlotResources();

                return _instance;
            }
        }

        #endregion


        public ChartPlotBase GetPlot(string id)
        {
            var fi = this.GetType().GetField(
                id, 
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            var fn = (Func<ChartPlotBase>)fi.GetValue(null);
            ChartPlotBase instance = fn();
            return instance;
        }


        #region Available plots

        private static readonly Func<ChartPlotBase> PL1 = () => new ChartPlotCartesianLinear()
        {
            InstanceId = "PL1",
            Description = "Plot #1",
            HorizontalAxisId = "TL",
            VerticalAxisId = "AX1",
            LineColor = Colors.Blue,
        };


        private static readonly Func<ChartPlotBase> PL2 = () => new ChartPlotCartesianLinear()
        {
            InstanceId = "PL2",
            Description = "Plot #2",
            HorizontalAxisId = "TL",
            VerticalAxisId = "AX2",
            LineColor = Colors.Red,
        };


        private static readonly Func<ChartPlotBase> PB1 = () => new ChartPlotCartesianBoolean()
        {
            InstanceId = "PB1",
            Description = "Boolean #1",
            IsStairStep = true,
            HorizontalAxisId = "TL",
            VerticalAxisId = "BX",
        };


        private static readonly Func<ChartPlotBase> PB2 = () => new ChartPlotCartesianBoolean()
        {
            InstanceId = "PB2",
            Description = "Boolean #2",
            IsStairStep = true,
            HorizontalAxisId = "TL",
            VerticalAxisId = "BX",
        };


        private static readonly Func<ChartPlotBase> EVT1 = () => new ChartPlotCartesianEvent()
        {
            InstanceId = "EVT1",
            Description = "Events group 1",
            HorizontalAxisId = "TL",
        };


        private static readonly Func<ChartPlotBase> EVT2 = () => new ChartPlotCartesianEvent()
        {
            InstanceId = "EVT2",
            Description = "Events group 2",
            HorizontalAxisId = "TL",
        };


        private static readonly Func<ChartPlotBase> SV1 = () => new ChartPlotCartesianSeverity()
        {
            InstanceId = "SV1",
            Description = "Severities",
            HorizontalAxisId = "TL",
        };

        #endregion

    }
}
