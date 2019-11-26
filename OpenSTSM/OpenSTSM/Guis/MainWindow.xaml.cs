using System.Windows;
using System.Windows.Input;
using NetworkModel;
using NetworkUI;
using OpenSTSM.Guis;
using OpenSTSM.ViewModels.MainWindow;

namespace OpenSTSM
{
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel(ApplicationService.Instance.EventAggregator);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();


            InitializeComponent();
        }

        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        private void networkControl_ConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var curDragPoint = Mouse.GetPosition(NetworkControl);

            var connection = ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            e.Connection = connection;
        }

        private void networkControl_QueryConnectionFeedback(object sender, QueryConnectionFeedbackEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var draggedOverConnector = (ConnectorViewModel)e.DraggedOverConnector;
            object feedbackIndicator;
            bool connectionOk;

            ViewModel.QueryConnnectionFeedback(draggedOutConnector, draggedOverConnector, out feedbackIndicator, out connectionOk);

            e.FeedbackIndicator = feedbackIndicator;
            e.ConnectionOk = connectionOk;
        }

        private void networkControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
            var curDragPoint = Mouse.GetPosition(NetworkControl);
            var connection = (ConnectionViewModel)e.Connection;
            ViewModel.ConnectionDragging(curDragPoint, connection);
        }

        private void networkControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            ViewModel.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
        }

        private void DeleteSelectedNodes_Executed(object sender, ExecutedRoutedEventArgs e) => ViewModel.DeleteSelectedNodes();

        private void CreateNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!Helper.IsWindowOpen<SimulinkElementsBrowserWindow>())
            {
                var sebw = new SimulinkElementsBrowserWindow();
                sebw.Left = this.Left + this.Width + 5;
                sebw.Top = this.Top;
                sebw.Owner = this;
                sebw.Show();
            }

            //var newNodePosition = Mouse.GetPosition(NetworkControl);
            //ViewModel.CreateNode("New Node!", newNodePosition, true);
        }

        private void DeleteNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var node = (NodeViewModel)e.Parameter;
            ViewModel.DeleteNode(node);
        }

        private void DeleteConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var connection = (ConnectionViewModel)e.Parameter;
            ViewModel.DeleteConnection(connection);
        }

        private void Node_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            var node = (NodeViewModel)element.DataContext;
            node.Size = new Size(element.ActualWidth, element.ActualHeight);
        }
    }
}
