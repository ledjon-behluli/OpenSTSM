using System.Windows.Input;
using Prism.Events;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

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
        public ICommand Choose_NN_ModelPathCommand { get; set; }
        public ICommand ChooseSimulinkOutputPathCommand { get; set; }
        
        #endregion

        public OptionsViewModel(PredictionParameters predictionParameters, Preferences preferences, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            PredictionParameters = predictionParameters;
            Preferences = preferences;

            UpdateCommand = new RelayCommand(UpdateOptions);
            Choose_NN_ModelPathCommand = new RelayCommand(Choose_NN_ModelPath);
            ChooseSimulinkOutputPathCommand = new RelayCommand(ChooseSimulinkOutputPath);
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
            Settings.Default.ImageResizeFactor = PredictionParameters.ImageResizeFactor;
            Settings.Default.NN_ModelPath = PredictionParameters.NN_ModelPath;

            // Preferences
            Settings.Default.LeafProbabilityThreshold = Preferences.LeafProbabilityThreshold;
            Settings.Default.NumberOfResultsPerElement = Preferences.NumberOfResultsPerElement;
            Settings.Default.UseGpuAcceleration = Preferences.UseGpuAcceleration;
            Settings.Default.Simulink_OutputPath = Preferences.Simulink_OutputPath;

            Settings.Default.Save();
            _eventAggregator.GetEvent<PreferencesUpdatedEvent>().Publish();

            base.Close();
        }

        private void Choose_NN_ModelPath(object sender)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Keras model file (*.model)|*.model"
            };

            if (ofd.ShowDialog() == true)
            {
                PredictionParameters.NN_ModelPath = ofd.FileName;
            }
        }

        private void ChooseSimulinkOutputPath(object sender)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Preferences.Simulink_OutputPath = fbd.SelectedPath.TrimEnd(new[] { '/' });
                }
            }
        }
    }
}
