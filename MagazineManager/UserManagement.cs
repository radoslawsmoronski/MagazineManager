using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace MagazineManager
{
    public static class UserManagement
    {
        public static string GetHashedPasswordFromLogin(string login)
        {
            string query = "SELECT HashedPassword FROM Users WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@Login", login)
            };

            List<string[]> result = DatabaseManager.GetSqlQueryResults(query, valuesToQuery);

            return DatabaseManager.GetSqlQueryResults(query, valuesToQuery)[0][0];
        }

        public static bool AddUser(string login, SecureString password)
        {
            if (isLoginExist(login)) return false;

            string HashedPassword = PasswordManager.GetHashPassword(password);

            string query = "INSERT INTO Users (Login, HashedPassword) VALUES(@Login, @HashedPassword)";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@Login", login),
                ("@HashedPassword", HashedPassword)
            };

            return DatabaseManager.ExecuteSqlStatement(query, valuesToQuery);
        }

        public static bool isLoginExist(string login)
        {
            string query = "SELECT count(*) FROM Users WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parameters
            {
                ("@Login", login)
            };

            try
            {
                if (int.Parse(DatabaseManager.GetSqlQueryResults(query, valuesToQuery)[0][0]) == 1) return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UserManagement error: " + ex);
            }

            return false;
        }

    }
}
