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
    /// Config.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Config : UserControl
    {
        #region window
        public Config()
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
    }
}
