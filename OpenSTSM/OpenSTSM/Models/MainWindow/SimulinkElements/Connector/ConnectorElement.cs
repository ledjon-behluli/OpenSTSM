using OpenSTSM.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public class ConnectorElement : ISimulinkConnectionElement<SimulinkConnectorType>
    {
        public SimulinkConnectorType SimulinkObjectType { get; private set; }


        public Guid Guid { get => Guid.NewGuid(); }

        public Guid StartNode { get; set; }

        public Guid EndNode { get; set; }
        

        public ConnectorElement(SimulinkConnectorType simulinkConnectorType)
        {
            SimulinkObjectType = simulinkConnectorType;
        }
    }
}
