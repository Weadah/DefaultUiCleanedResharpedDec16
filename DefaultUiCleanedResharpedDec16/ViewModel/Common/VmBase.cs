using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DefaultUiCleanedResharpedDec16.ViewModel.Common
{
    public class VmBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}