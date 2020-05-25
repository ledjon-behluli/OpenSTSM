using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{ 
    public enum SimulinkInputOutputType
    {
        [SimulinkElementProperties("Transfer Function", 1, 1, false)]
        TransferFunction,
        [SimulinkElementProperties("PID Controller", 1, 1, false)]
        PidController,
        [SimulinkElementProperties("Integrator", 1, 1, false)]
        Integrator,
        [SimulinkElementProperties("Sum", 1, 1, false)]
        Sum,
        [SimulinkElementProperties("Gain", 1, 1, false)]
        Gain
    }
}
