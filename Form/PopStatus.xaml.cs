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
    /// PopStatus.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PopStatus : Window
    {

        #region window
        private ViewModel _viewModel;
        Enum_PopStatus_ButtonEvent List = Enum_PopStatus_ButtonEvent.PopStatus_Error_Index;
        string Title = "";
        string Content = "";
        public PopStatus(ViewModel model, string title, string content, Enum_PopStatus_ButtonEvent list)
        {
            InitializeComponent();
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _viewModel = model;
            DataContext = _viewModel;

            List = list;
            Title = title;
            Content = content;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            func();
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
            try
            {
                if (txt_title != null)
                {
                    txt_title.Text = Title;

                }

                if (txt_content != null)
                {
                    txt_content.Text = Content;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (List == Enum_PopStatus_ButtonEvent.PopStatus_Error_Index)
            {

            }
            else if (List == Enum_PopStatus_ButtonEvent.PopStatus_Positive_Index)
            {

            }
            else
            {

            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
