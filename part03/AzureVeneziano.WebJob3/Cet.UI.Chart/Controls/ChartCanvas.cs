using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cet.UI.Chart
{
    public class ChartCanvas
        : Canvas
    {

        public string AreaId { get; set; }


        #region DP BorderBrush

        public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
            "BorderBrush",
            typeof(Brush),
            typeof(ChartCanvas),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender
                ));


        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        #endregion


        #region DP BorderThickness

        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            "BorderThickness",
            typeof(Thickness),
            typeof(ChartCanvas),
            new FrameworkPropertyMetadata(
                default(Thickness),
                FrameworkPropertyMetadataOptions.AffectsRender
                ));


        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        #endregion


        #region DP RenderTriggerId

        public static readonly DependencyProperty RenderTriggerIdProperty = DependencyProperty.Register(
            "RenderTriggerId",
            typeof(int),
            typeof(ChartCanvas),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.AffectsRender
                ));


        public int RenderTriggerId
        {
            get { return (int)GetValue(RenderTriggerIdProperty); }
            set { SetValue(RenderTriggerIdProperty, value); }
        }

        #endregion


        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var cdc = new CanvasDrawingContext(this, dc);

            var renderable = this.DataContext as ChartBaseVM;
            if (renderable != null)
            {
                renderable.OnRender(cdc);
            }

            //draw border, if applies
            this.RenderBorder(cdc);
        }


        private void RenderBorder(CanvasDrawingContext dc)
        {
            {
                //left border
                var th2 = 0;// this.BorderThickness.Left / 2;
                var pen = new Pen(
                    this.BorderBrush,
                    this.BorderThickness.Left
                    );

                pen.Freeze();

                dc.DrawLine(
                    pen,
                    new Point(th2, th2),
                    new Point(th2, dc.ClientArea.Height - th2)
                    );
            }
            {
                //upper border
                var th2 = 0;// this.BorderThickness.Top / 2;
                var pen = new Pen(
                    this.BorderBrush,
                    this.BorderThickness.Top
                    );

                pen.Freeze();

                dc.DrawLine(
                    pen,
                    new Point(th2, th2),
                    new Point(dc.ClientArea.Width - th2, th2)
                    );
            }
            {
                //right border
                var th2 = 0;// this.BorderThickness.Right / 2;
                var pen = new Pen(
                    this.BorderBrush,
                    this.BorderThickness.Right
                    );

                pen.Freeze();

                dc.DrawLine(
                    pen,
                    new Point(dc.ClientArea.Width - th2, th2),
                    new Point(dc.ClientArea.Width - th2, dc.ClientArea.Height - th2)
                    );
            }
            {
                //bottom border
                var th2 = 0;// this.BorderThickness.Bottom / 2;
                var pen = new Pen(
                    this.BorderBrush,
                    this.BorderThickness.Bottom
                    );

                pen.Freeze();

                dc.DrawLine(
                    pen,
                    new Point(th2, dc.ClientArea.Height - th2),
                    new Point(dc.ClientArea.Width - th2, dc.ClientArea.Height - th2)
                    );
            }

            //*** DEBUG ***
            //if (string.IsNullOrWhiteSpace(this.DebugText) == false)
            //{
            //    var ft = new FormattedText(
            //        this.DebugText,
            //        System.Globalization.CultureInfo.CurrentCulture,
            //        FlowDirection.LeftToRight,
            //        _tickFaces[1],
            //        11.0,
            //        Brushes.Black
            //        );

            //    dc.DrawText(
            //        ft,
            //        new Point(10, 10)
            //        );
            //}
        }


        public string DebugText { get; set; }

    }
}
