using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System.Windows;

namespace OpenSTSM.Guis
{
    public partial class SimulinkElementsBrowserWindow : Window
    {
        public SimulinkElementsBrowserWindow(SimulinkBrowserTabVisibilities simulinkBrowserTabVisibilities = null, int selectedTabIndex = -1)
        {
            SimulinkElementsBrowserViewModel viewModel = new SimulinkElementsBrowserViewModel(ApplicationService.Instance.EventAggregator, simulinkBrowserTabVisibilities, selectedTabIndex);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
