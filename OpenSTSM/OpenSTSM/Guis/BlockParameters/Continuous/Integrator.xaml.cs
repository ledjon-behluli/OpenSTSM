using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace OpenSTSM.Guis.BlockParameters.Continuous
{
    /// <summary>
    /// Interaction logic for Integrator.xaml
    /// </summary>
    public partial class Integrator : UserControl, INotifyPropertyChanged
    {
        public double _initialCondition;
        public double InitialCondition
        {
            get
            {
                return _initialCondition;
            }
            set
            {
                _initialCondition = value;
                OnPropertyChanged();
            }
        }

        public Integrator()
        {
            _initialCondition = 0;

            this.Name = "Integrator";
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
