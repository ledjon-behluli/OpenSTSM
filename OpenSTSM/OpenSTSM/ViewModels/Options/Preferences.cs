
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

        private int _numOfResultsPerElement;
        public int NumberOfResultsPerElement
        {
            get
            {
                return _numOfResultsPerElement;
            }
            set
            {
                _numOfResultsPerElement = value;
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
