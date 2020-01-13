using System;
using System.Windows;

namespace OpenSTSM
{
    public static class GlobalVariableManager
    {
        public static Guid? LastSelectedElementIdentifer;
        public static Point DefaultNodeLocation => new Point(55, 25);     // Deafult location (top left of nodes view) if its a new node (it does not come from the sketch)   
    }
}
