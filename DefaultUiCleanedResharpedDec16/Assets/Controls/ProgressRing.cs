using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DefaultUiCleanedResharpedDec16.Assets.Controls
{
    [TemplatePart(Name = "PART_StaticRing",  Type = typeof(Ellipse))]
    [TemplatePart(Name = "PART_ProgressRing",Type = typeof(DonutMachine))]
    [TemplatePart(Name = "PART_PipsElement", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_TextElement", Type = typeof(TextBlock))]
    public class ProgressRing : Control
    {
        public enum PipPositions
        {
            Inside,
            Outside
        }

        private Duration _controlFadeDuration;
        private double _pipsElementDiameter;
        private Storyboard _pipsStoryboard;
        private Grid _templatePipsElement;
        private DonutMachine _templateProgressRing;

        //
        // Main
        //        
        private Ellipse _templateStaticRing;
        private TextBlock _templateTextElement;

        static ProgressRing()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRing),
                new FrameworkPropertyMetadata(typeof(ProgressRing)));
        }

        public ProgressRing()
        {
            Loaded += ControlLoaded;
        }

        public void ControlLoaded(object o, RoutedEventArgs e)
        {
            _controlFadeDuration = new Duration(TimeSpan.FromMilliseconds(500));

            if (IsIndeterminate)
            {
                ShowPipsOnTrigger = false;
                _templateProgressRing.Visibility =
                    _templateTextElement.Visibility = Visibility.Collapsed;

                if (ShowPipsElement)
                {
                    CreatePips();
                    _pipsStoryboard.Begin();
                }
            }
            else
            {
                if (ShowPipsOnTrigger)
                {
                    _templatePipsElement.Visibility = Visibility.Visible;

                    _templateStaticRing.Opacity = _templateProgressRing.Opacity =
                        _templatePipsElement.Opacity = TextElementOpacity = 0.0;

                    ShowPipsElement = true;
                    CreatePips();
                }
                else
                {
                    if (ShowPipsElement)
                    {
                        CreatePips();
                        _pipsStoryboard.Begin();
                    }
                }
            }

            if (ProgressTrigger) ProgressTriggered();
        }

        public void ArrangeLayout()
        {
            // Control
            Width = ControlDiameter;
            Height = ControlDiameter;

            // Rings Layout
            var staticring = ShowStaticRing ? StaticRingThickness + StaticRingOffset : 0;
            var progressring = ShowProgressRing ? ProgressRingThickness + ProgressRingOffset : 0;
            var pipselement = ShowPipsElement ? PipSize + PipsElementOffset : 0;

            if (PipsPosition == PipPositions.Outside)
            {
                _pipsElementDiameter = ControlDiameter;
                ProgressRingDiameter = ControlDiameter - (pipselement + progressring);
                StaticRingDiameter = ControlDiameter - (pipselement + progressring + progressring/2);
            }
            else
            {
                ProgressRingDiameter = ControlDiameter;
                StaticRingDiameter = ControlDiameter - (progressring + progressring/2);
                _pipsElementDiameter = ControlDiameter - (progressring + staticring + pipselement);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _templateStaticRing = Template.FindName("PART_StaticRing", this) as Ellipse;
            _templateProgressRing = Template.FindName("PART_ProgressRing", this) as DonutMachine;
            _templatePipsElement = Template.FindName("PART_PipsElement", this) as Grid;
            _templateTextElement = Template.FindName("PART_TextElement", this) as TextBlock;

            if (!ShowStaticRing)
                if (_templateStaticRing != null) _templateStaticRing.Visibility = Visibility.Collapsed;
            if (!ShowProgressRing)
                if (_templateProgressRing != null) _templateProgressRing.Visibility = Visibility.Collapsed;
            if (!ShowPipsElement)
                if (_templatePipsElement != null) _templatePipsElement.Visibility = Visibility.Collapsed;
            if (!ShowTextElement)
                if (_templateTextElement != null) _templateTextElement.Visibility = Visibility.Collapsed;

            ArrangeLayout();
        }

        //
        // Updates
        //
        private void OnPercentageChanged()
        {
            TextElementOpacity = Percentage > 0 ? 1 : 0;
            TextBoxContent = $"{Math.Ceiling(Percentage)}%";
        }


        private void ProgressTriggered()
        {
            if (!IsLoaded) return;

            if (ProgressTrigger)
            {
                var fadeAnim = new DoubleAnimation(0, 1, _controlFadeDuration);

                if (ShowStaticRing)
                {
                    _templateStaticRing.Visibility = Visibility.Visible;
                    _templateStaticRing.BeginAnimation(OpacityProperty, fadeAnim);
                }
                if (ShowProgressRing) _templateProgressRing.BeginAnimation(OpacityProperty, fadeAnim);
                if (TextFadeOnComplete && ShowTextElement) BeginAnimation(TextElementOpacityProperty, fadeAnim);
                if (ShowPipsElement) _templatePipsElement.BeginAnimation(OpacityProperty, fadeAnim);
                if (ShowPipsOnTrigger) _pipsStoryboard.Begin();
            }
            else
            {
                var fadeAnim = new DoubleAnimation(1, 0, _controlFadeDuration);

                //if (ShowStaticRing) TemplateStaticRing.BeginAnimation(OpacityProperty, fade_anim);
                if (ShowStaticRing) _templateStaticRing.Visibility = Visibility.Collapsed;
                if (ShowProgressRing) _templateProgressRing.BeginAnimation(OpacityProperty, fadeAnim);
                if (TextFadeOnComplete && ShowTextElement) BeginAnimation(TextElementOpacityProperty, fadeAnim);

                if (ShowPipsOnTrigger)
                {
                    fadeAnim.Completed += (o, s) => { _pipsStoryboard.Stop(); };

                    _templatePipsElement.BeginAnimation(OpacityProperty, fadeAnim);
                }
            }
        }

        //
        // Pips
        //

        private void CreatePips()
        {
            Brush safebrush = (SolidColorBrush) new BrushConverter().ConvertFrom("#FFEE0000");

            int rotationTime = 0, alphaTime = 0, i = 0;
            int[] angles = {45, 90, 135, 180, 225, 270, 360};

            var rand = new Random();
            var duration = new Duration(TimeSpan.FromSeconds(PipAnimationSpeed));

            _templatePipsElement.Children.Clear();

            _pipsStoryboard = new Storyboard();
            _pipsStoryboard.Completed += (o, s) =>
            {
                if (ShowPipsElement)
                {
                    _templatePipsElement.RenderTransform = new RotateTransform(angles[rand.Next(0, angles.Length)]);
                    _pipsStoryboard.Begin();
                }
            };

            for (i = 0; i < Pips; i++)
            {
                var resource = Application.Current.TryFindResource($"Background_ProgressRing_Pip_{i}");

                var pip = new DonutMachine
                {
                    Opacity = 0,
                    Percentage = PipSegmentPercent,
                    Background = resource == null ? safebrush : (Brush) resource,
                    ControlDiameter = _pipsElementDiameter,
                    SegmentThickness = PipSize,
                    StrokeStartCap = PenLineCap.Flat,
                    StrokeEndCap = PenLineCap.Flat
                };

                var rotationAnim = new DoubleAnimation
                {
                    From = 0,
                    To = 720,
                    Duration = duration,
                    EasingFunction = new SineEase {EasingMode = EasingMode.EaseInOut},
                    BeginTime = TimeSpan.FromMilliseconds(rotationTime)
                };

                var alphaAnim = new DoubleAnimationUsingKeyFrames
                {
                    Duration = duration,
                    BeginTime = TimeSpan.FromMilliseconds(alphaTime)
                };

                var key1 = new LinearDoubleKeyFrame {Value = 0, KeyTime = TimeSpan.FromSeconds(0)};
                var key2 = new LinearDoubleKeyFrame {Value = 1, KeyTime = TimeSpan.FromSeconds(1)};
                var key3 = new LinearDoubleKeyFrame {Value = 1, KeyTime = TimeSpan.FromSeconds(PipAnimationSpeed - 1.5)};
                var key4 = new LinearDoubleKeyFrame {Value = 0, KeyTime = TimeSpan.FromSeconds(PipAnimationSpeed - 0.5)};

                alphaAnim.KeyFrames.Add(key1);
                alphaAnim.KeyFrames.Add(key2);
                alphaAnim.KeyFrames.Add(key3);
                alphaAnim.KeyFrames.Add(key4);

                Storyboard.SetTarget(rotationAnim, pip);
                Storyboard.SetTargetProperty(rotationAnim, new PropertyPath(DonutMachine.RotationAngleProperty));

                Storyboard.SetTarget(alphaAnim, pip);
                Storyboard.SetTargetProperty(alphaAnim, new PropertyPath(OpacityProperty));

                _templatePipsElement.Children.Add(pip);
                _pipsStoryboard.Children.Add(rotationAnim);
                _pipsStoryboard.Children.Add(alphaAnim);

                rotationTime += PipAnimationBeginDelay;
                alphaTime += PipAnimationBeginDelay;
            }
        }

        //
        // DP
        // 

        #region Dependency Properties

        //  Control  
        public static readonly DependencyProperty ControlDiameterProperty = DependencyProperty.Register(
            "ControlDiameter", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(100.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                ControlDiameterChanged, ControlDiameterCoerce));

        public static readonly DependencyProperty StaticRingOffsetProperty = DependencyProperty.Register(
            "StaticRingOffset", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(2.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ProgressRingOffsetProperty = DependencyProperty.Register(
            "ProgressRingOffset", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty PipsElementOffsetProperty = DependencyProperty.Register(
            "PipsElementOffset", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(2.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage", typeof(double), typeof(ProgressRing),
            new PropertyMetadata(0.0, PercentageChanged, PercentageCoerce));

        public static readonly DependencyProperty ProgressTriggerProperty = DependencyProperty.Register(
            "ProgressTrigger", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true, ProgressTriggerChanged));

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            "IsIndeterminate", typeof(bool), typeof(ProgressRing), new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ShowPipsOnTriggerProperty = DependencyProperty.Register(
            "ShowPipsOnTrigger", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));

        public static readonly DependencyProperty PipsPositionProperty = DependencyProperty.Register(
            "PipsPosition", typeof(PipPositions), typeof(ProgressRing),
            new FrameworkPropertyMetadata(PipPositions.Inside,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ShowStaticRingProperty = DependencyProperty.Register(
            "ShowStaticRing", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowProgressRingProperty = DependencyProperty.Register(
            "ShowProgressRing", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowPipsElementProperty = DependencyProperty.Register(
            "ShowPipsElement", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowTextElementProperty = DependencyProperty.Register(
            "ShowTextElement", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));

        // Static Ring 
        public static readonly DependencyProperty StaticRingDiameterProperty = DependencyProperty.Register(
            "StaticRingDiameter", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(80.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty StaticRingThicknessProperty = DependencyProperty.Register(
            "StaticRingThickness", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(5.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                MinimumThicknessCoerce));

        // Progress Ring
        public static readonly DependencyProperty ProgressRingDiameterProperty = DependencyProperty.Register(
            "ProgressRingDiameter", typeof(double), typeof(ProgressRing), new PropertyMetadata(90.0));

        public static readonly DependencyProperty ProgressRingThicknessProperty = DependencyProperty.Register(
            "ProgressRingThickness", typeof(double), typeof(ProgressRing), new PropertyMetadata(10.0));

        public static readonly DependencyProperty ProgressRingRotationAngleProperty = DependencyProperty.Register(
            "ProgressRingRotationAngle", typeof(double), typeof(DonutMachine), new PropertyMetadata(0.0));

        // Pips Element       
        public static readonly DependencyProperty PipsProperty = DependencyProperty.Register(
            "Pips", typeof(int), typeof(ProgressRing), new FrameworkPropertyMetadata(7,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                PipsCoerceCallback));

        public static readonly DependencyProperty PipSizeProperty = DependencyProperty.Register(
            "PipSize", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(5.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                PipSizeCoerceCallback));

        public static readonly DependencyProperty PipSegmentPercentProperty = DependencyProperty.Register(
            "PipSegmentPercent", typeof(double), typeof(ProgressRing), new FrameworkPropertyMetadata(10.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, null,
                PipSizeCoerceCallback));

        public static readonly DependencyProperty PipAnimationSpeedProperty = DependencyProperty.Register(
            "PipAnimationSpeed", typeof(int), typeof(ProgressRing), new PropertyMetadata(5));

        public static readonly DependencyProperty PipAnimationBeginDelayProperty = DependencyProperty.Register(
            "PipAnimationBeginDelay", typeof(int), typeof(ProgressRing), new PropertyMetadata(250));

        // Text Element 
        public static readonly DependencyProperty TextBoxContentProperty = DependencyProperty.Register(
            "TextBoxContent", typeof(string), typeof(ProgressRing), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TextFadeOnCompleteProperty = DependencyProperty.Register(
            "TextFadeOnComplete", typeof(bool), typeof(ProgressRing), new PropertyMetadata(true));

        public static readonly DependencyProperty TextElementOpacityProperty = DependencyProperty.Register(
            "TextElementOpacity", typeof(double), typeof(ProgressRing), new PropertyMetadata(0.0));

        // Control 
        public double ControlDiameter
        {
            get { return (double) GetValue(ControlDiameterProperty); }
            set { SetValue(ControlDiameterProperty, value); }
        }

        public double StaticRingOffset
        {
            get { return (double) GetValue(StaticRingOffsetProperty); }
            set { SetValue(StaticRingOffsetProperty, value); }
        }

        public double ProgressRingOffset
        {
            get { return (double) GetValue(ProgressRingOffsetProperty); }
            set { SetValue(ProgressRingOffsetProperty, value); }
        }

        public double PipsElementOffset
        {
            get { return (double) GetValue(PipsElementOffsetProperty); }
            set { SetValue(PipsElementOffsetProperty, value); }
        }

        public double Percentage
        {
            get { return (double) GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public bool ProgressTrigger
        {
            get { return (bool) GetValue(ProgressTriggerProperty); }
            set { SetValue(ProgressTriggerProperty, value); }
        }

        public bool IsIndeterminate
        {
            get { return (bool) GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        public bool ShowPipsOnTrigger
        {
            get { return (bool) GetValue(ShowPipsOnTriggerProperty); }
            set { SetValue(ShowPipsOnTriggerProperty, value); }
        }

        public PipPositions PipsPosition
        {
            get { return (PipPositions) GetValue(PipsPositionProperty); }
            set { SetValue(PipsPositionProperty, value); }
        }

        public bool ShowStaticRing
        {
            get { return (bool) GetValue(ShowStaticRingProperty); }
            set { SetValue(ShowStaticRingProperty, value); }
        }

        public bool ShowProgressRing
        {
            get { return (bool) GetValue(ShowProgressRingProperty); }
            set { SetValue(ShowProgressRingProperty, value); }
        }

        public bool ShowPipsElement
        {
            get { return (bool) GetValue(ShowPipsElementProperty); }
            set { SetValue(ShowPipsElementProperty, value); }
        }

        public bool ShowTextElement
        {
            get { return (bool) GetValue(ShowTextElementProperty); }
            set { SetValue(ShowTextElementProperty, value); }
        }

        // Static Ring
        public double StaticRingDiameter
        {
            get { return (double) GetValue(StaticRingDiameterProperty); }
            set { SetValue(StaticRingDiameterProperty, value); }
        }

        public double StaticRingThickness
        {
            get { return (double) GetValue(StaticRingThicknessProperty); }
            set { SetValue(StaticRingThicknessProperty, value); }
        }

        // Progress Ring
        public double ProgressRingDiameter
        {
            get { return (double) GetValue(ProgressRingDiameterProperty); }
            set { SetValue(ProgressRingDiameterProperty, value); }
        }

        public double ProgressRingThickness
        {
            get { return (double) GetValue(ProgressRingThicknessProperty); }
            set { SetValue(ProgressRingThicknessProperty, value); }
        }

        public double ProgressRingRotationAngle
        {
            get { return (double) GetValue(ProgressRingRotationAngleProperty); }
            set { SetValue(ProgressRingRotationAngleProperty, value); }
        }

        // Pips Element
        public int Pips
        {
            get { return (int) GetValue(PipsProperty); }
            set { SetValue(PipsProperty, value); }
        }

        public double PipSize
        {
            get { return (double) GetValue(PipSizeProperty); }
            set { SetValue(PipSizeProperty, value); }
        }

        public double PipSegmentPercent
        {
            get { return (double) GetValue(PipSegmentPercentProperty); }
            set { SetValue(PipSegmentPercentProperty, value); }
        }

        public int PipAnimationSpeed
        {
            get { return (int) GetValue(PipAnimationSpeedProperty); }
            set { SetValue(PipAnimationSpeedProperty, value); }
        }

        public int PipAnimationBeginDelay
        {
            get { return (int) GetValue(PipAnimationBeginDelayProperty); }
            set { SetValue(PipAnimationBeginDelayProperty, value); }
        }

        // Text Element
        public string TextBoxContent
        {
            get { return (string) GetValue(TextBoxContentProperty); }
            set { SetValue(TextBoxContentProperty, value); }
        }

        public bool TextFadeOnComplete
        {
            get { return (bool) GetValue(TextFadeOnCompleteProperty); }
            set { SetValue(TextFadeOnCompleteProperty, value); }
        }

        public double TextElementOpacity
        {
            get { return (double) GetValue(TextElementOpacityProperty); }
            set { SetValue(TextElementOpacityProperty, value); }
        }

        #endregion

        //
        // CallBack/Changed Methods 
        // 

        #region Callbacks

        // Changed  
        private static void PercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRing) d).OnPercentageChanged();
        }

        private static void ProgressTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRing) d).ProgressTriggered();
        }

        private static void ControlDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProgressRing) d).ArrangeLayout();
        }

        // Coercion
        private static object PercentageCoerce(DependencyObject d, object baseValue)
        {
            if ((double) baseValue > 99.999) return 99.999;
            if ((double) baseValue < 0.0) return 0.0;
            return baseValue;
        }

        private static object ControlDiameterCoerce(DependencyObject d, object baseValue)
        {
            if ((double) baseValue < 20.0) return 20.0;
            return baseValue;
        }

        private static object MinimumThicknessCoerce(DependencyObject d, object baseValue)
        {
            if ((double) baseValue < 2.0) return 2.0;
            return baseValue;
        }

        private static object PipsCoerceCallback(DependencyObject d, object baseValue)
        {
            if ((int) baseValue > 20) return 20;
            if ((int) baseValue < 1) return 1;

            return baseValue;
        }

        private static object PipSizeCoerceCallback(DependencyObject d, object baseValue)
        {
            if ((double) baseValue > 100.0) return 100.0;
            if ((double) baseValue < 3.0) return 3.0;

            return baseValue;
        }

        #endregion
    }
}