
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

        private string _Simulink_OutputPath;
        public string Simulink_OutputPath
        {
            get
            {
                return _Simulink_OutputPath;
            }
            set
            {
                _Simulink_OutputPath = value;
                OnPropertyChanged();
            }
        }

    }
}
