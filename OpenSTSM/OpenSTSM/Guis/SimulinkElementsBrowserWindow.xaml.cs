﻿using OpenSTSM.ViewModels.SimulinkElementsBrowser;
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
using System.Windows.Shapes;

namespace OpenSTSM.Guis
{
    public partial class SimulinkElementsBrowserWindow : Window
    {
        public SimulinkElementsBrowserWindow()
        {
            SimulinkElementsBrowserViewModel viewModel = new SimulinkElementsBrowserViewModel(ApplicationService.Instance.EventAggregator);
            DataContext = viewModel;
            viewModel.RequestClose += () => this.Close();

            InitializeComponent();
        }
    }
}