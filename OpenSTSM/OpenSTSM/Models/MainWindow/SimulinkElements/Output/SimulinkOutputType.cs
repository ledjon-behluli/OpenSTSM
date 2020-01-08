using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkOutputType
    {
        [SimulinkElementProperties("Scope", 1, 0)]
        Scope,

        [SimulinkElementProperties("Display", 1, 0)]
        Display
    }
}
