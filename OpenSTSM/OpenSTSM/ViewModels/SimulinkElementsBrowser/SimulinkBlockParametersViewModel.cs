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

        public ICommand SelectCommand;

        public SimulinkBlockParametersViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            SelectCommand = new RelayCommand(Select);
        }

        public void Select(object sender)
        {
            base.Close();
        }
    }
}
