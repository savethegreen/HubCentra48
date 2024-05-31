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
    /// Alarm_Positive_Unloading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_Positive_Unloading : Window
    {
        #region window
        public event EventHandler<Alarm_Positive_UnloadingEventArgs> OKClicked;
        public event EventHandler<Alarm_Positive_UnloadingEventArgs> CancelClicked;
        private ViewModel _viewModel;
        public int _idx = 0;
        public Alarm_Positive_Unloading(ViewModel model)
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
            timer.Interval = TimeSpan.FromMicroseconds(1000);
            timer.Start();
        }

        public void Timer_Stop()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
        }
        private void TimerCallbacks(object sender, EventArgs e)
        {

        }


        #endregion Timer


        private void btn_Popup_Alarm_Click(object sender, RoutedEventArgs e)
        {
            OKClicked?.Invoke(this, new Alarm_Positive_UnloadingEventArgs(_idx));
            this.Close();
        }

        private void btn_Popup_Alarm2_Click(object sender, RoutedEventArgs e)
        {
            CancelClicked?.Invoke(this, new Alarm_Positive_UnloadingEventArgs(_idx));
            this.Close();
        }
    }
    public class Alarm_Positive_UnloadingEventArgs : EventArgs
    {
        public int Idx { get; }

        public Alarm_Positive_UnloadingEventArgs(int idx)
        {
            Idx = idx;
        }
    }
}
