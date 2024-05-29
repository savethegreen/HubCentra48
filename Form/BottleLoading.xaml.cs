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
            func();
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

        public void func()
        {
            //try
            //{
            //    if (txt_title != null)
            //    {
            //        txt_title.Text = Title;

            //    }
            //    if (txt_system != null)
            //    {
            //        txt_system.Text = System;
            //    }
            //    if (txt_content != null)
            //    {
            //        txt_content.Text = Content;
            //    }
            //    if (txt_cell != null)
            //    {
            //        txt_cell.Text = Cell;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }
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
