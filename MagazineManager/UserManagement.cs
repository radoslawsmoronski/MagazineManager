using System;
using System.Collections;
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

            if (result.Count > 0)
            {
                return result[0][0];
            }
            else
            {
                return null;
            }
        }

        public static bool AddUser(string login, SecureString password, string name, string surname, string email,
            string position, int hierarchy, bool[] permissions)
        {
            if (User.Login == login || isLoginExist(login)) return false;

            string hashedPassword = PasswordManager.GetHashPassword(password);

            string accountQuery = "INSERT INTO Users (Login, HashedPassword, Name, Surname, Email)" +
                "VALUES(@Login, @HashedPassword, @Name, @Surname, @Email)";

            var valuesToAccountQuery = new (string, dynamic)[] //Parametres
            {
                ("@Login", login),
                ("@HashedPassword", hashedPassword),
                ("@Name", name),
                ("@Surname", surname),
                ("@Email", email)
            };

            if (!DatabaseManager.ExecuteSqlStatement(accountQuery, valuesToAccountQuery)) return false;

            int userId = GetUserId(login);

            string permissionsQuery = "INSERT INTO UsersPermissions" +
                "(UserId, Position, Hierarchy, CanAddUsers, CanDeleteUsers,CanEditUsers)" +
                "VALUES(@UserId, @Position, @Hierarchy, @CanAddUsers, @CanDeleteUsers, @CanEditUsers)";


            var valuesToPermissionsQuery = new (string, dynamic)[] //Parametres
            {
                ("@UserId", userId),
                ("@Position", position),
                ("@Hierarchy", hierarchy),
                ("@CanAddUsers", DatabaseManager.BoolToBit(permissions[0])),
                ("@CanDeleteUsers", DatabaseManager.BoolToBit(permissions[1])),
                ("@CanEditUsers", DatabaseManager.BoolToBit(permissions[2]))
            };

            return DatabaseManager.ExecuteSqlStatement(permissionsQuery, valuesToPermissionsQuery);
        }
        public static bool DeleteUser(string login)
        {
            if (!isLoginExist(login)) return false;

            int userId = GetUserId(login);

            string query = "DELETE FROM UsersPermissions WHERE UserId = @UserId;" +
                "DELETE FROM Users WHERE UserId = @UserId;";

            var valuesToQuery = new (string, dynamic)[] //Parametres
            {
                ("@UserId", userId)
            };

            return DatabaseManager.ExecuteSqlStatement(query, valuesToQuery);
        }
        public static bool EditUser(string type, string login, string editComponent, object value)
        {
            if ((editComponent.ToLower() == "userid" || !isLoginExist(login))
                && (type != "Account" || type != "Permissions")) return false;


            Dictionary<string, string> editTypeConverter = new Dictionary<string, string>
            {
                { "Account", "Users" },
                { "Permissions", "UsersPermissions" }
            };

            string query = $"UPDATE {editTypeConverter[type]} SET {editComponent} = @Value WHERE Login = @Login;";

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

            return (int.Parse(DatabaseManager.GetSqlQueryResults(query, valuesToQuery)[0][0])) > 0;
        }

        public static int GetUserId(string login)
        {
            string query = "SELECT UserId FROM Users WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parameters
            {
                ("@Login", login)
            };

            return int.Parse(DatabaseManager.GetSqlQueryResults(query, valuesToQuery)[0][0]);
        }

        public static List<string[]> GetUsersBasicDetails(string login)
        {
            string query = "SELECT u.UserId, u.Name, u.Surname, up.Position FROM Users u" +
                "JOIN UserPermissions up ON u.UserId = up.UserId WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[] //Parameters
            {
                ("@Login", login)
            };

            return DatabaseManager.GetSqlQueryResults(query, valuesToQuery);
        }

    }
}
