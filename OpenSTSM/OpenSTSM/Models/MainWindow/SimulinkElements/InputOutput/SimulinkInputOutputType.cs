using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{ 
    public enum SimulinkInputOutputType
    {
        [SimulinkElementProperties("Transfer Function", 1, 1, SimulinkGraphElementType.Node)]
        TransferFunction,
        [SimulinkElementProperties("PID Controller", 1, 1, SimulinkGraphElementType.Node)]
        PidController,
        [SimulinkElementProperties("Integrator", 1, 1, SimulinkGraphElementType.Node)]
        Integrator,
        [SimulinkElementProperties("Sum", 1, 1, SimulinkGraphElementType.Node)]
        Sum,
        [SimulinkElementProperties("Gain", 1, 1, SimulinkGraphElementType.Node)]
        Gain
    }
}
