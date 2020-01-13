using System.Windows;
using System.Collections.Generic;
using System;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class PredictedControlElement : ControlElement, IComparable<PredictedControlElement>
    {
        private List<PossibleControlElement> _possibleControlElements;

        public List<PossibleControlElement> PossibleControlElements
        {
            get
            {
                return _possibleControlElements;
            }
            set
            {
                _possibleControlElements = value;
                OnPropertyChanged();
            }
        }

        public PredictedControlElement(ControlElementType type, string predictedControlElementName, decimal probability, Point? location, Guid? origin, Guid? target)
            : base(type, predictedControlElementName, probability, location, origin, target)
        {
          
        }

        public int CompareTo(PredictedControlElement other)
        {
            if(this.Type == ControlElementType.Node && other.Type == ControlElementType.Node)
            {
                return 0;
            }
            else if(this.Type == ControlElementType.Node && other.Type == ControlElementType.Connector)
            {
                return -1;
            }
            else if(this.Type == ControlElementType.Connector && other.Type == ControlElementType.Node)
            {
                return 1;
            }
            else
            {
                return 1;
            }
        }
    }

    public class PredictedNodeControlElement : PredictedControlElement
    {
        public PredictedNodeControlElement(string predictedControlElementName, decimal probability, Point location)
            : base(ControlElementType.Node, predictedControlElementName, probability, location, null, null)
        {

        }
    }

    public class PredictedConnectorControlElement : PredictedControlElement
    {
        public PredictedConnectorControlElement(string predictedControlElementName, decimal probability, Guid? origin, Guid target)
            : base(ControlElementType.Connector, predictedControlElementName, probability, location: null, origin, target)
        {

        }
    }
}
