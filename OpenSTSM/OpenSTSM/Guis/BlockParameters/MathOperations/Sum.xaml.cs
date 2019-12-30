using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace OpenSTSM.Guis.BlockParameters.MathOperations
{
    /// <summary>
    /// Interaction logic for Sum.xaml
    /// </summary>
    public partial class Sum : UserControl, INotifyPropertyChanged
    {      
        private List<string> _signs;
        public List<string> Signs
        {
            get
            {
                return _signs;
            }
            set
            {
                _signs = value;
                OnPropertyChanged();
            }
        }        


        public Sum()
        {
            _signs = new List<string>() { "+", "+" };

            this.Name = "Sum";
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
