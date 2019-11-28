using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class PossibleControlElement : ControlElement
    {
        public PossibleControlElement(string possibleControlElement, decimal probability, bool needsLinking) : base(possibleControlElement, probability, needsLinking)
        {
          
        }            
    }
}
