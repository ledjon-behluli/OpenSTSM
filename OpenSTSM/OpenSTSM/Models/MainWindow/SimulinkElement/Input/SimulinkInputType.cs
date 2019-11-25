using OpenSTSM.Attributes;
using System.Windows;

namespace OpenSTSM.Models.MainWindow.SimulinkElement.Input
{
    public enum SimulinkInputType
    {
        [SimulinkElementProperties("Const", 0, 1)]
        Constant,

        [SimulinkElementProperties("Step", 0, 1)]
        Step,

        [SimulinkElementProperties("Ramp", 0, 1)]
        Ramp
    }
}
