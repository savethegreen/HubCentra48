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
    /// Calculator.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Calculator : Window
    {
        #region window
        private ViewModel _viewModel;
        public Calculator(ViewModel viewModel, string DisplayText)
        {
            InitializeComponent();
            this._viewModel = viewModel;
            DataContext = this._viewModel;
            _viewModel.Calculator_DisplayTextBlock = DisplayText;
        }

        #endregion window

        #region Initialize
        #endregion Initialize


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var buttonText = button.Content.ToString();
                switch (buttonText)
                {
                    case "C":
                        _viewModel.Calculator_DisplayTextBlock = "";
                        break;
                    case "☜":
                        if (_viewModel.Calculator_DisplayTextBlock.Length > 0)
                        {
                            _viewModel.Calculator_DisplayTextBlock = _viewModel.Calculator_DisplayTextBlock.Remove(_viewModel.Calculator_DisplayTextBlock.Length - 1);
                        }
                        break;
                    case "save":
                        this.DialogResult = true;
                        break;
                    case "close":
                        this.Close();
                        break;
                    default:
                        _viewModel.Calculator_DisplayTextBlock += buttonText;
                        break;
                }
            }
        }

    }
}
