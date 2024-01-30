using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagazineManager
{
    public static class User
    {
        public static string Login { get; set; }
        public static bool IsLoggedIn { get; set; }

        public static void loginUser(string login)
        {
            if (login == null)
            {
                MessageBox.Show("User error: login equal null");
                Application.Current.Shutdown();
            }

            Login = login;
            IsLoggedIn = true;

        }

        public static void logoutUser()
        {
            Login = null;
            IsLoggedIn = false;
        }
    }
}
