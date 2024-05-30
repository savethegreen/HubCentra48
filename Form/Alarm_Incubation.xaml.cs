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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HubCentra_A1
{
    /// <summary>
    /// Alarm_Incubation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_Incubation : Window
    {
        #region window
        private ViewModel _viewModel;
        public event EventHandler<PopupEventArgs> OKClicked;
        public event EventHandler<PopupEventArgs> CancelClicked;
        public int _idx = 0;
        public Alarm_Incubation(ViewModel model, int idx)
        {
            InitializeComponent();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;
            _idx = idx;

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

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            OKClicked?.Invoke(this, new PopupEventArgs(_idx));
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancelClicked?.Invoke(this, new PopupEventArgs(_idx));
            this.Close();
        }
    }
    public class PopupEventArgs : EventArgs
    {
        public int Idx { get; }

        public PopupEventArgs(int idx)
        {
            Idx = idx;
        }
    }
}
