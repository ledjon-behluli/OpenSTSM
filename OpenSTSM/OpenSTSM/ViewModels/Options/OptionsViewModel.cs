using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OpenSTSM.ViewModels;
using Prism.Events;

namespace OpenSTSM.ViewModels.Options
{
    public class OptionsViewModel : WorkspaceViewModel
    {
        private IEventAggregator _eventAggregator;

        #region "Properties"

        private PredictionParameters _predictionParameters;
        public PredictionParameters PredictionParameters
        {
            get
            {
                return _predictionParameters;
            }
            set
            {
                _predictionParameters = value;
                OnPropertyChanged();
            }
        }

        private Preferences _preferences;
        public Preferences Preferences
        {
            get
            {
                return _preferences;
            }
            set
            {
                _preferences = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region "Commands"

        public ICommand UpdateCommand { get; set; }
        
        #endregion

        public OptionsViewModel(PredictionParameters predictionParameters, Preferences preferences, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            PredictionParameters = predictionParameters;
            Preferences = preferences;

            UpdateCommand = new RelayCommand(UpdateOptions);
        }

        private void UpdateOptions(object sender)
        {
            // Prediction Parameters
            Settings.Default.NumberOfRegionProposals = PredictionParameters.NumberOfRegionProposals;
            Settings.Default.MiddlePointDistanceThreshold = PredictionParameters.MiddlePointDistanceThreshold;
            Settings.Default.OuterSelectionThreshold = PredictionParameters.OuterSelectionThreshold;
            Settings.Default.DecimalPointProbabilityRounding = PredictionParameters.DecimalPointProbabilityRounding;
            Settings.Default.RegionProposalsMultiplicity = PredictionParameters.RegionProposalsMultiplicity;
            Settings.Default.SpatialDistanceOfCoordinatePointsThreshold = PredictionParameters.SpatialDistanceOfCoordinatePointsThreshold;

            // Preferences
            Settings.Default.LeafProbabilityThreshold = Preferences.LeafProbabilityThreshold;

            Settings.Default.Save();
            _eventAggregator.GetEvent<PreferencesUpdatedEvent>().Publish();

            base.Close();
        }
    }
}
