using System;
using System.Collections.Specialized;
using System.Windows;
using OpenSTSM.Extensions;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{    
    public class InputElement : ISimulinkNodeElement<SimulinkInputType>
    {
        public SimulinkInputType SimulinkObjectType { get; private set; }


        public Guid Guid { get; set; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; set; }

        public int NumberOfOutputs { get; set; }

        public Point Location { get; set; }

        public bool IsFlippedHorizontally { get; set; }

        public ListDictionary Properties { get; set; }        



        public InputElement(SimulinkInputType simulinkInputType, Guid? guid = null)
        {
            SimulinkElementProperties sep = simulinkInputType.GetSimulinkElementPropertyValues();

            SimulinkObjectType = simulinkInputType;
            Guid = guid ?? Guid.NewGuid();
            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
            Properties = new ListDictionary();
            Location = new Point(0, 0);
            IsFlippedHorizontally = sep.IsFlippedHorizontally;
        }
    }
}
