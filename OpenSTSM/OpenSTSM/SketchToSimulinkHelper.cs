using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
