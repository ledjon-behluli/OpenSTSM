using System.Windows;
using OpenSTSM.ViewModels.MainWindow;

namespace OpenSTSM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(ApplicationService.Instance.EventAggregator);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
