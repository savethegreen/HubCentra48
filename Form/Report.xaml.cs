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
using static HubCentra_A1.EnumManager;
using static HubCentra_A1.Model.View;

namespace HubCentra_A1
{
    /// <summary>
    /// Report.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Report : UserControl
    {
        #region window
        public Report()
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
            raws.IsChecked = true;
        }
        #endregion Model

        #region Function 
        private void lv_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (lv.ActualWidth > SystemParameters.VerticalScrollBarWidth)
            {
                var totalAvailableWidth = lv.ActualWidth - SystemParameters.VerticalScrollBarWidth;

                // Set proportional widths
                colID.Width = totalAvailableWidth * 0.05; //
                colBarcode.Width = totalAvailableWidth * 0.12; //
                colPatient.Width = totalAvailableWidth * 0.12; //
                
                colLoading.Width = totalAvailableWidth * 0.16;// 
                colUnloading.Width = totalAvailableWidth * 0.16;// 
                colIncubationTime.Width = totalAvailableWidth * 0.16;//                
                colResult.Width = totalAvailableWidth * 0.1;//
                colPositiveTime.Width = totalAvailableWidth * 0.16;//       
            }
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv.SelectedItem is DatabaseManager_BarcodeList selectedItem)
            {
                string selectedBarcode = selectedItem._Barcode;
                DateTime? selectedloading = selectedItem.Loading;
                DateTime? selectedunloading;
                if (selectedItem.Unloading == null)
                {
                    selectedunloading = DateTime.Now;

                }
                else
                {
                    selectedunloading = selectedItem.Unloading;

                }
                if (selectedloading.HasValue && selectedunloading.HasValue && selectedBarcode != "")
                {
                    _viewModel.Report_DatePicke_Start = selectedloading.Value;
                    _viewModel.Report_DatePicke_End = selectedunloading.Value;
                    _viewModel.Report_Barcode = selectedBarcode;
                }
                else
                {
                    return;
                }

            }
            else
            {
                _viewModel.Report_Barcode = "";
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Model(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                _viewModel.Report_Model = (Enum_Report_Model)Enum.Parse(typeof(Enum_Report_Model), radioButton.Tag.ToString());
            }
        }
        #endregion Function
    }
}
