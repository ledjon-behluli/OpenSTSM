using System.Collections.Generic;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class ControlSystem : ViewModelBase
    {
        public List<PredictedControlElement> _predictedControlElements;
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
            PredictedControlElements = new List<PredictedControlElement>();
        }
    }
}
