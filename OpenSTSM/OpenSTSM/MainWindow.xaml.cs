using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using OpenSTSM.Guis;
using OpenSTSM.Args;

namespace OpenSTSM
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImportImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg"
            };

            if(ofd.ShowDialog() == true)
            {
                lblFileName.Text = ofd.FileName;
            }
        }

        private void RunPrediction(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lblFileName.Text))
            {
                MessageBox.Show("Please import an image!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OpenOptions(object sender, RoutedEventArgs e)
        {
            Options options = new Options();
            options.ShowDialog();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
