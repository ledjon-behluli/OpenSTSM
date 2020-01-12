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
using OpenSTSM.Extensions;
using OpenSTSM.Guis.BlockParameters.MathOperations;
using System.Collections;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private bool _isProcessing = false;
        public Guid? LastSelectedGuid; 

        #region Properties

        #region TreeView

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

        #region StatusBar

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

        public string SimulinkElementType { get; set; }
        public ICommand OpenSimulinkElementsBrowserCommand { get; set; }

        #endregion

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            ControlSystems = GetControlSystems();
            PopulateControlSystemView();

            ImportImageCommand = new RelayCommand(ImportImage, param => canExecute_ImportImage);
            AnalyseImageCommand = new RelayCommand(AnalyseImage, param => canExecute_AnalyseImage);
            GenerateSimulinkModelCommand = new RelayCommand(GenerateSimulinkModel, param => canExecute_GenerateSimulinkModel);
            DiagramOverviewCommand = new RelayCommand(OpenDiagramOverview);
            OptionsCommand = new RelayCommand(OpenOptions);
            DesignerHelpCommand = new RelayCommand(OpenDesignerHelp);
            AboutCommand = new RelayCommand(OpenAbout);
            OpenSimulinkElementsBrowserCommand = new RelayCommand(OpenSimulinkElementsBrowser);

            eventAggregator.GetEvent<PreferencesUpdatedEvent>().Subscribe(() => ControlSystems = GetControlSystems(), ThreadOption.UIThread);
            eventAggregator.GetEvent<SimulinkElementChosenEvent>().Subscribe(OnSimulinkElementChosen, ThreadOption.UIThread);
        }

       

        #region Main Window

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
            if (!WindowHelper.IsWindowOpen<OverviewWindow>())
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
            if (!WindowHelper.IsWindowOpen<HelpTextWindow>())
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
            if (!WindowHelper.IsWindowOpen<SimulinkElementsBrowserWindow>())
            {
                string elementName = sender as string;
                string identifier = elementName.StringBetweenCharacters('(', ')');
                GlobalVariableManager.LastSelectedElementIdentifer = Guid.Parse(identifier);
                //this.RemoveLinkingNeed(identifier);

                var sebw = new SimulinkElementsBrowserWindow(SketchToSimulinkHelper.GetSimulinkBrowserTabVisibilities(elementName),
                                                             SketchToSimulinkHelper.GetSimulinkBrowserCorrectTabIndex(elementName));
                sebw.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                sebw.Owner = App.Current.MainWindow;
                sebw.Show();
            }
        }

        private void OnSimulinkElementChosen(SimulinkElementChosenPayload payload)
        {
            if (payload.SimulinkNodeElement is ISimulinkNodeElement<SimulinkInputType>)
            {
                var element = (ISimulinkNodeElement<SimulinkInputType>)payload.SimulinkNodeElement;
                element.Location = this.GetCorrectNodeLocation(element.Guid);
                CreateNode(element, true);
            }                
            else if (payload.SimulinkNodeElement is ISimulinkNodeElement<SimulinkInputOutputType>)
            {
                var element = (ISimulinkNodeElement<SimulinkInputOutputType>)payload.SimulinkNodeElement;
                element.Location = this.GetCorrectNodeLocation(element.Guid);
                CreateNode(element, true);
            }
            else if (payload.SimulinkNodeElement is ISimulinkNodeElement<SimulinkOutputType>)
            {
                var element = (ISimulinkNodeElement<SimulinkOutputType>)payload.SimulinkNodeElement;
                element.Location = this.GetCorrectNodeLocation(element.Guid);
                CreateNode(element, true);
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

        private void RemoveLinkingNeed(string identifier)
        {
            if (!string.IsNullOrEmpty(identifier))
            {
                ControlSystems.FirstOrDefault().PredictedControlElements.ForEach(pre =>
                {
                    if (pre.Guid == Guid.Parse(identifier))
                    {
                        pre.NeedsLinking = false;
                        pre.PossibleControlElements.ForEach(poss => poss.NeedsLinking = false);
                    }
                    else
                    {
                        if (pre.PossibleControlElements.Any(poss => poss.Guid == Guid.Parse(identifier)))
                        {
                            pre.NeedsLinking = false;
                            pre.PossibleControlElements.ForEach(poss => poss.NeedsLinking = false);
                        }
                    }
                });
            }
        }

        private List<ControlSystem> GetControlSystems()
        {
            Point pce1 = new Point(100, 100);
            Point pce2 = new Point(200, 100);
            Point pce3 = new Point(300, 100);
            List<ControlSystem> controlSystems = new List<ControlSystem>();
            controlSystems.Add(new ControlSystem("System")
            {
                
                PredictedControlElements = new List<PredictedControlElement>()
                {
                    new PredictedControlElement("Controller", 96.24m, true, pce1)
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Input", 2.14m, true, pce1),
                             new PossibleControlElement("Output", 1.01m, true, pce1),
                             new PossibleControlElement("Process", 0.12m, true, pce1),
                             new PossibleControlElement("Feedback", 0.12m, true, pce1),
                             new PossibleControlElement("Comparator", 0.02m, true, pce1),
                         }
                    },
                    new PredictedControlElement("Output", 96.24m, true, pce3)
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Input", 2.14m, true, pce3),
                             new PossibleControlElement("Controller", 1.01m, true, pce3),
                             new PossibleControlElement("Process", 0.12m, true, pce3),
                             new PossibleControlElement("Feedback", 0.12m, true, pce3),
                             new PossibleControlElement("Comparator", 0.02m, true, pce3),
                         }
                    },
                    new PredictedControlElement("Arrow Right", 62.78m, false, pce2)
                    {
                         PossibleControlElements = new List<PossibleControlElement>()
                         {
                             new PossibleControlElement("Arrow Left", 22.46m, false, pce2),
                             new PossibleControlElement("Arrow Up", 14.76m, false, pce2)
                         }
                    }
                }
            }); ;

            return controlSystems;
        }

        #endregion

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
     
        public NodeViewModel CreateNode<T>(ISimulinkNodeElement<T> simulinkNodeElement, bool centerNode)
        {
            var node = new NodeViewModel(simulinkNodeElement.Name);
            node.X = simulinkNodeElement.Location.X;
            node.Y = simulinkNodeElement.Location.Y;
            node.Properties = simulinkNodeElement.Properties;
            node.Guid = simulinkNodeElement.Guid;

            #region Special case for Sum element

            var simulinkNE = simulinkNodeElement as ISimulinkNodeElement<SimulinkInputOutputType>;
            if (simulinkNE != null)
            {
                if (simulinkNE.SimulinkObjectType == SimulinkInputOutputType.Sum)
                {
                    foreach (DictionaryEntry de in simulinkNE.Properties)
                    {
                        if((string)de.Key == nameof(Sum.Signs))
                        {
                            IEnumerable<string> signs = (IEnumerable<string>)de.Value;
                            signs = signs.RemoveEmpty();
                            if (signs.Count() > 1)
                            {
                                foreach (string sign in (de.Value as IEnumerable<string>).RemoveEmpty())
                                    node.InputConnectors.Add(new ConnectorViewModel(sign.ToString()));

                                goto LineBreakCondition;
                            }
                            else
                            {
                                MessageBox.Show("Comparator needs at least 2 signs!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return null;
                            }
                        }
                    }
                }
            }

            #endregion

            for (int i = 1; i <= simulinkNodeElement.NumberOfInputs; i++)
                node.InputConnectors.Add(new ConnectorViewModel($"In{i}"));

    LineBreakCondition:

            for (int i = 1; i <= simulinkNodeElement.NumberOfOutputs; i++)
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

        private Point GetCorrectNodeLocation(Guid nodeIdentifier)
        {
            Point location = new Point(55, 25);     // Deafult location (top left of nodes view) if its a new node (it does not come from the sketch)
           
            foreach(var pre in ControlSystems.FirstOrDefault().PredictedControlElements)
            {
                if(pre.Guid == nodeIdentifier)
                {
                    location = pre.Location;
                    break;
                }
                else
                {
                    if (pre.PossibleControlElements.Any(poss => poss.Guid == nodeIdentifier))       
                    {
                        location = pre.Location;        // We can take the location of the parent since if a match in guid happens in any of the children, than that said children will have the same location as the parent
                        break;
                    }
                }
            }

            return location;
        }

        public void ArrangeNodes()
        {
            foreach(var node in this.network.Nodes)
            {
                var a = ControlSystems.FirstOrDefault();
                var b = a.PredictedControlElements;
                var c = b.Where(pre => pre.Guid == node.Guid);
                var d = c.FirstOrDefault().Location;
                
                Point location = ControlSystems.FirstOrDefault().PredictedControlElements.Where(pre => pre.Guid == node.Guid).FirstOrDefault().Location;
                node.X = location.X;
                node.Y = location.Y;
            }
        }

        private void PopulateControlSystemView()
        {
            this.Network = new NetworkViewModel();

            //var elements = SimulinkElementsTestData();
            //foreach(var elem in elements)
            //{

            //}



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

        #endregion

    }
}
