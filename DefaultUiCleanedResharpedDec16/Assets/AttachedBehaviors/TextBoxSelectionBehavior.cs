using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DefaultUiCleanedResharpedDec16.Assets.AttachedBehaviors
{
    public class TextBoxSelectionBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseDoubleClick += AssociatedObjectMouseDoubleClick;
            AssociatedObject.PreviewMouseDown += AssociatedObjectPreviewMouseDown;
            AssociatedObject.GotFocus += AssociatedObjectGotFocus;

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDoubleClick -= AssociatedObjectMouseDoubleClick;
            base.OnDetaching();
        }

        private void AssociatedObjectMouseDoubleClick(object sender, RoutedEventArgs routedEventArgs)
        {
            AssociatedObject.SelectAll();
        }

        private void AssociatedObjectGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            AssociatedObject.SelectAll();
        }

        private void AssociatedObjectPreviewMouseDown(object sender, RoutedEventArgs routedEventArgs)
        {
            var textBox = sender as TextBox;

            if (textBox == null || textBox.IsFocused) return;

            textBox.Focus();
            textBox.SelectAll();
            routedEventArgs.Handled = true;
        }
    }
}