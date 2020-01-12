using System;
using OpenSTSM.Extensions;
using OpenSTSM.ViewModels.SimulinkElementsBrowser;

namespace OpenSTSM
{
    public static class SketchToSimulinkHelper
    {
        public static int GetSimulinkBrowserCorrectTabIndex(string sketchElementName)
        {
            switch (sketchElementName.ToLower().Split(' ')[0])
            {
                case "input": 
                    return 0;
                case "output":
                    return 1;
                case "controller":
                case "process":                
                    return 2;
                case "comparator":
                case "feedback":
                    return 3;
                default: 
                    return 0;
            }
        }

        public static SimulinkBrowserTabVisibilities GetSimulinkBrowserTabVisibilities(string sketchElementName)
        {
            switch (sketchElementName.ToLower().Split(' ')[0])
            {
                case "input": 
                    return new SimulinkBrowserTabVisibilities(sourcesVisibility: true);
                case "output": 
                    return new SimulinkBrowserTabVisibilities(sinksVisibility: true);
                case "controller":
                case "process":
                    return new SimulinkBrowserTabVisibilities(continuousVisibility: true);
                case "comparator":
                    return new SimulinkBrowserTabVisibilities(mathOperationsVisibility: true);
                case "feedback":
                    return new SimulinkBrowserTabVisibilities(continuousVisibility: true, mathOperationsVisibility:true);
                default: 
                    return new SimulinkBrowserTabVisibilities(true, true, true, true);
            }
        }
    }
}
