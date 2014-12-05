using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AzureVeneziano.Report
{
    public class ReportDataTable
    {
        public ReportDataTable()
        {
            this._columns = new List<ReportDataTableColumn>();
            this._rows = new List<ReportDataTableRow>();
        }


        #region PROP Columns

        private readonly List<ReportDataTableColumn> _columns;

        public List<ReportDataTableColumn> Columns
        {
            get { return this._columns; }
        }

        #endregion


        #region PROP Rows

        private readonly List<ReportDataTableRow> _rows;

        public List<ReportDataTableRow> Rows
        {
            get { return this._rows; }
        }

        #endregion

    }
}
