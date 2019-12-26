using System.Collections.Specialized;
using OpenSTSM.Extensions;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public class OutputElement : ISimulinkElement
    {
        public SimulinkOutputType SimulinkOutputType { get; private set; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }

        public SimulinkGraphElementType GraphElementType { get; private set; }

        public ListDictionary Properties { get; set; }


        public OutputElement(SimulinkOutputType simulinkOutputType)
        {
            SimulinkElementProperties sep = simulinkOutputType.GetSimulinkElementPropertyValues();

            SimulinkOutputType = simulinkOutputType;
            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
            Properties = new ListDictionary();
            GraphElementType = sep.GraphElementType;
        }
    }
}
