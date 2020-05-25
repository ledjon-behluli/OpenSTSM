using Prism.Events;
using System;
using OpenSTSM.Models.MainWindow.SimulinkElements;
using System.Windows;
using OpenSTSM.Guis.BlockParameters.Sources;
using OpenSTSM.Guis.BlockParameters.MathOperations;
using OpenSTSM.Guis.BlockParameters.Continuous;
using OpenSTSM.Extensions;

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

        public SimulinkElementChosenPayload(FrameworkElement element, bool isFlippedHorizontally) 
        {
            this.SetPayload(element, isFlippedHorizontally);
        }

        public SimulinkElementChosenPayload(string elementName)
        {
            this.SetPayload(new FrameworkElement() { Name = elementName }, false);
        }

        private void SetPayload(FrameworkElement element, bool isFlippedHorizontally)
        {
            object simulinkNodeElement;
            Guid? guid = GlobalVariableManager.LastSelectedElementIdentifer;

            switch (element.Name)
            {
                case "Constant":
                    {
                        Constant constant = (element as Constant);
                        simulinkNodeElement = new InputElement(SimulinkInputType.Constant, guid);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(constant.Value), constant.Value);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "Step":
                    {
                        Step step = (element as Step);
                        simulinkNodeElement = new InputElement(SimulinkInputType.Step, guid);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.StepTime), step.StepTime);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.InitialValue), step.InitialValue);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.FinalValue), step.FinalValue);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(step.SampleTime), step.SampleTime);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "Ramp":
                    {
                        Ramp ramp = (element as Ramp);
                        simulinkNodeElement = new InputElement(SimulinkInputType.Ramp, guid);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(ramp.Slope), ramp.Slope);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(ramp.StartTime), ramp.StartTime);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add(nameof(ramp.InitialOutput), ramp.InitialOutput);
                        ((ISimulinkNodeElement<SimulinkInputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "Scope":
                    simulinkNodeElement = new OutputElement(SimulinkOutputType.Scope, guid);
                    break;
                case "Display":
                    simulinkNodeElement = new OutputElement(SimulinkOutputType.Display, guid);
                    break;
                case "TransferFunction":
                    {
                        TransferFunction transferFunction = (element as TransferFunction);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.TransferFunction, guid);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(transferFunction.NumeratorCoefficients), transferFunction.NumeratorCoefficients);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(transferFunction.DenominatorCoefficients), transferFunction.DenominatorCoefficients);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "PidController":
                    {
                        PidController pidController = (element as PidController);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.PidController, guid);
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
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "Integrator":
                    {
                        Integrator integrator = (element as Integrator);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.Integrator, guid);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(integrator.InitialCondition), integrator.InitialCondition);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "Sum":
                    {
                        Sum sum = (element as Sum);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.Sum, guid);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).NumberOfInputs = sum.Signs.CountNonEmpty();
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(sum.Signs), sum.Signs);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                case "Gain":
                    {
                        Gain gain = (element as Gain);
                        simulinkNodeElement = new InputOutputElement(SimulinkInputOutputType.Gain, guid);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add(nameof(gain.Value), gain.Value);
                        ((ISimulinkNodeElement<SimulinkInputOutputType>)simulinkNodeElement).Properties.Add("FlipHorizontally", isFlippedHorizontally);
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
            
            SimulinkNodeElement = simulinkNodeElement;
        }
    }
}
