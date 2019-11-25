using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using OpenSTSM.ViewModels.MainWindow;

namespace OpenSTSM.Guis
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        public OverviewWindow()
        {
            InitializeComponent();
        }

        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;


        private void overview_Loaded(object sender, RoutedEventArgs e) => Overview.ScaleToFit();

        private void overview_SizeChanged(object sender, SizeChangedEventArgs e) => Overview.ScaleToFit();

        private void overviewZoomRectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var newContentOffsetX = Math.Min(Math.Max(0.0, Canvas.GetLeft(OverviewZoomRectThumb) + e.HorizontalChange), ViewModel.ContentWidth - ViewModel.ContentViewportWidth);
            Canvas.SetLeft(OverviewZoomRectThumb, newContentOffsetX);

            var newContentOffsetY = Math.Min(Math.Max(0.0, Canvas.GetTop(OverviewZoomRectThumb) + e.VerticalChange), ViewModel.ContentHeight - ViewModel.ContentViewportHeight);
            Canvas.SetTop(OverviewZoomRectThumb, newContentOffsetY);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedPoint = e.GetPosition(NetworkControl);
            var newX = clickedPoint.X - (OverviewZoomRectThumb.Width / 2);
            var newY = clickedPoint.Y - (OverviewZoomRectThumb.Height / 2);
            Canvas.SetLeft(OverviewZoomRectThumb, newX);
            Canvas.SetTop(OverviewZoomRectThumb, newY);
        }
    }
}
