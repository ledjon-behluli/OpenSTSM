using Prism.Events;
using System;
using OpenSTSM.Models.MainWindow.SimulinkElements;
using System.Windows;
using OpenSTSM.Guis.BlockParameters.Sources;
using System.Collections.Generic;
using System.Collections.Specialized;
using OpenSTSM.Guis.BlockParameters.MathOperations;
using OpenSTSM.Guis.BlockParameters.Continuous;

namespace OpenSTSM
{
    public class PreferencesUpdatedEvent : PubSubEvent
    {

    }
    
    public class SimulinkElementChosenEvent : PubSubEvent<SimulinkElementChosenPayload>
    {

    }

    public class SimulinkElementChosenPayload
    {
        public object SimulinkNodeElement { get; private set; }
        

        public SimulinkElementChosenPayload(FrameworkElement element) 
        {
            this.SetPayload(element);
        }

        public SimulinkElementChosenPayload(string elementName)
        {
            this.SetPayload(new FrameworkElement() { Name = elementName });
        }

        private void SetPayload(FrameworkElement element)
        {
            object simulinkNodeElement;            

            switch (element.Name)
            {
                case "Constant":
                    {
                        Constant constant = (element as Constant);
                        simulinkNodeElement = new InputElement(SimulinkInputType.Constant);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(constant.Value), constant.Value);
                    }
                    break;
                case "Step":
                    {
                        Step step = (element as Step);
                        simulinkNodeElement = new InputElement(SimulinkInputType.Step);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.StepTime), step.StepTime);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.InitialValue), step.InitialValue);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.FinalValue), step.FinalValue);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.SampleTime), step.SampleTime);
                    }
                    break;
                case "Ramp":
                    {
                        Ramp ramp = (element as Ramp);
                        simulinkNodeElement = new InputElement(SimulinkInputType.Ramp);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(ramp.Slope), ramp.Slope);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(ramp.StartTime), ramp.StartTime);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(ramp.InitialOutput), ramp.InitialOutput);
                    }
                    break;
                case "Scope":
                    simulinkNodeElement = new OutputElement(SimulinkOutputType.Scope);
                    break;
                case "Display":
                    simulinkNodeElement = new OutputElement(SimulinkOutputType.Display);
                    break;
                case "TransferFunction":
                    {
                        TransferFunction transferFunction = (element as TransferFunction);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.TransferFunction);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(transferFunction.NumeratorCoefficients), transferFunction.NumeratorCoefficients);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(transferFunction.DenominatorCoefficients), transferFunction.DenominatorCoefficients);
                    }
                    break;
                case "PidController":
                    {
                        PidController pidController = (element as PidController);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.PidController);
                        // Selected Controller
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.SelectedPidController), pidController.SelectedPidController);
                        // Controller Parameters
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.Proportional), pidController.Proportional);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.Integral), pidController.Integral);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.Derivative), pidController.Derivative);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.FilterCoefficient), pidController.FilterCoefficient);
                        // Controller Initial Conditions
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.Integrator), pidController.Integrator);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(pidController.Filter), pidController.Filter);
                    }
                    break;
                case "Integrator":
                    {
                        Integrator integrator = (element as Integrator);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.Integrator);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(integrator.InitialCondition), integrator.InitialCondition);
                    }
                    break;
                case "Sum":
                    {
                        Sum sum = (element as Sum);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.Sum);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(sum.Signs), sum.Signs);
                    }
                    break;
                case "Gain":
                    {
                        Gain gain = (element as Gain);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.Gain);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(gain.Value), gain.Value);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }

            SimulinkNodeElement = simulinkNodeElement;
        }
    }
}
