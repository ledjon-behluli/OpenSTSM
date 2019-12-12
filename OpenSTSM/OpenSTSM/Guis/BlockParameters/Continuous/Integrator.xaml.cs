using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpenSTSM.Guis.BlockParameters.Continuous
{
    /// <summary>
    /// Interaction logic for Integrator.xaml
    /// </summary>
    public partial class Integrator : UserControl, INotifyPropertyChanged
    {
        public decimal _initialCondition;
        public decimal InitialCondition
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
