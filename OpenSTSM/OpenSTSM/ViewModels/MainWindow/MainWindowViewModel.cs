using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows;
using OpenSTSM.Guis;
using Prism.Events;
using NetworkModel;
using System.Diagnostics;
using OpenSTSM.Models.MainWindow.SimulinkElements;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private bool _isProcessing = false;

        #region Properties

        #region "TreeView"

        private List<ControlSystem> _controlSystems;
        public List<ControlSystem> ControlSystems
        {
            get
            {
                return _controlSystems;
            }
            set
            {
                _controlSystems = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region StatusBar"

        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand ImportImageCommand { get; set; }
        private bool canExecute_ImportImage = true;

        public ICommand AnalyseImageCommand { get; set; }
        private bool canExecute_AnalyseImage = false;

        public ICommand GenerateSimulinkModelCommand { get; set; }
        private bool canExecute_GenerateSimulinkModel = false;

        public ICommand DiagramOverviewCommand { get; set; }

        public ICommand OptionsCommand { get; set; }

        public ICommand DesignerHelpCommand { get; set; }

        public ICommand AboutCommand { get; set; }

        public ICommand OpenSimulinkElementsBrowserCommand { get; set; }

        #endregion

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            ControlSystems = GetControlSystems();
            PopulateWithTestData();

            ImportImageCommand = new RelayCommand(ImportImage, param => canExecute_ImportImage);
            AnalyseImageCommand = new RelayCommand(AnalyseImage, param => canExecute_AnalyseImage);
            GenerateSimulinkModelCommand = new RelayCommand(GenerateSimulinkModel, param => canExecute_GenerateSimulinkModel);
            DiagramOverviewCommand = new RelayCommand(OpenDiagramOverview);
            OptionsCommand = new RelayCommand(OpenOptions);
            DesignerHelpCommand = new RelayCommand(OpenDesignerHelp);
            AboutCommand = new RelayCommand(OpenAbout);
            OpenSimulinkElementsBrowserCommand = new RelayCommand(OpenSimulinkElementsBrowser);

            eventAggregator.GetEvent<PreferencesUpdatedEvent>().Subscribe(() => ControlSystems = GetControlSystems(), ThreadOption.UIThread);
        }


        private void ImportImage(object sender)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if (ofd.ShowDialog() == true)
            {
                FileName = ofd.FileName;
                ChangeCanExecute(true, ref canExecute_AnalyseImage);
            }
        }
        
        private void AnalyseImage(object sender)
        {
            _isProcessing = true;
            ChangeCanExecute(false, ref canExecute_ImportImage);
            ChangeCanExecute(false, ref canExecute_AnalyseImage);

            MessageBox.Show("Analysing"); // Some long running code ...            
            
            ChangeCanExecute(true, ref canExecute_AnalyseImage);
            ChangeCanExecute(true, ref canExecute_ImportImage);
            ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
            _isProcessing = false;
        }

        private void GenerateSimulinkModel(object sender)
        {
            _isProcessing = true;
            ChangeCanExecute(false, ref canExecute_ImportImage);
            ChangeCanExecute(false, ref canExecute_AnalyseImage);
            ChangeCanExecute(false, ref canExecute_GenerateSimulinkModel);

            MessageBox.Show("Generating"); // Some long running code ...            
            
            ChangeCanExecute(true, ref canExecute_AnalyseImage);
            ChangeCanExecute(true, ref canExecute_ImportImage);
            ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
            _isProcessing = false;
        }

        private void OpenDiagramOverview(object sender)
        {
            if (!Helper.IsWindowOpen<OverviewWindow>())
            {
                var overviewWindow = new OverviewWindow();
                overviewWindow.Left = App.Current.MainWindow.Left;
                overviewWindow.Top = App.Current.MainWindow.Top + App.Current.MainWindow.Height + 5;
                overviewWindow.Owner = App.Current.MainWindow;
                overviewWindow.DataContext = App.Current.MainWindow.DataContext;
                overviewWindow.Show();
            }
        }

        private void OpenOptions(object sender)
        {
            Guis.Options options = new Guis.Options();
            options.ShowDialog();
        }

        private void OpenAbout(object sender)
        {
            Guis.About about = new Guis.About();
            about.ShowDialog();
        }

        private void OpenDesignerHelp(object sender)
        {
            if (!Helper.IsWindowOpen<HelpTextWindow>())
            {
                var helpTextWindow = new HelpTextWindow();
                helpTextWindow.Left = App.Current.MainWindow.Left + App.Current.MainWindow.Width + 5;                
                helpTextWindow.Top = App.Current.MainWindow.Top;
                helpTextWindow.Owner = App.Current.MainWindow;
                helpTextWindow.Show();
            }
        }

        private void OpenSimulinkElementsBrowser(object sender)
        {
            if (!Helper.IsWindowOpen<SimulinkElementsBrowserWindow>())
            {
                var sebw = new SimulinkElementsBrowserWindow();
                sebw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                sebw.Owner = App.Current.MainWindow;
                sebw.Show();
            }
        }


        public void ChangeCanExecute(bool canExecute, ref bool canExecuteObj)
        {
            canExecuteObj = canExecute;
        }

        public override void Close()
        {
            if (_isProcessing)
            {
                if (MessageBox.Show("Processing is running!\nAre you sure you want to close the program?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    base.Close();
            }
            else
                base.Close();
        }


        private List<ControlSystem> GetControlSystems()
        {
            List<ControlSystem> controlSystems = new List<ControlSystem>();
            controlSystems.Add(new ControlSystem("System")
            {
                PredictedControlElements = new List<PredictedControlElement>()
                {
                    new PredictedControlElement("Controller", 98.34m, true)
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Output", 1.01m, true),
                             new PossibleControlElement("Process", 0.25m, true)
                         }
                    },
                    new PredictedControlElement("Arrow Right", 62.78m, false)
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Arrow Left", 22.46m, false),
                             new PossibleControlElement("Arrow Up", 14.76m, false)
                         }
                    }
                }
            });

            return controlSystems;
        }


        #region NetworkUI

        #region Internal Data Members

        /// <summary>
        /// This is the network that is displayed in the window.
        /// It is the main part of the view-model.
        /// </summary>
        public NetworkViewModel network = null;

        ///
        /// The current scale at which the content is being viewed.
        /// 
        private double contentScale = 1;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetX = 0;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetY = 0;

        ///
        /// The width of the content (in content coordinates).
        /// 
        private double contentWidth = 1000;

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        private double contentHeight = 1000;

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// view-model so that the value can be shared with the overview window.
        /// 
        private double contentViewportWidth = 0;

        ///
        /// The height of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// view-model so that the value can be shared with the overview window.
        /// 
        private double contentViewportHeight = 0;

        #endregion Internal Data Members

        #region Properties

        public NetworkViewModel Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;

                OnPropertyChanged("Network");
            }
        }

        public double ContentScale
        {
            get
            {
                return contentScale;
            }
            set
            {
                contentScale = value;

                OnPropertyChanged();
            }
        }

        public double ContentOffsetX
        {
            get
            {
                return contentOffsetX;
            }
            set
            {
                contentOffsetX = value;

                OnPropertyChanged();
            }
        }
        
        public double ContentOffsetY
        {
            get
            {
                return contentOffsetY;
            }
            set
            {
                contentOffsetY = value;

                OnPropertyChanged();
            }
        }

        public double ContentWidth
        {
            get
            {
                return contentWidth;
            }
            set
            {
                contentWidth = value;

                OnPropertyChanged();
            }
        }

        public double ContentHeight
        {
            get
            {
                return contentHeight;
            }
            set
            {
                contentHeight = value;

                OnPropertyChanged();
            }
        }

        public double ContentViewportWidth
        {
            get
            {
                return contentViewportWidth;
            }
            set
            {
                contentViewportWidth = value;

                OnPropertyChanged();
            }
        }

        public double ContentViewportHeight
        {
            get
            {
                return contentViewportHeight;
            }
            set
            {
                contentViewportHeight = value;

                OnPropertyChanged();
            }
        }

        #endregion
        

        public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
        {
            var connection = new ConnectionViewModel();

            if (draggedOutConnector.Type == ConnectorType.Output)
            {
                connection.SourceConnector = draggedOutConnector;
                connection.DestConnectorHotspot = curDragPoint;
            }
            else
            {
                connection.DestConnector = draggedOutConnector;
                connection.SourceConnectorHotspot = curDragPoint;
            }

            this.Network.Connections.Add(connection);

            return connection;
        }

        public void QueryConnnectionFeedback(ConnectorViewModel draggedOutConnector, ConnectorViewModel draggedOverConnector, out object feedbackIndicator, out bool connectionOk)
        {
            if (draggedOutConnector == draggedOverConnector)
            {
                feedbackIndicator = new ConnectionBadIndicator();
                connectionOk = false;
            }
            else
            {
                var sourceConnector = draggedOutConnector;
                var destConnector = draggedOverConnector;

                connectionOk = sourceConnector.ParentNode != destConnector.ParentNode &&
                                 sourceConnector.Type != destConnector.Type;

                if (connectionOk)
                {
                    feedbackIndicator = new ConnectionOkIndicator();
                }
                else
                {
                    feedbackIndicator = new ConnectionBadIndicator();
                }
            }
        }
        
        public void ConnectionDragging(Point curDragPoint, ConnectionViewModel connection)
        {
            if (connection.DestConnector == null)
            {
                connection.DestConnectorHotspot = curDragPoint;
            }
            else
            {
                connection.SourceConnectorHotspot = curDragPoint;
            }
        }

        public void ConnectionDragCompleted(ConnectionViewModel newConnection, ConnectorViewModel connectorDraggedOut, ConnectorViewModel connectorDraggedOver)
        {
            if (connectorDraggedOver == null)
            {
                this.Network.Connections.Remove(newConnection);
                return;
            }

            bool connectionOk = connectorDraggedOut.ParentNode != connectorDraggedOver.ParentNode &&
                                connectorDraggedOut.Type != connectorDraggedOver.Type;

            if (!connectionOk)
            {
                this.Network.Connections.Remove(newConnection);
                return;
            }

            var existingConnection = FindConnection(connectorDraggedOut, connectorDraggedOver);
            if (existingConnection != null)
            {
                this.Network.Connections.Remove(existingConnection);
            }

            if (newConnection.DestConnector == null)
            {
                newConnection.DestConnector = connectorDraggedOver;
            }
            else
            {
                newConnection.SourceConnector = connectorDraggedOver;
            }
        }

        public ConnectionViewModel FindConnection(ConnectorViewModel connector1, ConnectorViewModel connector2)
        {
            Trace.Assert(connector1.Type != connector2.Type);

            var sourceConnector = connector1.Type == ConnectorType.Output ? connector1 : connector2;
            var destConnector = connector1.Type == ConnectorType.Output ? connector2 : connector1;

            foreach (var connection in sourceConnector.AttachedConnections)
            {
                if (connection.DestConnector == destConnector)
                {
                    return connection;
                }
            }

            return null;
        }

        public void DeleteSelectedNodes()
        {
            var nodesCopy = this.Network.Nodes.ToArray();
            foreach (var node in nodesCopy)
            {
                if (node.IsSelected)
                {
                    DeleteNode(node);
                }
            }
        }

        public void DeleteNode(NodeViewModel node)
        {
            this.Network.Connections.RemoveRange(node.AttachedConnections);
            this.Network.Nodes.Remove(node);
        }

        public NodeViewModel CreateNode(string name, Point nodeLocation, bool centerNode)
        {
            var node = new NodeViewModel(name);
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;

            node.InputConnectors.Add(new ConnectorViewModel("In1"));
            node.InputConnectors.Add(new ConnectorViewModel("In2"));
            node.OutputConnectors.Add(new ConnectorViewModel("Out1"));
            node.OutputConnectors.Add(new ConnectorViewModel("Out2"));

            if (centerNode)
            {
                EventHandler<EventArgs> sizeChangedEventHandler = null;
                sizeChangedEventHandler =
                    delegate (object sender, EventArgs e)
                    {
                        node.X -= node.Size.Width / 2;
                        node.Y -= node.Size.Height / 2;                     
                        node.SizeChanged -= sizeChangedEventHandler;
                    };

                node.SizeChanged += sizeChangedEventHandler;
            }

            this.Network.Nodes.Add(node);

            return node;
        }

        public NodeViewModel CreateNode(ISimulinkElement simulinkElement, Point nodeLocation, bool centerNode)
        {
            var node = new NodeViewModel(simulinkElement.Name);
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;            

            for (int i = 1; i <= simulinkElement.NumberOfInputs; i++)
                node.InputConnectors.Add(new ConnectorViewModel($"In{i}"));

            for (int i = 1; i <= simulinkElement.NumberOfOutputs; i++)
                node.OutputConnectors.Add(new ConnectorViewModel($"Out{i}"));


            if (centerNode)
            {
                EventHandler<EventArgs> sizeChangedEventHandler = null;
                sizeChangedEventHandler =
                    delegate (object sender, EventArgs e)
                    {
                        node.X -= node.Size.Width / 2;
                        node.Y -= node.Size.Height / 2;
                        node.SizeChanged -= sizeChangedEventHandler;
                    };

                node.SizeChanged += sizeChangedEventHandler;
            }
          
            this.Network.Nodes.Add(node);

            return node;
        }

        public void DeleteConnection(ConnectionViewModel connection)
        {
            this.Network.Connections.Remove(connection);
        }

        private void PopulateWithTestData()
        {
            this.Network = new NetworkViewModel();

            var elements = SimulinkElementsTestData();
            foreach(var elem in elements)
            {

            }



            //NodeViewModel node1 = CreateNode(new InputElement(SimulinkInputType.Step), new Point(100, 80), false);            
            //NodeViewModel node2 = CreateNode(new ProcessElement(), new Point(350, 80), false);
            //NodeViewModel node3 = CreateNode(new OutputElement(SimulinkOutputType.ScopeWith1Input), new Point(600, 80), false);

            //ConnectionViewModel connection1 = new ConnectionViewModel();
            //connection1.SourceConnector = node1.OutputConnectors[0];
            //connection1.DestConnector = node2.InputConnectors[0];

            //ConnectionViewModel connection2 = new ConnectionViewModel();
            //connection2.SourceConnector = node2.OutputConnectors[0];
            //connection2.DestConnector = node3.InputConnectors[0];

            //this.Network.Connections.AddRange(new ConnectionViewModel[] { connection1, connection2 });

        }

        private List<ISimulinkElement> SimulinkElementsTestData()
        {
            return new List<ISimulinkElement>()
            {
                new InputElement(SimulinkInputType.Step),
                new ConnectorElement(SimulinkConnectorType.ArrowStraightRight),
                new OutputElement(SimulinkOutputType.Scope)
            };
        }

        #endregion

    }
}
