using System;
using OpenSTSM.Guis.BlockParameters.Sources;
using OpenSTSM.Guis.BlockParameters.Continuous;
using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System.Windows;

namespace OpenSTSM.Guis
{
    public partial class SimulinkBlockParameters : Window
    {
        public SimulinkBlockParameters()
        {
            SimulinkBlockParametersViewModel viewModel = new SimulinkBlockParametersViewModel(ApplicationService.Instance.EventAggregator);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();            

            InitializeComponent();
        }

        public void OpenCorrectUserControl(string blockElementName)
        {
            var userControl = GetUserControl(blockElementName);
            if(userControl != null)
                Container.Children.Add(userControl);
        }

        private FrameworkElement GetUserControl(string blockElementName)
        {
            switch (blockElementName)
            {
                case "Constant": return new Constant();
                case "Step": return new Step();
                case "Ramp": return new Ramp();
                case "Scope": return null;
                case "Display": return null;
                case "TransferFunction": return new TransferFunction();
                case "PidController": return null;                    
                default: return null;         
            }
        }
    }
}
