using System.Windows.Input;
using DefaultUiCleanedResharpedDec16.Assets.Controls;
using DefaultUiCleanedResharpedDec16.DataTypes.Interfaces;
using DefaultUiCleanedResharpedDec16.View;
using DefaultUiCleanedResharpedDec16.ViewModel.Common;

// StatusBarText

namespace DefaultUiCleanedResharpedDec16.ViewModel
{
    internal class MainWindowVm : VmBase
    {
        private ICommand _changePageCommand;

        public MainWindowVm()
        {
            ViewManager = new VmManager();
            ViewManager.RegisterDataTemplate<BlankViewVm, BlankView>();

            // Set starting page    
            ViewManager.CurrentPageViewModel = ViewManager.PageViewModels[0];

            StatusTextHandler.StatusText = "Test Status-Text set by MainwindowVM";
        }

        public VmManager ViewManager { get; set; }

        public ICommand ChangePageCommand
        {
            get
            {
                return _changePageCommand ??
                       (_changePageCommand = new VmCommand(p => ViewManager.ChangeViewModel((IPageViewModel) p)));                
            }
        }
    }
}