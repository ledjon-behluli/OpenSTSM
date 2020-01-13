using System;
using System.Windows;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class PossibleControlElement : ControlElement, IComparable<PossibleControlElement>
    {
        public PossibleControlElement(ControlElementType type, string possibleControlElement, decimal probability, Point? location, Guid? origin, Guid? target)
            : base(type, possibleControlElement, probability, location, origin, target)
        {
          
        }

        public int CompareTo(PossibleControlElement other)
        {
            return -1 * decimal.Compare(this.Probability, other.Probability);
        }
    }

    public class PossibleNodeControlElement : PossibleControlElement
    {
        public PossibleNodeControlElement(string possibleControlElement, decimal probability, Point location)
            : base(ControlElementType.Node, possibleControlElement, probability, location, null, null)
        {

        }
    }

    public class PossibleConnectorControlElement : PossibleControlElement
    {
        public PossibleConnectorControlElement(string possibleControlElement, decimal probability, Guid? origin, Guid target)
            : base(ControlElementType.Connector, possibleControlElement, probability, location: null, origin, target)
        {

        }
    }
}
