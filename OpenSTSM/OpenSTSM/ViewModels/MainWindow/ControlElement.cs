using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class ControlElement : ViewModelBase
    {
        public Guid Guid { get; private set; }

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

        private bool _needsLinking;
        public bool NeedsLinking
        {
            get
            {
                return _needsLinking;
            }
            set
            {
                _needsLinking = value;
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

        public string Identifier
        {
            get
            {
                return $"{ControlElementName} ({Guid.ToString()})";
            }
        }

        public ControlElement(string controlElementName, decimal probability, bool needsLinking)
        {
            Guid = Guid.NewGuid();
            _controlElementName = controlElementName;
            _probability = probability;
            _needsLinking = needsLinking;
        }
    }
}
