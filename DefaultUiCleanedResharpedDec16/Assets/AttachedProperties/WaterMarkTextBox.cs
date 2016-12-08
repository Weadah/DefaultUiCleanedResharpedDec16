using System.Windows;
using System.Windows.Controls;

namespace DefaultUiCleanedResharpedDec16.Assets.AttachedProperties
{
    internal class WaterMarkTextBox : DependencyObject
    {
        #region Attached Properties

        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(WaterMarkTextBox),
                new UIPropertyMetadata(false, OnIsMonitoringChanged));


        public static string GetWatermarkText(DependencyObject obj)
        {
            return (string) obj.GetValue(WatermarkTextProperty);
        }

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkTextProperty, value);
        }

        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(WaterMarkTextBox),
                new UIPropertyMetadata(string.Empty));


        public static int GetTextLength(DependencyObject obj)
        {
            return (int) obj.GetValue(TextLengthProperty);
        }

        public static void SetTextLength(DependencyObject obj, int value)
        {
            obj.SetValue(TextLengthProperty, value);
            obj.SetValue(HasTextProperty, value >= 1);
        }

        public static readonly DependencyProperty TextLengthProperty =
            DependencyProperty.RegisterAttached("TextLength", typeof(int), typeof(WaterMarkTextBox),
                new UIPropertyMetadata(0));

        #endregion

        #region Internal DependencyProperty

        public bool HasText
        {
            get { return (bool) GetValue(HasTextProperty); }
            set { SetValue(HasTextProperty, value); }
        }

        private static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText", typeof(bool), typeof(WaterMarkTextBox),
                new FrameworkPropertyMetadata(false));

        #endregion

        #region Implementation

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox)
            {
                var txtBox = (TextBox) d;

                if ((bool) e.NewValue)
                    txtBox.TextChanged += TextChanged;
                else
                    txtBox.TextChanged -= TextChanged;
            }
            else if (d is PasswordBox)
            {
                var passBox = (PasswordBox) d;

                if ((bool) e.NewValue)
                    passBox.PasswordChanged += PasswordChanged;
                else
                    passBox.PasswordChanged -= PasswordChanged;
            }
        }

        private static void TextChanged(object sender, TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox == null) return;
            SetTextLength(txtBox, txtBox.Text.Length);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passBox = sender as PasswordBox;
            if (passBox == null) return;
            SetTextLength(passBox, passBox.Password.Length);
        }

        #endregion
    }
}