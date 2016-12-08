using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace DefaultUiCleanedResharpedDec16.Assets.AttachedProperties
{
    public class ProgressBarSmoother
    {
        public static readonly DependencyProperty SmoothValueProperty =
            DependencyProperty.RegisterAttached("SmoothValue", typeof(double), typeof(ProgressBarSmoother),
                new PropertyMetadata(0.0, Changing));

        public static double GetSmoothValue(DependencyObject obj)
        {
            return (double) obj.GetValue(SmoothValueProperty);
        }

        public static void SetSmoothValue(DependencyObject obj, double value)
        {
            obj.SetValue(SmoothValueProperty, value);
        }

        private static void Changing(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var anim = new DoubleAnimation
            {
                From = (double) e.OldValue,
                To = (double) e.NewValue,
                Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                EasingFunction = new BackEase {Amplitude = 1, EasingMode = EasingMode.EaseOut}
            };

            ((ProgressBar) d).BeginAnimation(RangeBase.ValueProperty, anim, HandoffBehavior.Compose);
        }
    }
}