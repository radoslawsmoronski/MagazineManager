using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MagazineManager
{
    public static class User
    {
        public static string Login { get; set; }
        public static bool IsLoggedIn { get; set; }

        public static bool loginUser(string login, SecureString hashedPassword)
        {
            if(!PasswordManager.VerifyUserPassword(login, hashedPassword))
            {
                return false;
            }


            Login = login;
            IsLoggedIn = true;

            return true;
        }

        public static void logoutUser()
        {
            Login = null;
            IsLoggedIn = false;
        }
    }
}
