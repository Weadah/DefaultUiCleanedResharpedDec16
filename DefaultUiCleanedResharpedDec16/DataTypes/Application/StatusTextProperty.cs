using System;
using System.ComponentModel;

//
// Static Class for global Statusbar. 
// BINDING: InputText="{Binding Path=(ctrl:StatusTextHandler.StatusText)}"
// VIEWMODEL: Assets.Controls.StatusTextHandler.StatusText = "SomeString";
//

namespace DefaultUiCleanedResharpedDec16.DataTypes.Application
{
    class StatusTextProperty
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };

        private static string _value;
        public static string Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    StaticPropertyChanged(null, new PropertyChangedEventArgs("Value"));
                }
            }
        }
    }
}
