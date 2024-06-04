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

namespace HubCentra_A1
{
    /// <summary>
    /// WriteBarcode.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WriteBarcode : Window
    {

        #region window
        private ViewModel _viewModel;
        public WriteBarcode(ViewModel model)
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


        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.WriteBarcode_ID = "";
            _viewModel.WritePatient_ID = "";

            this.Close();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.Equipment_DataWithDB_presenceArray.Any(x => x.alive))
            {

            }
            else
            {
                _viewModel.Barcode_ID = _viewModel.WriteBarcode_ID;
                _viewModel.Patient_ID = _viewModel.WritePatient_ID;
                _viewModel.Barcode_ID_Loading = _viewModel.WriteBarcode_ID;
                _viewModel.Patient_ID_Loading = _viewModel.WritePatient_ID;
            }
            this.Close();
        }
    }
}
