using HubCentra_A1.Model;
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

namespace HubCentra_A1
{
    /// <summary>
    /// Alarm_Negative.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_Negative : Window
    {
        #region window
        private ViewModel _viewModel;
        public event EventHandler<Alarm_Negative_UnloadingEventArgs> OKClicked;
        public event EventHandler<Alarm_Negative_UnloadingEventArgs> CancelClicked;
        public int _idx = 0;
        public Alarm_Negative(ViewModel model, int idx)
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
            try
            {
                timer.Tick += TimerCallbacks;
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public void Timer_Stop()
        {
            try
            {
                if (timer.IsEnabled)
                {
                    timer.Tick -= TimerCallbacks;
                    timer.Stop();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void TimerCallbacks(object sender, EventArgs e)
        {

        }


        #endregion Timer

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OKClicked?.Invoke(this, new Alarm_Negative_UnloadingEventArgs(_idx));
                Timer_Stop();
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CancelClicked?.Invoke(this, new Alarm_Negative_UnloadingEventArgs(_idx));
                Timer_Stop();
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
    public class Alarm_Negative_UnloadingEventArgs : EventArgs
    {
        public int Idx { get; }

        public Alarm_Negative_UnloadingEventArgs(int idx)
        {
            Idx = idx;
        }
    }
}
