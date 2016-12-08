using System.Windows;

namespace DefaultUiCleanedResharpedDec16.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.MainWindowVm();
        }
    }
}
