using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OpenSTSM.ViewModels;

namespace OpenSTSM.ViewModels.Options
{
    public class OptionsViewModel : WorkspaceViewModel
    {
        #region "Properties"

        private PredictionParameters _predictionParametersViewModel;
        public PredictionParameters PredictionParameters
        {
            get
            {
                return _predictionParametersViewModel;
            }
            set
            {
                _predictionParametersViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region "Commands"

        public ICommand UpdateCommand { get; set; }

        public ICommand CancelCommand { get; set; }
        
        #endregion

        public OptionsViewModel(PredictionParameters predictionParameters)
        {
            PredictionParameters = predictionParameters;
            UpdateCommand = new RelayCommand(UpdateOptions, param => true);      // canExecute is default to true, this can be used as RelayCommand(UpdateOptions)
            CancelCommand = new RelayCommand(CloseWindow);
        }

        private void UpdateOptions(object sender)
        {
            Settings.Default.NumberOfRegionProposals = PredictionParameters.NumberOfRegionProposals;
            Settings.Default.MiddlePointDistanceThreshold = PredictionParameters.MiddlePointDistanceThreshold;
            Settings.Default.OuterSelectionThreshold = PredictionParameters.OuterSelectionThreshold;
            Settings.Default.DecimalPointProbabilityRounding = PredictionParameters.DecimalPointProbabilityRounding;
            Settings.Default.RegionProposalsMultiplicity = PredictionParameters.RegionProposalsMultiplicity;
            Settings.Default.SpatialDistanceOfCoordinatePointsThreshold = PredictionParameters.SpatialDistanceOfCoordinatePointsThreshold;

            CloseWindow(sender);
        }

        private void CloseWindow(object sender)
        {
            base.Close();
        }
    }
}
