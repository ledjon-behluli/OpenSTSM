using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenSTSM.Extensions;

namespace OpenSTSM.Models.MainWindow.SimulinkElement.Process
{
    public class ProcessElement : ISimulinkElement
    {
        public SimulinkProcessType SimulinkProcessType { get => SimulinkProcessType.Process; }

        public string Name { get; private set; }

        public int NumberOfInputs { get; private set; }

        public int NumberOfOutputs { get; private set; }


        public ProcessElement()
        {

            SimulinkElementProperties sep = SimulinkProcessType.Process.GetSimulinkElementPropertyValues();

            Name = sep.Name;
            NumberOfInputs = sep.NumberOfInputs;
            NumberOfOutputs = sep.NumberOfOutputs;
        }
    }
}
