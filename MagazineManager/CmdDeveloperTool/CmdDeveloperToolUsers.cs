﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                string login = GetRandomLogin();
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

            Console.WriteLine($"   [addUser-r]: {amount} account/s has been created.");

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

        public static void deleteUserByLogin(string fullCommand)
        {
            string[] commandParts = fullCommand.Split(' ');

            if (commandParts.Length == 1)
            {
                Console.WriteLine("   [deleteUser-l error]: You did not provide a login.");
                return;
            }

            string login = commandParts[1];

            if (UserManagement.DeleteUser(login))
            {
                Console.WriteLine($"   [deleteUser-l]: User {login} has been deleted.");
            }
            else
            {
                Console.WriteLine($"   [deleteUser-l]: User {login} doesn't exist.");
            }
        }

        public static void showUserDetailsByLogin(string fullCommand)
        {
            string[] commandParts = fullCommand.Split(' ');

            if (commandParts.Length == 1)
            {
                Console.WriteLine("   [showUser-l error]: You did not provide a login.");
                return;
            }

            string login = commandParts[1];

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