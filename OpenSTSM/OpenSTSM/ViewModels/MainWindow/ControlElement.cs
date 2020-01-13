using System;
using System.Windows;

namespace OpenSTSM.ViewModels.MainWindow
{
    public enum ControlElementType
    {
        Node,
        Connector
    }

    public class ControlElement : ViewModelBase
    {
        public Guid Guid { get; private set; }

        private string _controlElementName;
        public string ControlElementName
        {
            get
            {
                return _controlElementName;
            }
            set
            {
                _controlElementName = value;
                OnPropertyChanged();
            }
        }

        private decimal _probability = 0;    
        public decimal Probability
        {
            get
            {
                return _probability;
            }
            set
            {
                _probability = value;
                OnPropertyChanged();
            }
        }

        private Point _location;
        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        private bool _needsLinking;
        public bool NeedsLinking
        {
            get
            {
                return _needsLinking;
            }
            set
            {
                _needsLinking = value;
                OnPropertyChanged();
            }
        }

        public string Information
        {
            get
            {
                return $"{ControlElementName} ({Probability.ToString()}%)";
            }
        }

        public string Identifier
        {
            get
            {
                return $"{ControlElementName} ({Guid.ToString()})";
            }
        }

        private ControlElementType _type;
        public ControlElementType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        private Guid? _originIdentifier;
        public Guid? OriginIdentifier
        {
            get
            {
                return _originIdentifier; 
            }
            set
            {
                _originIdentifier = value;
                OnPropertyChanged();
            }
        }

        private Guid? _targetIdentifier;
        public Guid? TargetIdentifier
        {
            get
            {
                return _targetIdentifier;
            }
            set
            {
                _targetIdentifier = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="controlElementName"></param>
        /// <param name="probability"></param>
        /// <param name="location"></param>
        /// <param name="origin">The element identifier that represents the origin of the connector</param>
        /// <param name="target">The element identifier that represents the target of the connector</param>
        public ControlElement(ControlElementType type, string controlElementName, decimal probability, Point? location = null, Guid? origin = null, Guid? target = null)
        {
            Guid = Guid.NewGuid();
            _type = type;
            _controlElementName = controlElementName;
            _probability = probability;
            _needsLinking = true;
            _location = location ?? GlobalVariableManager.DefaultNodeLocation;
            _originIdentifier = origin;
            _targetIdentifier = target;
        }
    }
}
