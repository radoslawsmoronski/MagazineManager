using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
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

namespace MagazineManager
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

        private void addUserClick(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.hasPermission("CanAddUsers"))
            {
                MessageBox.Show("You do not have permission to adding user!");
                this.Close();
                return;
            }

            string login;
            SecureString password;
            string name;
            string surname;
            string email;
            string position;
            int hierarchy;
            bool[] permissions = new bool[3];

            if (!CurrentUser.hasPermission("CanAddUsers"))
            {
                MessageBox.Show("You do not have permission to adding user!");
                return;
            }

            try
            {
                login = newUserLoginTextBox.Text.ToString();
                password = PasswordManager.GetSecurePassword(newUserPasswordBox);
                name = newUserNameTextBox.Text.ToString();
                surname = newUserSurnameTextBox.Text.ToString();
                email = newUserEmailTextBox.Text.ToString();
                position = newUserPositionTextBox.Text.ToString();
                hierarchy = int.Parse(newUserHierarchyTextBox.Text);
                permissions[0] = newUserAddingUsersCheckBox.IsChecked.Value;
                permissions[1] = newUserDeleteUsersCheckBox.IsChecked.Value;
                permissions[2] = newUserEditingUsersCheckBox.IsChecked.Value;

            }
            catch (Exception ex)
            {
                MessageBox.Show("The data are/is wrong!");
                return;
            }

            if(UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
            {
                MessageBox.Show("The user has been added succesfully!");
            }
            else
            {
                MessageBox.Show("The data are/is wrong!");
            }
          
        }
    }
}
