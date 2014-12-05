using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AzureVeneziano.Report
{
    public class ReportDataTableColumn
    {
        public ReportDataTableColumn(
            string name,
            string title,
            GridLength width
            )
        {
            this.Name = name;
            this.Title = title;
            this.Width = width;
        }


        public readonly string Name;
        public readonly string Title;
        public readonly GridLength Width;

    }
}
