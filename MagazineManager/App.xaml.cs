using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MagazineManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private LoginWindow loginWindow = null;
        private MainWindow mainWindow = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            loginWindow = new LoginWindow();
            loginWindow.Show();

            loginWindow.LoginEvent += OnUserLoggedIn;
        }

        private void OnUserLoggedIn(object sender, EventArgs e)
        {
            if(mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.LogoutEvent += OnUserLoggedOut;
            }

            loginWindow.Hide();
            mainWindow.Show();
        }

        private void OnUserLoggedOut(object sender, EventArgs e)
        {
            mainWindow.Hide();
            loginWindow.Show();
        }

    }
}
