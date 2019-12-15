using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Events;

namespace OpenSTSM.ViewModels.SimulinkElementsBrowser
{
    public class SimulinkBlockParametersViewModel : WorkspaceViewModel
    {
        private IEventAggregator _eventAggregator;
        private string _elementName;

        public ICommand SelectCommand { get; set; }

        public SimulinkBlockParametersViewModel(IEventAggregator eventAggregator, string elementName)
        {
            _eventAggregator = eventAggregator;
            _elementName = elementName;
            SelectCommand = new RelayCommand(Select);
        }

        private void Select(object sender)
        {
            base.Close();
            _eventAggregator.GetEvent<SimulinkElementChosen>().Publish(new SimulinkElementChosenPayload(_elementName));
        }
    }
}
