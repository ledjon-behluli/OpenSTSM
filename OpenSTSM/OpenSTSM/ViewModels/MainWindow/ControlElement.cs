using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class ControlElement : ViewModelBase
    {
        private string _controlElementName = string.Empty;
        public string ControlElementName
        {
            get
            {
                return _controlElementName;
            }
            set
            {
                _controlElementName = value;
                OnPropertyChanged();
            }
        }

        private decimal _probability = 0;    
        public decimal Probability
        {
            get
            {
                return _probability;
            }
            set
            {
                _probability = value;
                OnPropertyChanged();
            }
        }

        public string Information
        {
            get
            {
                return $"{ControlElementName} ({Probability.ToString()}%)";
            }
        }

        public ControlElement(string controlElementName, decimal probability)
        {
            ControlElementName = controlElementName;
            Probability = probability;
        }
    }
}
