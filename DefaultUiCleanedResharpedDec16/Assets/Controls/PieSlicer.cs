using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DefaultUiCleanedResharpedDec16.Assets.Controls
{
    public class PieSlicer : Shape
    {
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
            "StartAngle", typeof(double), typeof(PieSlicer), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                CoerceAngleCallback));


        public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(
            "EndAngle", typeof(double), typeof(PieSlicer), new FrameworkPropertyMetadata(90.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                CoerceAngleCallback));

        // Angle that arc starts at
        public double StartAngle
        {
            get { return (double) GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Angle that arc ends at
        public double EndAngle
        {
            get { return (double) GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                var maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                var maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);

                var xStart = maxWidth/2.0*Math.Cos(StartAngle*Math.PI/180.0);
                var yStart = maxHeight/2.0*Math.Sin(StartAngle*Math.PI/180.0);

                var xEnd = maxWidth/2.0*Math.Cos(EndAngle*Math.PI/180.0);
                var yEnd = maxHeight/2.0*Math.Sin(EndAngle*Math.PI/180.0);

                var geom = new StreamGeometry();
                using (var ctx = geom.Open())
                {
                    ctx.BeginFigure(
                        new Point(RenderSize.Width/2.0 + xStart,
                            RenderSize.Height/2.0 - yStart),
                        true, // Filled
                        true); // Closed

                    ctx.ArcTo(
                        new Point(RenderSize.Width/2.0 + xEnd,
                            RenderSize.Height/2.0 - yEnd),
                        new Size(maxWidth/2.0, maxHeight/2),
                        0.0, // rotationAngle
                        EndAngle - StartAngle > 180, // greater than 180 deg?
                        SweepDirection.Counterclockwise,
                        true, // isStroked
                        false);

                    ctx.LineTo(new Point(RenderSize.Width/2.0, RenderSize.Height/2.0), true, false);
                }

                return geom;
            }
        }

        // Coerce Angles
        private static object CoerceAngleCallback(DependencyObject depObj, object baseVal)
        {
            var angle = (double) baseVal;
            angle = Math.Min(angle, 359.9);
            angle = Math.Max(angle, 0.0);
            return angle;
        }
    }
}