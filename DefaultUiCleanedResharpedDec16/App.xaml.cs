using DefaultUiCleanedResharpedDec16.View;
using DefaultUiCleanedResharpedDec16.ViewModel;
using System.Windows;

namespace DefaultUiCleanedResharpedDec16
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            MainWindowVm context = new MainWindowVm();

            window.Title = "UI Styles, Templates and Properties";
            window.DataContext = context;

            window.Show();
        }
    }
}
