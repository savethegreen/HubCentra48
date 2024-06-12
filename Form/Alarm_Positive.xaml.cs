﻿using HubCentra_A1.Model;
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
using System.Windows.Threading;
using static HubCentra_A1.EnumManager;

namespace HubCentra_A1
{
    /// <summary>
    /// Alarm_Positive.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_Positive : Window
    {


        #region window
        private ViewModel _viewModel;
        public int _idx = 0;
        public Alarm_Positive(ViewModel model)
        {
            InitializeComponent();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TimerInitialize();
        }

        #endregion window

        #region Initialize
        public void Initialize()
        {
            ViewModel();
        }

        public void ViewModel()
        {
        }
        #endregion Initialize

        #region Model
        #endregion Model

        #region Timer
        private DispatcherTimer timer = new DispatcherTimer();



        public async void TimerInitialize()
        {
            timer.Tick += TimerCallbacks;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        public void Timer_Stop()
        {
            if (timer.IsEnabled)
            {
                timer.Tick -= TimerCallbacks;
                timer.Stop();
            }
        }
        private void TimerCallbacks(object sender, EventArgs e)
        {
            if(_viewModel.System_PositiveFirstint == -1  || _viewModel.MainWindow_ButtonFlag == Enum_MainWindow_ButtonFlag.SystemRack1 || _viewModel.MainWindow_ButtonFlag == Enum_MainWindow_ButtonFlag.Report || _viewModel.MainWindow_ButtonFlag == Enum_MainWindow_ButtonFlag.Conguration)
            {
                Timer_Stop();
                this.Close();
            }
        }


        #endregion Timer


        private void btn_Popup_Positive_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
