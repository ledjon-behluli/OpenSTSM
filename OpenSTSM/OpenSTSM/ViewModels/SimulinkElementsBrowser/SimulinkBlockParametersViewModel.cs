using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OpenSTSM.Guis;
using OpenSTSM.Guis.BlockParameters.MathOperations;
using OpenSTSM.Guis.BlockParameters.Sources;
using Prism.Events;

namespace OpenSTSM.ViewModels.SimulinkElementsBrowser
{
    public class SimulinkBlockParametersViewModel : WorkspaceViewModel
    {
        private IEventAggregator _eventAggregator;
        private SimulinkBlockParameters _instance;
        private string _elementName;

        #region UserControl Visibilities

        public bool IsConstantVisible { get; set; }
        public bool IsStepVisible { get; set; }
        public bool IsRampVisible { get; set; }
        public bool IsTFVisible { get; set; }
        public bool IsPidVisible { get; set; }
        public bool IsIntegratorVisible { get; set; }
        public bool IsSumVisible { get; set; }
        public bool IsGainVisible { get; set; }

        #endregion

        public ICommand SelectCommand { get; set; }


        public SimulinkBlockParametersViewModel(IEventAggregator eventAggregator, SimulinkBlockParameters SimulinkBlockParametersInstance, string elementName)
        {
            _eventAggregator = eventAggregator;
            _instance = SimulinkBlockParametersInstance;
            _elementName = elementName;
            switch (elementName)
            {              
                case "Constant": IsConstantVisible = true;
                    break;
                case "Step": IsStepVisible = true;
                    break;
                case "Ramp": IsRampVisible = true;
                    break;
                case "TransferFunction": IsTFVisible = true;
                    break;
                case "PidController": IsPidVisible = true;
                    break;
                case "Integrator": IsIntegratorVisible = true;
                    break;
                case "Sum": IsSumVisible = true;                    
                    break;
                case "Gain": IsGainVisible = true;
                    break;
            }

            SelectCommand = new RelayCommand(Select);            
        }

        private void Select(object sender)
        {
            _eventAggregator.GetEvent<SimulinkElementChosenEvent>().Publish(new SimulinkElementChosenPayload(_instance.GetSelectedUserControl(_elementName)));
            base.Close();
        }
    }
}
