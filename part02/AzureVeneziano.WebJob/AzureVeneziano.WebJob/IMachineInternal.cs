using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.WebJob
{
    interface IMachineInternal
    {
        DateTimeOffset LastUpdate { get; set; }
        void Load();
        void Save();
    }
}
