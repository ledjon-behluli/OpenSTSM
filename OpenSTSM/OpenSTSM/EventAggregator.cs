using Prism.Events;
using System;
using OpenSTSM.Models.MainWindow.SimulinkElements;

namespace OpenSTSM
{
    public class PreferencesUpdatedEvent : PubSubEvent
    {

    }

    //public enum 

    public class SimulinkElementChosen : PubSubEvent<SimulinkElementChosenPayload>
    {

    }

    public class SimulinkElementChosenPayload
    {
        public ISimulinkElement SimulinkElement { get; private set; }

        public SimulinkElementChosenPayload(string elementName)
        {
            ISimulinkElement simulinkElement;

            switch (elementName) 
            {
                case "Constant": 
                    simulinkElement = new InputElement(SimulinkInputType.Constant);
                    break;
                case "Step":
                    simulinkElement = new InputElement(SimulinkInputType.Step);
                    break;
                case "Ramp":
                    simulinkElement = new InputElement(SimulinkInputType.Ramp);
                    break;
                case "Scope":
                    simulinkElement = new OutputElement(SimulinkOutputType.Scope);
                    break;
                case "Display":
                    simulinkElement = new OutputElement(SimulinkOutputType.Display);
                    break;
                case "TransferFunction":
                    simulinkElement = new InputOutputElement(SimulinkInputOutputType.TransferFunction);
                    break;
                case "PidController":
                    simulinkElement = new InputOutputElement(SimulinkInputOutputType.PidController);
                    break;
                case "Integrator":
                    simulinkElement = new InputOutputElement(SimulinkInputOutputType.Integrator);
                    break;
                case "Sum":
                    simulinkElement = new InputOutputElement(SimulinkInputOutputType.Sum);
                    break;
                case "Gain":
                    simulinkElement = new InputOutputElement(SimulinkInputOutputType.Gain);
                    break;
                default: 
                    throw new InvalidOperationException();
            }

            SimulinkElement = simulinkElement;
        }
    }
}
