using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenSTSM.ViewModels.About
{
    public class AboutViewModel : WorkspaceViewModel
    {   
        public ICommand RequestNavigateCommand { get; set; }     

        public AboutViewModel()
        {
            RequestNavigateCommand = new RelayCommand(RequestNavigate);     
        }

        private void RequestNavigate(object sender)
        {
            Process.Start(new ProcessStartInfo(Settings.Default.GitHubUri));           
        }
    }
}
