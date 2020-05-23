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
    /// Interaction logic for Step.xaml
    /// </summary>
    public partial class Step : UserControl, INotifyPropertyChanged
    {
        private double stepTime;
        public double StepTime
        {
            get
            {
                return stepTime;
            }
            set
            {
                stepTime = value;
                OnPropertyChanged();
            }
        }

        private double initialValue;
        public double InitialValue
        {
            get
            {
                return initialValue;
            }
            set
            {
                initialValue = value;
                OnPropertyChanged();
            }
        }

        private double finalValue;
        public double FinalValue
        {
            get
            {
                return finalValue;
            }
            set
            {
                finalValue = value;
                OnPropertyChanged();
            }
        }

        private double sampleTime;
        public double SampleTime
        {
            get
            {
                return sampleTime;
            }
            set
            {
                sampleTime = value;
                OnPropertyChanged();
            }
        }

        public Step()
        {
            stepTime = 1;
            initialValue = 0;
            finalValue = 1;
            sampleTime = 0;

            this.Name = "Step";
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
