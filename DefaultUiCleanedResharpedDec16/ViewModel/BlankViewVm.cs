using System.Windows.Input;
using DefaultUiCleanedResharpedDec16.DataTypes.Interfaces;
using DefaultUiCleanedResharpedDec16.ViewModel.Common;

namespace DefaultUiCleanedResharpedDec16.ViewModel
{
    public class BlankViewVm : VmBase, IPageViewModel
    {
        // Required by IPageViewModel
        public string ViewDisplayName => "Blank V-VM Template";
        public bool   ViewNavigationSelected { get; set; }

        // Props n Fields
        private string _contentString;
        public string ContentString
        {
            get { return _contentString; }
            set { _contentString = value; RaisePropertyChangedEvent(); }
        }

        //
        // Logic
        //


        //
        // Clicks
        //         
        public ICommand UpdateContentTextCommand => new VmCommand(UpdateContentText);

        private void UpdateContentText(object param)
        {
            ContentString = param.ToString();
        }
    }
}