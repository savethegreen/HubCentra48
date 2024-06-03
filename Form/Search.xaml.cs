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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HubCentra_A1
{
    /// <summary>
    /// Search.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Search : UserControl
    {
        #region window
        public Search()
        {
            InitializeComponent();
        }

        #endregion window

        #region Initialize
        #endregion Initialize

        #region Model
        private ViewModel _viewModel;

        public void ViewModel(ViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;
        }
        #endregion Model

        #region Function 
        private void lv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (lv.ActualWidth > SystemParameters.VerticalScrollBarWidth)
            {
                var totalAvailableWidth = lv.ActualWidth - SystemParameters.VerticalScrollBarWidth;

                // Set proportional widths
                colID.Width = totalAvailableWidth * 0.1; //
                colPatient.Width = totalAvailableWidth * 0.2; //

                colBarcode.Width = totalAvailableWidth * 0.2; //
                colLoading.Width = totalAvailableWidth * 0.2;// 
                colIncubationTime.Width = totalAvailableWidth * 0.2;//                
                colResult.Width = totalAvailableWidth * 0.1;//
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion Function
    }
}
