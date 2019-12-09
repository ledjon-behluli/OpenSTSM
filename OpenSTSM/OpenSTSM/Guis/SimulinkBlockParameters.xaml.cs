using OpenSTSM.Guis.BlockParameters.Sources;
using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Container.Children.Add(GetUserControl(blockElementName));
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
                case "TransferFunction": return null;
                case "PidController": return null;                    
                default: return null;         
            }
        }
    }
}
