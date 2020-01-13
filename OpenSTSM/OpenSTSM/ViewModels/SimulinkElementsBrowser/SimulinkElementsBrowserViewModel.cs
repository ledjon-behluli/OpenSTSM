using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OpenSTSM.Guis;
using Prism.Events;

namespace OpenSTSM.ViewModels.SimulinkElementsBrowser
{
    public class SimulinkElementsBrowserViewModel : WorkspaceViewModel
    {
        private IEventAggregator _eventAggregator;
        private List<string> elementsWithoutParameters = new List<string>() { "Scope", "Display" };
        private SimulinkBrowserTabVisibilities _simulinkBrowserTabVisibilities;
        private int _selectedTabIndex;

        #region Properties

        public int SelectedTabIndex { get; private set; }

        public bool SourcesVisibility
        {
            get
            {
                return _simulinkBrowserTabVisibilities.SourcesVisibility;
            }
        }
        public bool SinksVisibility
        {
            get
            {
                return _simulinkBrowserTabVisibilities.SinksVisibility;
            }
        }
        public bool ContinuousVisibility
        {
            get
            {
                return _simulinkBrowserTabVisibilities.ContinuousVisibility;
            }
        }
        public bool MathOperationsVisibility
        {
            get
            {
                return _simulinkBrowserTabVisibilities.MathOperationsVisibility;
            }
        }

        #endregion

        #region "Commands"

        public ICommand ChooseBlockCommand { get; set; }

        #endregion

        public SimulinkElementsBrowserViewModel(IEventAggregator eventAggregator, SimulinkBrowserTabVisibilities simulinkBrowserTabVisibilities, int selectedTabIndex)
        {
            _selectedTabIndex = selectedTabIndex;
            SelectedTabIndex = selectedTabIndex == -1 ? 0 : selectedTabIndex;
            _simulinkBrowserTabVisibilities = simulinkBrowserTabVisibilities ?? new SimulinkBrowserTabVisibilities(true, true, true, true);

            _eventAggregator = eventAggregator;
            ChooseBlockCommand = new RelayCommand(ChooseBlock);
            _eventAggregator.GetEvent<SimulinkElementChosenEvent>().Subscribe(OnSimulinkElementChosen, ThreadOption.UIThread);
        }

        private void ChooseBlock(object sender)
        {
            var element = sender as Button;            

            if (!elementsWithoutParameters.Contains(element.Name))
            {
                if (!WindowHelper.IsWindowOpen<SimulinkBlockParameters>())
                {
                    var sbp = new SimulinkBlockParameters(element.Name);
                    sbp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    sbp.Owner = App.Current.MainWindow;
                    sbp.Title += $" {element.Uid}";
                    sbp.Show();
                }
            }
            else
            {
                _eventAggregator.GetEvent<SimulinkElementChosenEvent>().Publish(new SimulinkElementChosenPayload(element.Name));
            }
        }

        private void OnSimulinkElementChosen(SimulinkElementChosenPayload payload)
        {
            base.Close();
        }
    }

    public class SimulinkBrowserTabVisibilities
    {
        public bool SourcesVisibility { get; set; } 
        public bool SinksVisibility { get; set; } 
        public bool ContinuousVisibility { get; set; }
        public bool MathOperationsVisibility { get; set; }

        public SimulinkBrowserTabVisibilities(bool sourcesVisibility = false, bool sinksVisibility = false, bool continuousVisibility = false, bool mathOperationsVisibility = false)
        {
            SourcesVisibility = sourcesVisibility;
            SinksVisibility = sinksVisibility;
            ContinuousVisibility = continuousVisibility;
            MathOperationsVisibility = mathOperationsVisibility;
        }
    }
}
