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

namespace OpenSTSM.Guis.BlockParameters.Sources
{
    /// <summary>
    /// Interaction logic for Ramp.xaml
    /// </summary>
    public partial class Ramp : UserControl, INotifyPropertyChanged
    {
        private double slope;
        public double Slope
        {
            get
            {
                return slope;
            }
            set
            {
                slope = value;
                OnPropertyChanged();
            }
        }

        private double startTime;
        public double StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged();
            }
        }

        private double initialOutput;
        public double InitialOutput
        {
            get
            {
                return initialOutput;
            }
            set
            {
                initialOutput = value;
                OnPropertyChanged();
            }
        }


        public Ramp()
        {
            slope = 1;
            startTime = 0;
            initialOutput = 0;

            this.Name = "Ramp";
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
