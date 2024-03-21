using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MagazineManager.CmdDeveloperToolNS
{
    public static class CmdDeveloperToolUsers
    {
        private static string[] names = {
            "Adam", "Anna", "Michał", "Karolina", "Tomasz",
            "Magdalena", "Kamil", "Monika", "Piotr", "Patrycja",
            "Łukasz", "Kinga", "Krzysztof", "Natalia", "Marcin",
            "Justyna", "Bartosz", "Agata", "Mateusz", "Aleksandra"};

        private static string[] surnames = {
            "Kowalski", "Nowak", "Wiśniewski", "Wójcik", "Kowalczyk",
            "Kamiński", "Lewandowski", "Zielińska", "Szymański", "Woźniak",
            "Dąbrowski", "Kozłowski", "Jankowski", "Mazur", "Wojciechowski",
            "Kwiatkowski", "Kaczmarek", "Majewski", "Grabowski", "Olszewski"};

        private static string[] positions = {
            "Storekeeper", "Warehouse Manager", "Production Worker", "Machine Operator", "Logistics Specialist",
            "Cleaner", "Engineer", "Sales Manager", "Accountant", "IT Administrator",
            "Technician", "Quality Assurance Specialist", "Production Supervisor", "Financial Analyst", "Customer Advisor",
            "HR Manager", "Marketing Specialist", "Graphic Designer", "Office Clerk", "Distributor"};

        public static void addUser(Dictionary<string, List<string>> attributes)
        {
            if(attributes["Flag"].Count != 1)
            {
                Console.WriteLine("   [addUser error] Wrong number of flags.");
                return;
            }

            switch (attributes["Flag"][0])
            {
                case "r": addRandomUser(attributes["Text"]); break;
                case "s": addSimpleUser(attributes["Text"]); break;
                case "a": addAdvancedUser(attributes["Text"]); break;
            }

        }

        public static void addSimpleUser(List<string> text)
        {
            if(text.Count != 2)
            {
                Console.WriteLine("   [addUser -s error] Wrong number of arguments.");
                return;
            }

            string login = text[0];

            if (UserManagement.isLoginExist(login))
            {
                Console.WriteLine("   [addUser -s error]: This login is exist. You have to use another.");
                return;
            }

            SecureString password = PasswordManager.ConvertToSecureString(text[1]);
            string name = GetRandomName();
            string surname = GetRandomSurname();
            string email = name+surname+"@email.com";
            string position = GetRandomPosition();
            int hierarchy = 10;
            bool[] permissions = new bool[3];
            permissions[0] = false;
            permissions[1] = false;
            permissions[2] = false;



            if (UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
            {
                Console.WriteLine("   [addUser -s]: The account has been created.");
            }
        }

        public static void addRandomUser(List<string> text)
        {

            int amount = 0;
            string login = "";

            if (text.Count == 1)
            {
                if (int.TryParse(text[0], out int number))
                {
                    if(number > 100)
                    {
                        if (!confirmationAnswer("Are you sure you want to create more than 100 accounts?")) return;     
                    }

                    amount = number;
                }
                else
                {
                    Console.WriteLine("   [addUser -r]: Argument is not a number.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("   [addUser -r error] Wrong number of arguments.");
                return;
            }


            for (int i = 0; i <= amount; i++)
            {
                login = GetRandomLogin();
                SecureString password = PasswordManager.ConvertToSecureString("qwerty");
                string name = GetRandomName();
                string surname = GetRandomSurname();
                string email = name + surname + "@email.com";
                string position = GetRandomPosition();
                int hierarchy = 10;
                bool[] permissions = new bool[3];
                permissions[0] = false;
                permissions[1] = false;
                permissions[2] = false;



                if (!UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
                {
                    Console.WriteLine($"   [addUser-r]: The account {i} has been not created.");
                }
            }

            if(amount == 1)
            {
                Console.WriteLine($"   [addUser-r]: {login} account has been created.");
            }
            else
            {
                Console.WriteLine($"   [addUser-r]: {amount} accounts has been created.");
            }

        }

        public static void addAdvancedUser(List<string> text)
        {

            if (text.Count != 10)
            {
                Console.WriteLine("   [addUser -a error] Wrong number of arguments.");
                return;
            }

            string login = text[0];
            SecureString password = PasswordManager.ConvertToSecureString(text[1]);
            string name = text[2];
            string surname = text[3];
            string email = text[4];
            string position = text[5];
            int hierarchy;
            bool[] permissions = new bool[3];

            if (!int.TryParse(text[6], out hierarchy))
            {
                Console.WriteLine("   [addUser-a error]: Hierarchy must be an integer.");
                return;
            }

            permissions[0] = text[7] == "true";
            permissions[1] = text[8] == "true";
            permissions[2] = text[9] == "true";

            try
            {
                if (UserManagement.isLoginExist(login))
                {
                    Console.WriteLine("   [addUser-a error]: This login already exists. You have to use another.");
                    return;
                }

                if (UserManagement.AddUser(login, password, name, surname, email, position, hierarchy, permissions))
                {
                    Console.WriteLine("   [addUser-a]: The account has been created.");
                }
                else
                {
                    Console.WriteLine("   [addUser-a error]: Failed to create the account.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   [addUser-a error]: {ex.Message}");
            }
        }

        public static void editUser(Dictionary<string, List<string>> attributes)
        {
            if (attributes["Flag"].Count < 1 || attributes["Flag"].Count > 9)
            {
                Console.WriteLine("   [editUser error] Wrong number of flags.");
                return;
            }

            if (attributes["Text"].Count < 1 || attributes["Flag"].Count > 10)
            {
                Console.WriteLine("   [editUser error] Wrong number of attributes.");
                return;
            }

            if (attributes["Text"].Count != attributes["Flag"].Count+1)
            {
                Console.WriteLine("   [editUser error] Flag numbers are not the same as attributes.");
                return;
            }

            string login = attributes["Text"][0];

            if (!UserManagement.isLoginExist(login))
            {
                Console.WriteLine("   [editUser error] This login doesn't exist.");
                return;
            }

            Dictionary<string, string> userDataAcronyms = new Dictionary<string, string>();
            userDataAcronyms.Add("l", "Login");
            userDataAcronyms.Add("n", "Name");
            userDataAcronyms.Add("s", "Surname");
            userDataAcronyms.Add("e", "Email");
            userDataAcronyms.Add("p", "Position");
            userDataAcronyms.Add("h", "Hierarchy");
            userDataAcronyms.Add("cau", "CanAddUsers");
            userDataAcronyms.Add("cdu", "CanDeleteUsers");
            userDataAcronyms.Add("ceu", "CanEditUsers");

            foreach (string flag in attributes["Flag"]) //Checking are flags are not the same more than one
            {
                int amount = 0;

                foreach (string flagToCheck in attributes["Flag"])
                {
                    if (flag == flagToCheck) amount++;
                }

                if (amount != 1)
                {
                    Console.WriteLine("   [editUser error] You can not use the same flag more than one.");
                    return;
                }
            }

            foreach (string flag in attributes["Flag"]) //Checking are flags correct
            {
                int amount = 0;

                foreach (KeyValuePair<string, string> check in userDataAcronyms)
                {
                    if (flag == check.Key) amount++;
                }


                if (amount == 0)
                {
                    Console.WriteLine("   [editUser error] You use wrong flag/s.");
                    return;
                }
            }

            for(int i = 0; i < attributes["Flag"].Count; i++)
            {
                if(attributes["Flag"][i] == "l" && UserManagement.isLoginExist(attributes["Text"][i+1]))
                {
                    Console.WriteLine("   [editUser error] This login already exists.");
                    return;
                }

                string editComponent = userDataAcronyms[attributes["Flag"][i]];

                bool checkIsPermissionsData = (
                    attributes["Flag"][i] == "p"
                    || attributes["Flag"][i] == "h"
                    || attributes["Flag"][i] == "cau"
                    || attributes["Flag"][i] == "cdu"
                    || attributes["Flag"][i] == "ceu");

                string type = "Account";
                object editComponentValue = attributes["Text"][i+1];

                if (checkIsPermissionsData)
                {
                    if (attributes["Flag"][i] == "h")
                    {
                        if (int.TryParse(attributes["Text"][i + 1], out int number)) editComponentValue = number;
                        else
                        {
                            Console.WriteLine("   [editUser error] Hierarchy attribute is a number.");
                            return;
                        }
                    }
                    else if (attributes["Flag"][i] == "cau" || attributes["Flag"][i] == "cdu" || attributes["Flag"][i] == "ceu")
                    {
                        editComponentValue = attributes["Text"][i+1] == "true";
                    }

                    type = "Permissions";
                }

                if(UserManagement.EditUser(type, login, editComponent, editComponentValue))
                {
                    Console.WriteLine("   [editUser] You edited user.");
                }
            }

        }

        public static void deleteUserByLogin(Dictionary<string, List<string>> attributes)
        {
            if (attributes["Flag"].Count > 0)
            {
                Console.WriteLine("   [deleteUser error] This command do not have any flags.");
                return;
            }

            if (attributes["Text"].Count == 0)
            {
                Console.WriteLine("   [deleteUser error]: You did not provide a login.");
                return;
            }

            string login = attributes["Text"][0];

            if (UserManagement.DeleteUser(login))
            {
                Console.WriteLine($"   [deleteUser]: User {login} has been deleted.");
            }
            else
            {
                Console.WriteLine($"   [deleteUser]: User {login} doesn't exist.");
            }
        }

        public static void showUserDetailsByLogin(Dictionary<string, List<string>> attributes)
        {
            if (attributes["Flag"].Count > 0)
            {
                Console.WriteLine("   [showUser error] This command do not have any flags.");
                return;
            }

            if (attributes["Text"].Count == 0)
            {
                Console.WriteLine("   [showUser error]: You did not provide a login.");
                return;
            }

            string login = attributes["Text"][0];

            if (!UserManagement.isLoginExist(login))
            {
                Console.WriteLine($"   [showUser-l error]: User {login} doesn't exist.");
                return;
            }

            UsersCollection.RefreshUsers();

            User user = UsersCollection.GetUserFromLogin(login);

            Console.WriteLine(
                $"   [User {login} details]" +
                $"\n   Login: {login}" +
                $"\n   Name: {user.Name}" +
                $"\n   Surname: {user.Surname}" +
                $"\n   Email: {user.Email}" +
                $"\n   Position: {user.Position}" +
                $"\n   Hierarchy: {user.Hierarchy}" +
                $"\n   Can Add User: {user.CanAddUsers}" +
                $"\n   Can Delete User: {user.CanDeleteUsers}" +
                $"\n   Can Edit User: {user.CanEditUsers}");

        }

        private static string GetRandomName()
        {
            Random random = new Random();

            int randomNumber = random.Next(0, 19 + 1);

            return names[randomNumber];
        }

        private static string GetRandomSurname()
        {
            Random random = new Random();

            int randomNumber = random.Next(0, 19 + 1);

            return surnames[randomNumber];
        }

        private static string GetRandomPosition()
        {
            Random random = new Random();

            int randomNumber = random.Next(0, 19 + 1);

            return positions[randomNumber];
        }

        private static string GetRandomLogin()
        {
            Random random = new Random();

            string possibleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            string login;

            while(true)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    int index = random.Next(possibleCharacters.Length);
                    builder.Append(possibleCharacters[index]);
                }

                string randomString = builder.ToString();

                if (!UserManagement.isLoginExist(randomString))
                {
                    login = randomString;
                    break;
                }
            }


            return login;
        }

        private static bool confirmationAnswer(string answer)
        {
            Console.WriteLine(
            $"   {answer}\n" +
            "   Write   Yes   to confirmation...");

            string confirmation = Console.ReadLine().ToString();
            if (confirmation == "Yes") return true;

            Console.WriteLine("   You didn't accept.");
            return false;
        }

    }
}