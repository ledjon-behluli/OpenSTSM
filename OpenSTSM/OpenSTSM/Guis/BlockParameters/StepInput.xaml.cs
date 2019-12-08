using System;
using System.Collections.Generic;
using System.Linq;
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

namespace OpenSTSM.Guis.BlockParameters
{
    public partial class StepInput : UserControl
    {
        public decimal StepTime { get; set; } = 1;
        public decimal InitialValue { get; set; } = 0;
        public decimal FinalValue { get; set; } = 1;
        public decimal SampleTime { get; set; } = 0;

        public StepInput()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
