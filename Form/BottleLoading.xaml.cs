using HubCentra_A1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// BottleLoading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BottleLoading : Window
    {
        #region window
        private ViewModel _viewModel;
        public int IDX = 0;
        public BottleLoading(ViewModel model , int idx)
        {
            InitializeComponent();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;
            IDX = idx;
            TimerInitialize();

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
                timer.Tick -= TimerCallbacks;
                timer.Stop();
            }
        }
        private void TimerCallbacks(object sender, EventArgs e)
        {
            if(!_viewModel.BottleLoading_Result[IDX])
                {
                _viewModel.BottleLoading_isPopupOpen = false;
                Timer_Stop();
                Dispatcher.Invoke(() =>
                {
                    this.Close();
                });
            }
        }
        #endregion Timer

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();

            _viewModel.BottleLoading_Result[IDX] = false;
        }

        private void btn_Popup_Alarm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Popup_Alarm2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
