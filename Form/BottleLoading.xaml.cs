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
        public event EventHandler<bool?> ClosedEvent;
        public BottleLoading(ViewModel model )
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
            TimerInitialize();
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
        private Timer _timer;
        private readonly object _timer_lock = new object();
        public async void TimerInitialize()
        {
            _timer = new Timer(TimerCallbacks, null, 0, 100);
        }
        private void TimerCallbacks(object state)
        {
            // 타이머 콜백이 호출될 때 수행할 작업을 여기에 작성합니다.
            lock (_timer_lock)
            {
                if (!_viewModel.BottleLoading_Result)
                {
                    Dispatcher.Invoke(() =>
                    {
                        this.Close();
                    });

                    // 타이머 중지
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _timer.Dispose();
                }
            }
        }
        #endregion Timer

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Popup_Alarm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Popup_Alarm2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
