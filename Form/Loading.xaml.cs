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
    /// Loading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Loading : Window
    {
        #region Window
        public Loading()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Ini();
            Timer_Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
        #endregion Window

        #region Initialize
        private ViewModel _viewModel;
        MainWindow mainWindow;
        public void Ini()
        {
            _viewModel = new ViewModel(new View());
            DataContext = _viewModel;
            mainWindow = new MainWindow(_viewModel);
        }
        #endregion Initialize

        #region Timer
        public void Timer_Start()
        {
            Timer_Progress_Start();
        }

        #region Progress
        public DispatcherTimer Timer_Progress = new DispatcherTimer();

        public void Timer_Progress_Start()
        {
            Timer_Progress.Tick += Timer_Progress_Tick;
            Timer_Progress.Interval = TimeSpan.FromMilliseconds(100);
            Timer_Progress.Start();
        }

        public void Timer_Progress_Stop()
        {
            if (Timer_Progress.IsEnabled)
            {
                Timer_Progress.Stop();
            }
        }
        private void Timer_Progress_Tick(object sender, EventArgs e)
        {
            _viewModel.Loading_formLoadingProgress = 100;
            ProgressBar.Value = _viewModel.Loading_formLoadingProgress;
            if (_viewModel.Loading_formLoadingProgress == 100)
            {
                Timer_Progress_Stop();
                mainWindow.Show();
                Close();
            }
        }
        #endregion Progress
        #endregion Timer
    }
}
