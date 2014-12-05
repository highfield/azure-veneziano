using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cet.UI.Chart
{
    public interface IAxisFormatter
        : IFormatProvider
    {
        string MeasureUnit { get; }

    }
}
