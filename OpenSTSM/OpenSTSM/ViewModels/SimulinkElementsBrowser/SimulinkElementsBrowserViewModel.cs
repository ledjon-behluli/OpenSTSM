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


        #region Properties



        #endregion

        #region "Commands"

        public object CommandParameter { get; set; }
        public ICommand ChooseBlockCommand { get; set; }

        public ICommand SelectCommand { get; set; }

        #endregion

        public SimulinkElementsBrowserViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ChooseBlockCommand = new RelayCommand(ChooseBlock);
            SelectCommand = new RelayCommand(Select);            
        }

        public void ChooseBlock(object sender)
        {
            string name = ((Button)sender).Name;

            if (!elementsWithoutParameters.Contains(name))
            {
                if (!Helper.IsWindowOpen<SimulinkBlockParameters>())
                {
                    var sbp = new SimulinkBlockParameters();
                    sbp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    sbp.Owner = App.Current.MainWindow;
                    sbp.Title = $"Block Parameters: {name}";
                    sbp.Show();
                    sbp.OpenCorrectUserControl(name);
                }
            }
        }

        public void Select(object sender)
        {
            _eventAggregator.GetEvent<SimulinkElementChosen>().Publish();

            base.Close();
        }
    }
}
