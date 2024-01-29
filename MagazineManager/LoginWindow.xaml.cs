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
            MessageBox.Show(DatabaseManager.GetPasswordByLogin("admin"));
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            SecureString enteredPassword = PasswordManager.GetSecurePassword(passwordBox);

            if (PasswordManager.VerifyPassword(enteredPassword, "$2a$11$bwDxMmamMnw1cu2mZYbsYOvyExuhZWRJVGtvJHz3mC2b8XckUqzYS"))
            {
                MessageBox.Show("ok");
            }
        }
    }
}

