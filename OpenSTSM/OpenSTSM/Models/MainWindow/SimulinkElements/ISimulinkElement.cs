using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public interface ISimulinkElement
    {
        string Name { get; }
        int NumberOfInputs { get; }
        int NumberOfOutputs { get; }
        ListDictionary Properties { get; set; }
        SimulinkGraphElementType GraphElementType { get; }
    }
}
