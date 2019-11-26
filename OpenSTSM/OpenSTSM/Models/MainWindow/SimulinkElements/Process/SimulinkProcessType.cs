using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkProcessType
    {
        [SimulinkElementProperties("Process", 1, 1, SimulinkGraphElementType.Node)]
        Process
    }
}
