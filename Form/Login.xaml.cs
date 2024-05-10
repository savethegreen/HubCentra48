using HubCentra_A1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using static HubCentra_A1.Model.View;

namespace HubCentra_A1
{
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        #region Window
        private ViewModel _viewModel;

        public Login(ViewModel model)
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

        private void Window_Closed(object sender, EventArgs e)
        {

        }
        #endregion Window

        #region Model

        public void ModelIni2()
        {


        }

        #endregion Model

        private void lbList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lbList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (lbList.ActualWidth > SystemParameters.VerticalScrollBarWidth)
            {
                var totalAvailableWidth = lbList.ActualWidth - SystemParameters.VerticalScrollBarWidth;
                colId.Width = totalAvailableWidth * 0.7;
                collevel.Width = totalAvailableWidth * 0.3;
            }
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void lbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbList.SelectedItem is DatabaseManager_Login selectedUser)
            {
                string userId = selectedUser.User_Id;
                _viewModel.LoginID = userId;
            }
        }

        private void CLOSE_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        public string ValidateAndAssignUser(string selectedUserId)
        {
            try
            {
                string passwordEntered = passwordBox.Password;
                return passwordEntered;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
