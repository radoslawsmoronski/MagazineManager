using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
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

            DatabaseManager.CreateConnectionString();
            DatabaseManager.ConnectionTest();

            loginWindow = new LoginWindow();
            loginWindow.Show();

            loginWindow.LoginEvent += OnUserLoggedIn;
            loginWindow.Closed += OnWindowClosed;

            CmdDeveloperTool cmdDeveloper = new CmdDeveloperTool();
        }

        private void OnUserLoggedIn(object sender, EventArgs e)
        {
            if(mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.LogoutEvent += OnUserLoggedOut;
                mainWindow.Closing += OnWindowClosing;
            }

            loginWindow.Hide();

            mainWindow.refreshData();
            mainWindow.Show();
        }

        private void OnUserLoggedOut(object sender, EventArgs e)
        {
            mainWindow.Hide();
            loginWindow.Show();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (sender == loginWindow) 
            {
                Shutdown();
            }
        }

        //Method to application closing support
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender == mainWindow && CurrentUser.IsLoggedIn)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure to close the application?", "Aplication closing..", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Shutdown();
                }

            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (CurrentUser.IsLoggedIn) CurrentUser.SetLoggedStatus(false);

            base.OnExit(e);
        }

    }
}
