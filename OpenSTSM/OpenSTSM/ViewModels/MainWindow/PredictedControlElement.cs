using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PredictedControlElement(string predictedControlElementName, string probability) : base(predictedControlElementName, probability)
        {
          
        }
    }
}
