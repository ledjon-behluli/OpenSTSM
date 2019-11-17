using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.ViewModels.Options
{
    public class PredictionParameters : ViewModelBase
    {
        private int _middlePointDistanceThreshold { get; set; }
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

        private int _numberOfRegionProposals { get; set; }
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

        private int _outerSelectionThreshold { get; set; } 
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

        private int _decimalPointProbabilityRounding { get; set; }
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

        private int _regionProposalsMultiplicity { get; set; }
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

        private int _spatialDistanceOfCoordinatePointsThreshold { get; set; }
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
    }
}
