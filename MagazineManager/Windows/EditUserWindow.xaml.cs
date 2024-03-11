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

namespace MagazineManager
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        User user = null;
        public EditUserWindow(User user_)
        {
            InitializeComponent();

            user = user_;

            editUserLoginTextBox.Text = user.Login;
            editUserNameTextBox.Text = user.Name; 
            editUserSurnameTextBox.Text = user.Surname;
            editUserEmailTextBox.Text = user.Email;
            editUserPositionTextBox.Text = user.Position;
            editUserHierarchyTextBox.Text = user.Hierarchy.ToString();
            editUserAddingUsersCheckBox.IsChecked = user.CanAddUsers;
            editUserDeleteUsersCheckBox.IsChecked = user.CanDeleteUsers;
            editUserEditingUsersCheckBox.IsChecked = user.CanEditUsers;


        }

        private void EditUserButton(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.hasPermission("CanEditUsers"))
            {
                MessageBox.Show("You do not have permission to editing user.");
                this.Close();
                return;
            }

            if(editUserLoginTextBox.Text == user.Login
            && editUserNameTextBox.Text == user.Name
            && editUserSurnameTextBox.Text == user.Surname
            && editUserEmailTextBox.Text == user.Email
            && editUserPositionTextBox.Text == user.Position
            && editUserHierarchyTextBox.Text == user.Hierarchy.ToString()
            && editUserAddingUsersCheckBox.IsChecked == user.CanAddUsers
            && editUserDeleteUsersCheckBox.IsChecked == user.CanDeleteUsers
            && editUserEditingUsersCheckBox.IsChecked == user.CanEditUsers)
            {
                MessageBox.Show("You haven't changed any value.");
                return;
            }
        }
    }
}
