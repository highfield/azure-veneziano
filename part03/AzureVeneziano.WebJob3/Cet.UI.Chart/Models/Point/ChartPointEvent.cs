using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.UI.Chart
{
    public class ChartPointEvent
    {
        public ChartPointEvent(
            double x,
            string description)
        {
            this.X = x;
            this.Description = description;
        }


        public double X;
        public string Description;

    }
}
