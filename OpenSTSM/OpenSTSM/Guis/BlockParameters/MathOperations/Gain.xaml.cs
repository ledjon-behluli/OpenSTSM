﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace OpenSTSM.Guis.BlockParameters.MathOperations
{
    /// <summary>
    /// Interaction logic for Gain.xaml
    /// </summary>
    public partial class Gain : UserControl, INotifyPropertyChanged
    {
        private decimal _value;
        public decimal Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public Gain()
        {
            _value = 1;

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
