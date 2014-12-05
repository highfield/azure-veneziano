using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.Report
{
    public class ReportDataTableRow
    {

        private readonly Dictionary<string, string> _cells = new Dictionary<string, string>();


        public string this[string name]
        {
            get
            {
                string result;
                if (this._cells.TryGetValue(name, out result) == false)
                {
                    result = string.Empty;
                }

                return result;
            }
            set
            {
                this._cells[name] = value;
            }
        }


        internal string[] AsArray(
            ReportDataTable owner
            )
        {
            var cells = new string[owner.Columns.Count];

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = this[owner.Columns[i].Name];
            }

            return cells;
        }

    }
}
