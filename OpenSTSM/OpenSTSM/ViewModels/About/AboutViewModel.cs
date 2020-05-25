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
        public ICommand GithubUrlCommand { get; set; }     
        public ICommand LinkedInUrlCommand { get; set; }

        public AboutViewModel()
        {
            GithubUrlCommand = new RelayCommand(GithubUrl);
            LinkedInUrlCommand = new RelayCommand(LinkedInUrl);
        }

        private void GithubUrl(object sender)
        {
            Process.Start(new ProcessStartInfo(Settings.Default.GitHubUri));           
        }

        private void LinkedInUrl(object sender)
        {
            Process.Start(new ProcessStartInfo(Settings.Default.LinkedInUrl));
        }
    }
}
