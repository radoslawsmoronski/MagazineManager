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

            return DatabaseManager.GetSingleResultFromDB(query, valuesToQuery);
        }
    }
}
