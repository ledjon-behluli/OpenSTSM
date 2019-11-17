using OpenSTSM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenSTSM.Guis
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            PredictionParametersViewModel viewModel = new PredictionParametersViewModel()
            {
                NumberOfRegionProposals = Settings.Default.NumberOfRegionProposals,
                MiddlePointDistanceThreshold = Settings.Default.MiddlePointDistanceThreshold,                
                OuterSelectionThreshold = Settings.Default.OuterSelectionThreshold,
                DecimalPointProbabilityRounding = Settings.Default.DecimalPointProbabilityRounding,
                RegionProposalsMultiplicity = Settings.Default.RegionProposalsMultiplicity,
                SpatialDistanceOfCoordinatePointsThreshold = Settings.Default.SpatialDistanceOfCoordinatePointsThreshold
            };

            DataContext = viewModel;

            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (udNumberOfRegionProposals.Value != null) Settings.Default.NumberOfRegionProposals = (int)udNumberOfRegionProposals.Value;
            if (udMiddlePointDistanceThreshold.Value != null) Settings.Default.MiddlePointDistanceThreshold = (int)udMiddlePointDistanceThreshold.Value;
            if (udOuterSelectionThreshold.Value != null) Settings.Default.OuterSelectionThreshold = (int)udOuterSelectionThreshold.Value;
            if (udDecimalPointProbabilityRounding.Value != null) Settings.Default.DecimalPointProbabilityRounding = (int)udDecimalPointProbabilityRounding.Value;
            if (udRegionProposalsMultiplicity.Value != null) Settings.Default.RegionProposalsMultiplicity = (int)udRegionProposalsMultiplicity.Value;
            if (udSpatialDistanceOfCoordinatePointsThreshold.Value != null) Settings.Default.SpatialDistanceOfCoordinatePointsThreshold = (int)udSpatialDistanceOfCoordinatePointsThreshold.Value;

            btnCancel_Click(sender, e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
