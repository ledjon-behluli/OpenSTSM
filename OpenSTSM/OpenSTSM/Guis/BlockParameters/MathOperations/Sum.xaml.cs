using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace OpenSTSM.Guis.BlockParameters.MathOperations
{
    /// <summary>
    /// Interaction logic for Sum.xaml
    /// </summary>
    public partial class Sum : UserControl, INotifyPropertyChanged
    {
        private string _listOfSigns;
        public string ListOfSigns
        {
            get
            {
                return _listOfSigns;
            }
            set
            {
                _listOfSigns = value;
                OnPropertyChanged();
            }
        }

        public Sum()
        {
            _listOfSigns = "++";

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
