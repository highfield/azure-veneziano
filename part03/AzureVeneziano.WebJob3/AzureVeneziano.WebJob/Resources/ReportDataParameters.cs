using Cet.UI.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.WebJob
{
    public class ReportDataParameters
    {
        public ReportDataParameters()
        {
            this.OverviewText = new List<string>();
            this.PlotIds = new List<string>();
            this.DateTimeEnd = DateTime.Now;
            this.DateTimeBegin = this.DateTimeEnd - TimeSpan.FromHours(1);
        }


        public readonly List<string> OverviewText;

        public readonly List<string> PlotIds;
        public DateTime DateTimeBegin;
        public DateTime DateTimeEnd;
    }
}
