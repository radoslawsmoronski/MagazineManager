using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MagazineManager.CmdDeveloperToolNS
{
    public static class CmdDeveloperToolUsers
    {
        public static void addSimpleUser(string fullCommand)
        {
            string[] commandParts = fullCommand.Split(' ');

            if (commandParts.Length == 1)
            {
                Console.WriteLine("   [addUser-s error]: Login and password have not been written.");
                return;
            }
            else if (commandParts.Length == 2)
            {
                Console.WriteLine("   [addUser-s error]: Password has not been written.");
                return;
            }

            string command = commandParts[0];
            string login = commandParts[1];

            if (UserManagement.isLoginExist(login))
            {
                Console.WriteLine("   [addUser-s error]: This login is exist. You have to use another.");
                return;
            }

            SecureString password = PasswordManager.ConvertToSecureString(commandParts[2]);
            string name = "[Sample name]";
            string surname = "[Sample surname]";
            string email = "[Sample email]";
            string position = "[Sample Position]";
            int hierarchy = 10;
            bool[] permissions = new bool[3];
            permissions[0] = false;
            permissions[1] = false;
            permissions[2] = false;



            if (UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
            {
                Console.WriteLine("   [addUser-s]: The account has been created.");
            }
        }
    }
}
