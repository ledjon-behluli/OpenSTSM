using OpenSTSM.Extensions;
using OpenSTSM.ViewModels.MainWindow;
using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System.Linq;
using System.Windows;

namespace OpenSTSM
{
    public static class SketchToSimulinkHelper
    {
        public static int GetSimulinkBrowserCorrectTabIndex(string sketchElementName)
        {
            switch (sketchElementName)
            {
                case "Input": 
                    return 0;
                case "Output":
                    return 1;
                case "Controller":
                case "Process":                
                    return 2;
                case "Comparator":
                case "Feedback":
                    return 3;
                default: 
                    return 0;
            }
        }

        public static SimulinkBrowserTabVisibilities GetSimulinkBrowserTabVisibilities(string sketchElementName)
        {
            switch (sketchElementName)
            {
                case "Input": 
                    return new SimulinkBrowserTabVisibilities(sourcesVisibility: true);
                case "Output": 
                    return new SimulinkBrowserTabVisibilities(sinksVisibility: true);
                case "Controller":
                case "Process":
                    return new SimulinkBrowserTabVisibilities(continuousVisibility: true);
                case "Comparator":
                    return new SimulinkBrowserTabVisibilities(mathOperationsVisibility: true);
                case "Feedback":
                    return new SimulinkBrowserTabVisibilities(continuousVisibility: true, mathOperationsVisibility:true);
                default: 
                    return new SimulinkBrowserTabVisibilities(true, true, true, true);
            }
        }        
    }
}
