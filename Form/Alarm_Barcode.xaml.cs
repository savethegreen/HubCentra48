using HubCentra_A1.Model;
using System;
using System.Windows;

namespace HubCentra_A1
{
    /// <summary>
    /// Alarm_Barcodexaml.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Alarm_Barcode : Window
    {
        #region window
        private ViewModel _viewModel;
        public event EventHandler<bool?> ClosedEvent;
        public Alarm_Barcode(ViewModel model)
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
