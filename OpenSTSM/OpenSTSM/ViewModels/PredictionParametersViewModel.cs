using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.ViewModels
{
    public class PredictionParametersViewModel
    {
        public int MiddlePointDistanceThreshold { get; set; }
        public int NumberOfRegionProposals { get; set; }
        public int OuterSelectionThreshold { get; set; }
        public int DecimalPointProbabilityRounding { get; set; }
        public int RegionProposalsMultiplicity { get; set; }
        public int SpatialDistanceOfCoordinatePointsThreshold { get; set; }
    }
}
