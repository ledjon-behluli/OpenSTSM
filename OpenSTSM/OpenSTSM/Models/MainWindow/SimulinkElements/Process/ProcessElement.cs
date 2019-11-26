using OpenSTSM.Extensions;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{ 
    public class ProcessElement : ISimulinkElement
    {
        public SimulinkProcessType SimulinkProcessType { get => SimulinkProcessType.Process; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }

        public SimulinkGraphElementType GraphElementType { get; private set; }


        public ProcessElement()
        {

            SimulinkElementProperties sep = SimulinkProcessType.Process.GetSimulinkElementPropertyValues();

            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
            GraphElementType = sep.GraphElementType;
        }
    }
}
