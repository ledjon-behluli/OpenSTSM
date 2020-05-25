using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public interface ISimulinkNodeElement<T>
    {
        Guid Guid { get; set; }
        string Name { get; }
        int NumberOfInputs { get; set; }
        int NumberOfOutputs { get; set; }
        Point Location { get; set; }
        bool IsFlippedHorizontally { get; set; }
        ListDictionary Properties { get; set; }
        T SimulinkObjectType { get; }
    }

    public interface ISimulinkConnectionElement<T>
    {
        Guid StartNode { get; set; }
        Guid EndNode { get; set; }
        T SimulinkObjectType { get; }
    }
}
