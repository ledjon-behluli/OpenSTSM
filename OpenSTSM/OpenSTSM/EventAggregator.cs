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
    
    public class SimulinkElementChosen : PubSubEvent<SimulinkElementChosenPayload>
    {

    }

    public class SimulinkElementChosenPayload
    {
        public ISimulinkElement SimulinkElement { get; private set; }


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
            ISimulinkElement simulinkElement;

            switch (element.Name)
            {
                case "Constant":
                    {
                        Constant constant = (element as Constant);
                        simulinkElement = new InputElement(SimulinkInputType.Constant);
                        simulinkElement.Properties.Add(nameof(constant.Value), constant.Value);
                    }
                    break;
                case "Step":
                    {
                        Step step = (element as Step);
                        simulinkElement = new InputElement(SimulinkInputType.Step);
                        simulinkElement.Properties.Add(nameof(step.StepTime), step.StepTime);
                        simulinkElement.Properties.Add(nameof(step.InitialValue), step.InitialValue);
                        simulinkElement.Properties.Add(nameof(step.FinalValue), step.FinalValue);
                        simulinkElement.Properties.Add(nameof(step.SampleTime), step.SampleTime);
                    }
                    break;
                case "Ramp":
                    {
                        Ramp ramp = (element as Ramp);
                        simulinkElement = new InputElement(SimulinkInputType.Ramp);
                        simulinkElement.Properties.Add(nameof(ramp.Slope), ramp.Slope);
                        simulinkElement.Properties.Add(nameof(ramp.StartTime), ramp.StartTime);
                        simulinkElement.Properties.Add(nameof(ramp.InitialOutput), ramp.InitialOutput);
                    }
                    break;
                case "Scope":
                    simulinkElement = new OutputElement(SimulinkOutputType.Scope);
                    break;
                case "Display":
                    simulinkElement = new OutputElement(SimulinkOutputType.Display);
                    break;
                case "TransferFunction":
                    {
                        TransferFunction transferFunction = (element as TransferFunction);
                        simulinkElement = new InputOutputElement(SimulinkInputOutputType.TransferFunction);
                        simulinkElement.Properties.Add(nameof(transferFunction.NumeratorCoefficients), transferFunction.NumeratorCoefficients);
                        simulinkElement.Properties.Add(nameof(transferFunction.DenominatorCoefficients), transferFunction.DenominatorCoefficients);
                    }
                    break;
                case "PidController":
                    {
                        PidController pidController = (element as PidController);
                        simulinkElement = new InputOutputElement(SimulinkInputOutputType.PidController);
                        // Selected Controller
                        simulinkElement.Properties.Add(nameof(pidController.SelectedPidController), pidController.SelectedPidController);
                        // Controller Parameters
                        simulinkElement.Properties.Add(nameof(pidController.Proportional), pidController.Proportional);
                        simulinkElement.Properties.Add(nameof(pidController.Integral), pidController.Integral);
                        simulinkElement.Properties.Add(nameof(pidController.Derivative), pidController.Derivative);
                        simulinkElement.Properties.Add(nameof(pidController.FilterCoefficient), pidController.FilterCoefficient);
                        // Controller Initial Conditions
                        simulinkElement.Properties.Add(nameof(pidController.Integrator), pidController.Integrator);
                        simulinkElement.Properties.Add(nameof(pidController.Filter), pidController.Filter);
                    }
                    break;
                case "Integrator":
                    {
                        Integrator integrator = (element as Integrator);
                        simulinkElement = new InputOutputElement(SimulinkInputOutputType.Integrator);
                        simulinkElement.Properties.Add(nameof(integrator.InitialCondition), integrator.InitialCondition);
                    }
                    break;
                case "Sum":
                    {
                        Sum sum = (element as Sum);
                        simulinkElement = new InputOutputElement(SimulinkInputOutputType.Sum);
                        simulinkElement.Properties.Add(nameof(sum.Signs), sum.Signs);
                    }
                    break;
                case "Gain":
                    {
                        Gain gain = (element as Gain);
                        simulinkElement = new InputOutputElement(SimulinkInputOutputType.Gain);
                        simulinkElement.Properties.Add(nameof(gain.Value), gain.Value);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }

            SimulinkElement = simulinkElement;
        }
    }
}
