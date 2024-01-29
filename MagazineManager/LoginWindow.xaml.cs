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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Security;

namespace MagazineManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            DatabaseManager.CreateConnectionString();
            DatabaseManager.ConnectionTest();
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text.ToString();
            SecureString enteredPassword = PasswordManager.GetSecurePassword(passwordBox);

            if (PasswordManager.VerifyPassword(login, enteredPassword))
            {
                MessageBox.Show("ok");
            }
        }
    }
}

