using NetworkModel;
using OpenSTSM.ViewModels.SimulinkElementsBrowser;
using System;
using System.CodeDom;
using System.Collections;
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
        
        public static T GetSketchElementsValue<T>(NodeViewModel node, string key)
        {
            foreach (DictionaryEntry de in node.Properties)
            {
                if ((string)de.Key == key)
                    return (T)de.Value;                                 
            }

            throw new ArgumentException();
        }
    }

    public class NormalizedLocation 
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public NormalizedLocation(NodeViewModel node)
        {
            X = (uint)Math.Abs(node.X);
            Y = (uint)Math.Abs(node.Y);
        }
    }
}
