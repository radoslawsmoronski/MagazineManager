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
}