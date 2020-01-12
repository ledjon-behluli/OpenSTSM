using System.Windows;
using System.Collections.Generic;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class PredictedControlElement : ControlElement
    {
        private List<PossibleControlElement> _possibleControlElements;

        public List<PossibleControlElement> PossibleControlElements
        {
            get
            {
                return _possibleControlElements;
            }
            set
            {
                _possibleControlElements = value;
                OnPropertyChanged();
            }
        }

        public PredictedControlElement(string predictedControlElementName, decimal probability, bool needsMapping, Point location) 
            : base(predictedControlElementName, probability, needsMapping, location)
        {
          
        }
    }
}
