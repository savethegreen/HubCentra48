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
using static HubCentra_A1.EnumManager;
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

        private void Password_Save_Click(object sender, RoutedEventArgs e)
        {
            if (lbList.SelectedItem is DatabaseManager_Login selectedItem) // Replace 'YourItemType' with the actual type of the items
            {
                string selectedUserId = selectedItem.User_Id; // Replace 'user_id' with the actual property name
                ValidateAndAssignUsers(selectedUserId);
            }
            else
            {
                // Handle the case where no item is selected
            }
        }

        public void ValidateAndAssignUsers(string selectedUserId)
        {
            string str_PasswordChange_OLDPASSWORD = PasswordChange_OLDPASSWORD.Password; // Get the entered password
            string str_PasswordChange_NEWPASSWORD1 = PasswordChange_NEWPASSWORD1.Password; // Get the entered password
            string str_PasswordChange_NEWPASSWORD2 = PasswordChange_NEWPASSWORD2.Password; // Get the entered password



            var usersList = _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Login].Select_LoginInfo(); // Load users from DB. Replace 'YourTableName' with the actual table name.

            var user = usersList.FirstOrDefault(u => u.User_Id == selectedUserId); // Find the user with the selected user ID.


            if (user != null && CheckPassword(user, str_PasswordChange_OLDPASSWORD)) // Check if user exists and password matches. You need to implement the CheckPassword method.
            {

                if (str_PasswordChange_NEWPASSWORD1 == str_PasswordChange_NEWPASSWORD2)
                {

                    string updateRestQuery = "UPDATE Login SET user_password = @NewPassword WHERE user_id = @UserId";
                    Dictionary<string, object> restParameters = new Dictionary<string, object>
                                    {
                                        { "@NewPassword", str_PasswordChange_NEWPASSWORD1 },
                                        { "@UserId", selectedUserId },

                                    };
                    _viewModel.databaseManagercs[(int)Enum_DatabaseManager.Login].UpdateLogin(updateRestQuery, restParameters);


                    // Optionally, you can immediately execute the queued update
                    MessageBox.Show("새 비밀번호가 변경 되엇습니다..");
                }
                else
                {
                    MessageBox.Show("새 비밀번호가 일치하지 않습니다.");
                }
            }
            else
            {
                MessageBox.Show("암호가 옳바르지 않습니다");
                // Handle the case where the password does not match or user is not found
            }
        }
        private bool CheckPassword(DatabaseManager_Login user, string password)
        {
            // return password == user.user_Password; // Directly comparing plain text passwords
            string hashedInputPassword = ComputeSha256Hash(password);
            string hashedOutputPassword = ComputeSha256Hash(user.User_Password);

            return hashedInputPassword == hashedOutputPassword; // Comparing hashed values
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

    }
}
