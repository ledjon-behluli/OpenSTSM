using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkInputType
    {
        [SimulinkElementProperties("Constant", 0, 1)]
        Constant,

        [SimulinkElementProperties("Step", 0, 1)]
        Step,

        [SimulinkElementProperties("Ramp", 0, 1)]
        Ramp
    }
}
