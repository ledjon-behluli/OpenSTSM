using OpenSTSM.Attributes;

namespace OpenSTSM.Models.MainWindow.SimulinkElements
{
    public enum SimulinkConnectorType
    {
        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowLeftDown,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowLeftUp,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowRightDown,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowRightUp,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowStraightDown,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowStraightLeft,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowStraightRight,

        [SimulinkElementProperties("", 0, 0, SimulinkGraphElementType.Connection)]
        ArrowStraightUp
    }
}
