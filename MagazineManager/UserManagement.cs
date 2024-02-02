using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public static bool AddUser(string login, SecureString password, bool[] permissions)
        {
            if (User.Login == login) return false;
            if (!User.hasPermission("CanAddUsers") || isLoginExist(login)) return false;

            string HashedPassword = PasswordManager.GetHashPassword(password);

            string query = "INSERT INTO Users (Login, HashedPassword, CanAddUsers, CanDeleteUsers, CanEditUsers)" +
                "VALUES(@Login, @HashedPassword,@CanAddUsers, @CanDeleteUsers, @CanEditUsers)";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@Login", login),
                ("@HashedPassword", HashedPassword),
                ("@CanAddUsers", DatabaseManager.BoolToBit(permissions[0])),
                ("@CanDeleteUsers", DatabaseManager.BoolToBit(permissions[1])),
                ("@CanEditUsers", DatabaseManager.BoolToBit(permissions[2]))
            };

            return DatabaseManager.ExecuteSqlStatement(query, valuesToQuery);
        }
        public static bool DeleteUser(string login)
        {
            if (!User.hasPermission("CanDeleteUsers") || !isLoginExist(login)) return false;

            string query = "DELETE FROM Users WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@Login", login)
            };

            return DatabaseManager.ExecuteSqlStatement(query, valuesToQuery);
        }
        public static bool EditUser(string login, string editComponent, object value)
        {
            if (!User.hasPermission("CanEditUsers")
                || editComponent.ToLower() == "id" 
                || !isLoginExist(login)) return false;

            string query = $"UPDATE Users SET {editComponent} = @Value WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@Value", value.ToString()),
                ("@Login", login)
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
