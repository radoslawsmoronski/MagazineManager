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
        public MainWindow()
        {
            InitializeComponent();
            userTextBlock.Text = "Zalogowano na konto: " + User.Login;
        }

        private void logoutButtonClick(object sender, RoutedEventArgs e)
        {
            UserLogoutEvent();
        }

        public delegate void LogoutEventHandler(object sender, EventArgs e);
        public event LogoutEventHandler LogoutEvent;

        protected virtual void UserLogoutEvent()
        {
            if (LogoutEvent != null && User.IsLoggedIn)
            {
                User.logout();
                LogoutEvent(this, EventArgs.Empty);
            }
        }
    }
}
