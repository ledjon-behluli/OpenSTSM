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
        public static FrameworkElement SelectedUserControl;
        
        public SimulinkBlockParameters(string elementName)
        {
            SimulinkBlockParametersViewModel viewModel = new SimulinkBlockParametersViewModel(ApplicationService.Instance.EventAggregator, elementName);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();            

            InitializeComponent();
        }

        public void OpenCorrectUserControl(string blockElementName)
        {
            var userControl = GetUserControl(blockElementName);            
            if (userControl != null)
            {
                Container.Children.Add(userControl);
                SelectedUserControl = userControl;
            }
        }

        private FrameworkElement GetUserControl(string blockElementName)
        {
            switch (blockElementName)
            {
                case nameof(Constant): return new Constant();
                case nameof(Step): return new Step();
                case nameof(Ramp): return new Ramp();
                case nameof(TransferFunction): return new TransferFunction();
                case nameof(PidController): return new PidController();
                case nameof(Integrator): return new Integrator();
                case nameof(Sum): return new Sum();
                case nameof(Gain): return new Gain();
                default: return null;         
            }
        }
    }
}
