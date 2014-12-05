using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.UI.Chart
{
    public class MyDataProvider
        : IChartDataConverter
    {
        static MyDataProvider()
        {
            Register("PL1", new DataConvertPL1());
            Register("PL2", new DataConvertPL2());
            Register("PB1", new DataConvertPB1());
            Register("PB2", new DataConvertPB2());
            Register("EVT1", new DataConvertEvt1());
            Register("EVT2", new DataConvertEvt2());
            Register("SV1", new DataConvertSev1());
        }


        private static readonly Dictionary<string, IDataConverter> _register = new Dictionary<string, IDataConverter>();


        private static void Register(string key, IDataConverter converter)
        {
            _register.Add(
                key,
                converter);
        }


        //public void Update(ChartModelBase cm, object ds)
        //{
        //    foreach (var plot in cm.Plots)
        //    {
        //        IChartDataConverter converter;
        //        if (this._register.TryGetValue(plot.InstanceId, out converter))
        //        {
        //            converter.Convert(
        //                plot,
        //                ds);
        //        }
        //    }
        //}


        public void Convert(ChartPlotBase target, object source)
        {
            IDataConverter converter;
            if (_register.TryGetValue(target.InstanceId, out converter))
            {
                converter.Convert(
                    target,
                    (MyDataSource)source
                    );
            }
        }

    }


    internal interface IDataConverter
    {
        void Convert(ChartPlotBase target, MyDataSource source);
    }
}
