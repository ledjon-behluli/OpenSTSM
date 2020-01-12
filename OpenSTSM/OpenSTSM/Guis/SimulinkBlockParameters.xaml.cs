using System;
using OpenSTSM.Guis.BlockParameters.Sources;
using OpenSTSM.Guis.BlockParameters.Continuous;
using OpenSTSM.Guis.BlockParameters.MathOperations;
using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System.Windows;

namespace OpenSTSM.Guis
{
    public partial class SimulinkBlockParameters : Window
    {        
        public SimulinkBlockParameters(string elementName)
        {
            SimulinkBlockParametersViewModel viewModel = new SimulinkBlockParametersViewModel(ApplicationService.Instance.EventAggregator, this, elementName);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();            

            InitializeComponent();
        }

        public FrameworkElement GetSelectedUserControl(string blockElementName)
        {
            switch (blockElementName)
            {
                case "Constant": return this.Constant;
                case "Step": return this.Step;
                case "Ramp": return this.Ramp;
                case "TransferFunction": return this.TransferFunction;
                case "PidController": return this.PidController;
                case "Integrator": return this.Integrator;
                case "Sum": return this.Sum;
                case "Gain": return this.Gain;
                default: return null;
            }
        }
    }
}
