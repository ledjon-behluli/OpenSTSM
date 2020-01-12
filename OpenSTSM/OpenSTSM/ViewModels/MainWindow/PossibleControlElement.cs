using System.Windows;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class PossibleControlElement : ControlElement
    {
        public PossibleControlElement(string possibleControlElement, decimal probability, bool needsLinking, Point location) 
            : base(possibleControlElement, probability, needsLinking, location)
        {
          
        }            
    }
}
