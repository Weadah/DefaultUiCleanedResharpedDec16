using System;
using System.Windows.Input;

namespace DefaultUiCleanedResharpedDec16.ViewModel.Common
{
    public class VmCommand : ICommand
    {
        private readonly Action<object> _action;

        public VmCommand(Action<object> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
#pragma warning restore 67
    }
}