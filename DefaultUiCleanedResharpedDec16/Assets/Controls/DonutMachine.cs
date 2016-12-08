using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DefaultUiCleanedResharpedDec16.Assets.Controls
{
    public class DonutMachine : Control
    {
        private readonly Duration _percentageShadowDuration = new Duration(TimeSpan.FromMilliseconds(300));
        private readonly BackEase _percentageShadowEasing = new BackEase {EasingMode = EasingMode.EaseOut};
        private double _arcRadius;
        private Point _center;

        //
        // Main 
        // 

        private double _radius;

        static DonutMachine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DonutMachine),
                new FrameworkPropertyMetadata(typeof(DonutMachine)));
        }

        public DonutMachine()
        {
            Loaded += OnLoaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var thickness = Math.Floor(SegmentThickness/2);

            Width = ControlDiameter;
            Height = ControlDiameter;

            _radius = Math.Floor(ControlDiameter/2);
            _arcRadius = Math.Floor(_radius - thickness);
            ArcSize = new Size(_arcRadius, _arcRadius);
            _center = new Point(_arcRadius, _arcRadius);
            MarginOffset = new Thickness(thickness, thickness, 0, 0);
        }

        public void OnLoaded(object o, RoutedEventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var angle = 360*(PercentageShadow/100);
            var radians = (angle - 180)*Math.PI/180;

            IsLarge = angle > 180;
            StartPoint = new Point(0, _arcRadius);
            EndPoint = new Point(_center.X + _arcRadius*Math.Cos(radians), _center.Y + _arcRadius*Math.Sin(radians));
        }

        private void UpdatePercentageShadow()
        {
            var anim = new DoubleAnimation
            {
                To = Percentage,
                Duration = _percentageShadowDuration,
                EasingFunction = _percentageShadowEasing
            };

            BeginAnimation(PercentageShadowProperty, anim);
        }

        //
        // Dependency Properties
        //

        #region Dependency Properties

        public static readonly DependencyProperty StrokeStartCapProperty = DependencyProperty.Register(
            "StrokeStartCap", typeof(PenLineCap), typeof(DonutMachine), new FrameworkPropertyMetadata(PenLineCap.Round,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty StrokeEndCapProperty = DependencyProperty.Register(
            "StrokeEndCap", typeof(PenLineCap), typeof(DonutMachine), new FrameworkPropertyMetadata(PenLineCap.Triangle,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
            "StartPoint", typeof(Point), typeof(DonutMachine), new FrameworkPropertyMetadata(new Point(0, 0),
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof(Point), typeof(DonutMachine), new FrameworkPropertyMetadata(new Point(0, 0),
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty SegmentThicknessProperty = DependencyProperty.Register(
            "SegmentThickness", typeof(double), typeof(DonutMachine), new FrameworkPropertyMetadata(16.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                SegmentThicknessCoerce));

        public static readonly DependencyProperty ControlDiameterProperty = DependencyProperty.Register(
            "ControlDiameter", typeof(double), typeof(DonutMachine), new FrameworkPropertyMetadata(100.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                ControlDiameterCoerce));

        public static readonly DependencyProperty MarginOffsetProperty = DependencyProperty.Register(
            "MarginOffset", typeof(Thickness), typeof(DonutMachine),
            new FrameworkPropertyMetadata(new Thickness(8, 8, 0, 0),
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RotationAngleProperty = DependencyProperty.Register(
            "RotationAngle", typeof(double), typeof(DonutMachine), new PropertyMetadata(0.0));

        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage", typeof(double), typeof(DonutMachine),
            new PropertyMetadata(0.0, PercentageChanged, PercentageCoerce));

        public static readonly DependencyProperty PercentageShadowProperty = DependencyProperty.Register(
            "PercentageShadow", typeof(double), typeof(DonutMachine),
            new PropertyMetadata(0.0, PercentageShadowChanged, PercentageCoerce));

        public static readonly DependencyProperty ArcSizeProperty = DependencyProperty.Register(
            "ArcSize", typeof(Size), typeof(DonutMachine), new PropertyMetadata(new Size(0, 0)));

        public static readonly DependencyProperty IsLargeProperty = DependencyProperty.Register(
            "IsLarge", typeof(bool), typeof(DonutMachine), new PropertyMetadata(false));

        public PenLineCap StrokeStartCap
        {
            get { return (PenLineCap) GetValue(StrokeStartCapProperty); }
            set { SetValue(StrokeStartCapProperty, value); }
        }

        public PenLineCap StrokeEndCap
        {
            get { return (PenLineCap) GetValue(StrokeEndCapProperty); }
            set { SetValue(StrokeEndCapProperty, value); }
        }

        public Point StartPoint
        {
            get { return (Point) GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public double ControlDiameter
        {
            get { return (double) GetValue(ControlDiameterProperty); }
            set { SetValue(ControlDiameterProperty, value); }
        }

        public Thickness MarginOffset
        {
            get { return (Thickness) GetValue(MarginOffsetProperty); }
            set { SetValue(MarginOffsetProperty, value); }
        }

        public double RotationAngle
        {
            get { return (double) GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        public double SegmentThickness
        {
            get { return (double) GetValue(SegmentThicknessProperty); }
            set { SetValue(SegmentThicknessProperty, value); }
        }

        public Point EndPoint
        {
            get { return (Point) GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public Size ArcSize
        {
            get { return (Size) GetValue(ArcSizeProperty); }
            set { SetValue(ArcSizeProperty, value); }
        }

        public bool IsLarge
        {
            get { return (bool) GetValue(IsLargeProperty); }
            set { SetValue(IsLargeProperty, value); }
        }

        public double Percentage
        {
            get { return (double) GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public double PercentageShadow
        {
            get { return (double) GetValue(PercentageShadowProperty); }
            set { SetValue(PercentageShadowProperty, value); }
        }

        #endregion

        //
        // CallBack/Changed Methods
        // 

        #region CallBacks

        // Changed  
        private static void PercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DonutMachine) d).UpdatePercentageShadow();
        }

        private static void PercentageShadowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DonutMachine) d).UpdateDisplay();
        }

        // Coercion
        private static object PercentageCoerce(DependencyObject d, object baseValue)
        {
            if ((double) baseValue > 99.999) return 99.999;
            if ((double) baseValue < 0.1) return 0.1;

            return baseValue;
        }

        private static object SegmentThicknessCoerce(DependencyObject d, object baseValue)
        {
            if ((double) baseValue < 2.0) return 2.0;

            return baseValue;
        }

        private static object ControlDiameterCoerce(DependencyObject d, object baseValue)
        {
            if ((double) baseValue < 20.0) return 20.0;

            return baseValue;
        }

        #endregion
    }
}