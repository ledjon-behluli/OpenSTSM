using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkInputType
    {
        [SimulinkElementProperties("Constant", 0, 1, SimulinkGraphElementType.Node)]
        Constant,

        [SimulinkElementProperties("Step", 0, 1, SimulinkGraphElementType.Node)]
        Step,

        [SimulinkElementProperties("Ramp", 0, 1, SimulinkGraphElementType.Node)]
        Ramp
    }
}
