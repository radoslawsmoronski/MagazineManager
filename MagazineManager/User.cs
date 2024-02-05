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

        public static bool login(string login, SecureString hashedPassword)
        {
            if(!PasswordManager.VerifyUserPassword(login, hashedPassword))
            {
                return false;
            }


            Login = login;
            SetLoggedStatus(true);

            return true;
        }

        public static void logout()
        {
            SetLoggedStatus(false);
            Login = null;
        }

        public static bool SetLoggedStatus(bool status)
        {
            if(UserManagement.EditUser(User.Login, "IsOnline", DatabaseManager.BoolToBit(status)))
            {
                IsLoggedIn = status;
                return true;
            }
            
           return false;
        }

        public static bool hasPermission(string permission)
        {
            if (!UserManagement.isLoginExist(Login))
            {
                MessageBox.Show("Your account has been deleted. Application clossing...");
                Application.Current.Shutdown();
            }

            string query = $"SELECT up.{permission} FROM Users u JOIN UsersPermissions up WHERE u.Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@Login", Login)
            };

            string result = DatabaseManager.GetSqlQueryResults(query, valuesToQuery)[0][0];

            if (result == "True") return true;

            return false;
        }

        //temp functions
        public static void loginUserTemp(string login)
        {
            Login = login;
            SetLoggedStatus(true);
        }
        public static void logoutUserTemp()
        {
            Login = null;
            SetLoggedStatus(false);
        }
        //temp funstions
    }
}
