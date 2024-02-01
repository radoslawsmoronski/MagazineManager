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

        public static bool SetUserLoginStatus(bool status)
        {
            string query = "UPDATE Users SET IsOnline = @IsOnline WHERE Login = @Login";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@IsOnline", DatabaseManager.BoolToBit(status)),
                ("@Login", Login)
            };

            IsLoggedIn = status;

            return DatabaseManager.ExecuteSqlStatement(query, valuesToQuery);
        }

        //temp functions
        public static void loginUserTemp(string login)
        {
            Login = login;
            SetUserLoginStatus(true);

        }
        public static void logoutUserTemp()
        {
            Login = null;
            SetUserLoginStatus(false);
        }
        //temp funstions
    }
}
