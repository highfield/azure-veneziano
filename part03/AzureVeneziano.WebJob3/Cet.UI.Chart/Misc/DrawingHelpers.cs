using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public static class DrawingHelpers
    {

        public static void DrawDashedLine(
            CanvasDrawingContext dc,
            Pen pen,
            Point point0,
            Point point1,
            double dashPeriod = 12.0,
            double dashDuty = 0.5
            )
        {
            if (dashDuty < 0.01)
            {
                //non ha senso tracciare perchè il duty è quasi zero
                return;
            }
            else if (
                dashPeriod < 2.0 ||
                dashDuty > 0.99)
            {
                //tratteggio che degenera in una linea continua
                dc.DrawLine(
                    pen,
                    point0,
                    point1
                    );
            }
            else
            {
                //linea trasparente in modo da mantenere l'hit-testing
                var pen_trans = new Pen(
                    Brushes.Transparent,
                    pen.Thickness
                    );

                dc.DrawLine(
                    pen_trans,
                    point0,
                    point1
                    );

                //tracciamento dei singoli tratti
                var clipper = new LiangBarskyClipping(dc.ClientArea);
                if (clipper.ClipLine(ref point0, ref point1))
                {
                    var dx = point1.X - point0.X;
                    var dy = point1.Y - point0.Y;
                    var d = Math.Sqrt(dx * dx + dy * dy);

                    var dashLen = dashPeriod * dashDuty;
                    if (d >= dashLen)
                    {
                        for (double l = 0; l < d; l += dashPeriod)
                        {
                            var a0 = l / d;
                            var a1 = Math.Min(a0 + dashLen / d, 1.0);

                            var q0 = new Point(
                                point0.X + dx * a0,
                                point0.Y + dy * a0
                                );

                            var q1 = new Point(
                                point0.X + dx * a1,
                                point0.Y + dy * a1
                                );

                            dc.DrawLine(
                                pen,
                                q0,
                                q1
                                );
                        }
                    }
                }
            }
        }


        public static void DrawTicks(
            CanvasDrawingContext dc,
            Rect area,
            ChartTickSegmentCollection ticks,
            Dock dock
            )
        {
            if (ticks == null)
                return;

            if (dock == Dock.Left || dock == Dock.Right)
            {
                var slots = new bool[(int)area.Height];

                var segmentGroup = ticks
                    .Where(_ => _.IsValid)
                    .SelectMany(_ => _)
                    .GroupBy(_ => _.Priority)
                    .OrderByDescending(_ => _.Key);

                foreach (var segment in segmentGroup)
                {
                    int prio = segment.Key;
                    var len = 5 + prio * 10.0;
                    var size = 11.0 + prio * 2.5;
                    var pen = dc.GetTickPen(prio);
                    var brush = dc.GetTickBrush(prio);
                    var face = dc.GetTickFace(prio);

                    foreach (ChartTickInfo ti in segment)
                    {
                        //calculate major and minor label texts
                        var szMajor = dc.MeasureText(
                            ti.MajorText ?? string.Empty,
                            face,
                            size
                            );

                        var szMinor = dc.MeasureText(
                            ti.MinorText ?? string.Empty,
                            face,
                            size
                            );

                        int bigger = 2 + (int)Math.Max(
                            szMajor.Height,
                            szMinor.Height);

                        var ipos = (int)Math.Round(ti.PixelPosition);
                        var pos = ti.PixelPosition;

                        if (dock == Dock.Left)
                        {
                            //draw tick line
                            dc.DrawLine(
                                pen,
                                new Point(0, pos),
                                new Point(len, pos)
                                );
                        }
                        else
                        {
                            pos = area.Height - pos;

                            //draw tick line
                            dc.DrawLine(
                                pen,
                                new Point(area.Width, pos),
                                new Point(area.Width - len, pos)
                                );
                        }

                        var a = ipos - bigger / 2;
                        var b = a + bigger;

                        if (a < 0)
                            a = 0;

                        if (b >= slots.Length)
                            b = slots.Length - 1;

                        bool canDraw = true;
                        for (int x = a; x <= b; x++)
                        {
                            if (slots[x])
                            {
                                canDraw = false;
                                break;
                            }
                        }

                        if (canDraw)
                        {
                            //fill slots
                            for (int x = a; x <= b; x++)
                            {
                                slots[x] = true;
                            }

                            if (dock == Dock.Left)
                            {
                                //draw major label text
                                var xd = len + 4;
                                dc.DrawText(
                                    ti.MajorText ?? string.Empty,
                                    face,
                                    size,
                                    brush,
                                    new Point(xd, pos - szMajor.Height / 2)
                                    );
                                //draw minor label text
                                //xd += size;
                                //dc.DrawText(
                                //    ftMinor,
                                //    new Point(xd, pos - ftMinor.Height / 2)
                                //    );
                            }
                            else
                            {
                                //draw major label text
                                var xd = area.Width - len - 4;
                                dc.DrawText(
                                    ti.MajorText ?? string.Empty,
                                    face,
                                    size,
                                    brush,
                                    new Point(xd - szMajor.Width, pos - szMajor.Height / 2)
                                    );
                                //draw minor label text
                                //xd += size;
                                //dc.DrawText(
                                //    ftMinor,
                                //    new Point(xd - ftMinor.Width, pos - ftMinor.Height / 2)
                                //    );
                            }
                        }
                    }
                }
            }
            else if (dock == Dock.Top || dock == Dock.Bottom)
            {
                var slots = new bool[(int)area.Width];

                var segmentGroup = ticks
                    .Where(_ => _.IsValid)
                    .SelectMany(_ => _)
                    .GroupBy(_ => _.Priority)
                    .OrderByDescending(_ => _.Key);

                foreach (var segment in segmentGroup)
                {
                    int prio = segment.Key;
                    var len = 5 + prio * 10.0;
                    var size = 11.0 + prio * 2.5;
                    var pen = dc.GetTickPen(prio);
                    var brush = dc.GetTickBrush(prio);
                    var face = dc.GetTickFace(prio);

                    foreach (ChartTickInfo ti in segment)
                    {
                        //calculate major and minor label texts
                        var szMajor = dc.MeasureText(
                            ti.MajorText ?? string.Empty,
                            face,
                            size
                            );

                        var szMinor = dc.MeasureText(
                            ti.MinorText ?? string.Empty,
                            face,
                            size
                            );

                        int bigger = 2 + (int)Math.Max(
                            szMajor.Width,
                            szMinor.Width);

                        var ipos = (int)Math.Round(ti.PixelPosition);
                        var pos = ti.PixelPosition;

                        if (dock == Dock.Top)
                        {
                            //draw tick line
                            dc.DrawLine(
                                pen,
                                new Point(pos, 0),
                                new Point(pos, len)
                                );
                        }
                        else
                        {
                            //draw tick line
                            dc.DrawLine(
                                pen,
                                new Point(pos, area.Height),
                                new Point(pos, area.Height - len)
                                );
                        }

                        var a = ipos - bigger / 2;
                        var b = a + bigger;

                        if (a < 0)
                            a = 0;

                        if (b >= slots.Length)
                            b = slots.Length - 1;

                        bool canDraw = true;
                        for (int x = a; x <= b; x++)
                        {
                            if (slots[x])
                            {
                                canDraw = false;
                                break;
                            }
                        }

                        if (canDraw)
                        {
                            //fill slots
                            for (int x = a; x <= b; x++)
                            {
                                slots[x] = true;
                            }

                            if (dock == Dock.Top)
                            {
                                //draw major label text
                                var yd = len + 4;
                                dc.DrawText(
                                    ti.MajorText ?? string.Empty,
                                    face,
                                    size,
                                    brush,
                                    new Point(pos - szMajor.Width / 2, yd)
                                    );

                                //draw minor label text
                                yd += size;
                                dc.DrawText(
                                    ti.MinorText ?? string.Empty,
                                    face,
                                    size,
                                    brush,
                                    new Point(pos - szMinor.Width / 2, yd)
                                    );
                            }
                            else
                            {
                                //draw major label text
                                var yd = area.Height - len - 4;
                                dc.DrawText(
                                    ti.MajorText ?? string.Empty,
                                    face,
                                    size,
                                    brush,
                                    new Point(pos - szMajor.Width / 2, yd)
                                    );

                                //draw minor label text
                                yd -= size;
                                dc.DrawText(
                                    ti.MinorText ?? string.Empty,
                                    face,
                                    size,
                                    brush,
                                    new Point(pos - szMinor.Width / 2, yd)
                                    );
                            }
                        }
                    }
                }
            }
        }


        public static void DrawDivisions(
            CanvasDrawingContext dc,
            Rect area,
            ChartTickSegmentCollection ticks,
            Orientation orientation
            )
        {
            if (ticks == null)
                return;

            if (orientation == Orientation.Vertical)
            {
                //draw vertical lines
                var yd = area.Height;

                var list = ticks
                    .Where(_ => _.IsValid)
                    .SelectMany(_ => _);

                foreach (ChartTickInfo ti in list)
                {
                    int prio = ti.Priority;
                    var pen = dc.GetTickPen(prio);

                    var xd = ti.PixelPosition;

                    //draw division line
                    dc.DrawLine(
                        pen,
                        new Point(xd, 0),
                        new Point(xd, yd)
                        );
                }
            }
            else
            {
                //draw horizontal lines
                var xd = area.Width;

                var list = ticks
                    .Where(_ => _.IsValid)
                    .SelectMany(_ => _);

                foreach (ChartTickInfo ti in list)
                {
                    int prio = ti.Priority;
                    var pen = dc.GetTickPen(prio);

                    var yd = ti.PixelPosition;

                    //draw division line
                    dc.DrawLine(
                        pen,
                        new Point(0, yd),
                        new Point(xd, yd)
                        );
                }
            }
        }

    }
}
