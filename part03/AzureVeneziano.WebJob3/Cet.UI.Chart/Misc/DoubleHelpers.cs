using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Globalization;


namespace Cet.UI.Chart
{
    internal static class DoubleHelpers
    {
        
        /// <summary>
        /// Restituisce la parte frazionaria di un valore reale
        /// </summary>
        /// <param name="value">Il valore reale originale</param>
        /// <returns>La parte frazionaria del valore originale</returns>
        public static Double Fraction(Double value)
        {
            if (value == 0)
                return value;

            if (value > 0)
            {
                return value - Math.Floor(value);
            }
            else
            {
                return value - Math.Ceiling(value);
            }
        }

    }
}
