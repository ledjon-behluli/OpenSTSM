using OpenSTSM.ViewModels.About;
using System.Windows;

namespace OpenSTSM.Guis
{
    public partial class About : Window
    {
        public About()
        {
            AboutViewModel viewModel = new AboutViewModel();
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}
