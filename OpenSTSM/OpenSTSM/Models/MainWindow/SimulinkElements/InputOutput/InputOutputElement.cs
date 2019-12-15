using OpenSTSM.Extensions;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public class InputOutputElement : ISimulinkElement
    {
        public SimulinkInputOutputType SimulinkInputOutputType { get; private set; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }

        public SimulinkGraphElementType GraphElementType { get; private set; }


        public InputOutputElement(SimulinkInputOutputType simulinkInputOutputType)
        {
            SimulinkElementProperties sep = simulinkInputOutputType.GetSimulinkElementPropertyValues();

            SimulinkInputOutputType = simulinkInputOutputType;
            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
            GraphElementType = sep.GraphElementType;
        }
    }
}
