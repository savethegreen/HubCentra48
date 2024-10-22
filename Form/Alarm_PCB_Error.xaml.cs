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
    /// Alarm_PCB_Error.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_PCB_Error : Window
    {

        #region window
        private ViewModel _viewModel;
        public event EventHandler<bool?> ClosedEvent;
        public Alarm_PCB_Error(ViewModel model)
        {
            InitializeComponent();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;
            this.Closed += BottleLoading_Closed;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TimerInitialize();
        }
        private void BottleLoading_Closed(object sender, EventArgs e)
        {
            ClosedEvent?.Invoke(this, true); // or false based on your logic
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
            try
            {
                if (_viewModel.FASTECH_IO_Connection_system1 && _viewModel.FASTECH_IO_Connection_system2)
                {
                    Timer_Stop();
                    this.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }


        #endregion Timer

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Timer_Stop();
                this.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
