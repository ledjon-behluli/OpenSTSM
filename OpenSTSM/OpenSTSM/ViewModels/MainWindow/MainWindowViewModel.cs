using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows;
using OpenSTSM.Guis;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private bool _isProcessing = false;
        #region Properties

        #region "TreeView"

        private List<ControlSystem> _controlSystems;
        public List<ControlSystem> ControlSystems
        {
            get
            {
                return _controlSystems;
            }
            set
            {
                _controlSystems = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region StatusBar"

        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand ImportImageCommand { get; set; }
        private bool canExecute_ImportImage = true;

        public ICommand AnalyseImageCommand { get; set; }
        private bool canExecute_AnalyseImage = false;

        public ICommand GenerateSimulinkModelCommand { get; set; }
        private bool canExecute_GenerateSimulinkModel = false;

        public ICommand OptionsCommand { get; set; }

        public ICommand AboutCommand { get; set; }

        #endregion

        public MainWindowViewModel()
        {
            ControlSystems = GetControlSystems();
            ImportImageCommand = new RelayCommand(ImportImage, param => canExecute_ImportImage);
            AnalyseImageCommand = new RelayCommand(AnalyseImage, param => canExecute_AnalyseImage);
            GenerateSimulinkModelCommand = new RelayCommand(GenerateSimulinkModel, param => canExecute_GenerateSimulinkModel);
            OptionsCommand = new RelayCommand(OpenOptions);
            AboutCommand = new RelayCommand(OpenAbout);
        }

        private void ImportImage(object sender)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (ofd.ShowDialog() == true)
            {
                FileName = ofd.FileName;
                ChangeCanExecute(true, ref canExecute_AnalyseImage);
            }
        }
        
        private void AnalyseImage(object sender)
        {
            _isProcessing = true;
            ChangeCanExecute(false, ref canExecute_ImportImage);
            ChangeCanExecute(false, ref canExecute_AnalyseImage);

            MessageBox.Show("Analysing"); // Some long running code ...            
            
            ChangeCanExecute(true, ref canExecute_AnalyseImage);
            ChangeCanExecute(true, ref canExecute_ImportImage);
            ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
            _isProcessing = false;
        }

        private void GenerateSimulinkModel(object sender)
        {
            _isProcessing = true;
            ChangeCanExecute(false, ref canExecute_ImportImage);
            ChangeCanExecute(false, ref canExecute_AnalyseImage);
            ChangeCanExecute(false, ref canExecute_GenerateSimulinkModel);

            MessageBox.Show("Generating"); // Some long running code ...            
            
            ChangeCanExecute(true, ref canExecute_AnalyseImage);
            ChangeCanExecute(true, ref canExecute_ImportImage);
            ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
            _isProcessing = false;
        }

        private void OpenOptions(object sender)
        {
            Guis.Options options = new Guis.Options();
            options.ShowDialog();
        }

        private void OpenAbout(object sender)
        {
            Guis.About about = new Guis.About();
            about.ShowDialog();
        }


        public void ChangeCanExecute(bool canExecute, ref bool canExecuteObj)
        {
            canExecuteObj = canExecute;
        }

        public override void Close()
        {
            if (_isProcessing)
            {
                if (MessageBox.Show("Processing is running!\nAre you sure you want to close the program?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    base.Close();
            }
            else
                base.Close();
        }


        private List<ControlSystem> GetControlSystems()
        {
            List<ControlSystem> controlSystems = new List<ControlSystem>();
            controlSystems.Add(new ControlSystem("System")
            {
                PredictedControlElements = new List<PredictedControlElement>()
                {
                    new PredictedControlElement("Controller", "98.34")
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Output", "1.01"),
                             new PossibleControlElement("Process", "0.25")
                         }
                    },
                    new PredictedControlElement("Arrow Right", "92.78")
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Arrow Left", "6.78"),
                             new PossibleControlElement("Arrow Up", "2.12")
                         }
                    }
                }
            });

            return controlSystems;
        }
    }
}
