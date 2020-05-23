using System;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public class SimulinkElementProperties
    {
        public string Name { get; set; }
        public int NumberOfInputs { get; set; }
        public int NumberOfOutputs { get; set; }
        public bool IsFlippedHorizontally { get; set; }
    }
}
