using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OpenSTSM.Guis
{
    /// <summary>
    /// Interaction logic for InfoBox.xaml
    /// </summary>
    public partial class InfoBox : Window
    {
        private ThreadedInfoBox TinfoBox;

        public InfoBox(ThreadedInfoBox _tInfoBox, string DisplayText)
        {
            InitializeComponent();

            this.Title = _tInfoBox.Title;
            TinfoBox = _tInfoBox;
            lblText.Content = DisplayText;

            btnClose.Click += (s, e) => this.Close();
            btnCancel.Click += (s, e) => {
                TinfoBox.Canceled?.Invoke(); 
                this.Close(); 
            };

            TinfoBox.DisplayTextChanged += (text) => this.Dispatcher.BeginInvoke(new Action(() => lblText.Content = text));
        }
    }
}
