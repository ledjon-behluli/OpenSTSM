using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElement
{
    public interface ISimulinkElement
    {
        string Name { get; }
        int NumberOfInputs { get; }
        int NumberOfOutputs { get; }
    }
}
