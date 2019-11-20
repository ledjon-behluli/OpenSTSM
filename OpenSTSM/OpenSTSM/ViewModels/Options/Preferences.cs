using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
