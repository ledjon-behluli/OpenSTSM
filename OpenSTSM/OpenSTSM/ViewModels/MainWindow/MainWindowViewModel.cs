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
using SimulinkModelGenerator.Modeler.Builders;
using OpenSTSM.Guis.BlockParameters.Continuous;
using OpenSTSM.Models.SimulinkBlocks;
using InputType = SimulinkModelGenerator.Modeler.Builders.SystemBlockBuilders.MathOperations.InputType;
using PidControllerType = OpenSTSM.Models.SimulinkBlocks.PidController;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Controls.Primitives;

namespace OpenSTSM.ViewModels.MainWindow
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private bool _isProcessing = false;        
        public Guid? LastSelectedGuid;
        private ImageAnalysisService analysisService;
        private ThreadedInfoBox TinfoBox;

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
            ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
            TinfoBox = new ThreadedInfoBox();
            TinfoBox.Canceled += () => { _isProcessing = false; };

            this.network = new NetworkViewModel();
            PopulateControlSystemsView(null);

            ImportImageCommand = new RelayCommand(ImportImage, param => canExecute_ImportImage);
            AnalyseImageCommand = new RelayCommand(AnalyseImage, param => canExecute_AnalyseImage);
            GenerateSimulinkModelCommand = new RelayCommand(GenerateSimulinkModel, param => canExecute_GenerateSimulinkModel);
            DiagramOverviewCommand = new RelayCommand(OpenDiagramOverview);
            OptionsCommand = new RelayCommand(OpenOptions);
            DesignerHelpCommand = new RelayCommand(OpenDesignerHelp);
            AboutCommand = new RelayCommand(OpenAbout);
            OpenSimulinkElementsBrowserCommand = new RelayCommand(OpenSimulinkElementsBrowser);

            eventAggregator.GetEvent<PreferencesUpdatedEvent>().Subscribe(() => PopulateControlSystemsView(null), ThreadOption.UIThread);
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
            this.Network = new NetworkViewModel();
            PopulateControlSystemsView(null);
            ChangeCanExecute(false, ref canExecute_ImportImage);
            ChangeCanExecute(false, ref canExecute_AnalyseImage);

            if (analysisService == null)
                analysisService = new ImageAnalysisService();

            if (analysisService.LoadModel())
            {
                if (analysisService.ImageDimessionCorrections(FileName))
                {
                    if (analysisService.RunSelectiveSearch())
                    {
                        if (analysisService.RunPrediction())
                        {
                            PopulateControlSystemsView(analysisService.Predictions);
                            ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
                        }
                    }
                }
            }
            
            ChangeCanExecute(true, ref canExecute_AnalyseImage);
            ChangeCanExecute(true, ref canExecute_ImportImage);
            
            _isProcessing = false;
        }

        private void GenerateSimulinkModel(object sender)
        {
            _isProcessing = true;
            //ChangeCanExecute(false, ref canExecute_ImportImage);
            //ChangeCanExecute(false, ref canExecute_AnalyseImage);
            //ChangeCanExecute(false, ref canExecute_GenerateSimulinkModel);           
                  
            if (this.network.Nodes.Count > 0)
            {
                TinfoBox.Start("Generating Simulink Model...", "Information");

                ModelBuilder builder = new ModelBuilder();
                builder.AddControlSystem(cs =>
                {
                    cs.AddSources(source =>
                    {
                        foreach (NodeViewModel node in this.network.Nodes)
                        {                            
                            NormalizedLocation nLoc = new NormalizedLocation(node);
                            string nodeGuid = node.Guid.ToString();

                            if (node.Name == "Constant")
                            {
                                source.AddConstant(c =>
                                {
                                    c.SetValue(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Value"))                                 
                                     .SetPosition(nLoc.X, nLoc.Y)
                                     .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        c.FlipHorizontally();
                                    }
                                });             
                            }
                            else if(node.Name == "Step")
                            {
                                source.AddStep(s =>
                                {
                                    s.SetStepTime(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "StepTime"))
                                     .SetInitialValue(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "InitialValue"))
                                     .SetFinalValue(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "FinalValue"))
                                     .SetSampleTime(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "SampleTime"))
                                     .SetPosition(nLoc.X, nLoc.Y)
                                     .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        s.FlipHorizontally();
                                    }
                                });                     
                            }
                            else if(node.Name == "Ramp")
                            {
                                source.AddRamp(r =>
                                {
                                    r.SetSlope(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Slope"))
                                     .SetInitialOutput(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "InitialOutput"))
                                     .SetStartTime(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "StartTime"))
                                     .SetPosition(nLoc.X, nLoc.Y)
                                     .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        r.FlipHorizontally();
                                    }
                                });                                                              
                            }
                        }
                    });
                    cs.AddSinks(sinks =>
                    {
                        foreach (NodeViewModel node in this.network.Nodes)
                        {
                            NormalizedLocation nLoc = new NormalizedLocation(node);
                            string nodeGuid = node.Guid.ToString();

                            if (node.Name == "Scope")
                            {
                                sinks.AddScope(s => s.SetPosition(nLoc.X, nLoc.Y).WithName(nodeGuid));
                            }
                            else if (node.Name == "Display")
                            {
                                sinks.AddDisplay(d => d.SetPosition(nLoc.X, nLoc.Y).WithName(nodeGuid));
                            }
                        }
                    });
                    cs.AddMathOperations(mo =>
                    {
                        foreach (NodeViewModel node in this.network.Nodes)
                        {
                            NormalizedLocation nLoc = new NormalizedLocation(node);
                            string nodeGuid = node.Guid.ToString();

                            if (node.Name == "Gain")
                            {
                                mo.AddGain(g =>
                                {
                                    g.SetGain(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Value"))
                                     .SetPosition(nLoc.X, nLoc.Y)
                                     .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        g.FlipHorizontally();
                                    }
                                });
                            }
                            else if (node.Name == "Sum")
                            {
                                List<string> signs = SketchToSimulinkHelper.GetSketchElementsValue<List<string>>(node, "Signs");
                                InputType[] inputTypes = new InputType[signs.Count];

                                for (int i = 0; i < signs.Count; i++)
                                {
                                    inputTypes[i] = signs[i] == "+" ? InputType.Plus : InputType.Minus;
                                }

                                mo.AddSum(sum =>
                                {
                                    sum.SetInputs(inputTypes)
                                       .SetPosition(nLoc.X, nLoc.Y)
                                       .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        sum.FlipHorizontally();
                                    }
                                });                            
                            }
                        }
                    });
                    cs.AddContinuous(c =>
                    {
                        foreach (NodeViewModel node in this.network.Nodes)
                        {
                            NormalizedLocation nLoc = new NormalizedLocation(node);
                            string nodeGuid = node.Guid.ToString();

                            if (node.Name == "Integrator")
                            {
                                c.AddIntegrator(i =>
                                {
                                    i.SetInitialCondition(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "InitialCondition"))
                                     .SetPosition(nLoc.X, nLoc.Y)
                                     .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        i.FlipHorizontally();
                                    }
                                });
                            }
                            else if (node.Name == "Transfer Function")
                            {
                                double[] numerator = SketchToSimulinkHelper.GetSketchElementsValue<List<double>>(node, "NumeratorCoefficients").ToArray();
                                double[] denominator = SketchToSimulinkHelper.GetSketchElementsValue<List<double>>(node, "DenominatorCoefficients").ToArray();

                                c.AddTransferFunction(tf =>
                                {
                                    tf.SetNumerator(numerator)
                                      .SetDenominator(denominator)
                                      .SetPosition(nLoc.X, nLoc.Y)
                                      .WithName(nodeGuid);

                                    if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                    {
                                        tf.FlipHorizontally();
                                    }
                                });
                            }
                            else if (node.Name == "PID Controller")
                            {
                                PidControllerType PidType = SketchToSimulinkHelper.GetSketchElementsValue<PidControllerType>(node, "SelectedPidController");
                                switch (PidType.Name)
                                {
                                    case "PID":
                                        {
                                            c.AddPIDController(pid =>
                                            {
                                                pid.SetProportional(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Proportional"))
                                                   .SetIntegral(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Integral"))
                                                   .SetDerivative(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Derivative"))
                                                   .SetFilterCoefficient(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "FilterCoefficient"))
                                                   .SetInitialConditionForIntegrator(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Integrator"))
                                                   .SetInitialConditionForFilter(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Filter"))
                                                   .SetPosition(nLoc.X, nLoc.Y)
                                                   .WithName(nodeGuid);

                                                if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                                {
                                                    pid.FlipHorizontally();
                                                }
                                            });
                                        }
                                        break;
                                    case "PD":
                                        {
                                            c.AddPDController(pd =>
                                            {
                                                pd.SetProportional(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Proportional"))
                                                  .SetDerivative(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Derivative"))
                                                  .SetFilterCoefficient(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "FilterCoefficient"))
                                                  .SetInitialConditionForFilter(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Filter"))
                                                  .SetPosition(nLoc.X, nLoc.Y)
                                                  .WithName(nodeGuid);

                                                if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                                {
                                                    pd.FlipHorizontally();
                                                }
                                            });                                            
                                        }
                                        break;
                                    case "PI":
                                        {
                                            c.AddPIController(pi =>
                                            {
                                                pi.SetProportional(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Proportional"))
                                                  .SetIntegral(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Integral"))
                                                  .SetInitialConditionForIntegrator(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Integrator"))
                                                  .SetPosition(nLoc.X, nLoc.Y)
                                                  .WithName(nodeGuid);

                                                if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                                {
                                                    pi.FlipHorizontally();
                                                }
                                            });
                                        }
                                        break;
                                    case "I":
                                        {
                                            c.AddIController(i =>
                                            {
                                                i.SetIntegral(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Integral"))
                                                 .SetInitialConditionForIntegrator(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Integrator"))
                                                 .SetPosition(nLoc.X, nLoc.Y)
                                                 .WithName(nodeGuid);

                                                if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                                {
                                                    i.FlipHorizontally();
                                                }
                                            });
                                        }
                                        break;
                                    case "P":
                                        {
                                            c.AddPController(p =>
                                            {
                                                p.SetProportional(SketchToSimulinkHelper.GetSketchElementsValue<double>(node, "Proportional"))
                                                 .SetPosition(nLoc.X, nLoc.Y)
                                                 .WithName(nodeGuid);

                                                if (SketchToSimulinkHelper.GetSketchElementsValue<bool>(node, "FlipHorizontally"))
                                                {
                                                    p.FlipHorizontally();
                                                }
                                            });                                           
                                        }
                                        break;
                                }
                            }
                        }
                    });
                    cs.AddConnections(this.network.Nodes.FirstOrDefault()?.Guid.ToString() ?? string.Empty, l =>
                    {
                        if (this.network.Connections.Count > 0)
                        {
                            List<ConnectorViewModel> trackedSourceConnectors = new List<ConnectorViewModel>();
                            List<NodeViewModel> trackedParentNode = new List<NodeViewModel>();

                            foreach (ConnectionViewModel con in this.network.Connections)
                            {
                                if (con.SourceConnector.AttachedConnections.Count == 1)
                                {
                                    if (!trackedSourceConnectors.Contains(con.SourceConnector))
                                    {
                                        if (!trackedParentNode.Contains(con.DestConnector.ParentNode))
                                        {
                                            l.Connect(con.SourceConnector.ParentNode.Guid.ToString(), con.DestConnector.ParentNode.Guid.ToString());
                                            trackedParentNode.Add(con.DestConnector.ParentNode);
                                        }
                                        else
                                        {
                                            int count = trackedParentNode.Where(n => n == con.DestConnector.ParentNode).Count();
                                            l.Connect(con.SourceConnector.ParentNode.Guid.ToString(), con.DestConnector.ParentNode.Guid.ToString(), 1, (uint)count + 1);
                                            trackedParentNode.Add(con.DestConnector.ParentNode);
                                        }
                                    }

                                    trackedSourceConnectors.Add(con.SourceConnector);
                                }
                                else if (con.SourceConnector.AttachedConnections.Count > 1)
                                {                                    
                                    if (!trackedSourceConnectors.Contains(con.SourceConnector))
                                    {
                                        for (int i = 0; i < con.SourceConnector.AttachedConnections.Count; i++)
                                        {
                                            l.Branch(b =>
                                            {
                                                b.Towards(con.SourceConnector.AttachedConnections[i].DestConnector.ParentNode.Guid.ToString());
                                            });
                                        }

                                        trackedSourceConnectors.Add(con.SourceConnector);
                                    }
                                }
                            }
                        }
                    });
                })
                .Build();

                if (_isProcessing)
                    builder.Save(Settings.Default.Simulink_OutputPath);

                TinfoBox.Close();
            }
            else
                MessageBox.Show("Add at least one element to generate a simulink model.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            //ChangeCanExecute(true, ref canExecute_AnalyseImage);
            //ChangeCanExecute(true, ref canExecute_ImportImage);
            //ChangeCanExecute(true, ref canExecute_GenerateSimulinkModel);
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

                string elementActualName = elementName.SplitStringOnCharTakeFirst(' ');
                if(!elementActualName.Contains("Arrow"))
                {
                    var parent = this.GetParentObjectOfPossibleControlElement(Guid.Parse(identifier));
                    if(parent != null)
                    {
                        GlobalVariableManager.LastSelectedElementIdentifer = parent.Guid;
                        (new SimulinkElementsBrowserWindow(SketchToSimulinkHelper.GetSimulinkBrowserTabVisibilities(elementActualName),
                                                             SketchToSimulinkHelper.GetSimulinkBrowserCorrectTabIndex(elementActualName))
                        {
                            WindowStartupLocation = WindowStartupLocation.CenterOwner,
                            Owner = App.Current.MainWindow,
                        }).Show();
                    }
                    else
                    {
                        MessageBox.Show($"No parent found for object with Guid={identifier}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    GlobalVariableManager.LastSelectedElementIdentifer = Guid.Parse(identifier);
                    if (this.LinkNodesWithRespectiveConnector((Guid)GlobalVariableManager.LastSelectedElementIdentifer))
                    {
                        this.RemoveLinkingNeed(GlobalVariableManager.LastSelectedElementIdentifer);
                    }
                    else
                    {
                        GlobalVariableManager.LastSelectedElementIdentifer = null;
                    }
                }                
            }
        }

        private void OnSimulinkElementChosen(SimulinkElementChosenPayload payload)
        {
            this.RemoveLinkingNeed(GlobalVariableManager.LastSelectedElementIdentifer);

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

            App.Current.MainWindow.Activate();
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
                {
                    base.Close();
                }
            }
            else
            {
                base.Close();
            }
        }

        public void TerminateAnalysisServiceConnections()
        {
            if (analysisService != null)
                analysisService.Close();
        }

        private void RemoveLinkingNeed(Guid? identifier)
        {
            if (identifier != null)
            {
                ControlSystems.FirstOrDefault().PredictedControlElements.ForEach(pre =>
                {
                    if (pre.Guid == identifier)
                    {
                        pre.NeedsLinking = false;
                        pre.PossibleControlElements.ForEach(poss => poss.NeedsLinking = false);
                    }
                    else
                    {
                        if (pre.PossibleControlElements.Any(poss => poss.Guid == identifier))
                        {
                            pre.NeedsLinking = false;
                            pre.PossibleControlElements.ForEach(poss => poss.NeedsLinking = false);
                        }
                    }
                });
            }
        }

        private PredictedControlElement GetParentObjectOfPossibleControlElement(Guid guid)
        {
            PredictedControlElement parent = null;
            foreach(var pre in ControlSystems.FirstOrDefault().PredictedControlElements)
            {
                if (pre.Guid == guid)
                {
                    parent = pre;
                }
                else if (pre.PossibleControlElements.Any(pce => pce.Guid == guid))
                {
                    parent = pre;
                }
            }

            return parent;
        }

        private void PopulateControlSystemsView(List<Prediction> predictions)
        {
            ControlSystem system = new ControlSystem("Control System");
            List<ControlSystem> controlSystems = new List<ControlSystem>() { system };

            if (predictions != null)
            {
                IEnumerable<IGrouping<int, Prediction>> predGroups = predictions.GroupBy(p => p.Id);
                foreach (IGrouping<int, Prediction> predGroup in predGroups)
                {
                    Prediction mostProbable = predGroup.MaxBy(g => g.Probability);
                    if (mostProbable.ControlElementType == ControlElementType.Node)
                    {
                        PredictedNodeControlElement pnce = new PredictedNodeControlElement(mostProbable.Name, mostProbable.Probability, mostProbable.Location);                        
                        foreach (var pred in predGroup)
                        {
                            if (pred.UniqueId != mostProbable.UniqueId)
                            {
                                if (pred.ControlElementType == ControlElementType.Node)
                                    pnce.PossibleControlElements.Add(new PossibleNodeControlElement(pred.Name, pred.Probability, pred.Location));
                                else
                                    pnce.PossibleControlElements.Add(new PossibleConnectorControlElement(pred.Name, pred.Probability, Guid.NewGuid(), Guid.NewGuid()));
                            }
                        }
                        system.PredictedControlElements.Add(pnce);
                    }
                }

                foreach (IGrouping<int, Prediction> predGroup in predGroups)
                {
                    Prediction mostProbable = predGroup.MaxBy(g => g.Probability);
                    if(mostProbable.ControlElementType == ControlElementType.Connector)
                    {
                        ConnectorNeighbors neighbors = FindNeighborsForConnector(mostProbable, system);
                        PredictedConnectorControlElement pcce = new PredictedConnectorControlElement(mostProbable.Name, mostProbable.Probability, neighbors.Origin, neighbors.Target);
                        foreach (var pred in predGroup)
                        {
                            if (pred.UniqueId != mostProbable.UniqueId)
                            {
                                if (pred.ControlElementType == ControlElementType.Node)
                                    pcce.PossibleControlElements.Add(new PossibleNodeControlElement(pred.Name, pred.Probability, pred.Location));
                            }
                        }
                        system.PredictedControlElements.Add(pcce);
                    }
                }

                ControlSystems = controlSystems;
            }    
            else
            {
                ControlSystems = ControlSystems ?? controlSystems;
            }
        }

        private ConnectorNeighbors FindNeighborsForConnector(Prediction connectorPrediction, ControlSystem system)
        {
            if (connectorPrediction.ControlElementType != ControlElementType.Connector)
                throw new ArgumentException("Prediction object must be of type 'Connector'");

            ConnectorNeighbors neighbors = new ConnectorNeighbors();

            double closestLeftDiagonal = int.MaxValue, closestRightDiagonal = int.MaxValue;            
            foreach (var node in system.PredictedControlElements.Where(pce => pce.Type == ControlElementType.Node && pce.Location.X < connectorPrediction.X))
            {
                double X = connectorPrediction.Location.X - node.Location.X;
                double Y = connectorPrediction.Location.Y - node.Location.Y;
                double diagonal = Math.Sqrt(X * X + Y * Y);
                if (diagonal < closestLeftDiagonal)
                {
                    closestLeftDiagonal = diagonal;
                    neighbors.Origin = node.Guid;
                }
            }
            foreach (var node in system.PredictedControlElements.Where(pce => pce.Type == ControlElementType.Node && pce.Location.X > connectorPrediction.X))
            {
                double X = connectorPrediction.Location.X - node.Location.X;
                double Y = connectorPrediction.Location.Y - node.Location.Y;
                double diagonal = Math.Sqrt(X * X + Y * Y);
                if (diagonal < closestRightDiagonal)
                {
                    closestRightDiagonal = diagonal;
                    neighbors.Target = node.Guid;
                }
            }

            return neighbors;
        }

        private class ConnectorNeighbors
        {
            public Guid Origin { get; set; }
            public Guid Target { get; set; }
        }

        private List<ControlSystem> GetControlSystems()
        {
            Point pce1 = new Point(100, 100);
            Point pce2 = new Point(200, 100);
            Point pce3 = new Point(300, 100);

            List<ControlSystem> controlSystems = new List<ControlSystem>();
            
            PredictedNodeControlElement pnce1 = new PredictedNodeControlElement("Controller", 96.24m, pce1)
            {
                PossibleControlElements = new List<PossibleControlElement>()
                {
                    new PossibleNodeControlElement("Feedback", 0.12m, pce1),
                    new PossibleNodeControlElement("Comparator", 0.02m, pce1),
                    new PossibleNodeControlElement("Input", 2.14m, pce1),
                    new PossibleNodeControlElement("Output", 1.01m, pce1),
                    new PossibleNodeControlElement("Process", 0.12m, pce1)
                }
            };

            PredictedNodeControlElement pnce2 = new PredictedNodeControlElement("Output", 96.24m, pce3)
            {
                PossibleControlElements = new List<PossibleControlElement>()
                {
                    new PossibleNodeControlElement("Input", 2.14m, pce3),
                    new PossibleNodeControlElement("Controller", 1.01m, pce3),
                    new PossibleNodeControlElement("Process", 0.12m, pce3),
                    new PossibleNodeControlElement("Feedback", 0.12m, pce3),
                    new PossibleNodeControlElement("Comparator", 0.01m, pce3),
                    new PossibleConnectorControlElement("Arrow", 0.01m, Guid.NewGuid(), Guid.NewGuid())
                }
            };

            PredictedConnectorControlElement pcce1 = new PredictedConnectorControlElement("Arrow", 62.78m, pnce1.Guid, pnce2.Guid)
            {
                PossibleControlElements = new List<PossibleControlElement>()
                {
                    new PossibleNodeControlElement("Process", 12.4m, pce2),
                    new PossibleNodeControlElement("Feedback", 12.4m, pce2),
                    new PossibleNodeControlElement("Comparator", 12.4m, pce2)
                }
            };

            controlSystems.Add(new ControlSystem("System")
            {
                PredictedControlElements = new List<PredictedControlElement>()
                {
                    pnce1, pnce2, pcce1
                }
            });

            controlSystems.ForEach(cs => 
            {
                cs.PredictedControlElements.ForEach(pce => pce.PossibleControlElements.Sort());
                cs.PredictedControlElements.Sort();
            });

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
            Point location = GlobalVariableManager.DefaultNodeLocation;

            foreach (var pre in ControlSystems.FirstOrDefault().PredictedControlElements)
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

        private bool LinkNodesWithRespectiveConnector(Guid connectorIdentifier)
        {
            ControlElement connectorElement = null;
            foreach (var pre in ControlSystems.FirstOrDefault().PredictedControlElements)
            {
                if (pre.GetType() == typeof(PredictedConnectorControlElement))
                {
                    if(pre.Guid == connectorIdentifier)
                    {
                        connectorElement = pre;
                        break;
                    }                        
                }
                else
                {
                    bool elementFound = false;
                    foreach(var pce in pre.PossibleControlElements)
                    {
                        if (pce.GetType() == typeof(PossibleConnectorControlElement))
                        {
                            if (pce.Guid == connectorIdentifier)
                            {
                                connectorElement = pce;        // Get parent to retrive the Guid because nodes in the network get associated with the PredictedControlElement (parent of PossibleControlElement's)
                                elementFound = true;
                                break;
                            }
                        }
                    }

                    if (elementFound) 
                        break;
                }
            }

            if(connectorElement != null)
            {
                var A = ControlSystems;
                var originNode = this.Network.Nodes.FirstOrDefault(n => n.Guid == connectorElement.OriginIdentifier);
                var targetNode = this.Network.Nodes.FirstOrDefault(n => n.Guid == connectorElement.TargetIdentifier);

                if (originNode == null)
                {
                    MessageBox.Show("Connectors origin element has not been mappend!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (targetNode == null)
                {
                    MessageBox.Show($"Connectors target element has not been mappend!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }


                ConnectionViewModel connection = new ConnectionViewModel();
               
                connection.SourceConnector = originNode.OutputConnectors.FirstOrDefault(oc => !oc.IsConnectionAttached);        // Connect to the first available connector in the origin node
                connection.DestConnector = targetNode.InputConnectors.FirstOrDefault(oc => !oc.IsConnectionAttached);        // Connect to the first available connector in the target node                              
                this.Network.Connections.Add(connection);

                return true;
            }

            return false;
        }

        #endregion

    }
}
