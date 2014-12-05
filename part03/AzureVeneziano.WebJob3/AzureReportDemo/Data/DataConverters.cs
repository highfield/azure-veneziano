using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public class DataConvertPL1 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianLinear)target;

            plot.Points = source
                .Records
                .Where(_ => _.Id == 123)
                .Select(_ => new Point(_.Timestamp, (double)_.Value))
                .ToList();
        }
    }


    public class DataConvertPL2 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianLinear)target;

            plot.Points = source
                .Records
                .Where(_ => _.Id == 456)
                .Select(_ => new Point(_.Timestamp, (double)_.Value))
                .ToList();
        }
    }


    public class DataConvertPB1 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianLinear)target;

            plot.Points = source
                .Records
                .Where(_ => _.Id == 100123)
                .Select(_ => new Point(_.Timestamp, (int)_.Value))
                .ToList();
        }
    }


    public class DataConvertPB2 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianLinear)target;

            plot.Points = source
                .Records
                .Where(_ => _.Id == 100456)
                .Select(_ => new Point(_.Timestamp, (int)_.Value))
                .ToList();
        }
    }


    public class DataConvertEvt1 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianEvent)target;

            plot.Points = source
                .Records
                .Where(_ => _.Id > 50000 && _.Id <= 50010)
                .Select(_ => new ChartPointEvent(_.Timestamp, string.Format("Event #{0}: {1}", _.Id - 50000, (bool)_.Value ? "ON" : "OFF")))
                .ToList();
        }
    }


    public class DataConvertEvt2 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianEvent)target;

            plot.Points = source
                .Records
                .Where(_ => _.Id > 50010 && _.Id <= 50020)
                .Select(_ => new ChartPointEvent(_.Timestamp, string.Format("Event #{0}: {1}", _.Id - 50000, (bool)_.Value ? "ON" : "OFF")))
                .ToList();
        }
    }


    public class DataConvertSev1 : IDataConverter
    {
        public void Convert(ChartPlotBase target, MyDataSource source)
        {
            var plot = (ChartPlotCartesianSeverity)target;
            var records = source
                .Records
                .Where(_ => _.Id > 70000 && _.Id <= 70010);

            var points = new List<ChartPointSeverity>();
            var dict = new Dictionary<int, Entry>();

            foreach (var rec in records)
            {
                var lev = (int)rec.Value;
                if (lev > 1)
                {
                    var si = new ChartPointSeverityItem
                    {
                        //Level = lev,
                        Color = GetLevelColor(lev),
                        Description = "Alarm #" + (rec.Id - 70000),
                    };

                    dict[rec.Id] = new Entry
                    {
                        Level = lev,
                        Item = si,
                    };
                }
                else
                {
                    dict.Remove(rec.Id);
                }

                if (dict.Count > 0)
                {
                    points.Add(
                        new ChartPointSeverity()
                        {
                            X = rec.Timestamp,
                            //Level = pointLevel,
                            Items = dict.Values.OrderBy(_ => _.Level).Select(_ => _.Item).ToList(),
                        });
                }
                else
                {
                    var si = new ChartPointSeverityItem
                    {
                        //Level = lev,
                        Color = GetLevelColor(1),
                        Description = "Regular",
                    };

                    points.Add(
                        new ChartPointSeverity()
                        {
                            X = rec.Timestamp,
                            //Level = pointLevel,
                            Items = new[] { si },
                        });
                }
            }

            plot.Points = points;
        }


        private static Color GetLevelColor(int level)
        {
            switch (level)
            {
                case 1: return Colors.LimeGreen;
                case 2: return Colors.LightCyan;
                case 3: return Colors.Yellow;
                case 4: return Colors.Red;

                default: return Colors.Gray;
            }
        }


        private class Entry
        {
            public int Level;
            public ChartPointSeverityItem Item;
        }
    }
}
