using System;
using OpenSTSM.Models.MainWindow.SimulinkElements;

namespace OpenSTSM
{
    public static class SketchToSimulinkHelper
    {
        public static int GetSimulinkBrowserCorrectTabIndex(string sketchElementName)
        {
            switch (sketchElementName.ToLower())
            {
                case "input": return 0;
                case "output": return 1;
                case "controller":
                case "process":                
                    return 2;
                case "comparator":
                case "feedback":
                    return 3;
                default: return 0;
            }
        }
    }
}
