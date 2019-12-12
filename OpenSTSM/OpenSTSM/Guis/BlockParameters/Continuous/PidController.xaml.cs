using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace OpenSTSM.Guis.BlockParameters.Continuous
{
    public partial class PidController : UserControl, INotifyPropertyChanged
    {
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
                System.Windows.Forms.MessageBox.Show(value.Name);
                OnPropertyChanged();
            }
        }


        public PidController()
        {
            _pidControllers = new ObservableCollection<Models.SimulinkBlocks.PidController>()
            {
                new Models.SimulinkBlocks.PidController(0, "PID"),
                new Models.SimulinkBlocks.PidController(1, "PI"),
                new Models.SimulinkBlocks.PidController(1, "PD"),
                new Models.SimulinkBlocks.PidController(1, "P"),
                new Models.SimulinkBlocks.PidController(1, "I")
            };
            _selectedPidController = new Models.SimulinkBlocks.PidController(_pidControllers.First());

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
