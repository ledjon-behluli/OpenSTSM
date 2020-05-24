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
                SpatialDistanceOfCoordinatePointsThreshold = Settings.Default.SpatialDistanceOfCoordinatePointsThreshold,
                ImageResizeFactor = Settings.Default.ImageResizeFactor,
                NN_ModelPath = Settings.Default.NN_ModelPath
            };
            Preferences Preferences = new Preferences()
            {
                UseGpuAcceleration = Settings.Default.UseGpuAcceleration,
                NumberOfResultsPerElement = Settings.Default.NumberOfResultsPerElement,
                LeafProbabilityThreshold = Settings.Default.LeafProbabilityThreshold,
                Simulink_OutputPath = Settings.Default.Simulink_OutputPath
            };

            OptionsViewModel viewModel = new OptionsViewModel(PredictionParameters, Preferences, ApplicationService.Instance.EventAggregator);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
