using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

namespace MagazineManager
{
    /// <summary>
    /// Interaction logic for UsersManagerPage.xaml
    /// </summary>
    public partial class UsersManagerPage : Page
    {
        public UsersManagerPage()
        {
            InitializeComponent();
            UsersCollection.LoadUsersFromDatabase();
            userListBox.ItemsSource = UsersCollection.GetUsers();
        }

        private void removeUserButton(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag != null)
            {
                if (int.TryParse(clickedButton.Tag.ToString(), out int userId))
                {
                    User user = UsersCollection.GetUserFromId(userId);
                    User currentUser = UsersCollection.GetUserFromLogin(CurrentUser.Login);

                    if (user != null)
                    {
                        if(currentUser.Hierarchy >= user.Hierarchy)
                        {
                            MessageBox.Show("You do not have permission to delete this user.");
                            return;
                        }

                        MessageBoxResult result = MessageBox.Show($"Are you sure to remove {user.Name} {user.Surname}?", "Removing user..", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                        {
                        }
                        else
                        {
                            if(UserManagement.DeleteUser(user.Login))
                            {
                                MessageBox.Show("Użytkownik został usunięty.");
                                userListBox.ItemsSource = null;
                                userListBox.ItemsSource = UsersCollection.GetUsers();
                            }
                        }
                    }
                }
            }
        }
    }
}
