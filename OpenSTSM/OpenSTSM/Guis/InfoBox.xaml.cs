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

            TinfoBox.DisplayTextChanged += (text) => this.Dispatcher.BeginInvoke(new Action(() =>
            {
                lblText.Content = text;

                if (text.Split(':')[0] == "Number of steps traveled")
                {
                    try
                    {
                        btnCancel.IsEnabled = false;
                        img.Margin = new Thickness(28, 10, 20, 35);
                        WpfAnimatedGif.ImageBehavior.SetRepeatBehavior(img, new System.Windows.Media.Animation.RepeatBehavior(1));
                        WpfAnimatedGif.ImageBehavior.SetAnimatedSource(img, new BitmapImage(new Uri("pack://application:,,,/Resources/done.gif")));
                    }
                    catch { }
                }
            }));
        }
    }
}
