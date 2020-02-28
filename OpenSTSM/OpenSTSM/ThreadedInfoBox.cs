using OpenSTSM.Guis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace OpenSTSM
{  
    public class ThreadedInfoBox
    {
        public string Title;
        private InfoBox infoBox;
        private readonly WindowStartupLocation WindowStartupLocation;
        private readonly int Left;
        private readonly int Top;
        public Action<string> DisplayTextChanged;
        public Action Canceled;
        private static object _lock = new object();

        public ThreadedInfoBox() : this(WindowStartupLocation.CenterScreen, 0, 0) { }
        public ThreadedInfoBox(WindowStartupLocation _windowStartupLocation, int _left, int _top)
        {
            this.WindowStartupLocation = _windowStartupLocation;
            this.Left = _left;
            this.Top = _top;
        }

        public void Start(string DisplayText, string Title)
        {
            Thread newInfoBoxThread = new Thread(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        this.Title = Title;
                        infoBox = new InfoBox(this, DisplayText)
                        {
                            WindowStartupLocation = WindowStartupLocation,
                            Left = Left,
                            Top = Top
                        };
                        infoBox.Closed += (s, args) => Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                        infoBox.Show();

                        Dispatcher.Run();
                    }
                    catch { }
                }
            });
            newInfoBoxThread.SetApartmentState(ApartmentState.STA);
            newInfoBoxThread.IsBackground = true;
            newInfoBoxThread.Start();
        }

        public void Close()
        {
            if (infoBox != null)
                try { infoBox.Dispatcher.BeginInvoke((Action)infoBox.Close); }
                catch { }
        }
    }    
}
