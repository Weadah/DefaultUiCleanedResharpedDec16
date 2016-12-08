using System.Windows;

namespace DefaultUiCleanedResharpedDec16.Assets.AttachedProperties
{
    public static class ScrollBarPositions
    {
        public static readonly DependencyProperty VerticalScrollBarOnLeftSideProperty =
            DependencyProperty.RegisterAttached("VerticalScrollBarOnLeftSide", typeof(bool), typeof(ScrollBarPositions),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));


        public static readonly DependencyProperty HorizontalScrollBarOnTopProperty =
            DependencyProperty.RegisterAttached("HorizontalScrollBarOnTop", typeof(bool), typeof(ScrollBarPositions),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetVerticalScrollBarOnLeftSide(DependencyObject obj)
        {
            return (bool) obj.GetValue(VerticalScrollBarOnLeftSideProperty);
        }

        public static void SetVerticalScrollBarOnLeftSide(DependencyObject obj, bool value)
        {
            obj.SetValue(VerticalScrollBarOnLeftSideProperty, value);
        }

        public static bool GetHorizontalScrollBarOnTop(DependencyObject obj)
        {
            return (bool) obj.GetValue(HorizontalScrollBarOnTopProperty);
        }

        public static void SetHorizontalScrollBarOnTop(DependencyObject obj, bool value)
        {
            obj.SetValue(HorizontalScrollBarOnTopProperty, value);
        }
    }
}