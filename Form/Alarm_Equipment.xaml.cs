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
using static HubCentra_A1.EnumManager;
using System.Windows.Threading;

namespace HubCentra_A1
{
    /// <summary>
    /// Alarm_Equipment.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_Equipment : Window
    {
        #region window
        private ViewModel _viewModel;
        public Alarm_Equipment(ViewModel model)
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
                if (_viewModel.FASTECH_Input[(int)Enum_FASTECH_Input.Door].Flag == true)
                {
                    _viewModel.Alarm_Door_Buzzer = false;
                    Timer_Stop();
                    this.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }


        #endregion Timer
    }
}
