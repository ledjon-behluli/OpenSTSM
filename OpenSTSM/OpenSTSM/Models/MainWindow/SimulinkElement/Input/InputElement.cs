using OpenSTSM.Extensions;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElement.Input
{    
    public class InputElement : ISimulinkElement
    {
        public SimulinkInputType SimulinkInputType { get; private set; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }


        public InputElement(SimulinkInputType simulinkInputType)
        {
            SimulinkElementProperties sep = simulinkInputType.GetSimulinkElementPropertyValues();

            SimulinkInputType = simulinkInputType;
            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
        }
    }
}
