using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private int _selectedTabIndex;

        #region Properties

        public int SelectedTabIndex { get; private set; }

        public bool SourcesVisibility
        {
            get
            {
                return SelectedTabIndex == 0 || _selectedTabIndex == -1;
            }
        }
        public bool SinksVisibility
        {
            get
            {
                return SelectedTabIndex == 1 || _selectedTabIndex == -1;
            }
        }
        public bool ContinuousVisibility
        {
            get
            {
                return SelectedTabIndex == 2 || _selectedTabIndex == -1;
            }
        }
        public bool MathOperationsVisibility
        {
            get
            {
                return SelectedTabIndex == 3 || _selectedTabIndex == -1;
            }
        }

        #endregion

        #region "Commands"

        public ICommand ChooseBlockCommand { get; set; }

        #endregion

        public SimulinkElementsBrowserViewModel(IEventAggregator eventAggregator, int selectedTabIndex)
        {
            _selectedTabIndex = selectedTabIndex;
            SelectedTabIndex = selectedTabIndex == -1 ? 0 : selectedTabIndex;

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
}
