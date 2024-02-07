using MagazineManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class OtherUserCollection
{
    private static List<OtherUser> otherUsers = new List<OtherUser>();

    public static void AddUser(OtherUser user)
    {
        otherUsers.Add(user);
    }

    public static List<OtherUser> GetOtherUsers()
    {
        return otherUsers;
    }

    public static void LoadUsersFromDatabase()
    {
        string query =
         "SELECT u.UserId, u.Login, u.Name, u.Surname, u.Email," +
         " up.Position, up.Hierarchy, up.CanAddUsers, up.CanDeleteUsers, up.CanEditUsers" +
         " FROM Users u" +
         " JOIN UsersPermissions up ON u.UserId = up.UserId;";

        List<string[]> result = DatabaseManager.GetSqlQueryResults(query);

        foreach (string[] userDetails in result)
        {
            int id = int.Parse(userDetails[0]);
            string login = userDetails[1];
            string name = userDetails[2];
            string surname = userDetails[3];
            string email = userDetails[4];
            string position = userDetails[5];
            int hierarchy = int.Parse(userDetails[6]);
            bool canAddUsers = bool.Parse(userDetails[7]);
            bool canDeleteUsers = bool.Parse(userDetails[8]);
            bool canEditUsers = bool.Parse(userDetails[9]);

            OtherUser otherUser = new OtherUser(id, login, name, surname, email,
                position, hierarchy, canAddUsers, canDeleteUsers, canEditUsers);

            otherUsers.Add(otherUser);
        }
    }

}