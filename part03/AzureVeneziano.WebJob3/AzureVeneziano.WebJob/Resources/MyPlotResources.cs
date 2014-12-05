using Cet.UI.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace AzureVeneziano.WebJob
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

        private static readonly Func<ChartPlotBase> Analog0 = () => new ChartPlotCartesianLinear()
        {
            InstanceId = "Analog0",
            Description = "Analog #0",
            HorizontalAxisId = "TL",
            VerticalAxisId = "AX1",
            LineColor = Colors.Blue,
        };


        private static readonly Func<ChartPlotBase> Analog1 = () => new ChartPlotCartesianLinear()
        {
            InstanceId = "Analog1",
            Description = "Analog #1",
            HorizontalAxisId = "TL",
            VerticalAxisId = "AX1",
            LineColor = Colors.Red,
        };


        private static readonly Func<ChartPlotBase> Ramp20min = () => new ChartPlotCartesianLinear()
        {
            InstanceId = "Ramp20min",
            Description = "Ramp 20'",
            HorizontalAxisId = "TL",
            VerticalAxisId = "AX2",
            LineColor = Colors.DarkGreen,
        };


        private static readonly Func<ChartPlotBase> Ramp30min = () => new ChartPlotCartesianLinear()
        {
            InstanceId = "Ramp30min",
            Description = "Ramp 30'",
            HorizontalAxisId = "TL",
            VerticalAxisId = "AX2",
            LineColor = Colors.Brown,
        };


        private static readonly Func<ChartPlotBase> Switch0 = () => new ChartPlotCartesianBoolean()
        {
            InstanceId = "Switch0",
            Description = "Switch #0",
            IsStairStep = true,
            HorizontalAxisId = "TL",
            VerticalAxisId = "BX",
        };


        private static readonly Func<ChartPlotBase> Switch1 = () => new ChartPlotCartesianBoolean()
        {
            InstanceId = "Switch1",
            Description = "Switch #1",
            IsStairStep = true,
            HorizontalAxisId = "TL",
            VerticalAxisId = "BX",
        };


        private static readonly Func<ChartPlotBase> Event1 = () => new ChartPlotCartesianEvent()
        {
            InstanceId = "Event1",
            Description = "Events group 1",
            HorizontalAxisId = "TL",
        };


        private static readonly Func<ChartPlotBase> Event2 = () => new ChartPlotCartesianEvent()
        {
            InstanceId = "Event2",
            Description = "Events group 2",
            HorizontalAxisId = "TL",
        };


        private static readonly Func<ChartPlotBase> Severity1 = () => new ChartPlotCartesianSeverity()
        {
            InstanceId = "Severity1",
            Description = "Severities",
            HorizontalAxisId = "TL",
        };

        #endregion

    }
}
