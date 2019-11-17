using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class ControlSystem : ViewModelBase
    {
        private List<PredictedControlElement> _predictedControlElements;

        public List<PredictedControlElement> PredictedControlElements
        {
            get
            {
                return _predictedControlElements;
            }
            set
            {
                _predictedControlElements = value;
                OnPropertyChanged();
            }
        }

        public string ControlSystemName { get; set; }

        public ControlSystem(string controlSystemName)
        {
            ControlSystemName = controlSystemName;
        }
    }
}
