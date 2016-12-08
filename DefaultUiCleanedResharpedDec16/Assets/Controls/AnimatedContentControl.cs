using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DefaultUiCleanedResharpedDec16.Assets.Controls
{
    [TemplatePart(Name = "Part_Visual", Type = typeof(Rectangle))]
    [TemplatePart(Name = "Part_Content", Type = typeof(ContentPresenter))]
    public class AnimatedContentControl : ContentControl
    {
        //private readonly IEasingFunction ease   = new BackEase { Amplitude = 0.3, EasingMode = EasingMode.EaseOut };
        private readonly IEasingFunction _ease = new PowerEase {Power = 3, EasingMode = EasingMode.EaseOut};
        private readonly Duration _fadeDuration = new Duration(TimeSpan.FromMilliseconds(200));
        private readonly Duration _slideDuration = new Duration(TimeSpan.FromMilliseconds(300));
        private ContentPresenter _templateContentElement;
        private Rectangle _templateVisualElement;

        private readonly int ContentItemGap = 20; // pixels

        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl),
                new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }

        public override void OnApplyTemplate()
        {
            _templateVisualElement = Template.FindName("Part_Visual", this) as Rectangle;
            _templateContentElement = Template.FindName("Part_Content", this) as ContentPresenter;

            base.OnApplyTemplate();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (_templateVisualElement != null && _templateContentElement != null)
            {
                _templateVisualElement.Fill = CreateBrushFromControl(_templateContentElement);
                BeginAnimateContentReplacement();
            }

            base.OnContentChanged(oldContent, newContent);
        }

        private void BeginAnimateContentReplacement()
        {
            var newContentTransform = new TranslateTransform();

            _templateContentElement.RenderTransform = newContentTransform;
            _templateVisualElement.Visibility = Visibility.Visible;

            newContentTransform.BeginAnimation(TranslateTransform.XProperty,
                SlideAnimation(ActualWidth/3 + ContentItemGap, 0));
            _templateVisualElement.BeginAnimation(OpacityProperty, FadeAnimation());
        }

        private AnimationTimeline FadeAnimation()
        {
            var anim = new DoubleAnimation(1, 0, _fadeDuration);

            anim.Completed += (s, e) => _templateVisualElement.Visibility = Visibility.Hidden;
            anim.Freeze();

            return anim;
        }

        private AnimationTimeline SlideAnimation(double from, double to)
        {
            var anim = new DoubleAnimation(from, to, _slideDuration) {EasingFunction = _ease};
            anim.Freeze();
            return anim;
        }

        private Brush CreateBrushFromControl(Visual vis)
        {
            if (vis == null)
                throw new ArgumentNullException("ERROR: Cannot create ImageBrush from Control. (Assets.Controls...)");

            var target = new RenderTargetBitmap((int) ActualWidth, (int) ActualHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(vis);

            var brush = new ImageBrush(target);
            brush.Freeze();

            return brush;
        }
    }
}