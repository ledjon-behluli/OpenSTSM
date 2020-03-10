
namespace OpenSTSM.ViewModels.Options
{
    public class PredictionParameters : ViewModelBase
    {
        private int _middlePointDistanceThreshold;
        public int MiddlePointDistanceThreshold
        {
            get
            {
                return _middlePointDistanceThreshold;
            }
            set
            {
                _middlePointDistanceThreshold = value;
                OnPropertyChanged();
            }
        }

        private int _numberOfRegionProposals;
        public int NumberOfRegionProposals
        {
            get
            {
                return _numberOfRegionProposals;
            }
            set
            {
                _numberOfRegionProposals = value;
                OnPropertyChanged();
            }
        }

        private int _outerSelectionThreshold;
        public int OuterSelectionThreshold
        {
            get
            {
                return _outerSelectionThreshold;
            }
            set
            {
                _outerSelectionThreshold = value;
                OnPropertyChanged();
            }
        }

        private int _decimalPointProbabilityRounding;
        public int DecimalPointProbabilityRounding
        {
            get
            {
                return _decimalPointProbabilityRounding;
            }
            set
            {
                _decimalPointProbabilityRounding = value;
                OnPropertyChanged();
            }
        }

        private int _regionProposalsMultiplicity;
        public int RegionProposalsMultiplicity
        {
            get
            {
                return _regionProposalsMultiplicity;
            }
            set
            {
                _regionProposalsMultiplicity = value;
                OnPropertyChanged();
            }
        }

        private int _spatialDistanceOfCoordinatePointsThreshold;
        public int SpatialDistanceOfCoordinatePointsThreshold
        {
            get
            {
                return _spatialDistanceOfCoordinatePointsThreshold;
            }
            set
            {
                _spatialDistanceOfCoordinatePointsThreshold = value;
                OnPropertyChanged();
            }
        }

        private string _NN_ModelPath;
        public string NN_ModelPath
        {
            get
            {
                return _NN_ModelPath;
            }
            set
            {
                _NN_ModelPath = value;
                OnPropertyChanged();
            }
        }

        private float _imageResizeFactor;
        public float ImageResizeFactor
        {
            get
            {
                return _imageResizeFactor;
            }
            set
            {
                _imageResizeFactor = value;
                OnPropertyChanged();
            }
        }
    }
}
