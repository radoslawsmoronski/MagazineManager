using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineManager
{
    public static class UserManagement
    {
        public static string GetHashedPasswordFromLogin(string login)
        {
            string query = "SELECT HashedPassword FROM Users WHERE Login = @Login;";

            var valuesToQuery = new (string, dynamic)[]
            {
                ("@Login", login)
            };

            List<string[]> result = DatabaseManager.GetSqlQueryResults(query, valuesToQuery);

            return DatabaseManager.GetSqlQueryResults(query, valuesToQuery)[0][0];
        }

    }
}
