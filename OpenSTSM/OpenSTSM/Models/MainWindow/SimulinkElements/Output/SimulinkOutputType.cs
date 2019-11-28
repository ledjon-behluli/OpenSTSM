using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkOutputType
    {
        [SimulinkElementProperties("Scope", 1, 0, SimulinkGraphElementType.Node)]
        Scope,

        [SimulinkElementProperties("Display", 1, 0, SimulinkGraphElementType.Node)]
        Display
    }
}
