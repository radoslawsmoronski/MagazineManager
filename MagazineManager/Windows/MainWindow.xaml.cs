using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MagazinePage magazinePage = new MagazinePage();
        SettingsPage settingsPage = new SettingsPage();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(magazinePage);
        }

        public void refreshData()
        {
            User user = UsersCollection.GetUserFromLogin(CurrentUser.Login);

            userTextBlock.Text = $"{user.Name} {user.Surname}\n{user.Position}";
        }

        private void logoutButtonClick(object sender, RoutedEventArgs e)
        {
            UserLogoutEvent();
        }

        public delegate void LogoutEventHandler(object sender, EventArgs e);
        public event LogoutEventHandler LogoutEvent;

        protected virtual void UserLogoutEvent()
        {
            if (LogoutEvent != null && CurrentUser.IsLoggedIn)
            {
                CurrentUser.logout();
                LogoutEvent(this, EventArgs.Empty);
            }

        }
        private void changeMainPageClick(object sender, RoutedEventArgs e)
        {
            if(sender == magazineButton)
            {
                MainFrame.Navigate(magazinePage);
            }
            else if(sender == settingsButton)
            {
                MainFrame.Navigate(settingsPage);
            }
        }

    }
}
