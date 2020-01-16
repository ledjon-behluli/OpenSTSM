using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenSTSM.ViewModels.Options
{
    public class Preferences : ViewModelBase
    {
        private decimal _leafProbabilityThreshold;
        public decimal LeafProbabilityThreshold
        {
            get
            {
                return _leafProbabilityThreshold;
            }
            set
            {
                _leafProbabilityThreshold = value;
                OnPropertyChanged();
            }
        }

        private bool _useGpuAcceleration;
        public bool UseGpuAcceleration
        {
            get
            {
                return _useGpuAcceleration;
            }
            set
            {
                _useGpuAcceleration = value;
                OnPropertyChanged();
            }
        }
    }
}
