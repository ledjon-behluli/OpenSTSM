using OpenSTSM.ViewModels.Options;
using System.Windows;

namespace OpenSTSM.Guis
{
    public partial class Options : Window
    {
        public Options()
        {
            PredictionParameters PredictionParameters = new PredictionParameters()
            {
                NumberOfRegionProposals = Settings.Default.NumberOfRegionProposals,
                MiddlePointDistanceThreshold = Settings.Default.MiddlePointDistanceThreshold,
                OuterSelectionThreshold = Settings.Default.OuterSelectionThreshold,
                DecimalPointProbabilityRounding = Settings.Default.DecimalPointProbabilityRounding,
                RegionProposalsMultiplicity = Settings.Default.RegionProposalsMultiplicity,
                SpatialDistanceOfCoordinatePointsThreshold = Settings.Default.SpatialDistanceOfCoordinatePointsThreshold
            };
            Preferences Preferences = new Preferences()
            {
                LeafProbabilityThreshold = Settings.Default.LeafProbabilityThreshold
            };

            OptionsViewModel viewModel = new OptionsViewModel(PredictionParameters, Preferences, ApplicationService.Instance.EventAggregator);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
