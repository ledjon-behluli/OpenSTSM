﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace OpenSTSM.Guis.BlockParameters.Continuous
{
    public partial class PidController : UserControl, INotifyPropertyChanged
    {
        #region Controllers

        private ObservableCollection<Models.SimulinkBlocks.PidController> _pidControllers;
        public ObservableCollection<Models.SimulinkBlocks.PidController> PidControllers
        {
            get
            {
                return _pidControllers;
            }
            set
            {
                _pidControllers = value;
                OnPropertyChanged();
            }
        }

        private Models.SimulinkBlocks.PidController _selectedPidController;
        public Models.SimulinkBlocks.PidController SelectedPidController
        {
            get
            {
                return _selectedPidController;
            }
            set
            {
                _selectedPidController = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Controller Parameters

        private double _proportional;
        public double Proportional
        {
            get
            {
                return _proportional;
            }
            set
            {
                _proportional = value;
                OnPropertyChanged();
            }
        }

        private double _integral;
        public double Integral
        {
            get
            {
                return _integral;
            }
            set
            {
                _integral = value;
                OnPropertyChanged();
            }
        }

        private double _derivative;
        public double Derivative
        {
            get
            {
                return _derivative;
            }
            set
            {
                _derivative = value;
                OnPropertyChanged();
            }
        }

        private double _filterCoefficient;
        public double FilterCoefficient
        {
            get
            {
                return _filterCoefficient;
            }
            set
            {
                _filterCoefficient = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Controller Initial Conditions

        private double _integrator;
        public double Integrator
        {
            get
            {
                return _integrator;
            }
            set
            {
                _integrator = value;
                OnPropertyChanged();
            }
        }

        private double _filter;
        public double Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public PidController()
        {
            _pidControllers = new ObservableCollection<Models.SimulinkBlocks.PidController>()
            {
                new Models.SimulinkBlocks.PidController(0, "PID"),
                new Models.SimulinkBlocks.PidController(1, "PI"),
                new Models.SimulinkBlocks.PidController(2, "PD"),
                new Models.SimulinkBlocks.PidController(3, "P"),
                new Models.SimulinkBlocks.PidController(4, "I")
            };
            _selectedPidController = new Models.SimulinkBlocks.PidController(_pidControllers.First());

            _proportional = 1;
            _integral = 1;
            _derivative = 1;
            _filterCoefficient = 100;

            _integrator = 0;
            _filter = 0;

            this.Name = "PidController";
            DataContext = this;
            InitializeComponent();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
    }
}
