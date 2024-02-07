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
            OtherUserCollection.LoadUsersFromDatabase();
            userListBox.ItemsSource = OtherUserCollection.GetOtherUsers();
        }

        private void removeUserButton(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            int userId = Convert.ToInt32(clickedButton.Tag);

            OtherUser user = OtherUserCollection.GetUserFromId(userId);

            MessageBox.Show(user.Name);

        }
    }
}
