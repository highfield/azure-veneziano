using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.WebJob
{
    class LogicVar
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public bool IsChanged { get; set; }
        public DateTimeOffset LastUpdate { get; set; }


        public override string ToString()
        {
            return string.Format(
                "{0}: val={1}; chg={2}; upd={3}",
                this.Name,
                this.Value,
                this.IsChanged,
                this.LastUpdate
                );
        }
    }
}
