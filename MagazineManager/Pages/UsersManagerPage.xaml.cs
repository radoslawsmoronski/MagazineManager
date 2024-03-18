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
        private AddUserWindow addUserWindow = null;
        private EditUserWindow editUserWindow = null;

        public UsersManagerPage()
        {
            InitializeComponent();
            UsersCollection.LoadUsersFromDatabase();
            userListBox.ItemsSource = UsersCollection.GetUsers();
        }

        public void refreshPage()
        {
            UsersCollection.RefreshUsers();
            userListBox.ItemsSource = null;
            userListBox.ItemsSource = UsersCollection.GetUsers();
        }

        private void editUserButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag != null)
            {
                if (int.TryParse(clickedButton.Tag.ToString(), out int userId))
                {
                    User user = UsersCollection.GetUserFromId(userId);
                    User currentUser = UsersCollection.GetUserFromLogin(CurrentUser.Login);

                    if (user != null)
                    {
                        if ((currentUser.Hierarchy >= user.Hierarchy)
                            && CurrentUser.hasPermission("CanDeleteUsers"))
                        {
                            MessageBox.Show("You do not have permission to delete this user.");
                            return;
                        }

                        editUserWindow = new EditUserWindow(user);
                        editUserWindow.OnAccountEditedEvent += OnUserEditedAccount;

                        editUserWindow.Show();
                    }
                }
            }
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
                        if((currentUser.Hierarchy >= user.Hierarchy)
                            && CurrentUser.hasPermission("CanDeleteUsers"))
                        {
                            MessageBox.Show("You do not have permission to delete this user.");
                            return;
                        }

                        MessageBoxResult result = MessageBox.Show($"Are you sure to remove {user.Name} {user.Surname}?",
                            "Removing user..", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.No)
                        {
                        }
                        else
                        {
                            if(UserManagement.DeleteUser(user.Login))
                            {
                                MessageBox.Show("User has been removed.");
                                userListBox.ItemsSource = null;
                                userListBox.ItemsSource = UsersCollection.GetUsers();
                            }
                        }
                    }
                }
            }
        }

        private void userSerachTextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = userSearchTextBox.Text;

            if (filter == null || filter == "")
            {
                userListBox.ItemsSource = null;
                userListBox.ItemsSource = UsersCollection.GetUsers();

                return;
            }

            filter = filter.ToLower();

            string[] name = filter.Split(' ');

            if (name.Length < 2)
            {
                name = new string[] { name[0], "" };
            }

            List<User> searchDirectlyUsersList = new List<User>();
            List<User> searchNotDirectlyUsersList = new List<User>();
            List<User> searchPositionUsersList = new List<User>();

            foreach (User user in UsersCollection.GetUsers())
            {
                bool nameFirst = (name[0] == user.Name.ToLower());
                bool nameSecond = (name[1] == user.Name.ToLower());
                bool surnameFirst = (name[0] == user.Surname.ToLower());
                bool surnameSecond = (name[1] == user.Surname.ToLower());
                bool positionFirst = (name[0] == user.Position.ToLower());

                if ((nameFirst && surnameSecond) || (surnameFirst && nameSecond))
                {
                    searchDirectlyUsersList.Add(user);

                }
                else if ((nameFirst || surnameFirst))
                {
                    searchNotDirectlyUsersList.Add(user);
                }
                else if (positionFirst)
                { 
                    searchPositionUsersList.Add(user);
                }
            }

            userListBox.ItemsSource = null;

            if(searchPositionUsersList.Any())
            {
                userListBox.ItemsSource = searchPositionUsersList;
            }
            else if (searchDirectlyUsersList.Any())
            {
                userListBox.ItemsSource = searchDirectlyUsersList;
            }
            else
            {
                userListBox.ItemsSource = searchNotDirectlyUsersList;
            }

        }

        private void AddUserButtonClick(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.hasPermission("CanAddUsers"))
            {
                MessageBox.Show("You do not have permission to adding user!");
                return;
            }

            addUserWindow = new AddUserWindow();
            addUserWindow.OnAccountCreatedEvent += OnUserAddedAccount;

            addUserWindow.Show();
        }

        private void OnUserAddedAccount(object sender, EventArgs e)
        {
            refreshPage();
        }

        private void OnUserEditedAccount(object sender, EventArgs e)
        {
            refreshPage();
        }
    }
}
