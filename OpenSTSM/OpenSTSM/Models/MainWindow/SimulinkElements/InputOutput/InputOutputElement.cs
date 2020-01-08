using System;
using System.Collections.Specialized;
using System.Windows;
using OpenSTSM.Extensions;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public class InputOutputElement : ISimulinkNodeElement<SimulinkInputOutputType>
    {
        public SimulinkInputOutputType SimulinkObjectType { get; private set; }


        public Guid Guid { get => Guid.NewGuid(); }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }

        public Point Location { get; set; }

        public ListDictionary Properties { get; set; }



        public InputOutputElement(SimulinkInputOutputType simulinkInputOutputType)
        {
            SimulinkElementProperties sep = simulinkInputOutputType.GetSimulinkElementPropertyValues();

            SimulinkObjectType = simulinkInputOutputType;
            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
            Properties = new ListDictionary();
            Location = new Point(0, 0);
        }
    }
}
