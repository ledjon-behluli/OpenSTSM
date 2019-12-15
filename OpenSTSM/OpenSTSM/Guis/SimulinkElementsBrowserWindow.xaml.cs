using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System.Windows;

namespace OpenSTSM.Guis
{
    public partial class SimulinkElementsBrowserWindow : Window
    {
        public SimulinkElementsBrowserWindow(int selectedTabIndex = -1)
        {
            SimulinkElementsBrowserViewModel viewModel = new SimulinkElementsBrowserViewModel(ApplicationService.Instance.EventAggregator, selectedTabIndex);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
