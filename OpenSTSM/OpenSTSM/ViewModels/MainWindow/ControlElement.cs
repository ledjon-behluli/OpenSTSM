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

        private string _probability = string.Empty;
        public string Probability
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
                return $"{ControlElementName} ({Probability}%)";
            }
        }

        public ControlElement(string controlElementName, string probability)
        {
            ControlElementName = controlElementName;
            Probability = probability;
        }
    }
}
