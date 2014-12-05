using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cet.UI.Chart
{
    /// <summary>
    /// Algoritmo di line-clipping di Liang-Barsky
    /// </summary>
    /// <remarks>
    /// Vedi: http://pastebin.com/NA01gacf
    /// Vedi: http://graphics.ethz.ch/teaching/viscomp11/downloads/part2_11_clipping.pdf
    /// </remarks>
    internal sealed class LiangBarskyClipping
    {
        public LiangBarskyClipping(Rect rect)
        {
            _clipMin = rect.TopLeft;
            _clipMax = rect.BottomRight;
        }


        private Point _clipMin, _clipMax;


        public bool ClipLine(ref Point start, ref Point end)
        {
            Vector P = end - start;
            double tMinimum = 0, tMaximum = 1;

            Func<double, double, bool> pqClip = (directedProjection, directedDistance) =>
            {
                if (directedProjection == 0)
                {
                    if (directedDistance < 0)
                        return false;
                }
                else
                {
                    double amount = directedDistance / directedProjection;
                    if (directedProjection < 0)
                    {
                        if (amount > tMaximum)
                            return false;
                        else if (amount > tMinimum)
                            tMinimum = amount;
                    }
                    else
                    {
                        if (amount < tMinimum)
                            return false;
                        else if (amount < tMaximum)
                            tMaximum = amount;
                    }
                }
                return true;
            };

            if (pqClip(-P.X, start.X - _clipMin.X))
            {
                if (pqClip(P.X, _clipMax.X - start.X))
                {
                    if (pqClip(-P.Y, start.Y - _clipMin.Y))
                    {
                        if (pqClip(P.Y, _clipMax.Y - start.Y))
                        {
                            if (tMaximum < 1)
                            {
                                end.X = start.X + tMaximum * P.X;
                                end.Y = start.Y + tMaximum * P.Y;
                            }
                            if (tMinimum > 0)
                            {
                                start.X += tMinimum * P.X;
                                start.Y += tMinimum * P.Y;
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}