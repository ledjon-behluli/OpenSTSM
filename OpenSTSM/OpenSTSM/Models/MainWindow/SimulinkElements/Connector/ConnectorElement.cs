using OpenSTSM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public class ConnectorElement : ISimulinkElement
    {
        public SimulinkConnectorType SimulinkConnectorType { get; private set; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }

        public SimulinkGraphElementType GraphElementType { get; private set; }


        public ConnectorElement(SimulinkConnectorType simulinkConnectorType)
        {
            SimulinkElementProperties sep = simulinkConnectorType.GetSimulinkElementPropertyValues();

            SimulinkConnectorType = simulinkConnectorType;
            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
            GraphElementType = sep.GraphElementType;
        }
    }
}
