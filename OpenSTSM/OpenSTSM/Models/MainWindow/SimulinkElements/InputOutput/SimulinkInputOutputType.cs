using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{ 
    public enum SimulinkInputOutputType
    {
        [SimulinkElementProperties("Transfer Function", 1, 1)]
        TransferFunction,
        [SimulinkElementProperties("PID Controller", 1, 1)]
        PidController,
        [SimulinkElementProperties("Integrator", 1, 1)]
        Integrator,
        [SimulinkElementProperties("Sum", 1, 1)]
        Sum,
        [SimulinkElementProperties("Gain", 1, 1)]
        Gain
    }
}
