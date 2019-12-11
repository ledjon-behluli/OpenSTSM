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
    /// Interaction logic for TransferFunction.xaml
    /// </summary>
    public partial class TransferFunction : UserControl, INotifyPropertyChanged
    {
        private List<decimal> _numeratorCoefficients;
        public List<decimal> NumeratorCoefficients
        {
            get
            {
                return _numeratorCoefficients;
            }
            set
            {
                _numeratorCoefficients = value;
                OnPropertyChanged();
            }
        }

        private List<decimal> _denominatorCoefficients;
        public List<decimal> DenominatorCoefficients
        {
            get
            {
                return _denominatorCoefficients;
            }
            set
            {
                _denominatorCoefficients = value;
                OnPropertyChanged();
            }
        }


        public TransferFunction()
        {
            _numeratorCoefficients = new List<decimal>() { 1 };
            _denominatorCoefficients = new List<decimal>() { 1, 1 };

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
