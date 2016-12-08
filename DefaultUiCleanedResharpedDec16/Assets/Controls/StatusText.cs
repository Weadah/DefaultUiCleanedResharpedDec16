using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace DefaultUiCleanedResharpedDec16.Assets.Controls
{
    //
    // Static Class for global Statusbar. 
    // BINDING: InputText="{Binding Path=(ctrl:StatusTextHandler.StatusText)}"
    // VIEWMODEL: Assets.Controls.StatusTextHandler.StatusText = "SomeString";
    //

    #region StatusTextHandler

    public static class StatusTextHandler
    {
        private static string _statusText;

        public static string StatusText
        {
            get { return _statusText; }
            set
            {
                if (value == _statusText) return;
                _statusText = value;
                StaticPropertyChanged(null, new PropertyChangedEventArgs("StatusText"));
            }
        }

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };
    }

    #endregion

    //
    // Control CLass
    // 

    public class StatusText : Control
    {
        public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(
            "InputText", typeof(string), typeof(StatusText), new PropertyMetadata(string.Empty));

        static StatusText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusText),
                new FrameworkPropertyMetadata(typeof(StatusText)));
        }

        public string InputText
        {
            get { return (string) GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }
    }
}