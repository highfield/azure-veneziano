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
    public class CanvasDrawingContext
    {
        static CanvasDrawingContext()
        {
            //brushes
            _tickBrushes = new Brush[3]
            {
                Brushes.Gray,
                Brushes.Black,
                Brushes.Black,
            };

            //pens
            _tickPens = new Pen[3]
            {
                new Pen(Brushes.Gray, 0.5),
                new Pen(Brushes.Gray, 1.0),
                new Pen(Brushes.Gray, 2.0),
            };

            foreach (var pen in _tickPens)
            {
                pen.Freeze();
            }

            //font faces
            var family = new FontFamily("Tahoma");

            _tickFaces = new Typeface[3]
            {
                new Typeface(family, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                new Typeface(family, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                new Typeface(family, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
            };
        }


        private static readonly Brush[] _tickBrushes;
        private static readonly Pen[] _tickPens;
        private static readonly Typeface[] _tickFaces;


        #region Resources

        public Brush GetTickBrush(int index)
        {
            return _tickBrushes[index];
        }


        public Pen GetTickPen(int index)
        {
            return _tickPens[index];
        }


        public Typeface GetTickFace(int index)
        {
            return _tickFaces[index];
        }

        #endregion


        public CanvasDrawingContext(
            ChartCanvas owner,
            DrawingContext dc
            )
        {
            this._owner = owner;
            this.Id = owner.AreaId;

            this._originalContext = dc;
            this.ClientArea = new Rect(
                0.0,
                0.0,
                owner.ActualWidth,
                owner.ActualHeight);

            //calcolo displacement
            var window = Window.GetWindow(owner);
            if (window != null)
            {
                var pt = owner.TransformToAncestor(window).Transform(new Point());
                //Console.WriteLine(pt);
                this._displacement = new Point(
                    DoubleHelpers.Fraction(pt.X),
                    DoubleHelpers.Fraction(pt.Y)
                    );
            }
        }


        private ChartCanvas _owner;
        private Point _displacement;
        private DrawingContext _originalContext;

        public Rect ClientArea { get; private set; }
        public string Id { get; private set; }


        public void PushTransform(Transform transform)
        {
            this._originalContext.PushTransform(transform);
        }


        public void Pop()
        {
            this._originalContext.Pop();
        }


        public void DrawLine(
            Pen pen,
            Point pt1,
            Point pt2)
        {
            this._originalContext.DrawLine(
                pen,
                this.Snapper(pt1 + (Vector)this.ClientArea.Location),
                this.Snapper(pt2 + (Vector)this.ClientArea.Location)
                );
        }


        public void DrawRectangle(
            Brush brush,
            Pen pen,
            Rect rectangle)
        {
            var left = this.Snapper(this.ClientArea.Left + rectangle.Left, this._displacement.X);
            var top = this.Snapper(this.ClientArea.Top + rectangle.Top, this._displacement.Y);
            var right = this.Snapper(this.ClientArea.Left + rectangle.Left + rectangle.Width, this._displacement.X);
            var bottom = this.Snapper(this.ClientArea.Top + rectangle.Top + rectangle.Height, this._displacement.Y);

            this._originalContext.DrawRectangle(
                brush,
                pen,
                new Rect(left, top, right - left, bottom - top)
                );
        }


        public Size MeasureText(string text, Typeface face, double size)
        {
            var ft = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                face,
                size,
                Brushes.Black
                );

            return new Size(ft.Width, ft.Height);
        }


        public void DrawText(
            string text,
            Typeface face,
            double size,
            Brush foreground,
            Point origin
            )
        {
            var ft = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                face,
                size,
                foreground
                );

            this._originalContext.DrawText(
                ft,
                this.Snapper(origin + (Vector)this.ClientArea.Location)
                );
        }


        public void DrawGeometry(
            Brush brush,
            Pen pen,
            Geometry geometry)
        {
            this._originalContext.DrawGeometry(
                brush,
                pen,
                geometry);
        }


        #region Snappers

        private Point Snapper(Point point)
        {
            return new Point(
                this.Snapper(point.X, this._displacement.X),
                this.Snapper(point.Y, this._displacement.Y)
                );
        }


        private double Snapper(
            double value,
            double displacement)
        {
            if (displacement < 0.5)
            {
                return Math.Round(value) + (0.5 + displacement);
            }
            else
            {
                return Math.Round(value) - (displacement - 0.5);
            }
        }

        #endregion

    }
}
