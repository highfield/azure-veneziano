using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.UI.Chart
{
    public class MyDataSource
    {
        private static readonly DateTime InitialTimestamp = new DateTime(2014, 1, 1);


        public MyDataSource()
        {
            this.Records = this.GenerateData();
        }


        public IEnumerable<MyDataRecord> Records { get; private set; }


        private MyDataRecord[] GenerateData()
        {
            var records = new List<MyDataRecord>();

            /**
             * ANALOG
             **/
            {
                var dt = InitialTimestamp + TimeSpan.FromDays(7);
                for (int i = 0; i < 1500; i++)
                {
                    records.Add(
                        new MyDataRecord(dt.Ticks, 123, 40 + 60 * Math.Sin(i * 6.28318 / 10))
                        );

                    dt += TimeSpan.FromDays(1);
                }
            }
            {
                var dt = InitialTimestamp + TimeSpan.FromDays(14);
                for (int i = 0; i < 1500; i++)
                {
                    records.Add(
                        new MyDataRecord(dt.Ticks, 456, 40 + 60 * Math.Cos(i * 6.28318 / 10))
                        );

                    dt += TimeSpan.FromDays(1);
                }
            }


            /**
             * BOOLEAN
             **/
            {
                var dt = InitialTimestamp + TimeSpan.FromDays(3);
                for (int i = 0; i < 3000; i++)
                {
                    records.Add(
                        new MyDataRecord(dt.Ticks, 100123, i & 1)
                        );

                    dt += TimeSpan.FromHours(12);
                }
            }
            {
                var dt = InitialTimestamp + TimeSpan.FromDays(10);
                for (int i = 0; i < 1000; i++)
                {
                    records.Add(
                        new MyDataRecord(dt.Ticks, 100456, i & 1)
                        );

                    dt += TimeSpan.FromHours(48);
                }
            }


            /**
             * EVENT
             **/
            {
                var dt = InitialTimestamp.Ticks;
                for (int i = 0; i < 100; i++)
                {
                    var idx = 1 + i % 10;
                    records.Add(
                        new MyDataRecord(dt, 50000 + idx, (i / 10 & 1) == 0)
                        );

                    dt += (long)((1.5 + Math.Cos(i * 6.28318 / 40)) * TimeSpan.FromHours(24).Ticks);
                }
            }
            {
                var dt = InitialTimestamp.Ticks;
                for (int i = 0; i < 100; i++)
                {
                    var idx = 1 + i % 10;
                    records.Add(
                        new MyDataRecord(dt, 50010 + idx, (i / 10 & 1) != 0)
                        );

                    dt += (long)((1.5 + Math.Sin(i * 6.28318 / 60)) * TimeSpan.FromHours(16).Ticks);
                }
            }


            /**
             * SEVERITY
             **/
            {
                var t = new int[] { 1, 2, 3, 4, 3, 2, 1, 0, 4, 1 };
                var dt = (InitialTimestamp + TimeSpan.FromDays(1.5)).Ticks;
                for (int i = 0; i < 100; i++)
                {
                    var idx = 1 + i % 10;
                    var lev = i / 10;
                    records.Add(
                        new MyDataRecord(dt, 70000 + idx, t[lev])
                        );

                    dt += (long)((1.5 + Math.Cos(i * 6.28318 / 100)) * TimeSpan.FromHours(13).Ticks);
                }
            }


            //final sorting by timestamp
            return records
                .OrderBy(_ => _.Timestamp)
                .ToArray();
        }

    }


    public class MyDataRecord
    {
        public MyDataRecord(long timestamp, int id, object value)
        {
            this.Timestamp = timestamp;
            this.Id = id;
            this.Value = value;
        }

        public readonly long Timestamp;
        public readonly int Id;
        public readonly object Value;
    }
}
