using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkOutputType
    {
        [SimulinkElementProperties("Scope", 1, 0, false)]
        Scope,

        [SimulinkElementProperties("Display", 1, 0, false)]
        Display
    }
}
