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
        private LoginWindow loginWindow;
        private MainWindow mainWindow;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            loginWindow = new LoginWindow();
            loginWindow.Show();

            loginWindow.LoginEvent += LoginEventHandlerMethod;
        }

        private void LoginEventHandlerMethod(object sender, EventArgs e)
        {
            loginWindow.Hide();
            
            mainWindow = new MainWindow();
            mainWindow.Show();
        }

    }
}
