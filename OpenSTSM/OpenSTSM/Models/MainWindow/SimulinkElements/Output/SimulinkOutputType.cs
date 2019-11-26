using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkOutputType
    {
        [SimulinkElementProperties("Scope", 1, 0, SimulinkGraphElementType.Node)]
        ScopeWith1Input,

        [SimulinkElementProperties("Scope", 2, 0, SimulinkGraphElementType.Node)]
        ScopeWith2Inputs,

        [SimulinkElementProperties("Scope", 3, 0, SimulinkGraphElementType.Node)]
        ScopeWith3Inputs
    }
}
